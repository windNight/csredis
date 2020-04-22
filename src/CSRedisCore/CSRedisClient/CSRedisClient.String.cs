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
        #region String
        /// <summary>
        /// 如果 key 已经存在并且是一个字符串， APPEND 命令将指定的 value 追加到该 key 原来值（value）的末尾
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">字符串</param>
        /// <returns>追加指定值之后， key 中字符串的长度</returns>
        public long Append(string key, object value)
        {
            var args = this.SerializeRedisValueInternal(value);
            return ExecuteScalar(key, (c, k) => c.Value.Append(k, args));
        }
        /// <summary>
        /// 计算给定位置被设置为 1 的比特位的数量
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置</param>
        /// <param name="end">结束位置</param>
        /// <returns></returns>
        public long BitCount(string key, long start, long end) => ExecuteScalar(key, (c, k) => c.Value.BitCount(k, start, end));
        /// <summary>
        /// 对一个或多个保存二进制位的字符串 key 进行位元操作，并将结果保存到 destkey 上
        /// </summary>
        /// <param name="op">And | Or | XOr | Not</param>
        /// <param name="destKey">不含prefix前辍</param>
        /// <param name="keys">不含prefix前辍</param>
        /// <returns>保存到 destkey 的长度，和输入 key 中最长的长度相等</returns>
        public long BitOp(RedisBitOp op, string destKey, params string[] keys)
        {
            if (string.IsNullOrEmpty(destKey)) throw new Exception("destKey 不能为空");
            if (keys == null || keys.Length == 0) throw new Exception("keys 不能为空");
            return NodesNotSupport(new[] { destKey }.Concat(keys).ToArray(), 0, (c, k) => c.Value.BitOp(op, k.First(), k.Skip(1).ToArray()));
        }
        /// <summary>
        /// 对 key 所储存的值，查找范围内第一个被设置为1或者0的bit位
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="bit">查找值</param>
        /// <param name="start">开始位置，-1是最后一个，-2是倒数第二个</param>
        /// <param name="end">结果位置，-1是最后一个，-2是倒数第二个</param>
        /// <returns>返回范围内第一个被设置为1或者0的bit位</returns>
        public long BitPos(string key, bool bit, long? start = null, long? end = null) => ExecuteScalar(key, (c, k) => c.Value.BitPos(k, bit, start, end));
        /// <summary>
        /// 获取指定 key 的值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public string Get(string key) => ExecuteScalar(key, (c, k) => c.Value.Get(k));
        /// <summary>
        /// 获取指定 key 的值
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public T Get<T>(string key) => this.DeserializeRedisValueInternal<T>(ExecuteScalar(key, (c, k) => c.Value.GetBytes(k)));
        /// <summary>
        /// 对 key 所储存的值，获取指定偏移量上的位(bit)
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="offset">偏移量</param>
        /// <returns></returns>
        public bool GetBit(string key, uint offset) => ExecuteScalar(key, (c, k) => c.Value.GetBit(k, offset));
        /// <summary>
        /// 返回 key 中字符串值的子字符
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="end">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        public string GetRange(string key, long start, long end) => ExecuteScalar(key, (c, k) => c.Value.GetRange(k, start, end));
        /// <summary>
        /// 返回 key 中字符串值的子字符
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="end">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        public T GetRange<T>(string key, long start, long end) => this.DeserializeRedisValueInternal<T>(ExecuteScalar(key, (c, k) => c.Value.GetRangeBytes(k, start, end)));
        /// <summary>
        /// 将给定 key 的值设为 value ，并返回 key 的旧值(old value)
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public string GetSet(string key, object value)
        {
            var args = this.SerializeRedisValueInternal(value);
            return ExecuteScalar(key, (c, k) => c.Value.GetSet(k, args));
        }
        /// <summary>
        /// 将给定 key 的值设为 value ，并返回 key 的旧值(old value)
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public T GetSet<T>(string key, object value)
        {
            var args = this.SerializeRedisValueInternal(value);
            return this.DeserializeRedisValueInternal<T>(ExecuteScalar(key, (c, k) => c.Value.GetSetBytes(k, args)));
        }
        /// <summary>
        /// 将 key 所储存的值加上给定的增量值（increment）
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">增量值(默认=1)</param>
        /// <returns></returns>
        public long IncrBy(string key, long value = 1) => ExecuteScalar(key, (c, k) => c.Value.IncrBy(k, value));
        /// <summary>
        /// 将 key 所储存的值加上给定的浮点增量值（increment）
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">增量值(默认=1)</param>
        /// <returns></returns>
        public decimal IncrByFloat(string key, decimal value) => ExecuteScalar(key, (c, k) => c.Value.IncrByFloat(k, value));
        /// <summary>
        /// 获取多个指定 key 的值(数组)
        /// </summary>
        /// <param name="keys">不含prefix前辍</param>
        /// <returns></returns>
        public string[] MGet(params string[] keys) => ExecuteArray(keys, (c, k) => c.Value.MGet(k));
        /// <summary>
        /// 获取多个指定 key 的值(数组)
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="keys">不含prefix前辍</param>
        /// <returns></returns>
        public T[] MGet<T>(params string[] keys) => this.DeserializeRedisValueArrayInternal<T>(ExecuteArray(keys, (c, k) => c.Value.MGetBytes(k)));
        /// <summary>
        /// 同时设置一个或多个 key-value 对
        /// </summary>
        /// <param name="keyValues">key1 value1 [key2 value2]</param>
        /// <returns></returns>
        public bool MSet(params object[] keyValues) => MSetInternal(RedisExistence.Xx, keyValues);
        /// <summary>
        /// 同时设置一个或多个 key-value 对，当且仅当所有给定 key 都不存在
        /// </summary>
        /// <param name="keyValues">key1 value1 [key2 value2]</param>
        /// <returns></returns>
        public bool MSetNx(params object[] keyValues) => MSetInternal(RedisExistence.Nx, keyValues);
        internal bool MSetInternal(RedisExistence exists, params object[] keyValues)
        {
            if (keyValues == null || keyValues.Any() == false) return false;
            if (keyValues.Length % 2 != 0) throw new Exception("keyValues 参数是键值对，不应该出现奇数(数量)，请检查使用姿势。");
            var dic = new Dictionary<string, object>();
            for (var a = 0; a < keyValues.Length; a += 2)
            {
                var k = string.Concat(keyValues[a]);
                var v = this.SerializeRedisValueInternal(keyValues[a + 1]);
                if (string.IsNullOrEmpty(k)) throw new Exception("keyValues 参数是键值对，并且 key 不可为空");
                if (dic.ContainsKey(k)) dic[k] = v;
                else dic.Add(k, v);
            }
            Func<Object<RedisClient>, string[], long> handle = (c, k) =>
            {
                var prefix = (c.Pool as RedisClientPool)?.Prefix;
                var parms = new object[k.Length * 2];
                for (var a = 0; a < k.Length; a++)
                {
                    parms[a * 2] = k[a];
                    parms[a * 2 + 1] = dic[string.IsNullOrEmpty(prefix) ? k[a] : k[a].Substring(prefix.Length)];
                }
                if (exists == RedisExistence.Nx) return c.Value.MSetNx(parms) ? 1 : 0;
                return c.Value.MSet(parms) == "OK" ? 1 : 0;
            };
            if (exists == RedisExistence.Nx) return NodesNotSupport(dic.Keys.ToArray(), 0, handle) > 0;
            return ExecuteNonQuery(dic.Keys.ToArray(), handle) > 0;
        }
        /// <summary>
        /// 设置指定 key 的值，所有写入参数object都支持string | byte[] | 数值 | 对象
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">值</param>
        /// <param name="expireSeconds">过期(秒单位)</param>
        /// <param name="exists">Nx, Xx</param>
        /// <returns></returns>
        public bool Set(string key, object value, int expireSeconds = -1, RedisExistence? exists = null)
        {
            object redisValule = this.SerializeRedisValueInternal(value);
            if (expireSeconds <= 0 && exists == null) return ExecuteScalar(key, (c, k) => c.Value.Set(k, redisValule)) == "OK";
            if (expireSeconds <= 0 && exists != null) return ExecuteScalar(key, (c, k) => c.Value.Set(k, redisValule, null, exists)) == "OK";
            if (expireSeconds > 0 && exists == null) return ExecuteScalar(key, (c, k) => c.Value.Set(k, redisValule, expireSeconds, null)) == "OK";
            if (expireSeconds > 0 && exists != null) return ExecuteScalar(key, (c, k) => c.Value.Set(k, redisValule, expireSeconds, exists)) == "OK";
            return false;
        }
        public bool Set(string key, object value, TimeSpan expire, RedisExistence? exists = null)
        {
            object redisValule = this.SerializeRedisValueInternal(value);
            if (expire <= TimeSpan.Zero && exists == null) return ExecuteScalar(key, (c, k) => c.Value.Set(k, redisValule)) == "OK";
            if (expire <= TimeSpan.Zero && exists != null) return ExecuteScalar(key, (c, k) => c.Value.Set(k, redisValule, null, exists)) == "OK";
            if (expire > TimeSpan.Zero && exists == null) return ExecuteScalar(key, (c, k) => c.Value.Set(k, redisValule, expire, null)) == "OK";
            if (expire > TimeSpan.Zero && exists != null) return ExecuteScalar(key, (c, k) => c.Value.Set(k, redisValule, expire, exists)) == "OK";
            return false;
        }
        /// <summary>
        /// 对 key 所储存的字符串值，设置或清除指定偏移量上的位(bit)
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="offset">偏移量</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool SetBit(string key, uint offset, bool value) => ExecuteScalar(key, (c, k) => c.Value.SetBit(k, offset, value));
        /// <summary>
        /// 只有在 key 不存在时设置 key 的值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool SetNx(string key, object value)
        {
            var args = this.SerializeRedisValueInternal(value);
            return ExecuteScalar(key, (c, k) => c.Value.SetNx(k, args));
        }
        /// <summary>
        /// 用 value 参数覆写给定 key 所储存的字符串值，从偏移量 offset 开始
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="offset">偏移量</param>
        /// <param name="value">值</param>
        /// <returns>被修改后的字符串长度</returns>
        public long SetRange(string key, uint offset, object value)
        {
            var args = this.SerializeRedisValueInternal(value);
            return ExecuteScalar(key, (c, k) => c.Value.SetRange(k, offset, args));
        }
        /// <summary>
        /// 返回 key 所储存的字符串值的长度
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public long StrLen(string key) => ExecuteScalar(key, (c, k) => c.Value.StrLen(k));
        #endregion


