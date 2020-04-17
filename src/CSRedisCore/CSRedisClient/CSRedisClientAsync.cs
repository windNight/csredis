
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

#if !net40
namespace CSRedis
{
    public partial class CSRedisClient
    {
        ConcurrentDictionary<string, AutoPipe> _autoPipe = new ConcurrentDictionary<string, AutoPipe>();
        class AutoPipe
        {
            public Object<RedisClient> Client;
            public long GetTimes;
            public long TimesZore;
            public bool IsSingleEndPipe;
            public Exception ReturnException;
        }
        async Task<AutoPipe> GetClientAsync(RedisClientPool pool)
        {
            if (pool._policy._asyncPipeline == false)
                return new AutoPipe { Client = await pool.GetAsync(), GetTimes = 1, TimesZore = 0, IsSingleEndPipe = false };

            if (_autoPipe.TryGetValue(pool.Key, out var ap) && ap.IsSingleEndPipe == false)
            {
                if (pool.UnavailableException != null)
                    throw new Exception($"【{pool._policy.Name}】状态不可用，等待后台检查程序恢复方可使用。{pool.UnavailableException?.Message}", pool.UnavailableException);
                Interlocked.Increment(ref ap.GetTimes);
                return ap;
            }
            ap = new AutoPipe { Client = await pool.GetAsync(), GetTimes = 1, TimesZore = 0, IsSingleEndPipe = false };
            if (_autoPipe.TryAdd(pool.Key, ap))
            {
                ap.Client.Value._asyncPipe = new ConcurrentQueue<TaskCompletionSource<object>>();
                new Thread(() =>
                {
                    var rc = ap.Client.Value;

                    void trySetException(Exception ex)
                    {
                        pool.SetUnavailable(ex);
                        while (rc._asyncPipe?.IsEmpty == false)
                        {
                            TaskCompletionSource<object> trytsc = null;
                            if (rc._asyncPipe?.TryDequeue(out trytsc) == true)
                                trytsc.TrySetException(ex);
                        }
                        rc._asyncPipe = null;
                        pool.Return(ap.Client);
                        _autoPipe.TryRemove(pool.Key, out var oldap);
                    }
                    while (true)
                    {
                        Thread.CurrentThread.Join(1);
                        if (rc._asyncPipe?.IsEmpty == false)
                        {
                            try
                            {
                                var ret = rc.EndPipe();
                                if (ret.Length == 1) ap.IsSingleEndPipe = true;
                                else if (ret.Length > 1) ap.IsSingleEndPipe = false;
                                foreach (var rv in ret)
                                {
                                    TaskCompletionSource<object> trytsc = null;
                                    if (rc._asyncPipe?.TryDequeue(out trytsc) == true)
                                        trytsc.TrySetResult(rv);
                                }
                            }
                            catch (Exception ex)
                            {
                                trySetException(ex);
                                return;
                            }
                            continue;
                        }

                        if (ap.ReturnException != null)
                        {
                            trySetException(ap.ReturnException);
                            return;
                        }
                        var tmpTimes = Interlocked.Increment(ref ap.TimesZore);
                        if (tmpTimes >= 10) ap.IsSingleEndPipe = false;
                        if (tmpTimes >= 1000)
                        {
                            rc._asyncPipe = null;
                            pool.Return(ap.Client, ap.ReturnException);
                            _autoPipe.TryRemove(pool.Key, out var oldap);
                            break;
                        }
                    }
                }).Start();
            }
            return ap;
        }
        void ReturnClient(AutoPipe ap, Object<RedisClient> obj, RedisClientPool pool, Exception ex)
        {
            if (ap == null) return;
            var times = Interlocked.Decrement(ref ap.GetTimes);
            if (times <= 0)
                Interlocked.Exchange(ref ap.TimesZore, 0);
            ap.ReturnException = ex;
            if (_autoPipe.TryGetValue(pool.Key, out var dicap) == false || dicap != ap)
                pool.Return(ap.Client, ap.ReturnException);
        }


        private async Task<T> NodesNotSupportAsync<T>(string[] keys, T defaultValue, Func<Object<RedisClient>, string[], Task<T>> callbackAsync)
        {
            if (keys == null || keys.Any() == false) return defaultValue;
            var rules = Nodes.Count > 1 ? keys.Select(a => NodeRuleRaw(a)).Distinct() : new[] { Nodes.FirstOrDefault().Key };
            if (rules.Count() > 1) throw new Exception("由于开启了分区模式，keys 分散在多个节点，无法使用此功能");
            var pool = Nodes.TryGetValue(rules.First(), out var b) ? b : Nodes.First().Value;
            string[] rkeys = new string[keys.Length];
            for (int a = 0; a < keys.Length; a++) rkeys[a] = string.Concat(pool.Prefix, keys[a]);
            if (rkeys.Length == 0) return defaultValue;
            return await GetAndExecuteAsync(pool, conn => callbackAsync(conn, rkeys));
        }

        private async Task<T> NodesNotSupportAsync<T>(string key, Func<Object<RedisClient>, string, Task<T>> callback)
        {
            if (IsMultiNode) throw new Exception("由于开启了分区模式，无法使用此功能");
            return await ExecuteScalarAsync<T>(key, callback);
        }



        #region 分区方式 ExecuteAsync

