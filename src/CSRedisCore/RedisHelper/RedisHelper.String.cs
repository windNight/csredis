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

    #region String
    /// <summary>
    /// 如果 key 已经存在并且是一个字符串， APPEND 命令将指定的 value 追加到该 key 原来值（value）的末尾
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="value">字符串</param>
    /// <returns>追加指定值之后， key 中字符串的长度</returns>
    public static long Append(string key, object value) => Instance.Append(key, value);
    /// <summary>
    /// 计算给定位置被设置为 1 的比特位的数量
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置</param>
    /// <param name="end">结束位置</param>
    /// <returns></returns>
    public static long BitCount(string key, long start, long end) => Instance.BitCount(key, start, end);
    /// <summary>
    /// 对一个或多个保存二进制位的字符串 key 进行位元操作，并将结果保存到 destkey 上
    /// </summary>
    /// <param name="op">And | Or | XOr | Not</param>
    /// <param name="destKey">不含prefix前辍</param>
    /// <param name="keys">不含prefix前辍</param>
    /// <returns>保存到 destkey 的长度，和输入 key 中最长的长度相等</returns>
    public static long BitOp(RedisBitOp op, string destKey, params string[] keys) => Instance.BitOp(op, destKey, keys);
    /// <summary>
    /// 对 key 所储存的值，查找范围内第一个被设置为1或者0的bit位
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="bit">查找值</param>
    /// <param name="start">开始位置，-1是最后一个，-2是倒数第二个</param>
    /// <param name="end">结果位置，-1是最后一个，-2是倒数第二个</param>
    /// <returns>返回范围内第一个被设置为1或者0的bit位</returns>
    public static long BitPos(string key, bool bit, long? start = null, long? end = null) => Instance.BitPos(key, bit, start, end);
    /// <summary>
    /// 获取指定 key 的值
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static string Get(string key) => Instance.Get(key);
    /// <summary>
    /// 获取指定 key 的值
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static T Get<T>(string key) => Instance.Get<T>(key);
    /// <summary>
    /// 对 key 所储存的值，获取指定偏移量上的位(bit)
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="offset">偏移量</param>
    /// <returns></returns>
    public static bool GetBit(string key, uint offset) => Instance.GetBit(key, offset);
    /// <summary>
    /// 返回 key 中字符串值的子字符
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <param name="end">结束位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <returns></returns>
    public static string GetRange(string key, long start, long end) => Instance.GetRange(key, start, end);
    /// <summary>
    /// 返回 key 中字符串值的子字符
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <param name="end">结束位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <returns></returns>
    public static T GetRange<T>(string key, long start, long end) => Instance.GetRange<T>(key, start, end);
    /// <summary>
    /// 将给定 key 的值设为 value ，并返回 key 的旧值(old value)
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="value">值</param>
    /// <returns></returns>
    public static string GetSet(string key, object value) => Instance.GetSet(key, value);
    /// <summary>
    /// 将给定 key 的值设为 value ，并返回 key 的旧值(old value)
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="value">值</param>
    /// <returns></returns>
    public static T GetSet<T>(string key, object value) => Instance.GetSet<T>(key, value);
    /// <summary>
    /// 将 key 所储存的值加上给定的增量值（increment）
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="value">增量值(默认=1)</param>
    /// <returns></returns>
    public static long IncrBy(string key, long value = 1) => Instance.IncrBy(key, value);
    /// <summary>
    /// 将 key 所储存的值加上给定的浮点增量值（increment）
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="value">增量值(默认=1)</param>
    /// <returns></returns>
    public static decimal IncrByFloat(string key, decimal value = 1) => Instance.IncrByFloat(key, value);
    /// <summary>
    /// 获取多个指定 key 的值(数组)
    /// </summary>
    /// <param name="keys">不含prefix前辍</param>
    /// <returns></returns>
    public static string[] MGet(params string[] keys) => Instance.MGet(keys);
    /// <summary>
    /// 获取多个指定 key 的值(数组)
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="keys">不含prefix前辍</param>
    /// <returns></returns>
    public static T[] MGet<T>(params string[] keys) => Instance.MGet<T>(keys);
    /// <summary>
    /// 同时设置一个或多个 key-value 对
    /// </summary>
    /// <param name="keyValues">key1 value1 [key2 value2]</param>
    /// <returns></returns>
    public static bool MSet(params object[] keyValues) => Instance.MSet(keyValues);
    /// <summary>
    /// 同时设置一个或多个 key-value 对，当且仅当所有给定 key 都不存在
    /// </summary>
    /// <param name="keyValues">key1 value1 [key2 value2]</param>
    /// <returns></returns>
    public static bool MSetNx(params object[] keyValues) => Instance.MSetNx(keyValues);
    /// <summary>
    /// 设置指定 key 的值，所有写入参数object都支持string | byte[] | 数值 | 对象
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="value">值</param>
    /// <param name="expireSeconds">过期(秒单位)</param>
    /// <param name="exists">Nx, Xx</param>
    /// <returns></returns>
    public static bool Set(string key, object value, int expireSeconds = -1, RedisExistence? exists = null) => Instance.Set(key, value, expireSeconds, exists);
    public static bool Set(string key, object value, TimeSpan expire, RedisExistence? exists = null) => Instance.Set(key, value, expire, exists);
    /// <summary>
    /// 对 key 所储存的字符串值，设置或清除指定偏移量上的位(bit)
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="offset">偏移量</param>
    /// <param name="value">值</param>
    /// <returns></returns>
    public static bool SetBit(string key, uint offset, bool value) => Instance.SetBit(key, offset, value);
    /// <summary>
    /// 只有在 key 不存在时设置 key 的值
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="value">值</param>
    /// <returns></returns>
    public static bool SetNx(string key, object value) => Instance.SetNx(key, value);
    /// <summary>
    /// 用 value 参数覆写给定 key 所储存的字符串值，从偏移量 offset 开始
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="offset">偏移量</param>
    /// <param name="value">值</param>
    /// <returns>被修改后的字符串长度</returns>
    public static long SetRange(string key, uint offset, object value) => Instance.SetRange(key, offset, value);
    /// <summary>
    /// 返回 key 所储存的字符串值的长度
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static long StrLen(string key) => Instance.StrLen(key);
    #endregion

