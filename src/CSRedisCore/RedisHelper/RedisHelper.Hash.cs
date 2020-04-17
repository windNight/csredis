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

    #region Hash

    /// <summary>
    /// 删除一个或多个哈希表字段
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="fields">字段</param>
    /// <returns></returns>
    public static long HDel(string key, params string[] fields) => Instance.HDel(key, fields);
    /// <summary>
    /// 查看哈希表 key 中，指定的字段是否存在
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="field">字段</param>
    /// <returns></returns>
    public static bool HExists(string key, string field) => Instance.HExists(key, field);
    /// <summary>
    /// 获取存储在哈希表中指定字段的值
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="field">字段</param>
    /// <returns></returns>
    public static string HGet(string key, string field) => Instance.HGet(key, field);
    /// <summary>
    /// 获取存储在哈希表中指定字段的值
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="field">字段</param>
    /// <returns></returns>
    public static T HGet<T>(string key, string field) => Instance.HGet<T>(key, field);
    /// <summary>
    /// 获取在哈希表中指定 key 的所有字段和值
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Dictionary<string, string> HGetAll(string key) => Instance.HGetAll(key);
    /// <summary>
    /// 获取在哈希表中指定 key 的所有字段和值
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Dictionary<string, T> HGetAll<T>(string key) => Instance.HGetAll<T>(key);
    /// <summary>
    /// 为哈希表 key 中的指定字段的整数值加上增量 increment
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="field">字段</param>
    /// <param name="value">增量值(默认=1)</param>
    /// <returns></returns>
    public static long HIncrBy(string key, string field, long value = 1) => Instance.HIncrBy(key, field, value);
    /// <summary>
    /// 为哈希表 key 中的指定字段的整数值加上增量 increment
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="field">字段</param>
    /// <param name="value">增量值(默认=1)</param>
    /// <returns></returns>
    public static decimal HIncrByFloat(string key, string field, decimal value = 1) => Instance.HIncrByFloat(key, field, value);
    /// <summary>
    /// 获取所有哈希表中的字段
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static string[] HKeys(string key) => Instance.HKeys(key);
    /// <summary>
    /// 获取哈希表中字段的数量
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static long HLen(string key) => Instance.HLen(key);
    /// <summary>
    /// 获取存储在哈希表中多个字段的值
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="fields">字段</param>
    /// <returns></returns>
    public static string[] HMGet(string key, params string[] fields) => Instance.HMGet(key, fields);
    /// <summary>
    /// 获取存储在哈希表中多个字段的值
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="fields">一个或多个字段</param>
    /// <returns></returns>
    public static T[] HMGet<T>(string key, params string[] fields) => Instance.HMGet<T>(key, fields);
    /// <summary>
    /// 同时将多个 field-value (域-值)对设置到哈希表 key 中
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="keyValues">key1 value1 [key2 value2]</param>
    /// <returns></returns>
    public static bool HMSet(string key, params object[] keyValues) => Instance.HMSet(key, keyValues);

    /// <summary>
    /// Set multiple hash fields to multiple values
    /// </summary>
    /// <param name="key">Hash key</param>
    /// <param name="dict">Dictionary mapping of hash</param>
    /// <returns>Status code</returns>
    public static bool HMSet(string key, Dictionary<string, object> dict) => Instance.HMSet(key, dict);

    /// <summary>
    /// 将哈希表 key 中的字段 field 的值设为 value
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="field">字段</param>
    /// <param name="value">值</param>
    /// <returns>如果字段是哈希表中的一个新建字段，并且值设置成功，返回true。如果哈希表中域字段已经存在且旧值已被新值覆盖，返回false。</returns>
    public static bool HSet(string key, string field, object value) => Instance.HSet(key, field, value);
    /// <summary>
    /// 只有在字段 field 不存在时，设置哈希表字段的值
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="field">字段</param>
    /// <param name="value">值(string 或 byte[])</param>
    /// <returns></returns>
    public static bool HSetNx(string key, string field, object value) => Instance.HSetNx(key, field, value);
    /// <summary>
    /// 获取哈希表中所有值
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static string[] HVals(string key) => Instance.HVals(key);
    /// <summary>
    /// 获取哈希表中所有值
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static T[] HVals<T>(string key) => Instance.HVals<T>(key);
    /// <summary>
    /// 迭代哈希表中的键值对
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="cursor">位置</param>
    /// <param name="pattern">模式</param>
    /// <param name="count">数量</param>
    /// <returns></returns>
    public static RedisScan<(string field, string value)> HScan(string key, long cursor, string pattern = null, long? count = null) =>
        Instance.HScan(key, cursor, pattern, count);
    /// <summary>
    /// 迭代哈希表中的键值对
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="cursor">位置</param>
    /// <param name="pattern">模式</param>
    /// <param name="count">数量</param>
    /// <returns></returns>
    public static RedisScan<(string field, T value)> HScan<T>(string key, long cursor, string pattern = null, long? count = null) =>
        Instance.HScan<T>(key, cursor, pattern, count);

    #endregion

