using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using CSRedis;
#if !net40
using System.Threading.Tasks;
#endif

partial class RedisHelper<TMark>
{
    #region Key
    /// <summary>
    /// 用于在 key 存在时删除 key
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static long Del(params string[] key) => Instance.Del(key);
    /// <summary>
    /// 序列化给定 key ，并返回被序列化的值
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static byte[] Dump(string key) => Instance.Dump(key);
    /// <summary>
    /// 检查给定 key 是否存在
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static bool Exists(string key) => Instance.Exists(key);
    /// <summary>
    /// [redis-server 3.0] 检查给定多个 key 是否存在
    /// </summary>
    /// <param name="keys">不含prefix前辍</param>
    /// <returns></returns>
    public static long Exists(string[] keys) => Instance.Exists(keys);
    /// <summary>
    /// 为给定 key 设置过期时间
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="seconds">过期秒数</param>
    /// <returns></returns>
    public static bool Expire(string key, int seconds) => Instance.Expire(key, seconds);
    /// <summary>
    /// 为给定 key 设置过期时间
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="expire">过期时间</param>
    /// <returns></returns>
    public static bool Expire(string key, TimeSpan expire) => Instance.Expire(key, expire);
    /// <summary>
    /// 为给定 key 设置过期时间
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="expire">过期时间</param>
    /// <returns></returns>
    public static bool ExpireAt(string key, DateTime expire) => Instance.ExpireAt(key, expire);
    /// <summary>
    /// 查找所有分区节点中符合给定模式(pattern)的 key
    /// </summary>
    /// <param name="pattern">如：runoob*</param>
    /// <returns></returns>
    public static string[] Keys(string pattern) => Instance.Keys(pattern);
    /// <summary>
    /// 将当前数据库的 key 移动到给定的数据库 db 当中
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="database">数据库</param>
    /// <returns></returns>
    public static bool Move(string key, int database) => Instance.Move(key, database);
    /// <summary>
    /// 该返回给定 key 锁储存的值所使用的内部表示(representation)
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static string ObjectEncoding(string key) => Instance.ObjectEncoding(key);
    /// <summary>
    /// 该返回给定 key 引用所储存的值的次数。此命令主要用于除错
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static long? ObjectRefCount(string key) => Instance.ObjectRefCount(key);
    /// <summary>
    /// 返回给定 key 自储存以来的空转时间(idle， 没有被读取也没有被写入)，以秒为单位
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static long? ObjectIdleTime(string key) => Instance.ObjectIdleTime(key);
    /// <summary>
    /// 移除 key 的过期时间，key 将持久保持
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static bool Persist(string key) => Instance.Persist(key);
    /// <summary>
    /// 为给定 key 设置过期时间（毫秒）
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="milliseconds">过期毫秒数</param>
    /// <returns></returns>
    public static bool PExpire(string key, int milliseconds) => Instance.PExpire(key, milliseconds);
    /// <summary>
    /// 为给定 key 设置过期时间（毫秒）
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="expire">过期时间</param>
    /// <returns></returns>
    public static bool PExpire(string key, TimeSpan expire) => Instance.PExpire(key, expire);
    /// <summary>
    /// 为给定 key 设置过期时间（毫秒）
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="expire">过期时间</param>
    /// <returns></returns>
    public static bool PExpireAt(string key, DateTime expire) => Instance.PExpireAt(key, expire);
    /// <summary>
    /// 以毫秒为单位返回 key 的剩余的过期时间
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static long PTtl(string key) => Instance.PTtl(key);
    /// <summary>
    /// 从所有节点中随机返回一个 key
    /// </summary>
    /// <returns>返回的 key 如果包含 prefix前辍，则会去除后返回</returns>
    public static string RandomKey() => Instance.RandomKey();
    /// <summary>
    /// 修改 key 的名称
    /// </summary>
    /// <param name="key">旧名称，不含prefix前辍</param>
    /// <param name="newKey">新名称，不含prefix前辍</param>
    /// <returns></returns>
    public static bool Rename(string key, string newKey) => Instance.Rename(key, newKey);
    /// <summary>
    /// 修改 key 的名称
    /// </summary>
    /// <param name="key">旧名称，不含prefix前辍</param>
    /// <param name="newKey">新名称，不含prefix前辍</param>
    /// <returns></returns>
    public static bool RenameNx(string key, string newKey) => Instance.RenameNx(key, newKey);
    /// <summary>
    /// 反序列化给定的序列化值，并将它和给定的 key 关联
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="serializedValue">序列化值</param>
    /// <returns></returns>
    public static bool Restore(string key, byte[] serializedValue) => Instance.Restore(key, serializedValue);
    /// <summary>
    /// 反序列化给定的序列化值，并将它和给定的 key 关联
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="ttlMilliseconds">毫秒为单位为 key 设置生存时间</param>
    /// <param name="serializedValue">序列化值</param>
    /// <returns></returns>
    public static bool Restore(string key, long ttlMilliseconds, byte[] serializedValue) => Instance.Restore(key, ttlMilliseconds, serializedValue);
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
    public static string[] Sort(string key, long? count = null, long offset = 0, string by = null, RedisSortDir? dir = null, bool? isAlpha = null, params string[] get) =>
        Instance.Sort(key, count, offset, by, dir, isAlpha, get);
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
    public static long SortAndStore(string key, string destination, long? count = null, long offset = 0, string by = null, RedisSortDir? dir = null, bool? isAlpha = null, params string[] get) =>
        Instance.SortAndStore(key, destination, count, offset, by, dir, isAlpha, get);
    /// <summary>
    /// 以秒为单位，返回给定 key 的剩余生存时间
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static long Ttl(string key) => Instance.Ttl(key);
    /// <summary>
    /// 返回 key 所储存的值的类型
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static KeyType Type(string key) => Instance.Type(key);
    /// <summary>
    /// 迭代当前数据库中的数据库键
    /// </summary>
    /// <param name="cursor">位置</param>
    /// <param name="pattern">模式</param>
    /// <param name="count">数量</param>
    /// <returns></returns>
    public static RedisScan<string> Scan(long cursor, string pattern = null, long? count = null) => Instance.Scan(cursor, pattern, count);
    /// <summary>
    /// 迭代当前数据库中的数据库键
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="cursor">位置</param>
    /// <param name="pattern">模式</param>
    /// <param name="count">数量</param>
    /// <returns></returns>
    public static RedisScan<T> Scan<T>(string key, long cursor, string pattern = null, long? count = null) => Instance.Scan<T>(cursor, pattern, count);
    #endregion

#if !net40

