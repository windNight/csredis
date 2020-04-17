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
    #region List
    
    /// <summary>
    /// 它是 LPOP 命令的阻塞版本，当给定列表内没有任何元素可供弹出的时候，连接将被 BLPOP 命令阻塞，直到等待超时或发现可弹出元素为止，超时返回null
    /// </summary>
    /// <param name="timeout">超时(秒)</param>
    /// <param name="keys">一个或多个列表，不含prefix前辍</param>
    /// <returns></returns>
    public static (string key, string value)? BLPopWithKey(int timeout, params string[] keys) => Instance.BLPopWithKey(timeout, keys);
    
    /// <summary>
    /// 它是 LPOP 命令的阻塞版本，当给定列表内没有任何元素可供弹出的时候，连接将被 BLPOP 命令阻塞，直到等待超时或发现可弹出元素为止，超时返回null
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="timeout">超时(秒)</param>
    /// <param name="keys">一个或多个列表，不含prefix前辍</param>
    /// <returns></returns>
    public static (string key, T value)? BLPopWithKey<T>(int timeout, params string[] keys) => Instance.BLPopWithKey<T>(timeout, keys);
    
    /// <summary>
    /// 它是 LPOP 命令的阻塞版本，当给定列表内没有任何元素可供弹出的时候，连接将被 BLPOP 命令阻塞，直到等待超时或发现可弹出元素为止，超时返回null
    /// </summary>
    /// <param name="timeout">超时(秒)</param>
    /// <param name="keys">一个或多个列表，不含prefix前辍</param>
    /// <returns></returns>
    public static string BLPop(int timeout, params string[] keys) => Instance.BLPop(timeout, keys);
   
    /// <summary>
    /// 它是 LPOP 命令的阻塞版本，当给定列表内没有任何元素可供弹出的时候，连接将被 BLPOP 命令阻塞，直到等待超时或发现可弹出元素为止，超时返回null
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="timeout">超时(秒)</param>
    /// <param name="keys">一个或多个列表，不含prefix前辍</param>
    /// <returns></returns>
    public static T BLPop<T>(int timeout, params string[] keys) => Instance.BLPop<T>(timeout, keys);
    
    /// <summary>
    /// 它是 RPOP 命令的阻塞版本，当给定列表内没有任何元素可供弹出的时候，连接将被 BRPOP 命令阻塞，直到等待超时或发现可弹出元素为止，超时返回null
    /// </summary>
    /// <param name="timeout">超时(秒)</param>
    /// <param name="keys">一个或多个列表，不含prefix前辍</param>
    /// <returns></returns>
    public static (string key, string value)? BRPopWithKey(int timeout, params string[] keys) => Instance.BRPopWithKey(timeout, keys);
    
    /// <summary>
    /// 它是 RPOP 命令的阻塞版本，当给定列表内没有任何元素可供弹出的时候，连接将被 BRPOP 命令阻塞，直到等待超时或发现可弹出元素为止，超时返回null
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="timeout">超时(秒)</param>
    /// <param name="keys">一个或多个列表，不含prefix前辍</param>
    /// <returns></returns>
    public static (string key, T value)? BRPopWithKey<T>(int timeout, params string[] keys) => Instance.BRPopWithKey<T>(timeout, keys);
    
    /// <summary>
    /// 它是 RPOP 命令的阻塞版本，当给定列表内没有任何元素可供弹出的时候，连接将被 BRPOP 命令阻塞，直到等待超时或发现可弹出元素为止，超时返回null
    /// </summary>
    /// <param name="timeout">超时(秒)</param>
    /// <param name="keys">一个或多个列表，不含prefix前辍</param>
    /// <returns></returns>
    public static string BRPop(int timeout, params string[] keys) => Instance.BRPop(timeout, keys);
   
    /// <summary>
    /// 它是 RPOP 命令的阻塞版本，当给定列表内没有任何元素可供弹出的时候，连接将被 BRPOP 命令阻塞，直到等待超时或发现可弹出元素为止，超时返回null
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="timeout">超时(秒)</param>
    /// <param name="keys">一个或多个列表，不含prefix前辍</param>
    /// <returns></returns>
    public static T BRPop<T>(int timeout, params string[] keys) => Instance.BRPop<T>(timeout, keys);
  
    /// <summary>
    /// BRPOPLPUSH 是 RPOPLPUSH 的阻塞版本，当给定列表 source 不为空时， BRPOPLPUSH 的表现和 RPOPLPUSH 一样。
    /// 当列表 source 为空时， BRPOPLPUSH 命令将阻塞连接，直到等待超时，或有另一个客户端对 source 执行 LPUSH 或 RPUSH 命令为止。
    /// </summary>
    /// <param name="source">源key，不含prefix前辍</param>
    /// <param name="destination">目标key，不含prefix前辍</param>
    /// <param name="timeout">超时(秒)</param>
    /// <returns></returns>
    public static string BRPopLPush(string source, string destination, int timeout) => Instance.BRPopLPush(source, destination, timeout);
   
    /// <summary>
    /// BRPOPLPUSH 是 RPOPLPUSH 的阻塞版本，当给定列表 source 不为空时， BRPOPLPUSH 的表现和 RPOPLPUSH 一样。
    /// 当列表 source 为空时， BRPOPLPUSH 命令将阻塞连接，直到等待超时，或有另一个客户端对 source 执行 LPUSH 或 RPUSH 命令为止。
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="source">源key，不含prefix前辍</param>
    /// <param name="destination">目标key，不含prefix前辍</param>
    /// <param name="timeout">超时(秒)</param>
    /// <returns></returns>
    public static T BRPopLPush<T>(string source, string destination, int timeout) => Instance.BRPopLPush<T>(source, destination, timeout);
    
    /// <summary>
    /// 通过索引获取列表中的元素
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="index">索引</param>
    /// <returns></returns>
    public static string LIndex(string key, long index) => Instance.LIndex(key, index);
   
    /// <summary>
    /// 通过索引获取列表中的元素
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="index">索引</param>
    /// <returns></returns>
    public static T LIndex<T>(string key, long index) => Instance.LIndex<T>(key, index);
    
    /// <summary>
    /// 在列表中的元素前面插入元素
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="pivot">列表的元素</param>
    /// <param name="value">新元素</param>
    /// <returns></returns>
    public static long LInsertBefore(string key, object pivot, object value) => Instance.LInsertBefore(key, pivot, value);
    
    /// <summary>
    /// 在列表中的元素后面插入元素
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="pivot">列表的元素</param>
    /// <param name="value">新元素</param>
    /// <returns></returns>
    public static long LInsertAfter(string key, object pivot, object value) => Instance.LInsertAfter(key, pivot, value);
    
    /// <summary>
    /// 获取列表长度
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static long LLen(string key) => Instance.LLen(key);
   
    /// <summary>
    /// 移出并获取列表的第一个元素
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static string LPop(string key) => Instance.LPop(key);
    
    /// <summary>
    /// 移出并获取列表的第一个元素
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static T LPop<T>(string key) => Instance.LPop<T>(key);
    
    /// <summary>
    /// 将一个或多个值插入到列表头部
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="value">一个或多个值</param>
    /// <returns>执行 LPUSH 命令后，列表的长度</returns>
    public static long LPush<T>(string key, params T[] value) => Instance.LPush(key, value);
    
    /// <summary>
    /// 将一个值插入到已存在的列表头部
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="value">值</param>
    /// <returns>执行 LPUSHX 命令后，列表的长度。</returns>
    public static long LPushX(string key, object value) => Instance.LPushX(key, value);
   
    /// <summary>
    /// 获取列表指定范围内的元素
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <returns></returns>
    public static string[] LRange(string key, long start, long stop) => Instance.LRange(key, start, stop);
   
    /// <summary>
    /// 获取列表指定范围内的元素
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <returns></returns>
    public static T[] LRange<T>(string key, long start, long stop) => Instance.LRange<T>(key, start, stop);
   
    /// <summary>
    /// 根据参数 count 的值，移除列表中与参数 value 相等的元素
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="count">移除的数量，大于0时从表头删除数量count，小于0时从表尾删除数量-count，等于0移除所有</param>
    /// <param name="value">元素</param>
    /// <returns></returns>
    public static long LRem(string key, long count, object value) => Instance.LRem(key, count, value);
    
    /// <summary>
    /// 通过索引设置列表元素的值
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="index">索引</param>
    /// <param name="value">值</param>
    /// <returns></returns>
    public static bool LSet(string key, long index, object value) => Instance.LSet(key, index, value);
    
    /// <summary>
    /// 对一个列表进行修剪，让列表只保留指定区间内的元素，不在指定区间之内的元素都将被删除
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <returns></returns>
    public static bool LTrim(string key, long start, long stop) => Instance.LTrim(key, start, stop);
    
    /// <summary>
    /// 移除并获取列表最后一个元素
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static string RPop(string key) => Instance.RPop(key);
   
    /// <summary>
    /// 移除并获取列表最后一个元素
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static T RPop<T>(string key) => Instance.RPop<T>(key);
   
    /// <summary>
    /// 将列表 source 中的最后一个元素(尾元素)弹出，并返回给客户端。
    /// 将 source 弹出的元素插入到列表 destination ，作为 destination 列表的的头元素。
    /// </summary>
    /// <param name="source">源key，不含prefix前辍</param>
    /// <param name="destination">目标key，不含prefix前辍</param>
    /// <returns></returns>
    public static string RPopLPush(string source, string destination) => Instance.RPopLPush(source, destination);
    /// <summary>
    /// 将列表 source 中的最后一个元素(尾元素)弹出，并返回给客户端。
    /// 将 source 弹出的元素插入到列表 destination ，作为 destination 列表的的头元素。
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="source">源key，不含prefix前辍</param>
    /// <param name="destination">目标key，不含prefix前辍</param>
    /// <returns></returns>
    public static T RPopLPush<T>(string source, string destination) => Instance.RPopLPush<T>(source, destination);
   
    /// <summary>
    /// 在列表中添加一个或多个值
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="value">一个或多个值</param>
    /// <returns>执行 RPUSH 命令后，列表的长度</returns>
    public static long RPush<T>(string key, params T[] value) => Instance.RPush(key, value);
    
    /// <summary>
    /// 为已存在的列表添加值
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="value">一个或多个值</param>
    /// <returns>执行 RPUSHX 命令后，列表的长度</returns>
    public static long RPushX(string key, object value) => Instance.RPushX(key, value);

    #endregion