        private async Task<T> GetAndExecuteAsync<T>(RedisClientPool pool, Func<Object<RedisClient>, Task<T>> handerAsync, int jump = 100, int errtimes = 0)
        {
            AutoPipe ap = null;
            Object<RedisClient> obj = null;
            Exception ex = null;
            var redirect = ParseClusterRedirect(null);
            try
            {
                //obj = await pool.GetAsync();
                ap = await GetClientAsync(pool);
                obj = ap.Client;
                while (true)
                { //因网络出错重试，默认1次
                    try
                    {
                        var ret = await handerAsync(obj);
                        return ret;
                    }
                    catch (RedisException ex3)
                    {
                        redirect = ParseClusterRedirect(ex3); //官方集群跳转
                        if (redirect == null || jump <= 0)
                        {
                            ex = ex3;
                            if (SentinelManager != null && ex.Message.Contains("READONLY"))
                            { //哨兵轮询
                                if (pool.SetUnavailable(ex) == true)
                                    BackgroundGetSentinelMasterValue();
                            }
                            throw ex;
                        }
                        break;
                    }
                    catch (Exception ex2)
                    {
                        ex = ex2;
                        var isPong = false;
                        try
                        {
                            await obj.Value.PingAsync();
                            isPong = true;
                        }
                        catch
                        {
                            obj.ResetValue();
                        }

                        if (isPong == false || ++errtimes > pool._policy._tryit)
                        {
                            if (SentinelManager != null)
                            { //哨兵轮询
                                if (pool.SetUnavailable(ex) == true)
                                    BackgroundGetSentinelMasterValue();
                                throw new Exception($"Redis Sentinel Master is switching：{ex.Message}");
                            }
                            throw ex; //重试次数完成
                        }
                        else
                        {
                            ex = null;
                            Trace.WriteLine($"csredis tryit ({errtimes}) ...");
                        }
                    }
                }
            }
            finally
            {
                //pool.Return(obj, ex);
                ReturnClient(ap, obj, pool, ex);
            }
            if (redirect == null)
                return await GetAndExecuteAsync<T>(pool, handerAsync, jump - 1, errtimes);

            var redirectHanderAsync = redirect.Value.isMoved ? handerAsync : async redirectObj =>
            {
                await redirectObj.Value.CallAsync("ASKING");
                return await handerAsync(redirectObj);
            };
            return await GetAndExecuteAsync<T>(GetRedirectPool(redirect.Value, pool), redirectHanderAsync, jump - 1);
        }

        private async Task<T> ExecuteScalarAsync<T>(string key, Func<Object<RedisClient>, string, Task<T>> handerAsync)
        {
            if (key == null) return default(T);
            var pool = NodeRuleRaw == null || Nodes.Count == 1 ? Nodes.First().Value : (Nodes.TryGetValue(NodeRuleRaw(key), out var b) ? b : Nodes.First().Value);
            key = string.Concat(pool.Prefix, key);
            return await GetAndExecuteAsync(pool, conn => handerAsync(conn, key));
        }

        private async Task<T[]> ExecuteArrayAsync<T>(string[] key, Func<Object<RedisClient>, string[], Task<T[]>> handerAsync)
        {
            if (key == null || key.Any() == false) return new T[0];
            if (NodeRuleRaw == null || Nodes.Count == 1)
            {
                var pool = Nodes.First().Value;
                var keys = key.Select(a => string.Concat(pool.Prefix, a)).ToArray();
                return await GetAndExecuteAsync(pool, conn => handerAsync(conn, keys));
            }
            var rules = new Dictionary<string, List<(string, int)>>();
            for (var a = 0; a < key.Length; a++)
            {
                var rule = NodeRuleRaw(key[a]);
                if (rules.ContainsKey(rule)) rules[rule].Add((key[a], a));
                else rules.Add(rule, new List<(string, int)> { (key[a], a) });
            }
            T[] ret = new T[key.Length];
            foreach (var r in rules)
            {
                var pool = Nodes.TryGetValue(r.Key, out var b) ? b : Nodes.First().Value;
                var keys = r.Value.Select(a => string.Concat(pool.Prefix, a.Item1)).ToArray();
                await GetAndExecuteAsync(pool, async conn =>
                {
                    var vals = await handerAsync(conn, keys);
                    for (var z = 0; z < r.Value.Count; z++)
                    {
                        ret[r.Value[z].Item2] = vals == null || z >= vals.Length ? default(T) : vals[z];
                    }
                    return 0;
                });
            }
            return ret;
        }

        private async Task<long> ExecuteNonQueryAsync(string[] key, Func<Object<RedisClient>, string[], Task<long>> handerAsync)
        {
            if (key == null || key.Any() == false) return 0;
            if (NodeRuleRaw == null || Nodes.Count == 1)
            {
                var pool = Nodes.First().Value;
                var keys = key.Select(a => string.Concat(pool.Prefix, a)).ToArray();
                return await GetAndExecuteAsync(pool, conn => handerAsync(conn, keys));
            }
            var rules = new Dictionary<string, List<string>>();
            for (var a = 0; a < key.Length; a++)
            {
                var rule = NodeRuleRaw(key[a]);
                if (rules.ContainsKey(rule)) rules[rule].Add(key[a]);
                else rules.Add(rule, new List<string> { key[a] });
            }
            long affrows = 0;
            foreach (var r in rules)
            {
                var pool = Nodes.TryGetValue(r.Key, out var b) ? b : Nodes.First().Value;
                var keys = r.Value.Select(a => string.Concat(pool.Prefix, a)).ToArray();
                affrows += await GetAndExecuteAsync(pool, conn => handerAsync(conn, keys));
            }
            return affrows;
        }

        #endregion

    }
}
#endif