#if !net40

        #region String
        /// <summary>
        /// 如果 key 已经存在并且是一个字符串， APPEND 命令将指定的 value 追加到该 key 原来值（value）的末尾
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">字符串</param>
        /// <returns>追加指定值之后， key 中字符串的长度</returns>
        public Task<long> AppendAsync(string key, object value)
        {
            var args = this.SerializeRedisValueInternal(value);
            return ExecuteScalarAsync(key, (c, k) => c.Value.AppendAsync(k, args));
        }
        /// <summary>
        /// 计算给定位置被设置为 1 的比特位的数量
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置</param>
        /// <param name="end">结束位置</param>
        /// <returns></returns>
        public Task<long> BitCountAsync(string key, long start, long end) => ExecuteScalarAsync(key, (c, k) => c.Value.BitCountAsync(k, start, end));
        /// <summary>
        /// 对一个或多个保存二进制位的字符串 key 进行位元操作，并将结果保存到 destkey 上
        /// </summary>
        /// <param name="op">And | Or | XOr | Not</param>
        /// <param name="destKey">不含prefix前辍</param>
        /// <param name="keys">不含prefix前辍</param>
        /// <returns>保存到 destkey 的长度，和输入 key 中最长的长度相等</returns>
        public async Task<long> BitOpAsync(RedisBitOp op, string destKey, params string[] keys)
        {
            if (string.IsNullOrEmpty(destKey)) throw new Exception("destKey 不能为空");
            if (keys == null || keys.Length == 0) throw new Exception("keys 不能为空");
            return await NodesNotSupportAsync(new[] { destKey }.Concat(keys).ToArray(), 0, (c, k) => c.Value.BitOpAsync(op, k.First(), k.Skip(1).ToArray()));
        }
        /// <summary>
        /// 对 key 所储存的值，查找范围内第一个被设置为1或者0的bit位
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="bit">查找值</param>
        /// <param name="start">开始位置，-1是最后一个，-2是倒数第二个</param>
        /// <param name="end">结果位置，-1是最后一个，-2是倒数第二个</param>
        /// <returns>返回范围内第一个被设置为1或者0的bit位</returns>
        public Task<long> BitPosAsync(string key, bool bit, long? start = null, long? end = null) => ExecuteScalarAsync(key, (c, k) => c.Value.BitPosAsync(k, bit, start, end));
        /// <summary>
        /// 获取指定 key 的值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public Task<string> GetAsync(string key) => ExecuteScalarAsync(key, (c, k) => c.Value.GetAsync(k));
        /// <summary>
        /// 获取指定 key 的值
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key) => this.DeserializeRedisValueInternal<T>(await ExecuteScalarAsync(key, (c, k) => c.Value.GetBytesAsync(k)));
        /// <summary>
        /// 对 key 所储存的值，获取指定偏移量上的位(bit)
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="offset">偏移量</param>
        /// <returns></returns>
        public Task<bool> GetBitAsync(string key, uint offset) => ExecuteScalarAsync(key, (c, k) => c.Value.GetBitAsync(k, offset));
        /// <summary>
        /// 返回 key 中字符串值的子字符
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="end">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        public Task<string> GetRangeAsync(string key, long start, long end) => ExecuteScalarAsync(key, (c, k) => c.Value.GetRangeAsync(k, start, end));
        /// <summary>
        /// 返回 key 中字符串值的子字符
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="end">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        public async Task<T> GetRangeAsync<T>(string key, long start, long end) => this.DeserializeRedisValueInternal<T>(await ExecuteScalarAsync(key, (c, k) => c.Value.GetRangeBytesAsync(k, start, end)));
        /// <summary>
        /// 将给定 key 的值设为 value ，并返回 key 的旧值(old value)
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public Task<string> GetSetAsync(string key, object value)
        {
            var args = this.SerializeRedisValueInternal(value);
            return ExecuteScalarAsync(key, (c, k) => c.Value.GetSetAsync(k, args));
        }
        /// <summary>
        /// 将给定 key 的值设为 value ，并返回 key 的旧值(old value)
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public async Task<T> GetSetAsync<T>(string key, object value)
        {
            var args = this.SerializeRedisValueInternal(value);
            return this.DeserializeRedisValueInternal<T>(await ExecuteScalarAsync(key, (c, k) => c.Value.GetSetBytesAsync(k, args)));
        }
        /// <summary>
        /// 将 key 所储存的值加上给定的增量值（increment）
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">增量值(默认=1)</param>
        /// <returns></returns>
        public Task<long> IncrByAsync(string key, long value = 1) => ExecuteScalarAsync(key, (c, k) => c.Value.IncrByAsync(k, value));
        /// <summary>
        /// 将 key 所储存的值加上给定的浮点增量值（increment）
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">增量值(默认=1)</param>
        /// <returns></returns>
        public Task<decimal> IncrByFloatAsync(string key, decimal value) => ExecuteScalarAsync(key, (c, k) => c.Value.IncrByFloatAsync(k, value));
        /// <summary>
        /// 获取多个指定 key 的值(数组)
        /// </summary>
        /// <param name="keys">不含prefix前辍</param>
        /// <returns></returns>
        public Task<string[]> MGetAsync(params string[] keys) => ExecuteArrayAsync(keys, (c, k) => c.Value.MGetAsync(k));
        /// <summary>
        /// 获取多个指定 key 的值(数组)
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="keys">不含prefix前辍</param>
        /// <returns></returns>
        public async Task<T[]> MGetAsync<T>(params string[] keys) => this.DeserializeRedisValueArrayInternal<T>(await ExecuteArrayAsync(keys, (c, k) => c.Value.MGetBytesAsync(k)));
        /// <summary>
        /// 同时设置一个或多个 key-value 对
        /// </summary>
        /// <param name="keyValues">key1 value1 [key2 value2]</param>
        /// <returns></returns>
        public Task<bool> MSetAsync(params object[] keyValues) => MSetInternalAsync(RedisExistence.Xx, keyValues);
        /// <summary>
        /// 同时设置一个或多个 key-value 对，当且仅当所有给定 key 都不存在
        /// </summary>
        /// <param name="keyValues">key1 value1 [key2 value2]</param>
        /// <returns></returns>
        public Task<bool> MSetNxAsync(params object[] keyValues) => MSetInternalAsync(RedisExistence.Nx, keyValues);
        async internal Task<bool> MSetInternalAsync(RedisExistence exists, params object[] keyValues)
        {
            if (keyValues == null || keyValues.Any() == false) return false;
            if (keyValues.Length % 2 != 0) throw new Exception("keyValues 参数是键值对，不应该出现奇数(数量)，请检查使用姿势。");
            var dic = new Dictionary<string, object>();
            for (var a = 0; a < keyValues.Length; a += 2)
            {
                var k = string.Concat(keyValues[a]);
                var v = this.SerializeRedisValueInternal(keyValues[a + 1]);
                if (string.IsNullOrEmpty(k)) throw new Exception("keyValues 参数是键值对，并且 key 不可为空");
                if (dic.ContainsKey(k)) dic[k] = v;
                else dic.Add(k, v);
            }
            Func<Object<RedisClient>, string[], Task<long>> handle = async (c, k) =>
            {
                var prefix = (c.Pool as RedisClientPool)?.Prefix;
                var parms = new object[k.Length * 2];
                for (var a = 0; a < k.Length; a++)
                {
                    parms[a * 2] = k[a];
                    parms[a * 2 + 1] = dic[string.IsNullOrEmpty(prefix) ? k[a] : k[a].Substring(prefix.Length)];
                }
                if (exists == RedisExistence.Nx) return await c.Value.MSetNxAsync(parms) ? 1 : 0;
                return await c.Value.MSetAsync(parms) == "OK" ? 1 : 0;
            };
            if (exists == RedisExistence.Nx) return await NodesNotSupportAsync(dic.Keys.ToArray(), 0, handle) > 0;
            return await ExecuteNonQueryAsync(dic.Keys.ToArray(), handle) > 0;
        }
        /// <summary>
        /// 设置指定 key 的值，所有写入参数object都支持string | byte[] | 数值 | 对象
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">值</param>
        /// <param name="expireSeconds">过期(秒单位)</param>
        /// <param name="exists">Nx, Xx</param>
        /// <returns></returns>
        public async Task<bool> SetAsync(string key, object value, int expireSeconds = -1, RedisExistence? exists = null)
        {
            object redisValule = this.SerializeRedisValueInternal(value);
            if (expireSeconds <= 0 && exists == null) return await ExecuteScalarAsync(key, (c, k) => c.Value.SetAsync(k, redisValule)) == "OK";
            if (expireSeconds <= 0 && exists != null) return await ExecuteScalarAsync(key, (c, k) => c.Value.SetAsync(k, redisValule, null, exists)) == "OK";
            if (expireSeconds > 0 && exists == null) return await ExecuteScalarAsync(key, (c, k) => c.Value.SetAsync(k, redisValule, expireSeconds, null)) == "OK";
            if (expireSeconds > 0 && exists != null) return await ExecuteScalarAsync(key, (c, k) => c.Value.SetAsync(k, redisValule, expireSeconds, exists)) == "OK";
            return false;
        }
        public async Task<bool> SetAsync(string key, object value, TimeSpan expire, RedisExistence? exists = null)
        {
            object redisValule = this.SerializeRedisValueInternal(value);
            if (expire <= TimeSpan.Zero && exists == null) return await ExecuteScalar(key, (c, k) => c.Value.SetAsync(k, redisValule)) == "OK";
            if (expire <= TimeSpan.Zero && exists != null) return await ExecuteScalar(key, (c, k) => c.Value.SetAsync(k, redisValule, null, exists)) == "OK";
            if (expire > TimeSpan.Zero && exists == null) return await ExecuteScalar(key, (c, k) => c.Value.SetAsync(k, redisValule, expire, null)) == "OK";
            if (expire > TimeSpan.Zero && exists != null) return await ExecuteScalar(key, (c, k) => c.Value.SetAsync(k, redisValule, expire, exists)) == "OK";
            return false;
        }
        /// <summary>
        /// 对 key 所储存的字符串值，设置或清除指定偏移量上的位(bit)
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="offset">偏移量</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public Task<bool> SetBitAsync(string key, uint offset, bool value) => ExecuteScalarAsync(key, (c, k) => c.Value.SetBitAsync(k, offset, value));
        /// <summary>
        /// 只有在 key 不存在时设置 key 的值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public Task<bool> SetNxAsync(string key, object value)
        {
            var args = this.SerializeRedisValueInternal(value);
            return ExecuteScalarAsync(key, (c, k) => c.Value.SetNxAsync(k, args));
        }
        /// <summary>
        /// 用 value 参数覆写给定 key 所储存的字符串值，从偏移量 offset 开始
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="offset">偏移量</param>
        /// <param name="value">值</param>
        /// <returns>被修改后的字符串长度</returns>
        public Task<long> SetRangeAsync(string key, uint offset, object value)
        {
            var args = this.SerializeRedisValueInternal(value);
            return ExecuteScalarAsync(key, (c, k) => c.Value.SetRangeAsync(k, offset, args));
        }
        /// <summary>
        /// 返回 key 所储存的字符串值的长度
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public Task<long> StrLenAsync(string key) => ExecuteScalarAsync(key, (c, k) => c.Value.StrLenAsync(k));
        #endregion



#endif

    }
}