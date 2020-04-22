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
        #region Key
        /// <summary>
        /// [redis-server 3.2.1] 修改指定key(s) 最后访问时间 若key不存在，不做操作
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public long Touch(params string[] key) => ExecuteNonQuery(key, (c, k) => c.Value.Touch(k));
        /// <summary>
        /// [redis-server 4.0.0] Delete a key, 该命令和DEL十分相似：删除指定的key(s),若key不存在则该key被跳过。但是，相比DEL会产生阻塞，该命令会在另一个线程中回收内存，因此它是非阻塞的。 这也是该命令名字的由来：仅将keys从keyspace元数据中删除，真正的删除会在后续异步操作。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public long UnLink(params string[] key) => ExecuteNonQuery(key, (c, k) => c.Value.UnLink(k));
        /// <summary>
        /// 用于在 key 存在时删除 key
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public long Del(params string[] key) => ExecuteNonQuery(key, (c, k) => c.Value.Del(k));
        /// <summary>
        /// 序列化给定 key ，并返回被序列化的值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public byte[] Dump(string key) => ExecuteScalar(key, (c, k) => c.Value.Dump(k));
        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public bool Exists(string key) => ExecuteScalar(key, (c, k) => c.Value.Exists(k));
        /// <summary>
		/// [redis-server 3.0] 检查给定多个 key 是否存在
		/// </summary>
		/// <param name="keys">不含prefix前辍</param>
		/// <returns></returns>
		public long Exists(string[] keys) => NodesNotSupport(keys, 0, (c, k) => c.Value.Exists(k));
        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="seconds">过期秒数</param>
        /// <returns></returns>
        public bool Expire(string key, int seconds) => ExecuteScalar(key, (c, k) => c.Value.Expire(k, seconds));
        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="expire">过期时间</param>
        /// <returns></returns>
        public bool Expire(string key, TimeSpan expire) => ExecuteScalar(key, (c, k) => c.Value.Expire(k, expire));
        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="expire">过期时间</param>
        /// <returns></returns>
        public bool ExpireAt(string key, DateTime expire) => ExecuteScalar(key, (c, k) => c.Value.ExpireAt(k, expire));
        /// <summary>
        /// 查找所有分区节点中符合给定模式(pattern)的 key
        /// </summary>
        /// <param name="pattern">如：runoob*</param>
        /// <returns></returns>
        public string[] Keys(string pattern)
        {
            List<string> ret = new List<string>();
            foreach (var pool in Nodes)
                ret.AddRange(GetAndExecute(pool.Value, conn => conn.Value.Keys(pattern)));
            return ret.ToArray();
        }
        /// <summary>
        /// 将当前数据库的 key 移动到给定的数据库 db 当中
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="database">数据库</param>
        /// <returns></returns>
        public bool Move(string key, int database) => ExecuteScalar(key, (c, k) => c.Value.Move(k, database));
        /// <summary>
        /// 该返回给定 key 锁储存的值所使用的内部表示(representation)
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public string ObjectEncoding(string key) => ExecuteScalar(key, (c, k) => c.Value.ObjectEncoding(k));
        /// <summary>
        /// 该返回给定 key 引用所储存的值的次数。此命令主要用于除错
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public long? ObjectRefCount(string key) => ExecuteScalar(key, (c, k) => c.Value.Object(RedisObjectSubCommand.RefCount, k));
        /// <summary>
        /// 返回给定 key 自储存以来的空转时间(idle， 没有被读取也没有被写入)，以秒为单位
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public long? ObjectIdleTime(string key) => ExecuteScalar(key, (c, k) => c.Value.Object(RedisObjectSubCommand.IdleTime, k));
        /// <summary>
        /// 移除 key 的过期时间，key 将持久保持
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public bool Persist(string key) => ExecuteScalar(key, (c, k) => c.Value.Persist(k));
        /// <summary>
        /// 为给定 key 设置过期时间（毫秒）
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="milliseconds">过期毫秒数</param>
        /// <returns></returns>
        public bool PExpire(string key, int milliseconds) => ExecuteScalar(key, (c, k) => c.Value.PExpire(k, milliseconds));
        /// <summary>
        /// 为给定 key 设置过期时间（毫秒）
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="expire">过期时间</param>
        /// <returns></returns>
        public bool PExpire(string key, TimeSpan expire) => ExecuteScalar(key, (c, k) => c.Value.PExpire(k, expire));
        /// <summary>
        /// 为给定 key 设置过期时间（毫秒）
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="expire">过期时间</param>
        /// <returns></returns>
        public bool PExpireAt(string key, DateTime expire) => ExecuteScalar(key, (c, k) => c.Value.PExpireAt(k, expire));
        /// <summary>
        /// 以毫秒为单位返回 key 的剩余的过期时间
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public long PTtl(string key) => ExecuteScalar(key, (c, k) => c.Value.PTtl(k));
        /// <summary>
        /// 从所有节点中随机返回一个 key
        /// </summary>
        /// <returns>返回的 key 如果包含 prefix前辍，则会去除后返回</returns>
        public string RandomKey() => GetAndExecute(Nodes[NodesIndex[_rnd.Next(0, NodesIndex.Count)]], c =>
        {
            var rk = c.Value.RandomKey();
            var prefix = (c.Pool as RedisClientPool).Prefix;
            if (string.IsNullOrEmpty(prefix) == false && rk.StartsWith(prefix)) return rk.Substring(prefix.Length);
            return rk;
        });
        /// <summary>
        /// 修改 key 的名称
        /// </summary>
        /// <param name="key">旧名称，不含prefix前辍</param>
        /// <param name="newKey">新名称，不含prefix前辍</param>
        /// <returns></returns>
        public bool Rename(string key, string newKey)
        {
            string rule = string.Empty;
            if (Nodes.Count > 1)
            {
                var rule1 = NodeRuleRaw(key);
                var rule2 = NodeRuleRaw(newKey);
                if (rule1 != rule2)
                {
                    var ret = StartPipe(a => a.Dump(key).Del(key));
                    int.TryParse(ret[1]?.ToString(), out var tryint);
                    if (ret[0] == null || tryint <= 0) return false;
                    return Restore(newKey, (byte[])ret[0]);
                }
                rule = rule1;
            }
            var pool = Nodes.TryGetValue(rule, out var b) ? b : Nodes.First().Value;
            var key1 = string.Concat(pool.Prefix, key);
            var key2 = string.Concat(pool.Prefix, newKey);
            return GetAndExecute(pool, conn => conn.Value.Rename(key1, key2)) == "OK";
        }
        /// <summary>
        /// 修改 key 的名称
        /// </summary>
        /// <param name="key">旧名称，不含prefix前辍</param>
        /// <param name="newKey">新名称，不含prefix前辍</param>
        /// <returns></returns>
        public bool RenameNx(string key, string newKey) => NodesNotSupport(new[] { key, newKey }, false, (c, k) => c.Value.RenameNx(k.First(), k.Last()));
        /// <summary>
        /// 反序列化给定的序列化值，并将它和给定的 key 关联
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="serializedValue">序列化值</param>
        /// <returns></returns>
        public bool Restore(string key, byte[] serializedValue) => ExecuteScalar(key, (c, k) => c.Value.Restore(k, 0, serializedValue)) == "OK";
        /// <summary>
        /// 反序列化给定的序列化值，并将它和给定的 key 关联
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="ttlMilliseconds">毫秒为单位为 key 设置生存时间</param>
        /// <param name="serializedValue">序列化值</param>
        /// <returns></returns>
        public bool Restore(string key, long ttlMilliseconds, byte[] serializedValue) => ExecuteScalar(key, (c, k) => c.Value.Restore(k, ttlMilliseconds, serializedValue)) == "OK";
        /// <summary>
        /// 返回给定列表、集合、有序集合 key 中经过排序的元素，参数资料：http://doc.redisfans.com/key/sort.html
        /// </summary>
        /// <param name="key">列表、集合、有序集合，不含prefix前辍</param>
        /// <param name="count">数量</param>
        /// <param name="offset">偏移量</param>
        /// <param name="by">排序字段</param>
        /// <param name="dir">排序方式</param>
        /// <param name="isAlpha">对字符串或数字进行排序</param>
        /// <param name="get">根据排序的结果来取出相应的键值</param>
        /// <returns></returns>
        public string[] Sort(string key, long? count = null, long offset = 0, string by = null, RedisSortDir? dir = null, bool? isAlpha = null, params string[] get) =>
            NodesNotSupport(key, (c, k) => c.Value.Sort(k, offset, count, by, dir, isAlpha, get));
        /// <summary>
        /// 保存给定列表、集合、有序集合 key 中经过排序的元素，参数资料：http://doc.redisfans.com/key/sort.html
        /// </summary>
        /// <param name="key">列表、集合、有序集合，不含prefix前辍</param>
        /// <param name="destination">目标key，不含prefix前辍</param>
        /// <param name="count">数量</param>
        /// <param name="offset">偏移量</param>
        /// <param name="by">排序字段</param>
        /// <param name="dir">排序方式</param>
        /// <param name="isAlpha">对字符串或数字进行排序</param>
        /// <param name="get">根据排序的结果来取出相应的键值</param>
        /// <returns></returns>
        public long SortAndStore(string key, string destination, long? count = null, long offset = 0, string by = null, RedisSortDir? dir = null, bool? isAlpha = null, params string[] get) =>
            NodesNotSupport(key, (c, k) => c.Value.SortAndStore(k, (c.Pool as RedisClientPool)?.Prefix + destination, offset, count, by, dir, isAlpha, get));
        /// <summary>
        /// 以秒为单位，返回给定 key 的剩余生存时间
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public long Ttl(string key) => ExecuteScalar(key, (c, k) => c.Value.Ttl(k));
        /// <summary>
        /// 返回 key 所储存的值的类型
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public KeyType Type(string key) => Enum.TryParse(ExecuteScalar(key, (c, k) => c.Value.Type(k)), true, out KeyType tryenum) ? tryenum : KeyType.None;
        /// <summary>
        /// 迭代当前数据库中的数据库键
        /// </summary>
        /// <param name="cursor">位置</param>
        /// <param name="pattern">模式</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public RedisScan<string> Scan(long cursor, string pattern = null, long? count = null) => NodesNotSupport("Scan", (c, k) => c.Value.Scan(cursor, pattern, count));
        /// <summary>
        /// 迭代当前数据库中的数据库键
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="cursor">位置</param>
        /// <param name="pattern">模式</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public RedisScan<T> Scan<T>(long cursor, string pattern = null, long? count = null)
        {
            var scan = NodesNotSupport("Scan<T>", (c, k) => c.Value.ScanBytes(cursor, pattern, count));
            return new RedisScan<T>(scan.Cursor, this.DeserializeRedisValueArrayInternal<T>(scan.Items));
        }
        #endregion


