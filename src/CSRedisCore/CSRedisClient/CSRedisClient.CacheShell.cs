using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
#if !net40
using System.Threading.Tasks;
#endif

namespace CSRedis
{
    public partial class CSRedisClient
    {

        #region 缓存壳
        /// <summary>
        /// 缓存壳
        /// </summary>
        /// <typeparam name="T">缓存类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="timeoutSeconds">缓存秒数</param>
        /// <param name="getData">获取源数据的函数</param>
        /// <returns></returns>
        public T CacheShell<T>(string key, int timeoutSeconds, Func<T> getData)
        {
            if (timeoutSeconds == 0) return getData();
            var cacheValue = Get(key);
            if (cacheValue != null)
            {
                try
                {
                    return this.DeserializeObject<T>(cacheValue);
                }
                catch
                {
                    Del(key);
                    throw;
                }
            }
            var ret = getData();
            Set(key, this.SerializeObject(ret), timeoutSeconds);
            return ret;
        }
        /// <summary>
        /// 缓存壳(哈希表)
        /// </summary>
        /// <typeparam name="T">缓存类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="field">字段</param>
        /// <param name="timeoutSeconds">缓存秒数</param>
        /// <param name="getData">获取源数据的函数</param>
        /// <returns></returns>
        public T CacheShell<T>(string key, string field, int timeoutSeconds, Func<T> getData)
        {
            if (timeoutSeconds == 0) return getData();
            var cacheValue = HGet(key, field);
            if (cacheValue != null)
            {
                try
                {
                    var value = this.DeserializeObject<(T, long)>(cacheValue);
                    if (DateTime.Now.Subtract(_dt1970.AddSeconds(value.Item2)).TotalSeconds <= timeoutSeconds) return value.Item1;
                }
                catch
                {
                    HDel(key, field);
                    throw;
                }
            }
            var ret = getData();
            HSet(key, field, this.SerializeObject((ret, (long)DateTime.Now.Subtract(_dt1970).TotalSeconds)));
            return ret;
        }
        /// <summary>
        /// 缓存壳(哈希表)，将 fields 每个元素存储到单独的缓存片，实现最大化复用
        /// </summary>
        /// <typeparam name="T">缓存类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="fields">字段</param>
        /// <param name="timeoutSeconds">缓存秒数</param>
        /// <param name="getData">获取源数据的函数，输入参数是没有缓存的 fields，返回值应该是 (field, value)[]</param>
        /// <returns></returns>
        public (string key, T value)[] CacheShell<T>(string key, string[] fields, int timeoutSeconds, Func<string[], (string, T)[]> getData)
        {
            fields = fields?.Distinct().ToArray();
            if (fields == null || fields.Length == 0) return new (string, T)[0];
            if (timeoutSeconds == 0) return getData(fields);

            var ret = new (string, T)[fields.Length];
            var cacheValue = HMGet(key, fields);
            var fieldsMGet = new Dictionary<string, int>();

            for (var a = 0; a < ret.Length; a++)
            {
                if (cacheValue[a] != null)
                {
                    try
                    {
                        var value = this.DeserializeObject<(T, long)>(cacheValue[a]);
                        if (DateTime.Now.Subtract(_dt1970.AddSeconds(value.Item2)).TotalSeconds <= timeoutSeconds)
                        {
                            ret[a] = (fields[a], value.Item1);
                            continue;
                        }
                    }
                    catch
                    {
                        HDel(key, fields[a]);
                        throw;
                    }
                }
                fieldsMGet.Add(fields[a], a);
            }

            if (fieldsMGet.Any())
            {
                var getDataIntput = fieldsMGet.Keys.ToArray();
                var data = getData(getDataIntput);
                var mset = new object[fieldsMGet.Count * 2];
                var msetIndex = 0;
                foreach (var d in data)
                {
                    if (fieldsMGet.ContainsKey(d.Item1) == false) throw new Exception($"使用 CacheShell 请确认 getData 返回值 (string, T)[] 中的 Item1 值: {d.Item1} 存在于 输入参数: {string.Join(",", getDataIntput)}");
                    ret[fieldsMGet[d.Item1]] = d;
                    mset[msetIndex++] = d.Item1;
                    mset[msetIndex++] = this.SerializeObject((d.Item2, (long)DateTime.Now.Subtract(_dt1970).TotalSeconds));
                    fieldsMGet.Remove(d.Item1);
                }
                foreach (var fieldNull in fieldsMGet.Keys)
                {
                    ret[fieldsMGet[fieldNull]] = (fieldNull, default(T));
                    mset[msetIndex++] = fieldNull;
                    mset[msetIndex++] = this.SerializeObject((default(T), (long)DateTime.Now.Subtract(_dt1970).TotalSeconds));
                }
                if (mset.Any()) HMSet(key, mset);
            }
            return ret;
        }
        #endregion


#if !net40