    #region Key
    /// <summary>
    /// 用于在 key 存在时删除 key
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<long> DelAsync(params string[] key) => Instance.DelAsync(key);
    /// <summary>
    /// 序列化给定 key ，并返回被序列化的值
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<byte[]> DumpAsync(string key) => Instance.DumpAsync(key);
    /// <summary>
    /// 检查给定 key 是否存在
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<bool> ExistsAsync(string key) => Instance.ExistsAsync(key);
    /// <summary>
    /// [redis-server 3.0] 检查给定多个 key 是否存在
    /// </summary>
    /// <param name="keys">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<long> ExistsAsync(string[] keys) => Instance.ExistsAsync(keys);
    /// <summary>
    /// 为给定 key 设置过期时间
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="seconds">过期秒数</param>
    /// <returns></returns>
    public static Task<bool> ExpireAsync(string key, int seconds) => Instance.ExpireAsync(key, seconds);
    /// <summary>
    /// 为给定 key 设置过期时间
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="expire">过期时间</param>
    /// <returns></returns>
    public static Task<bool> ExpireAsync(string key, TimeSpan expire) => Instance.ExpireAsync(key, expire);
    /// <summary>
    /// 为给定 key 设置过期时间
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="expire">过期时间</param>
    /// <returns></returns>
    public static Task<bool> ExpireAtAsync(string key, DateTime expire) => Instance.ExpireAtAsync(key, expire);
    /// <summary>
    /// 查找所有分区节点中符合给定模式(pattern)的 key
    /// </summary>
    /// <param name="pattern">如：runoob*</param>
    /// <returns></returns>
    public static Task<string[]> KeysAsync(string pattern) => Instance.KeysAsync(pattern);
    /// <summary>
    /// 将当前数据库的 key 移动到给定的数据库 db 当中
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="database">数据库</param>
    /// <returns></returns>
    public static Task<bool> MoveAsync(string key, int database) => Instance.MoveAsync(key, database);
    /// <summary>
    /// 该返回给定 key 锁储存的值所使用的内部表示(representation)
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<string> ObjectEncodingAsync(string key) => Instance.ObjectEncodingAsync(key);
    /// <summary>
    /// 该返回给定 key 引用所储存的值的次数。此命令主要用于除错
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<long?> ObjectRefCountAsync(string key) => Instance.ObjectRefCountAsync(key);
    /// <summary>
    /// 返回给定 key 自储存以来的空转时间(idle， 没有被读取也没有被写入)，以秒为单位
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<long?> ObjectIdleTimeAsync(string key) => Instance.ObjectIdleTimeAsync(key);
    /// <summary>
    /// 移除 key 的过期时间，key 将持久保持
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<bool> PersistAsync(string key) => Instance.PersistAsync(key);
    /// <summary>
    /// 为给定 key 设置过期时间（毫秒）
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="milliseconds">过期毫秒数</param>
    /// <returns></returns>
    public static Task<bool> PExpireAsync(string key, int milliseconds) => Instance.PExpireAsync(key, milliseconds);
    /// <summary>
    /// 为给定 key 设置过期时间（毫秒）
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="expire">过期时间</param>
    /// <returns></returns>
    public static Task<bool> PExpireAsync(string key, TimeSpan expire) => Instance.PExpireAsync(key, expire);
    /// <summary>
    /// 为给定 key 设置过期时间（毫秒）
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="expire">过期时间</param>
    /// <returns></returns>
    public static Task<bool> PExpireAtAsync(string key, DateTime expire) => Instance.PExpireAtAsync(key, expire);
    /// <summary>
    /// 以毫秒为单位返回 key 的剩余的过期时间
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<long> PTtlAsync(string key) => Instance.PTtlAsync(key);
    /// <summary>
    /// 从所有节点中随机返回一个 key
    /// </summary>
    /// <returns>返回的 key 如果包含 prefix前辍，则会去除后返回</returns>
    public static Task<string> RandomKeyAsync() => Instance.RandomKeyAsync();
    /// <summary>
    /// 修改 key 的名称
    /// </summary>
    /// <param name="key">旧名称，不含prefix前辍</param>
    /// <param name="newKey">新名称，不含prefix前辍</param>
    /// <returns></returns>
    public static Task<bool> RenameAsync(string key, string newKey) => Instance.RenameAsync(key, newKey);
    /// <summary>
    /// 修改 key 的名称
    /// </summary>
    /// <param name="key">旧名称，不含prefix前辍</param>
    /// <param name="newKey">新名称，不含prefix前辍</param>
    /// <returns></returns>
    public static Task<bool> RenameNxAsync(string key, string newKey) => Instance.RenameNxAsync(key, newKey);
    /// <summary>
    /// 反序列化给定的序列化值，并将它和给定的 key 关联
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="serializedValue">序列化值</param>
    /// <returns></returns>
    public static Task<bool> RestoreAsync(string key, byte[] serializedValue) => Instance.RestoreAsync(key, serializedValue);
    /// <summary>
    /// 反序列化给定的序列化值，并将它和给定的 key 关联
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="ttlMilliseconds">毫秒为单位为 key 设置生存时间</param>
    /// <param name="serializedValue">序列化值</param>
    /// <returns></returns>
    public static Task<bool> RestoreAsync(string key, long ttlMilliseconds, byte[] serializedValue) => Instance.RestoreAsync(key, ttlMilliseconds, serializedValue);
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
    public static Task<string[]> SortAsync(string key, long? count = null, long offset = 0, string by = null, RedisSortDir? dir = null, bool? isAlpha = null, params string[] get) =>
           Instance.SortAsync(key, count, offset, by, dir, isAlpha, get);
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
    public static Task<long> SortAndStoreAsync(string key, string destination, long? count = null, long offset = 0, string by = null, RedisSortDir? dir = null, bool? isAlpha = null, params string[] get) =>
           Instance.SortAndStoreAsync(key, destination, count, offset, by, dir, isAlpha, get);
    /// <summary>
    /// 以秒为单位，返回给定 key 的剩余生存时间
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<long> TtlAsync(string key) => Instance.TtlAsync(key);
    /// <summary>
    /// 返回 key 所储存的值的类型
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<KeyType> TypeAsync(string key) => Instance.TypeAsync(key);
    /// <summary>
    /// 迭代当前数据库中的数据库键
    /// </summary>
    /// <param name="cursor">位置</param>
    /// <param name="pattern">模式</param>
    /// <param name="count">数量</param>
    /// <returns></returns>
    public static Task<RedisScan<string>> ScanAsync(long cursor, string pattern = null, long? count = null) => Instance.ScanAsync(cursor, pattern, count);
    /// <summary>
    /// 迭代当前数据库中的数据库键
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="cursor">位置</param>
    /// <param name="pattern">模式</param>
    /// <param name="count">数量</param>
    /// <returns></returns>
    public static Task<RedisScan<T>> ScanAsync<T>(long cursor, string pattern = null, long? count = null) => Instance.ScanAsync<T>(cursor, pattern, count);
    #endregion

#endif

}