#if !net40

    #region List
    /// <summary>
    /// 通过索引获取列表中的元素
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="index">索引</param>
    /// <returns></returns>
    public static Task<string> LIndexAsync(string key, long index) => Instance.LIndexAsync(key, index);
    /// <summary>
    /// 通过索引获取列表中的元素
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="index">索引</param>
    /// <returns></returns>
    public static Task<T> LIndexAsync<T>(string key, long index) => Instance.LIndexAsync<T>(key, index);
    /// <summary>
    /// 在列表中的元素前面插入元素
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="pivot">列表的元素</param>
    /// <param name="value">新元素</param>
    /// <returns></returns>
    public static Task<long> LInsertBeforeAsync(string key, object pivot, object value) => Instance.LInsertBeforeAsync(key, pivot, value);
    /// <summary>
    /// 在列表中的元素后面插入元素
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="pivot">列表的元素</param>
    /// <param name="value">新元素</param>
    /// <returns></returns>
    public static Task<long> LInsertAfterAsync(string key, object pivot, object value) => Instance.LInsertAfterAsync(key, pivot, value);
    /// <summary>
    /// 获取列表长度
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<long> LLenAsync(string key) => Instance.LLenAsync(key);
    /// <summary>
    /// 移出并获取列表的第一个元素
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<string> LPopAsync(string key) => Instance.LPopAsync(key);
    /// <summary>
    /// 移出并获取列表的第一个元素
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<T> LPopAsync<T>(string key) => Instance.LPopAsync<T>(key);
    /// <summary>
    /// 将一个或多个值插入到列表头部
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="value">一个或多个值</param>
    /// <returns>执行 LPUSH 命令后，列表的长度</returns>
    public static Task<long> LPushAsync<T>(string key, params T[] value) => Instance.LPushAsync(key, value);
    /// <summary>
    /// 将一个值插入到已存在的列表头部
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="value">值</param>
    /// <returns>执行 LPUSHX 命令后，列表的长度。</returns>
    public static Task<long> LPushXAsync(string key, object value) => Instance.LPushXAsync(key, value);
    /// <summary>
    /// 获取列表指定范围内的元素
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <returns></returns>
    public static Task<string[]> LRangeAsync(string key, long start, long stop) => Instance.LRangeAsync(key, start, stop);
    /// <summary>
    /// 获取列表指定范围内的元素
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <returns></returns>
    public static Task<T[]> LRangeAsync<T>(string key, long start, long stop) => Instance.LRangeAsync<T>(key, start, stop);
    /// <summary>
    /// 根据参数 count 的值，移除列表中与参数 value 相等的元素
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="count">移除的数量，大于0时从表头删除数量count，小于0时从表尾删除数量-count，等于0移除所有</param>
    /// <param name="value">元素</param>
    /// <returns></returns>
    public static Task<long> LRemAsync(string key, long count, object value) => Instance.LRemAsync(key, count, value);
    /// <summary>
    /// 通过索引设置列表元素的值
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="index">索引</param>
    /// <param name="value">值</param>
    /// <returns></returns>
    public static Task<bool> LSetAsync(string key, long index, object value) => Instance.LSetAsync(key, index, value);
    /// <summary>
    /// 对一个列表进行修剪，让列表只保留指定区间内的元素，不在指定区间之内的元素都将被删除
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <returns></returns>
    public static Task<bool> LTrimAsync(string key, long start, long stop) => Instance.LTrimAsync(key, start, stop);
    /// <summary>
    /// 移除并获取列表最后一个元素
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<string> RPopAsync(string key) => Instance.RPopAsync(key);
    /// <summary>
    /// 移除并获取列表最后一个元素
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<T> RPopAsync<T>(string key) => Instance.RPopAsync<T>(key);
    /// <summary>
    /// 将列表 source 中的最后一个元素(尾元素)弹出，并返回给客户端。
    /// 将 source 弹出的元素插入到列表 destination ，作为 destination 列表的的头元素。
    /// </summary>
    /// <param name="source">源key，不含prefix前辍</param>
    /// <param name="destination">目标key，不含prefix前辍</param>
    /// <returns></returns>
    public static Task<string> RPopLPushAsync(string source, string destination) => Instance.RPopLPushAsync(source, destination);
    /// <summary>
    /// 将列表 source 中的最后一个元素(尾元素)弹出，并返回给客户端。
    /// 将 source 弹出的元素插入到列表 destination ，作为 destination 列表的的头元素。
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="source">源key，不含prefix前辍</param>
    /// <param name="destination">目标key，不含prefix前辍</param>
    /// <returns></returns>
    public static Task<T> RPopLPushAsync<T>(string source, string destination) => Instance.RPopLPushAsync<T>(source, destination);
    /// <summary>
    /// 在列表中添加一个或多个值
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="value">一个或多个值</param>
    /// <returns>执行 RPUSH 命令后，列表的长度</returns>
    public static Task<long> RPushAsync<T>(string key, params T[] value) => Instance.RPushAsync(key, value);
    /// <summary>
    /// 为已存在的列表添加值
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="value">一个或多个值</param>
    /// <returns>执行 RPUSHX 命令后，列表的长度</returns>
    public static Task<long> RPushXAsync(string key, object value) => Instance.RPushAsync(key, value);
    #endregion

#endif

}