        #region 缓存壳
        /// <summary>
        /// 缓存壳
        /// </summary>
        /// <typeparam name="T">缓存类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="timeoutSeconds">缓存秒数</param>
        /// <param name="getDataAsync">获取源数据的函数</param>
        /// <returns></returns>
        async public Task<T> CacheShellAsync<T>(string key, int timeoutSeconds, Func<Task<T>> getDataAsync)
        {
            if (timeoutSeconds == 0) return await getDataAsync();
            var cacheValue = await GetAsync(key);
            if (cacheValue != null)
            {
                try
                {
                    return this.DeserializeObject<T>(cacheValue);
                }
                catch
                {
                    await DelAsync(key);
                    throw;
                }
            }
            var ret = await getDataAsync();
            await SetAsync(key, this.SerializeObject(ret), timeoutSeconds);
            return ret;
        }
        /// <summary>
        /// 缓存壳(哈希表)
        /// </summary>
        /// <typeparam name="T">缓存类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="field">字段</param>
        /// <param name="timeoutSeconds">缓存秒数</param>
        /// <param name="getDataAsync">获取源数据的函数</param>
        /// <returns></returns>
        async public Task<T> CacheShellAsync<T>(string key, string field, int timeoutSeconds, Func<Task<T>> getDataAsync)
        {
            if (timeoutSeconds == 0) return await getDataAsync();
            var cacheValue = await HGetAsync(key, field);
            if (cacheValue != null)
            {
                try
                {
                    var value = this.DeserializeObject<(T, long)>(cacheValue);
                    if (DateTime.Now.Subtract(_dt1970.AddSeconds(value.Item2)).TotalSeconds <= timeoutSeconds) return value.Item1;
                }
                catch
                {
                    await HDelAsync(key, field);
                    throw;
                }
            }
            var ret = await getDataAsync();
            await HSetAsync(key, field, this.SerializeObject((ret, (long)DateTime.Now.Subtract(_dt1970).TotalSeconds)));
            return ret;
        }
        /// <summary>
        /// 缓存壳(哈希表)，将 fields 每个元素存储到单独的缓存片，实现最大化复用
        /// </summary>
        /// <typeparam name="T">缓存类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="fields">字段</param>
        /// <param name="timeoutSeconds">缓存秒数</param>
        /// <param name="getDataAsync">获取源数据的函数，输入参数是没有缓存的 fields，返回值应该是 (field, value)[]</param>
        /// <returns></returns>
        public async Task<(string key, T value)[]> CacheShellAsync<T>(string key, string[] fields, int timeoutSeconds, Func<string[], Task<(string, T)[]>> getDataAsync)
        {
            fields = fields?.Distinct().ToArray();
            if (fields == null || fields.Length == 0) return new (string, T)[0];
            if (timeoutSeconds == 0) return await getDataAsync(fields);

            var ret = new (string, T)[fields.Length];
            var cacheValue = await HMGetAsync(key, fields);
            var fieldsMGet = new Dictionary<string, int>();

            for (var a = 0; a < ret.Length; a++)
            {
                if (cacheValue[a] != null)
                {
                    try
                    {
                        var value = this.DeserializeObject<(T, long)>(cacheValue[a]);
                        if (DateTime.Now.Subtract(_dt1970.AddSeconds(value.Item2)).TotalSeconds <= timeoutSeconds)
                        {
                            ret[a] = (fields[a], value.Item1);
                            continue;
                        }
                    }
                    catch
                    {
                        await HDelAsync(key, fields[a]);
                        throw;
                    }
                }
                fieldsMGet.Add(fields[a], a);
            }

            if (fieldsMGet.Any())
            {
                var getDataIntput = fieldsMGet.Keys.ToArray();
                var data = await getDataAsync(getDataIntput);
                var mset = new object[fieldsMGet.Count * 2];
                var msetIndex = 0;
                foreach (var d in data)
                {
                    if (fieldsMGet.ContainsKey(d.Item1) == false) throw new Exception($"使用 CacheShell 请确认 getData 返回值 (string, T)[] 中的 Item1 值: {d.Item1} 存在于 输入参数: {string.Join(",", getDataIntput)}");
                    ret[fieldsMGet[d.Item1]] = d;
                    mset[msetIndex++] = d.Item1;
                    mset[msetIndex++] = this.SerializeObject((d.Item2, (long)DateTime.Now.Subtract(_dt1970).TotalSeconds));
                    fieldsMGet.Remove(d.Item1);
                }
                foreach (var fieldNull in fieldsMGet.Keys)
                {
                    ret[fieldsMGet[fieldNull]] = (fieldNull, default(T));
                    mset[msetIndex++] = fieldNull;
                    mset[msetIndex++] = this.SerializeObject((default(T), (long)DateTime.Now.Subtract(_dt1970).TotalSeconds));
                }
                if (mset.Any()) await HMSetAsync(key, mset);
            }
            return ret;
        }
        #endregion

#endif

    }
}