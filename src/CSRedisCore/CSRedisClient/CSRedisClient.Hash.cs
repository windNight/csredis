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
        #region Hash
        /// <summary>
        /// [redis-server 3.2.0] 返回hash指定field的value的字符串长度，如果hash或者field不存在，返回0.
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public long HStrLen(string key, string field) => ExecuteScalar(key, (c, k) => c.Value.HStrLen(k, field));
        /// <summary>
        /// 删除一个或多个哈希表字段
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="fields">字段</param>
        /// <returns></returns>
        public long HDel(string key, params string[] fields) => fields == null || fields.Any() == false ? 0 : ExecuteScalar(key, (c, k) => c.Value.HDel(k, fields));
        /// <summary>
        /// 查看哈希表 key 中，指定的字段是否存在
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public bool HExists(string key, string field) => ExecuteScalar(key, (c, k) => c.Value.HExists(k, field));
        /// <summary>
        /// 获取存储在哈希表中指定字段的值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public string HGet(string key, string field) => ExecuteScalar(key, (c, k) => c.Value.HGet(k, field));
        /// <summary>
        /// 获取存储在哈希表中指定字段的值
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public T HGet<T>(string key, string field) => this.DeserializeRedisValueInternal<T>(ExecuteScalar(key, (c, k) => c.Value.HGetBytes(k, field)));
        /// <summary>
        /// 获取在哈希表中指定 key 的所有字段和值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public Dictionary<string, string> HGetAll(string key) => ExecuteScalar(key, (c, k) => c.Value.HGetAll(k));
        /// <summary>
        /// 获取在哈希表中指定 key 的所有字段和值
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public Dictionary<string, T> HGetAll<T>(string key) => this.DeserializeRedisValueDictionaryInternal<string, T>(ExecuteScalar(key, (c, k) => c.Value.HGetAllBytes(k)));
        /// <summary>
        /// 为哈希表 key 中的指定字段的整数值加上增量 increment
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="field">字段</param>
        /// <param name="value">增量值(默认=1)</param>
        /// <returns></returns>
        public long HIncrBy(string key, string field, long value = 1) => ExecuteScalar(key, (c, k) => c.Value.HIncrBy(k, field, value));
        /// <summary>
        /// 为哈希表 key 中的指定字段的整数值加上增量 increment
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="field">字段</param>
        /// <param name="value">增量值(默认=1)</param>
        /// <returns></returns>
        public decimal HIncrByFloat(string key, string field, decimal value) => ExecuteScalar(key, (c, k) => c.Value.HIncrByFloat(k, field, value));
        /// <summary>
        /// 获取所有哈希表中的字段
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public string[] HKeys(string key) => ExecuteScalar(key, (c, k) => c.Value.HKeys(k));
        /// <summary>
        /// 获取哈希表中字段的数量
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public long HLen(string key) => ExecuteScalar(key, (c, k) => c.Value.HLen(k));
        /// <summary>
        /// 获取存储在哈希表中多个字段的值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="fields">字段</param>
        /// <returns></returns>
        public string[] HMGet(string key, params string[] fields) => fields == null || fields.Any() == false ? new string[0] : ExecuteScalar(key, (c, k) => c.Value.HMGet(k, fields));
        /// <summary>
        /// 获取存储在哈希表中多个字段的值
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="fields">一个或多个字段</param>
        /// <returns></returns>
        public T[] HMGet<T>(string key, params string[] fields) => fields == null || fields.Any() == false ? new T[0] : this.DeserializeRedisValueArrayInternal<T>(ExecuteScalar(key, (c, k) => c.Value.HMGetBytes(k, fields)));
        /// <summary>
        /// 同时将多个 field-value (域-值)对设置到哈希表 key 中
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="keyValues">key1 value1 [key2 value2]</param>
        /// <returns></returns>
        public bool HMSet(string key, params object[] keyValues)
        {
            if (keyValues == null || keyValues.Any() == false) return false;
            if (keyValues.Length % 2 != 0) throw new Exception("keyValues 参数是键值对，不应该出现奇数(数量)，请检查使用姿势。");
            var parms = new List<object>();
            for (var a = 0; a < keyValues.Length; a += 2)
            {
                var k = string.Concat(keyValues[a]);
                var v = keyValues[a + 1];
                if (string.IsNullOrEmpty(k)) throw new Exception("keyValues 参数是键值对，并且 key 不可为空");
                parms.Add(k);
                parms.Add(this.SerializeRedisValueInternal(v));
            }
            return ExecuteScalar(key, (c, k) => c.Value.HMSet(k, parms.ToArray())) == "OK";
        }

        public bool HMSet(string key, Dictionary<string, object> dict)
        {
            if (dict == null || dict.Any() == false) return false;

            return ExecuteScalar(key, (c, k) => c.Value.HMSet(k, dict)) == "OK";
        }

        /// <summary>
        /// 将哈希表 key 中的字段 field 的值设为 value
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <returns>如果字段是哈希表中的一个新建字段，并且值设置成功，返回true。如果哈希表中域字段已经存在且旧值已被新值覆盖，返回false。</returns>
        public bool HSet(string key, string field, object value)
        {
            var args = this.SerializeRedisValueInternal(value);
            return ExecuteScalar(key, (c, k) => c.Value.HSet(k, field, args));
        }
        /// <summary>
        /// 只有在字段 field 不存在时，设置哈希表字段的值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="field">字段</param>
        /// <param name="value">值(string 或 byte[])</param>
        /// <returns></returns>
        public bool HSetNx(string key, string field, object value)
        {
            var args = this.SerializeRedisValueInternal(value);
            return ExecuteScalar(key, (c, k) => c.Value.HSetNx(k, field, args));
        }
        /// <summary>
        /// 获取哈希表中所有值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public string[] HVals(string key) => ExecuteScalar(key, (c, k) => c.Value.HVals(k));
        /// <summary>
        /// 获取哈希表中所有值
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public T[] HVals<T>(string key) => this.DeserializeRedisValueArrayInternal<T>(ExecuteScalar(key, (c, k) => c.Value.HValsBytes(k)));
        /// <summary>
        /// 迭代哈希表中的键值对
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="cursor">位置</param>
        /// <param name="pattern">模式</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public RedisScan<(string field, string value)> HScan(string key, long cursor, string pattern = null, long? count = null)
        {
            var scan = ExecuteScalar(key, (c, k) => c.Value.HScan(k, cursor, pattern, count));
            return new RedisScan<(string, string)>(scan.Cursor, scan.Items.Select(z => (z.Item1, z.Item2)).ToArray());
        }
        /// <summary>
        /// 迭代哈希表中的键值对
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="cursor">位置</param>
        /// <param name="pattern">模式</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public RedisScan<(string field, T value)> HScan<T>(string key, long cursor, string pattern = null, long? count = null)
        {
            var scan = ExecuteScalar(key, (c, k) => c.Value.HScanBytes(k, cursor, pattern, count));
            return new RedisScan<(string, T)>(scan.Cursor, scan.Items.Select(z => (z.Item1, this.DeserializeRedisValueInternal<T>(z.Item2))).ToArray());
        }
        #endregion