#if !net40

    #region String
    /// <summary>
    /// 如果 key 已经存在并且是一个字符串， APPEND 命令将指定的 value 追加到该 key 原来值（value）的末尾
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="value">字符串</param>
    /// <returns>追加指定值之后， key 中字符串的长度</returns>
    public static Task<long> AppendAsync(string key, object value) => Instance.AppendAsync(key, value);
    /// <summary>
    /// 计算给定位置被设置为 1 的比特位的数量
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置</param>
    /// <param name="end">结束位置</param>
    /// <returns></returns>
    public static Task<long> BitCountAsync(string key, long start, long end) => Instance.BitCountAsync(key, start, end);
    /// <summary>
    /// 对一个或多个保存二进制位的字符串 key 进行位元操作，并将结果保存到 destkey 上
    /// </summary>
    /// <param name="op">And | Or | XOr | Not</param>
    /// <param name="destKey">不含prefix前辍</param>
    /// <param name="keys">不含prefix前辍</param>
    /// <returns>保存到 destkey 的长度，和输入 key 中最长的长度相等</returns>
    public static Task<long> BitOpAsync(RedisBitOp op, string destKey, params string[] keys) => Instance.BitOpAsync(op, destKey, keys);
    /// <summary>
    /// 对 key 所储存的值，查找范围内第一个被设置为1或者0的bit位
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="bit">查找值</param>
    /// <param name="start">开始位置，-1是最后一个，-2是倒数第二个</param>
    /// <param name="end">结果位置，-1是最后一个，-2是倒数第二个</param>
    /// <returns>返回范围内第一个被设置为1或者0的bit位</returns>
    public static Task<long> BitPosAsync(string key, bool bit, long? start = null, long? end = null) => Instance.BitPosAsync(key, bit, start, end);
    /// <summary>
    /// 获取指定 key 的值
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<string> GetAsync(string key) => Instance.GetAsync(key);
    /// <summary>
    /// 获取指定 key 的值
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<T> GetAsync<T>(string key) => Instance.GetAsync<T>(key);
    /// <summary>
    /// 对 key 所储存的值，获取指定偏移量上的位(bit)
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="offset">偏移量</param>
    /// <returns></returns>
    public static Task<bool> GetBitAsync(string key, uint offset) => Instance.GetBitAsync(key, offset);
    /// <summary>
    /// 返回 key 中字符串值的子字符
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <param name="end">结束位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <returns></returns>
    public static Task<string> GetRangeAsync(string key, long start, long end) => Instance.GetRangeAsync(key, start, end);
    /// <summary>
    /// 返回 key 中字符串值的子字符
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <param name="end">结束位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <returns></returns>
    public static Task<T> GetRangeAsync<T>(string key, long start, long end) => Instance.GetRangeAsync<T>(key, start, end);
    /// <summary>
    /// 将给定 key 的值设为 value ，并返回 key 的旧值(old value)
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="value">值</param>
    /// <returns></returns>
    public static Task<string> GetSetAsync(string key, object value) => Instance.GetSetAsync(key, value);
    /// <summary>
    /// 将给定 key 的值设为 value ，并返回 key 的旧值(old value)
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="value">值</param>
    /// <returns></returns>
    public static Task<T> GetSetAsync<T>(string key, object value) => Instance.GetSetAsync<T>(key, value);
    /// <summary>
    /// 将 key 所储存的值加上给定的增量值（increment）
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="value">增量值(默认=1)</param>
    /// <returns></returns>
    public static Task<long> IncrByAsync(string key, long value = 1) => Instance.IncrByAsync(key, value);
    /// <summary>
    /// 将 key 所储存的值加上给定的浮点增量值（increment）
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="value">增量值(默认=1)</param>
    /// <returns></returns>
    public static Task<decimal> IncrByFloatAsync(string key, decimal value = 1) => Instance.IncrByFloatAsync(key, value);
    /// <summary>
    /// 获取多个指定 key 的值(数组)
    /// </summary>
    /// <param name="keys">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<string[]> MGetAsync(params string[] keys) => Instance.MGetAsync(keys);
    /// <summary>
    /// 获取多个指定 key 的值(数组)
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="keys">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<T[]> MGetAsync<T>(params string[] keys) => Instance.MGetAsync<T>(keys);
    /// <summary>
    /// 同时设置一个或多个 key-value 对
    /// </summary>
    /// <param name="keyValues">key1 value1 [key2 value2]</param>
    /// <returns></returns>
    public static Task<bool> MSetAsync(params object[] keyValues) => Instance.MSetAsync(keyValues);
    /// <summary>
    /// 同时设置一个或多个 key-value 对，当且仅当所有给定 key 都不存在
    /// </summary>
    /// <param name="keyValues">key1 value1 [key2 value2]</param>
    /// <returns></returns>
    public static Task<bool> MSetNxAsync(params object[] keyValues) => Instance.MSetNxAsync(keyValues);
    /// <summary>
    /// 设置指定 key 的值，所有写入参数object都支持string | byte[] | 数值 | 对象
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="value">值</param>
    /// <param name="expireSeconds">过期(秒单位)</param>
    /// <param name="exists">Nx, Xx</param>
    /// <returns></returns>
    public static Task<bool> SetAsync(string key, object value, int expireSeconds = -1, RedisExistence? exists = null) => Instance.SetAsync(key, value, expireSeconds, exists);
    public static Task<bool> SetAsync(string key, object value, TimeSpan expire, RedisExistence? exists = null) => Instance.SetAsync(key, value, expire, exists);
    /// <summary>
    /// 对 key 所储存的字符串值，设置或清除指定偏移量上的位(bit)
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="offset">偏移量</param>
    /// <param name="value">值</param>
    /// <returns></returns>
    public static Task<bool> SetBitAsync(string key, uint offset, bool value) => Instance.SetBitAsync(key, offset, value);
    /// <summary>
    /// 只有在 key 不存在时设置 key 的值
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="value">值</param>
    /// <returns></returns>
    public static Task<bool> SetNxAsync(string key, object value) => Instance.SetNxAsync(key, value);
    /// <summary>
    /// 用 value 参数覆写给定 key 所储存的字符串值，从偏移量 offset 开始
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="offset">偏移量</param>
    /// <param name="value">值</param>
    /// <returns>被修改后的字符串长度</returns>
    public static Task<long> SetRangeAsync(string key, uint offset, object value) => Instance.SetRangeAsync(key, offset, value);
    /// <summary>
    /// 返回 key 所储存的字符串值的长度
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<long> StrLenAsync(string key) => Instance.StrLenAsync(key);
    #endregion

#endif

}