#if !net40

    #region Hash

    /// <summary>
    /// 删除一个或多个哈希表字段
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="fields">字段</param>
    /// <returns></returns>
    public static Task<long> HDelAsync(string key, params string[] fields) => Instance.HDelAsync(key, fields);
    
    /// <summary>
    /// 查看哈希表 key 中，指定的字段是否存在
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="field">字段</param>
    /// <returns></returns>
    public static Task<bool> HExistsAsync(string key, string field) => Instance.HExistsAsync(key, field);
    
    /// <summary>
    /// 获取存储在哈希表中指定字段的值
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="field">字段</param>
    /// <returns></returns>
    public static Task<string> HGetAsync(string key, string field) => Instance.HGetAsync(key, field);
    
    /// <summary>
    /// 获取存储在哈希表中指定字段的值
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="field">字段</param>
    /// <returns></returns>
    public static Task<T> HGetAsync<T>(string key, string field) => Instance.HGetAsync<T>(key, field);
    
    /// <summary>
    /// 获取在哈希表中指定 key 的所有字段和值
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<Dictionary<string, string>> HGetAllAsync(string key) => Instance.HGetAllAsync(key);
   
    /// <summary>
    /// 获取在哈希表中指定 key 的所有字段和值
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<Dictionary<string, T>> HGetAllAsync<T>(string key) => Instance.HGetAllAsync<T>(key);
    
    /// <summary>
    /// 为哈希表 key 中的指定字段的整数值加上增量 increment
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="field">字段</param>
    /// <param name="value">增量值(默认=1)</param>
    /// <returns></returns>
    public static Task<long> HIncrByAsync(string key, string field, long value = 1) => Instance.HIncrByAsync(key, field, value);
    
    /// <summary>
    /// 为哈希表 key 中的指定字段的整数值加上增量 increment
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="field">字段</param>
    /// <param name="value">增量值(默认=1)</param>
    /// <returns></returns>
    public static Task<decimal> HIncrByFloatAsync(string key, string field, decimal value = 1) => Instance.HIncrByFloatAsync(key, field, value);
   
    /// <summary>
    /// 获取所有哈希表中的字段
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<string[]> HKeysAsync(string key) => Instance.HKeysAsync(key);
   
    /// <summary>
    /// 获取哈希表中字段的数量
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<long> HLenAsync(string key) => Instance.HLenAsync(key);
    
    /// <summary>
    /// 获取存储在哈希表中多个字段的值
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="fields">字段</param>
    /// <returns></returns>
    public static Task<string[]> HMGetAsync(string key, params string[] fields) => Instance.HMGetAsync(key, fields);
    
    /// <summary>
    /// 获取存储在哈希表中多个字段的值
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="fields">一个或多个字段</param>
    /// <returns></returns>
    public static Task<T[]> HMGetAsync<T>(string key, params string[] fields) => Instance.HMGetAsync<T>(key, fields);
    
    /// <summary>
    /// 同时将多个 field-value (域-值)对设置到哈希表 key 中
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="keyValues">key1 value1 [key2 value2]</param>
    /// <returns></returns>
    public static Task<bool> HMSetAsync(string key, params object[] keyValues) => Instance.HMSetAsync(key, keyValues);

    public static Task<bool> HMSetAsync(string key, Dictionary<string, object> dict) => Instance.HMSetAsync(key, dict);
    
    /// <summary>
    /// 将哈希表 key 中的字段 field 的值设为 value
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="field">字段</param>
    /// <param name="value">值</param>
    /// <returns>如果字段是哈希表中的一个新建字段，并且值设置成功，返回true。如果哈希表中域字段已经存在且旧值已被新值覆盖，返回false。</returns>
    public static Task<bool> HSetAsync(string key, string field, object value) => Instance.HSetAsync(key, field, value);
   
    /// <summary>
    /// 只有在字段 field 不存在时，设置哈希表字段的值
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="field">字段</param>
    /// <param name="value">值(string 或 byte[])</param>
    /// <returns></returns>
    public static Task<bool> HSetNxAsync(string key, string field, object value) => Instance.HSetNxAsync(key, field, value);
   
    /// <summary>
    /// 获取哈希表中所有值
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<string[]> HValsAsync(string key) => Instance.HValsAsync(key);
    
    /// <summary>
    /// 获取哈希表中所有值
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<T[]> HValsAsync<T>(string key) => Instance.HValsAsync<T>(key);
    
    /// <summary>
    /// 迭代哈希表中的键值对
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="cursor">位置</param>
    /// <param name="pattern">模式</param>
    /// <param name="count">数量</param>
    /// <returns></returns>
    public static Task<RedisScan<(string field, string value)>> HScanAsync(string key, long cursor, string pattern = null, long? count = null) =>
           Instance.HScanAsync(key, cursor, pattern, count);
    
    /// <summary>
    /// 迭代哈希表中的键值对
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="cursor">位置</param>
    /// <param name="pattern">模式</param>
    /// <param name="count">数量</param>
    /// <returns></returns>
    public static Task<RedisScan<(string field, T value)>> HScanAsync<T>(string key, long cursor, string pattern = null, long? count = null) =>
           Instance.HScanAsync<T>(key, cursor, pattern, count);

    #endregion

#endif

}
