using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CSRedis
{
    public partial class CSRedisClient : IDisposable
    {
        /// <summary>
        /// 按 key 规则分区存储
        /// </summary>
        public ConcurrentDictionary<string, RedisClientPool> Nodes { get; } = new ConcurrentDictionary<string, RedisClientPool>();

        private int NodesIndexIncrement = -1;

        public ConcurrentDictionary<int, string> NodesIndex { get; } = new ConcurrentDictionary<int, string>();

        private ConcurrentDictionary<string, int> NodesKey { get; } = new ConcurrentDictionary<string, int>();

        internal Func<string, string> NodeRuleRaw;

        internal Func<string, string> NodeRuleExternal;

        internal RedisSentinelManager SentinelManager;

        internal string SentinelMasterName;

        internal string SentinelMasterValue;

        internal bool IsMultiNode => Nodes.Count > 1 && SentinelManager == null;

        private object NodesLock = new object();

        public ConcurrentDictionary<ushort, ushort> SlotCache = new ConcurrentDictionary<ushort, ushort>();


        private Func<JsonSerializerSettings> JsonSerializerSettings = () =>
        {
            var st = new JsonSerializerSettings();
            st.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            st.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            st.DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
            return st;
        };

        /// <summary>
		/// 自定义序列化(全局默认)
		/// </summary>
		public static Func<object, string> Serialize;

        /// <summary>
        /// 自定义反序列化(全局默认)
        /// </summary>
        public static Func<string, Type, object> Deserialize;

        /// <summary>
        /// 自定义序列化
        /// </summary>
        public Func<object, string> CurrentSerialize;

        /// <summary>
        /// 自定义反序列化
        /// </summary>
        public Func<string, Type, object> CurrentDeserialize;

        DateTime _dt1970 = new DateTime(1970, 1, 1);

        Random _rnd = new Random();

        #region 序列化写入，反序列化
        internal string SerializeObject(object value)
        {
            if (CurrentSerialize != null) return CurrentSerialize(value);
            if (Serialize != null) return Serialize(value);
            return JsonConvert.SerializeObject(value, this.JsonSerializerSettings());
        }
        internal T DeserializeObject<T>(string value)
        {
            if (CurrentDeserialize != null) return (T)CurrentDeserialize(value, typeof(T));
            if (Deserialize != null) return (T)Deserialize(value, typeof(T));
            return JsonConvert.DeserializeObject<T>(value, this.JsonSerializerSettings());
        }

        internal object SerializeRedisValueInternal(object value)
        {

            if (value == null) return null;
            var type = value.GetType();
            var typename = type.ToString().TrimEnd(']');
            if (typename == "System.Byte[" ||
                typename == "System.String") return value;

            if (type.IsValueType)
            {
                bool isNullable = typename.StartsWith("System.Nullable`1[");
                var basename = isNullable ? typename.Substring(18) : typename;

                switch (basename)
                {
                    case "System.Boolean": return value.ToString() == "True" ? "1" : "0";
                    case "System.Byte": return value.ToString();
                    case "System.Char": return value.ToString()[0];
                    case "System.Decimal":
                    case "System.Double":
                    case "System.Single":
                    case "System.Int32":
                    case "System.Int64":
                    case "System.SByte":
                    case "System.Int16":
                    case "System.UInt32":
                    case "System.UInt64":
                    case "System.UInt16": return value.ToString();
                    case "System.DateTime": return ((DateTime)value).ToString("yyyy-MM-ddTHH:mm:sszzzz", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    case "System.DateTimeOffset": return value.ToString();
                    case "System.TimeSpan": return ((TimeSpan)value).Ticks;
                    case "System.Guid": return value.ToString();
                }
            }

            return this.SerializeObject(value);
        }
        internal T DeserializeRedisValueInternal<T>(byte[] value)
        {
            if (value == null) return default(T);
            var type = typeof(T);
            var typename = type.ToString().TrimEnd(']');
            if (typename == "System.Byte[") return (T)Convert.ChangeType(value, type);
            if (typename == "System.String") return (T)Convert.ChangeType(Nodes.First().Value.Encoding.GetString(value), type);
            if (typename == "System.Boolean[") return (T)Convert.ChangeType(value.Select(a => a == 49).ToArray(), type);

            var valueStr = Nodes.First().Value.Encoding.GetString(value);
            if (string.IsNullOrEmpty(valueStr)) return default(T);
            if (type.IsValueType)
            {
                bool isNullable = typename.StartsWith("System.Nullable`1[");
                var basename = isNullable ? typename.Substring(18) : typename;

                bool isElse = false;
                object obj = null;
                switch (basename)
                {
                    case "System.Boolean":
                        if (valueStr == "1") obj = true;
                        else if (valueStr == "0") obj = false;
                        break;
                    case "System.Byte":
                        if (byte.TryParse(valueStr, out var trybyte)) obj = trybyte;
                        break;
                    case "System.Char":
                        if (valueStr.Length > 0) obj = valueStr[0];
                        break;
                    case "System.Decimal":
                        if (Decimal.TryParse(valueStr, out var trydec)) obj = trydec;
                        break;
                    case "System.Double":
                        if (Double.TryParse(valueStr, out var trydb)) obj = trydb;
                        break;
                    case "System.Single":
                        if (Single.TryParse(valueStr, out var trysg)) obj = trysg;
                        break;
                    case "System.Int32":
                        if (Int32.TryParse(valueStr, out var tryint32)) obj = tryint32;
                        break;
                    case "System.Int64":
                        if (Int64.TryParse(valueStr, out var tryint64)) obj = tryint64;
                        break;
                    case "System.SByte":
                        if (SByte.TryParse(valueStr, out var trysb)) obj = trysb;
                        break;
                    case "System.Int16":
                        if (Int16.TryParse(valueStr, out var tryint16)) obj = tryint16;
                        break;
                    case "System.UInt32":
                        if (UInt32.TryParse(valueStr, out var tryuint32)) obj = tryuint32;
                        break;
                    case "System.UInt64":
                        if (UInt64.TryParse(valueStr, out var tryuint64)) obj = tryuint64;
                        break;
                    case "System.UInt16":
                        if (UInt16.TryParse(valueStr, out var tryuint16)) obj = tryuint16;
                        break;
                    case "System.DateTime":
                        if (DateTime.TryParse(valueStr, out var trydt)) obj = trydt;
                        break;
                    case "System.DateTimeOffset":
                        if (DateTimeOffset.TryParse(valueStr, out var trydtos)) obj = trydtos;
                        break;
                    case "System.TimeSpan":
                        if (Int64.TryParse(valueStr, out tryint64)) obj = new TimeSpan(tryint64);
                        break;
                    case "System.Guid":
                        if (Guid.TryParse(valueStr, out var tryguid)) obj = tryguid;
                        break;
                    default:
                        isElse = true;
                        break;
                }

                if (isElse == false)
                {
                    if (obj == null) return default(T);
                    return (T)obj;
                    //return (T)Convert.ChangeType(obj, typeof(T));
                }
            }

            return this.DeserializeObject<T>(valueStr);
        }
        internal T[] DeserializeRedisValueArrayInternal<T>(byte[][] value)
        {
            if (value == null) return null;
            var list = new T[value.Length];
            for (var a = 0; a < value.Length; a++) list[a] = this.DeserializeRedisValueInternal<T>(value[a]);
            return list;
        }
        internal (T1, T2)[] DeserializeRedisValueTuple1Internal<T1, T2>(Tuple<byte[], T2>[] value)
        {
            if (value == null) return null;
            var list = new (T1, T2)[value.Length];
            for (var a = 0; a < value.Length; a++) list[a] = (this.DeserializeRedisValueInternal<T1>(value[a].Item1), value[a].Item2);
            return list;
        }
        internal (T2, T1)[] DeserializeRedisValueTuple2Internal<T2, T1>(Tuple<T2, byte[]>[] value)
        {
            if (value == null) return null;
            var list = new (T2, T1)[value.Length];
            for (var a = 0; a < value.Length; a++) list[a] = (value[a].Item1, this.DeserializeRedisValueInternal<T1>(value[a].Item2));
            return list;
        }
        internal Dictionary<TKey, TValue> DeserializeRedisValueDictionaryInternal<TKey, TValue>(Dictionary<TKey, byte[]> value)
        {
            if (value == null) return null;
            var dic = new Dictionary<TKey, TValue>();
            foreach (var kv in value) dic.Add(kv.Key, this.DeserializeRedisValueInternal<TValue>(kv.Value));
            return dic;
        }
        #endregion

        /// <summary>
        /// 创建redis访问类(支持单机或集群)
        /// </summary>
        /// <param name="connectionString">127.0.0.1[:6379],password=123456,defaultDatabase=13,poolsize=50,ssl=false,writeBuffer=10240,prefix=key前辍</param>
        public CSRedisClient(string connectionString) : this(null, new string[0], false, connectionString) { }

        /// <summary>
        /// 创建redis哨兵访问类(Redis Sentinel)
        /// </summary>
        /// <param name="connectionString">mymaster,password=123456,poolsize=50,connectTimeout=200,ssl=false</param>
        /// <param name="sentinels">哨兵节点，如：ip1:26379、ip2:26379</param>
        /// <param name="readOnly">false: 只获取master节点进行读写操作<para></para>true: 只获取可用slave节点进行只读操作</param>
        public CSRedisClient(string connectionString, string[] sentinels, bool readOnly = false) : this(null, sentinels, readOnly, connectionString) { }

        /// <summary>
        /// 创建redis分区访问类，通过 KeyRule 对 key 进行分区，连接对应的 connectionString
        /// </summary>
        /// <param name="NodeRule">按key分区规则，返回值格式：127.0.0.1:6379/13，默认方案(null)：取key哈希与节点数取模</param>
        /// <param name="connectionStrings">127.0.0.1[:6379],password=123456,defaultDatabase=13,poolsize=50,ssl=false,writeBuffer=10240,prefix=key前辍</param>
        public CSRedisClient(Func<string, string> NodeRule, params string[] connectionStrings) : this(NodeRule, null, false, connectionStrings) { }

        CSRedisClient(Func<string, string> NodeRule, string[] sentinels, bool readOnly, params string[] connectionStrings)
        {
            if (connectionStrings == null || connectionStrings.Any() == false) throw new Exception("Redis ConnectionString 未设置");
            var tmppoolPolicy = new RedisClientPoolPolicy();
            tmppoolPolicy.ConnectionString = connectionStrings.First() + ",preheat=false";

            if (sentinels?.Any() == true)
            {
                if (connectionStrings.Length > 1) throw new Exception("Redis Sentinel 不可设置多个 ConnectionString");
                SentinelManager = new RedisSentinelManager(readOnly, sentinels);
                SentinelManager.Connected += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(tmppoolPolicy._password))
                    {
                        try
                        {
                            SentinelManager.Call(c => c.Auth(tmppoolPolicy._password));
                        }
                        catch (Exception authEx)
                        {
                            if (authEx.Message != "ERR Client sent AUTH, but no password is set")
                                throw authEx;
                        }
                    }
                };
                SentinelMasterName = connectionStrings.First().Split(',').FirstOrDefault() ?? "mymaster";
                try
                {
                    SentinelMasterValue = SentinelManager.Connect(SentinelMasterName, tmppoolPolicy._connectTimeout);
                }
                catch
                {
                    //没有可用的master
                }
            }
            RedisClientPool firstPool = null;
            this.NodeRuleRaw = key =>
            {
                if (Nodes.Count <= 1) return NodesIndex[0];

                var prefix = firstPool?.Prefix;
                var slot = GetClusterSlot(string.Concat(prefix, key)); //redis-cluster 模式，选取第一个 connectionString prefix 前辍求 slot
                if (SlotCache.TryGetValue(slot, out var slotIndex) && NodesIndex.TryGetValue(slotIndex, out var slotKey))
                {
                    if (Nodes.TryGetValue(slotKey, out var b) && b.IsAvailable == false)
                    {
                        var availableNode = Nodes.FirstOrDefault(a => a.Value.IsAvailable);
                        if (string.IsNullOrEmpty(availableNode.Key) == false) return availableNode.Key; //随便连向一个可用的节点
                    }
                    return slotKey; //按上一次 MOVED 记录查找节点
                }
                if (this.NodeRuleExternal == null)
                {
                    if (string.IsNullOrEmpty(prefix) == false) slot = GetClusterSlot(key ?? string.Empty);
                    var idx = slot % NodesIndex.Count;
                    slotKey = idx < 0 || idx >= NodesIndex.Count ? NodesIndex[0] : NodesIndex[idx];
                    if (Nodes.TryGetValue(slotKey, out var b) && b.IsAvailable == false)
                    {
                        var availableNode = Nodes.FirstOrDefault(a => a.Value.IsAvailable);
                        if (string.IsNullOrEmpty(availableNode.Key) == false) return availableNode.Key; //随便连向一个可用的节点
                    }
                    return slotKey;
                }
                return this.NodeRuleExternal(key);
            };
            this.NodeRuleExternal = NodeRule;

            foreach (var connectionString in connectionStrings)
            {
                var connStr = connectionString;
                if (SentinelManager != null)
                {
                    var startIdx = connStr.IndexOf(',');
                    connStr = startIdx == -1 ? "" : connStr.Substring(startIdx);
                    if (string.IsNullOrEmpty(SentinelMasterValue))
                        connStr = $"255.255.255.255:19736{connStr},preheat=false"; //这是一个等待恢复的 pool
                    else
                        connStr = $"{SentinelMasterValue}{connStr}";
                }

                var pool = new RedisClientPool(connStr, client => { });
                var nodeKey = SentinelMasterName ?? pool.Key;
                if (Nodes.ContainsKey(nodeKey)) throw new Exception($"Node: {nodeKey} 重复，请检查");
                if (this.TryAddNode(nodeKey, pool) == false)
                {
                    pool.Dispose();
                    pool = null;
                    throw new Exception($"Node: {nodeKey} 无法添加");
                }
                if (firstPool == null) firstPool = pool;
            }
            this.NodesServerManager = new NodesServerManagerProvider(this);
            if (firstPool._policy._testCluster)
            {
                //尝试求出其他节点，并缓存slot
                try
                {
                    byte[] cnret = null;
                    using (var obj = firstPool.Get())
                    {
                        cnret = obj.Value.Call("cluster nodes") as byte[];
                    }
                    if (cnret != null)
                    {
                        var cnodes = firstPool.Encoding.GetString(cnret).Split('\n');
                        foreach (var cnode in cnodes)
                        {
                            if (string.IsNullOrEmpty(cnode)) continue;
                            var dt = cnode.Trim().Split(' ');
                            if (dt.Length >= 9)
                            {
                                if (dt[2].StartsWith("master") || dt[2].EndsWith("master"))
                                {
                                    if (dt[7] == "connected")
                                    {
                                        var endpoint = dt[1];
                                        var at40 = endpoint.IndexOf('@');
                                        if (at40 != -1) endpoint = endpoint.Remove(at40);

                                        for (var slotIndex = 8; slotIndex < dt.Length; slotIndex++)
                                        {
                                            var slots = dt[slotIndex].Split('-');
                                            if (ushort.TryParse(slots[0], out var tryslotStart) &&
                                                ushort.TryParse(slots[1], out var tryslotEnd))
                                            {
                                                for (var slot = tryslotStart; slot <= tryslotEnd; slot++)
                                                {
                                                    GetRedirectPool((true, false, slot, endpoint), firstPool);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch { }
            }
        }

        public void Dispose()
        {
            foreach (var pool in this.Nodes.Values) pool.Dispose();
            SentinelManager?.Dispose();
        }

        bool BackgroundGetSentinelMasterValueIng = false;

        object BackgroundGetSentinelMasterValueIngLock = new object();

        bool BackgroundGetSentinelMasterValue()
        {
            if (SentinelManager == null) return false;
            if (Nodes.Count > 1) return false;

            var ing = false;
            if (BackgroundGetSentinelMasterValueIng == false)
            {
                lock (BackgroundGetSentinelMasterValueIngLock)
                {
                    if (BackgroundGetSentinelMasterValueIng == false)
                    {
                        BackgroundGetSentinelMasterValueIng = ing = true;
                    }
                }
            }

            if (ing)
            {
                var pool = Nodes.First().Value;
                new Thread(() =>
                {
                    while (true)
                    {
                        Thread.CurrentThread.Join(1000);
                        try
                        {
                            SentinelMasterValue = SentinelManager.Connect(SentinelMasterName, pool._policy._connectTimeout);
                            pool._policy.SetHost(SentinelMasterValue);
                            if (pool.CheckAvailable())
                            {

                                var bgcolor = Console.BackgroundColor;
                                var forecolor = Console.ForegroundColor;
                                Console.BackgroundColor = ConsoleColor.DarkGreen;
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write($"Redis Sentinel Pool 已切换至 {SentinelMasterValue}");
                                Console.BackgroundColor = bgcolor;
                                Console.ForegroundColor = forecolor;
                                Console.WriteLine();

                                BackgroundGetSentinelMasterValueIng = false;
                                return;
                            }
                        }
                        catch (Exception ex21)
                        {
                            Trace.WriteLine($"Redis Sentinel: {ex21.Message}");
                        }
                    }
                }).Start();
            }
            return ing;
        }

        T GetAndExecute<T>(RedisClientPool pool, Func<Object<RedisClient>, T> handler, int jump = 100, int errtimes = 0)
        {
            Object<RedisClient> obj = null;
            Exception ex = null;
            var redirect = ParseClusterRedirect(null);
            try
            {
                obj = pool.Get();
                while (true)
                { //因网络出错重试，默认1次
                    try
                    {
                        var ret = handler(obj);
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
                        if (pool.UnavailableException != null) throw ex;
                        var isPong = false;
                        try
                        {
                            obj.Value.Ping();
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
                pool.Return(obj, ex);
            }
            if (redirect == null)
                return GetAndExecute(pool, handler, jump - 1, errtimes);

            var redirectHander = redirect.Value.isMoved ? handler : redirectObj =>
            {
                redirectObj.Value.Call("ASKING");
                return handler(redirectObj);
            };
            return GetAndExecute<T>(GetRedirectPool(redirect.Value, pool), redirectHander, jump - 1);
        }
        bool TryAddNode(string nodeKey, RedisClientPool pool)
        {
            if (Nodes.TryAdd(nodeKey, pool))
            {
                var nodeIndex = Interlocked.Increment(ref NodesIndexIncrement);
                if (NodesIndex.TryAdd(nodeIndex, nodeKey) && NodesKey.TryAdd(nodeKey, nodeIndex)) return true;
                Nodes.TryRemove(nodeKey, out var rempool);
                Interlocked.Decrement(ref NodesIndexIncrement);
            }
            return false;
        }

        RedisClientPool GetRedirectPool((bool isMoved, bool isAsk, ushort slot, string endpoint) redirect, RedisClientPool pool)
        {
            if (redirect.endpoint.StartsWith("127.0.0.1"))
                redirect.endpoint = $"{pool._policy._ip}:{redirect.endpoint.Substring(10)}";
            else if (redirect.endpoint.StartsWith("localhost", StringComparison.CurrentCultureIgnoreCase))
                redirect.endpoint = $"{pool._policy._ip}:{redirect.endpoint.Substring(10)}";

            var nodeKey = $"{redirect.endpoint}/{pool._policy._database}";
            if (Nodes.TryGetValue(nodeKey, out var movedPool) == false)
            {
                lock (NodesLock)
                {
                    if (Nodes.TryGetValue(nodeKey, out movedPool) == false)
                    {
                        var connectionString = pool._policy.BuildConnectionString(redirect.endpoint);
                        movedPool = new RedisClientPool(connectionString, client => { });
                        if (this.TryAddNode(nodeKey, movedPool) == false)
                        {
                            movedPool.Dispose();
                            movedPool = null;
                        }
                    }
                }
                if (movedPool == null)
                    throw new Exception($"{(redirect.isMoved ? "MOVED" : "ASK")} {redirect.slot} {redirect.endpoint}");
            }
            // moved 永久定向，ask 临时性一次定向
            if (redirect.isMoved && NodesKey.TryGetValue(nodeKey, out var nodeIndex2))
            {
                SlotCache.AddOrUpdate(redirect.slot, (ushort)nodeIndex2, (oldkey, oldvalue) => (ushort)nodeIndex2);
            }
            return movedPool;
        }



        (bool isMoved, bool isAsk, ushort slot, string endpoint)? ParseClusterRedirect(Exception ex)
        {
            if (ex == null) return null;
            bool isMoved = ex.Message.StartsWith("MOVED ");
            bool isAsk = ex.Message.StartsWith("ASK ");
            if (isMoved == false && isAsk == false) return null;
            var parts = ex.Message.Split(new string[] { "\r\n" }, StringSplitOptions.None).FirstOrDefault().Split(new[] { ' ' }, 3);
            if (parts.Length != 3 ||
                ushort.TryParse(parts[1], out var slot) == false) return null;
            return (isMoved, isAsk, slot, parts[2]);
        }

        T NodesNotSupport<T>(string[] keys, T defaultValue, Func<Object<RedisClient>, string[], T> callback)
        {
            if (keys == null || keys.Any() == false) return defaultValue;
            var rules = Nodes.Count > 1 ? keys.Select(a => NodeRuleRaw(a)).Distinct() : new[] { Nodes.FirstOrDefault().Key };
            if (rules.Count() > 1) throw new Exception("由于开启了分区模式，keys 分散在多个节点，无法使用此功能");
            var pool = Nodes.TryGetValue(rules.First(), out var b) ? b : Nodes.First().Value;
            string[] rkeys = new string[keys.Length];
            for (int a = 0; a < keys.Length; a++) rkeys[a] = string.Concat(pool.Prefix, keys[a]);
            if (rkeys.Length == 0) return defaultValue;
            return GetAndExecute(pool, conn => callback(conn, rkeys));
        }

        T NodesNotSupport<T>(string key, Func<Object<RedisClient>, string, T> callback)
        {
            if (IsMultiNode) throw new Exception("由于开启了分区模式，无法使用此功能");
            return ExecuteScalar<T>(key, callback);
        }

        RedisClientPool GetNodeOrThrowNotFound(string nodeKey)
        {
            if (Nodes.Count == 1) return Nodes.First().Value;
            if (Nodes.ContainsKey(nodeKey) == false) throw new Exception($"找不到群集节点：{nodeKey}");
            return Nodes[nodeKey];
        }



        #region 分区方式 Execute

        internal T ExecuteScalar<T>(string key, Func<Object<RedisClient>, string, T> hander)
        {
            if (key == null) return default(T);
            var pool = NodeRuleRaw == null || Nodes.Count == 1 ? Nodes.First().Value : (Nodes.TryGetValue(NodeRuleRaw(key), out var b) ? b : Nodes.First().Value);
            key = string.Concat(pool.Prefix, key);
            return GetAndExecute(pool, conn => hander(conn, key));
        }

        internal T[] ExecuteArray<T>(string[] key, Func<Object<RedisClient>, string[], T[]> hander)
        {
            if (key == null || key.Any() == false) return new T[0];
            if (NodeRuleRaw == null || Nodes.Count == 1)
            {
                var pool = Nodes.First().Value;
                var keys = key.Select(a => string.Concat(pool.Prefix, a)).ToArray();
                return GetAndExecute(pool, conn => hander(conn, keys));
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
                GetAndExecute(pool, conn =>
                {
                    var vals = hander(conn, keys);
                    for (var z = 0; z < r.Value.Count; z++)
                    {
                        ret[r.Value[z].Item2] = vals == null || z >= vals.Length ? default(T) : vals[z];
                    }
                    return 0;
                });
            }
            return ret;
        }

        internal long ExecuteNonQuery(string[] key, Func<Object<RedisClient>, string[], long> hander)
        {
            if (key == null || key.Any() == false) return 0;
            if (NodeRuleRaw == null || Nodes.Count == 1)
            {
                var pool = Nodes.First().Value;
                var keys = key.Select(a => string.Concat(pool.Prefix, a)).ToArray();
                return GetAndExecute(pool, conn => hander(conn, keys));
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
                affrows += GetAndExecute(pool, conn => hander(conn, keys));
            }
            return affrows;
        }

        #region crc16
        private static readonly ushort[] crc16tab = {
            0x0000,0x1021,0x2042,0x3063,0x4084,0x50a5,0x60c6,0x70e7,
            0x8108,0x9129,0xa14a,0xb16b,0xc18c,0xd1ad,0xe1ce,0xf1ef,
            0x1231,0x0210,0x3273,0x2252,0x52b5,0x4294,0x72f7,0x62d6,
            0x9339,0x8318,0xb37b,0xa35a,0xd3bd,0xc39c,0xf3ff,0xe3de,
            0x2462,0x3443,0x0420,0x1401,0x64e6,0x74c7,0x44a4,0x5485,
            0xa56a,0xb54b,0x8528,0x9509,0xe5ee,0xf5cf,0xc5ac,0xd58d,
            0x3653,0x2672,0x1611,0x0630,0x76d7,0x66f6,0x5695,0x46b4,
            0xb75b,0xa77a,0x9719,0x8738,0xf7df,0xe7fe,0xd79d,0xc7bc,
            0x48c4,0x58e5,0x6886,0x78a7,0x0840,0x1861,0x2802,0x3823,
            0xc9cc,0xd9ed,0xe98e,0xf9af,0x8948,0x9969,0xa90a,0xb92b,
            0x5af5,0x4ad4,0x7ab7,0x6a96,0x1a71,0x0a50,0x3a33,0x2a12,
            0xdbfd,0xcbdc,0xfbbf,0xeb9e,0x9b79,0x8b58,0xbb3b,0xab1a,
            0x6ca6,0x7c87,0x4ce4,0x5cc5,0x2c22,0x3c03,0x0c60,0x1c41,
            0xedae,0xfd8f,0xcdec,0xddcd,0xad2a,0xbd0b,0x8d68,0x9d49,
            0x7e97,0x6eb6,0x5ed5,0x4ef4,0x3e13,0x2e32,0x1e51,0x0e70,
            0xff9f,0xefbe,0xdfdd,0xcffc,0xbf1b,0xaf3a,0x9f59,0x8f78,
            0x9188,0x81a9,0xb1ca,0xa1eb,0xd10c,0xc12d,0xf14e,0xe16f,
            0x1080,0x00a1,0x30c2,0x20e3,0x5004,0x4025,0x7046,0x6067,
            0x83b9,0x9398,0xa3fb,0xb3da,0xc33d,0xd31c,0xe37f,0xf35e,
            0x02b1,0x1290,0x22f3,0x32d2,0x4235,0x5214,0x6277,0x7256,
            0xb5ea,0xa5cb,0x95a8,0x8589,0xf56e,0xe54f,0xd52c,0xc50d,
            0x34e2,0x24c3,0x14a0,0x0481,0x7466,0x6447,0x5424,0x4405,
            0xa7db,0xb7fa,0x8799,0x97b8,0xe75f,0xf77e,0xc71d,0xd73c,
            0x26d3,0x36f2,0x0691,0x16b0,0x6657,0x7676,0x4615,0x5634,
            0xd94c,0xc96d,0xf90e,0xe92f,0x99c8,0x89e9,0xb98a,0xa9ab,
            0x5844,0x4865,0x7806,0x6827,0x18c0,0x08e1,0x3882,0x28a3,
            0xcb7d,0xdb5c,0xeb3f,0xfb1e,0x8bf9,0x9bd8,0xabbb,0xbb9a,
            0x4a75,0x5a54,0x6a37,0x7a16,0x0af1,0x1ad0,0x2ab3,0x3a92,
            0xfd2e,0xed0f,0xdd6c,0xcd4d,0xbdaa,0xad8b,0x9de8,0x8dc9,
            0x7c26,0x6c07,0x5c64,0x4c45,0x3ca2,0x2c83,0x1ce0,0x0cc1,
            0xef1f,0xff3e,0xcf5d,0xdf7c,0xaf9b,0xbfba,0x8fd9,0x9ff8,
            0x6e17,0x7e36,0x4e55,0x5e74,0x2e93,0x3eb2,0x0ed1,0x1ef0
        };
        public static ushort GetClusterSlot(string key)
        {
            //HASH_SLOT = CRC16(key) mod 16384
            var blob = Encoding.ASCII.GetBytes(key);
            int offset = 0, count = blob.Length, start = -1, end = -1;
            byte lt = (byte)'{', rt = (byte)'}';
            for (int a = 0; a < count - 1; a++)
                if (blob[a] == lt)
                {
                    start = a;
                    break;
                }
            if (start >= 0)
            {
                for (int a = start + 1; a < count; a++)
                    if (blob[a] == rt)
                    {
                        end = a;
                        break;
                    }
            }

            if (start >= 0
                && end >= 0
                && --end != start)
            {
                offset = start + 1;
                count = end - start;
            }

            uint crc = 0;
            for (int i = 0; i < count; i++)
                crc = ((crc << 8) ^ crc16tab[((crc >> 8) ^ blob[offset++]) & 0x00FF]) & 0x0000FFFF;
            return (ushort)(crc % 16384);
        }

        #endregion

        #endregion

        /// <summary>
        /// 创建管道传输，注意：官方集群时请务必预热slotCache，否则会产生moved错误
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public object[] StartPipe(Action<CSRedisClientPipe<string>> handler)
        {
            if (handler == null) return new object[0];
            var pipe = new CSRedisClientPipe<string>(this);
            handler(pipe);
            return pipe.EndPipe();
        }

        /// <summary>
        /// 创建管道传输，注意：官方集群时请务必预热slotCache，否则会产生moved错误，打包提交如：RedisHelper.StartPipe().Set("a", "1").HSet("b", "f", "2").EndPipe();
        /// </summary>
        /// <returns></returns>
        public CSRedisClientPipe<string> StartPipe()
        {
            return new CSRedisClientPipe<string>(this);
        }

    }



    public enum KeyType { None, String, List, Set, ZSet, Hash, Stream }
    public enum InfoSection { Server, Clients, Memory, Persistence, Stats, Replication, CPU, CommandStats, Cluster, Keyspace }
    public enum ClientKillType { normal, slave, pubsub }
}