#if !net40

        #region Key
        /// <summary>
        /// [redis-server 3.2.1] 修改指定key(s) 最后访问时间 若key不存在，不做操作
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public Task<long> TouchAsync(params string[] key) => ExecuteNonQueryAsync(key, (c, k) => c.Value.TouchAsync(k));
        /// <summary>
        /// [redis-server 4.0.0] Delete a key, 该命令和DEL十分相似：删除指定的key(s),若key不存在则该key被跳过。但是，相比DEL会产生阻塞，该命令会在另一个线程中回收内存，因此它是非阻塞的。 这也是该命令名字的由来：仅将keys从keyspace元数据中删除，真正的删除会在后续异步操作。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public Task<long> UnLinkAsync(params string[] key) => ExecuteNonQueryAsync(key, (c, k) => c.Value.UnLinkAsync(k));
        /// <summary>
        /// 用于在 key 存在时删除 key
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public Task<long> DelAsync(params string[] key) => ExecuteNonQueryAsync(key, (c, k) => c.Value.DelAsync(k));
        /// <summary>
        /// 序列化给定 key ，并返回被序列化的值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public Task<byte[]> DumpAsync(string key) => ExecuteScalarAsync(key, (c, k) => c.Value.DumpAsync(k));
        /// <summary>
        /// 检查给定 key 是否存在
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public Task<bool> ExistsAsync(string key) => ExecuteScalarAsync(key, (c, k) => c.Value.ExistsAsync(k));
        /// <summary>
		/// [redis-server 3.0] 检查给定多个 key 是否存在
		/// </summary>
		/// <param name="keys">不含prefix前辍</param>
		/// <returns></returns>
		public Task<long> ExistsAsync(string[] keys) => NodesNotSupportAsync(keys, 0, (c, k) => c.Value.ExistsAsync(k));
        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="seconds">过期秒数</param>
        /// <returns></returns>
        public Task<bool> ExpireAsync(string key, int seconds) => ExecuteScalarAsync(key, (c, k) => c.Value.ExpireAsync(k, seconds));
        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="expire">过期时间</param>
        /// <returns></returns>
        public Task<bool> ExpireAsync(string key, TimeSpan expire) => ExecuteScalarAsync(key, (c, k) => c.Value.ExpireAsync(k, expire));
        /// <summary>
        /// 为给定 key 设置过期时间
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="expire">过期时间</param>
        /// <returns></returns>
        public Task<bool> ExpireAtAsync(string key, DateTime expire) => ExecuteScalarAsync(key, (c, k) => c.Value.ExpireAtAsync(k, expire));
        /// <summary>
        /// 查找所有分区节点中符合给定模式(pattern)的 key
        /// </summary>
        /// <param name="pattern">如：runoob*</param>
        /// <returns></returns>
        public async Task<string[]> KeysAsync(string pattern)
        {
            List<string> ret = new List<string>();
            foreach (var pool in Nodes)
                ret.AddRange(await GetAndExecuteAsync(pool.Value, conn => conn.Value.KeysAsync(pattern)));
            return ret.ToArray();
        }
        /// <summary>
        /// 将当前数据库的 key 移动到给定的数据库 db 当中
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="database">数据库</param>
        /// <returns></returns>
        public Task<bool> MoveAsync(string key, int database) => ExecuteScalarAsync(key, (c, k) => c.Value.MoveAsync(k, database));
        /// <summary>
        /// 该返回给定 key 锁储存的值所使用的内部表示(representation)
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public Task<string> ObjectEncodingAsync(string key) => ExecuteScalarAsync(key, (c, k) => c.Value.ObjectEncodingAsync(k));
        /// <summary>
        /// 该返回给定 key 引用所储存的值的次数。此命令主要用于除错
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public Task<long?> ObjectRefCountAsync(string key) => ExecuteScalarAsync(key, (c, k) => c.Value.ObjectAsync(RedisObjectSubCommand.RefCount, k));
        /// <summary>
        /// 返回给定 key 自储存以来的空转时间(idle， 没有被读取也没有被写入)，以秒为单位
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public Task<long?> ObjectIdleTimeAsync(string key) => ExecuteScalarAsync(key, (c, k) => c.Value.ObjectAsync(RedisObjectSubCommand.IdleTime, k));
        /// <summary>
        /// 移除 key 的过期时间，key 将持久保持
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public Task<bool> PersistAsync(string key) => ExecuteScalarAsync(key, (c, k) => c.Value.PersistAsync(k));
        /// <summary>
        /// 为给定 key 设置过期时间（毫秒）
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="milliseconds">过期毫秒数</param>
        /// <returns></returns>
        public Task<bool> PExpireAsync(string key, int milliseconds) => ExecuteScalarAsync(key, (c, k) => c.Value.PExpireAsync(k, milliseconds));
        /// <summary>
        /// 为给定 key 设置过期时间（毫秒）
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="expire">过期时间</param>
        /// <returns></returns>
        public Task<bool> PExpireAsync(string key, TimeSpan expire) => ExecuteScalarAsync(key, (c, k) => c.Value.PExpireAsync(k, expire));
        /// <summary>
        /// 为给定 key 设置过期时间（毫秒）
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="expire">过期时间</param>
        /// <returns></returns>
        public Task<bool> PExpireAtAsync(string key, DateTime expire) => ExecuteScalarAsync(key, (c, k) => c.Value.PExpireAtAsync(k, expire));
        /// <summary>
        /// 以毫秒为单位返回 key 的剩余的过期时间
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public Task<long> PTtlAsync(string key) => ExecuteScalarAsync(key, (c, k) => c.Value.PTtlAsync(k));
        /// <summary>
        /// 从所有节点中随机返回一个 key
        /// </summary>
        /// <returns>返回的 key 如果包含 prefix前辍，则会去除后返回</returns>
        public Task<string> RandomKeyAsync() => GetAndExecuteAsync(Nodes[NodesIndex[_rnd.Next(0, NodesIndex.Count)]], async c =>
        {
            var rk = await c.Value.RandomKeyAsync();
            var prefix = (c.Pool as RedisClientPool).Prefix;
            if (string.IsNullOrEmpty(prefix) == false && rk.StartsWith(prefix)) return rk.Substring(prefix.Length);
            return rk;
        });
        /// <summary>
        /// 修改 key 的名称
        /// </summary>
        /// <param name="key">旧名称，不含prefix前辍</param>
        /// <param name="newKey">新名称，不含prefix前辍</param>
        /// <returns></returns>
        public async Task<bool> RenameAsync(string key, string newKey)
        {
            string rule = string.Empty;
            if (Nodes.Count > 1)
            {
                var rule1 = NodeRuleRaw(key);
                var rule2 = NodeRuleRaw(newKey);
                if (rule1 != rule2)
                {
                    var ret = StartPipe(a => a.Dump(key).Del(key));
                    int.TryParse(ret[1]?.ToString(), out var tryint);
                    if (ret[0] == null || tryint <= 0) return false;
                    return await RestoreAsync(newKey, (byte[])ret[0]);
                }
                rule = rule1;
            }
            var pool = Nodes.TryGetValue(rule, out var b) ? b : Nodes.First().Value;
            var key1 = string.Concat(pool.Prefix, key);
            var key2 = string.Concat(pool.Prefix, newKey);
            return await GetAndExecuteAsync(pool, conn => conn.Value.RenameAsync(key1, key2)) == "OK";
        }
        /// <summary>
        /// 修改 key 的名称
        /// </summary>
        /// <param name="key">旧名称，不含prefix前辍</param>
        /// <param name="newKey">新名称，不含prefix前辍</param>
        /// <returns></returns>
        public Task<bool> RenameNxAsync(string key, string newKey) => NodesNotSupportAsync(new[] { key, newKey }, false, (c, k) => c.Value.RenameNxAsync(k.First(), k.Last()));
        /// <summary>
        /// 反序列化给定的序列化值，并将它和给定的 key 关联
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="serializedValue">序列化值</param>
        /// <returns></returns>
        public Task<bool> RestoreAsync(string key, byte[] serializedValue) => ExecuteScalarAsync(key, async (c, k) => await c.Value.RestoreAsync(k, 0, serializedValue) == "OK");
        /// <summary>
        /// 反序列化给定的序列化值，并将它和给定的 key 关联
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="ttlMilliseconds">毫秒为单位为 key 设置生存时间</param>
        /// <param name="serializedValue">序列化值</param>
        /// <returns></returns>
        public Task<bool> RestoreAsync(string key, long ttlMilliseconds, byte[] serializedValue) => ExecuteScalarAsync(key, async (c, k) => await c.Value.RestoreAsync(k, ttlMilliseconds, serializedValue) == "OK");
        /// <summary>
        /// 返回给定列表、集合、有序集合 key 中经过排序的元素，参数资料：http://doc.redisfans.com/key/sort.html
        /// </summary>
        /// <param name="key">列表、集合、有序集合，不含prefix前辍</param>
        /// <param name="offset">偏移量</param>
        /// <param name="count">数量</param>
        /// <param name="by">排序字段</param>
        /// <param name="dir">排序方式</param>
        /// <param name="isAlpha">对字符串或数字进行排序</param>
        /// <param name="get">根据排序的结果来取出相应的键值</param>
        /// <returns></returns>
        public Task<string[]> SortAsync(string key, long? count = null, long offset = 0, string by = null, RedisSortDir? dir = null, bool? isAlpha = null, params string[] get) =>
            NodesNotSupportAsync(key, (c, k) => c.Value.SortAsync(k, offset, count, by, dir, isAlpha, get));
        /// <summary>
        /// 保存给定列表、集合、有序集合 key 中经过排序的元素，参数资料：http://doc.redisfans.com/key/sort.html
        /// </summary>
        /// <param name="key">列表、集合、有序集合，不含prefix前辍</param>
        /// <param name="destination">目标key，不含prefix前辍</param>
        /// <param name="offset">偏移量</param>
        /// <param name="count">数量</param>
        /// <param name="by">排序字段</param>
        /// <param name="dir">排序方式</param>
        /// <param name="isAlpha">对字符串或数字进行排序</param>
        /// <param name="get">根据排序的结果来取出相应的键值</param>
        /// <returns></returns>
        public Task<long> SortAndStoreAsync(string key, string destination, long? count = null, long offset = 0, string by = null, RedisSortDir? dir = null, bool? isAlpha = null, params string[] get) =>
            NodesNotSupportAsync(key, (c, k) => c.Value.SortAndStoreAsync(k, (c.Pool as RedisClientPool)?.Prefix + destination, offset, count, by, dir, isAlpha, get));
        /// <summary>
        /// 以秒为单位，返回给定 key 的剩余生存时间
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public Task<long> TtlAsync(string key) => ExecuteScalarAsync(key, (c, k) => c.Value.TtlAsync(k));
        /// <summary>
        /// 返回 key 所储存的值的类型
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public async Task<KeyType> TypeAsync(string key) => Enum.TryParse(await ExecuteScalarAsync(key, (c, k) => c.Value.TypeAsync(k)), true, out KeyType tryenum) ? tryenum : KeyType.None;
        /// <summary>
        /// 迭代当前数据库中的数据库键
        /// </summary>
        /// <param name="cursor">位置</param>
        /// <param name="pattern">模式</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public Task<RedisScan<string>> ScanAsync(long cursor, string pattern = null, long? count = null) => NodesNotSupportAsync("ScanAsync", (c, k) => c.Value.ScanAsync(cursor, pattern, count));
        /// <summary>
        /// 迭代当前数据库中的数据库键
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="cursor">位置</param>
        /// <param name="pattern">模式</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public async Task<RedisScan<T>> ScanAsync<T>(long cursor, string pattern = null, long? count = null)
        {
            var scan = await NodesNotSupportAsync("ScanAsync<T>", (c, k) => c.Value.ScanBytesAsync(cursor, pattern, count));
            return new RedisScan<T>(scan.Cursor, this.DeserializeRedisValueArrayInternal<T>(scan.Items));
        }
        #endregion



#endif

    }
}