#if !net40

        #region Hash
        /// <summary>
        /// [redis-server 3.2.0] 返回hash指定field的value的字符串长度，如果hash或者field不存在，返回0.
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public Task<long> HStrLenAsync(string key, string field) => ExecuteScalarAsync(key, (c, k) => c.Value.HStrLenAsync(k, field));
        /// <summary>
        /// 删除一个或多个哈希表字段
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="fields">字段</param>
        /// <returns></returns>
        public async Task<long> HDelAsync(string key, params string[] fields) => fields == null || fields.Any() == false ? 0 :
            await ExecuteScalarAsync(key, (c, k) => c.Value.HDelAsync(k, fields));
        /// <summary>
        /// 查看哈希表 key 中，指定的字段是否存在
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public Task<bool> HExistsAsync(string key, string field) => ExecuteScalarAsync(key, (c, k) => c.Value.HExistsAsync(k, field));
        /// <summary>
        /// 获取存储在哈希表中指定字段的值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public Task<string> HGetAsync(string key, string field) => ExecuteScalarAsync(key, (c, k) => c.Value.HGetAsync(k, field));
        /// <summary>
        /// 获取存储在哈希表中指定字段的值
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public async Task<T> HGetAsync<T>(string key, string field) => this.DeserializeRedisValueInternal<T>(await ExecuteScalarAsync(key, (c, k) => c.Value.HGetBytesAsync(k, field)));
        /// <summary>
        /// 获取在哈希表中指定 key 的所有字段和值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public Task<Dictionary<string, string>> HGetAllAsync(string key) => ExecuteScalarAsync(key, (c, k) => c.Value.HGetAllAsync(k));
        /// <summary>
        /// 获取在哈希表中指定 key 的所有字段和值
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public async Task<Dictionary<string, T>> HGetAllAsync<T>(string key) => this.DeserializeRedisValueDictionaryInternal<string, T>(await ExecuteScalarAsync(key, (c, k) => c.Value.HGetAllBytesAsync(k)));
        /// <summary>
        /// 为哈希表 key 中的指定字段的整数值加上增量 increment
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="field">字段</param>
        /// <param name="value">增量值(默认=1)</param>
        /// <returns></returns>
        public Task<long> HIncrByAsync(string key, string field, long value = 1) => ExecuteScalarAsync(key, (c, k) => c.Value.HIncrByAsync(k, field, value));
        /// <summary>
        /// 为哈希表 key 中的指定字段的整数值加上增量 increment
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="field">字段</param>
        /// <param name="value">增量值(默认=1)</param>
        /// <returns></returns>
        public Task<decimal> HIncrByFloatAsync(string key, string field, decimal value) => ExecuteScalarAsync(key, (c, k) => c.Value.HIncrByFloatAsync(k, field, value));
        /// <summary>
        /// 获取所有哈希表中的字段
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public Task<string[]> HKeysAsync(string key) => ExecuteScalarAsync(key, (c, k) => c.Value.HKeysAsync(k));
        /// <summary>
        /// 获取哈希表中字段的数量
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public Task<long> HLenAsync(string key) => ExecuteScalarAsync(key, (c, k) => c.Value.HLenAsync(k));
        /// <summary>
        /// 获取存储在哈希表中多个字段的值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="fields">字段</param>
        /// <returns></returns>
        public async Task<string[]> HMGetAsync(string key, params string[] fields) => fields == null || fields.Any() == false ? new string[0] :
            await ExecuteScalarAsync(key, (c, k) => c.Value.HMGetAsync(k, fields));
        /// <summary>
        /// 获取存储在哈希表中多个字段的值
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="fields">一个或多个字段</param>
        /// <returns></returns>
        public async Task<T[]> HMGetAsync<T>(string key, params string[] fields) => fields == null || fields.Any() == false ? new T[0] :
            this.DeserializeRedisValueArrayInternal<T>(await ExecuteScalarAsync(key, (c, k) => c.Value.HMGetBytesAsync(k, fields)));
        /// <summary>
        /// 同时将多个 field-value (域-值)对设置到哈希表 key 中
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="keyValues">key1 value1 [key2 value2]</param>
        /// <returns></returns>
        public async Task<bool> HMSetAsync(string key, params object[] keyValues)
        {
            if (keyValues == null || keyValues.Any() == false) return false;
            if (keyValues.Length % 2 != 0) throw new Exception("keyValues 参数是键值对，不应该出现奇数(数量)，请检查使用姿势。");
            var parms = new List<object>();
            for (var a = 0; a < keyValues.Length; a += 2)
            {
                var k = string.Concat(keyValues[a]);
                var v = keyValues[a + 1];
                if (string.IsNullOrEmpty(k)) throw new Exception("keyValues 参数是键值对，并且 key 不可为空");
                parms.Add(k);
                parms.Add(this.SerializeRedisValueInternal(v));
            }
            return await ExecuteScalarAsync(key, (c, k) => c.Value.HMSetAsync(k, parms.ToArray())) == "OK";
        }

        public async Task<bool> HMSetAsync(string key, Dictionary<string, object> dict)
        {
            if (dict == null || dict.Any() == false) return false;
            return await ExecuteScalarAsync(key, (c, k) => c.Value.HMSetAsync(k, dict)) == "OK";
        }


        /// <summary>
        /// 将哈希表 key 中的字段 field 的值设为 value
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="field">字段</param>
        /// <param name="value">值</param>
        /// <returns>如果字段是哈希表中的一个新建字段，并且值设置成功，返回true。如果哈希表中域字段已经存在且旧值已被新值覆盖，返回false。</returns>
        public Task<bool> HSetAsync(string key, string field, object value)
        {
            var args = this.SerializeRedisValueInternal(value);
            return ExecuteScalarAsync(key, (c, k) => c.Value.HSetAsync(k, field, args));
        }
        /// <summary>
        /// 只有在字段 field 不存在时，设置哈希表字段的值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="field">字段</param>
        /// <param name="value">值(string 或 byte[])</param>
        /// <returns></returns>
        public Task<bool> HSetNxAsync(string key, string field, object value)
        {
            var args = this.SerializeRedisValueInternal(value);
            return ExecuteScalarAsync(key, (c, k) => c.Value.HSetNxAsync(k, field, args));
        }
        /// <summary>
        /// 获取哈希表中所有值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public Task<string[]> HValsAsync(string key) => ExecuteScalarAsync(key, (c, k) => c.Value.HValsAsync(k));
        /// <summary>
        /// 获取哈希表中所有值
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public async Task<T[]> HValsAsync<T>(string key) => this.DeserializeRedisValueArrayInternal<T>(await ExecuteScalarAsync(key, (c, k) => c.Value.HValsBytesAsync(k)));
        /// <summary>
        /// 迭代哈希表中的键值对
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="cursor">位置</param>
        /// <param name="pattern">模式</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public async Task<RedisScan<(string field, string value)>> HScanAsync(string key, long cursor, string pattern = null, long? count = null)
        {
            var scan = await ExecuteScalarAsync(key, (c, k) => c.Value.HScanAsync(k, cursor, pattern, count));
            return new RedisScan<(string, string)>(scan.Cursor, scan.Items.Select(z => (z.Item1, z.Item2)).ToArray());
        }
        /// <summary>
        /// 迭代哈希表中的键值对
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="cursor">位置</param>
        /// <param name="pattern">模式</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public async Task<RedisScan<(string field, T value)>> HScanAsync<T>(string key, long cursor, string pattern = null, long? count = null)
        {
            var scan = await ExecuteScalarAsync(key, (c, k) => c.Value.HScanBytesAsync(k, cursor, pattern, count));
            return new RedisScan<(string, T)>(scan.Cursor, scan.Items.Select(z => (z.Item1, this.DeserializeRedisValueInternal<T>(z.Item2))).ToArray());
        }
        #endregion



#endif

    }
}