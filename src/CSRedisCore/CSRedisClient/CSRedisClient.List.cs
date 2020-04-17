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
        #region List
        /// <summary>
        /// 它是 LPOP 命令的阻塞版本，当给定列表内没有任何元素可供弹出的时候，连接将被 BLPOP 命令阻塞，直到等待超时或发现可弹出元素为止，超时返回null
        /// </summary>
        /// <param name="timeout">超时(秒)</param>
        /// <param name="keys">一个或多个列表，不含prefix前辍</param>
        /// <returns></returns>
        public (string key, string value)? BLPopWithKey(int timeout, params string[] keys)
        {
            string[] rkeys = null;
            var tuple = NodesNotSupport(keys, null, (c, k) => c.Value.BLPopWithKey(timeout, rkeys = k));
            if (tuple == null) return null;
            return (rkeys?.Where(b => b == tuple.Item1).First() ?? tuple.Item1, tuple.Item2);
        }
        /// <summary>
        /// 它是 LPOP 命令的阻塞版本，当给定列表内没有任何元素可供弹出的时候，连接将被 BLPOP 命令阻塞，直到等待超时或发现可弹出元素为止，超时返回null
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="timeout">超时(秒)</param>
        /// <param name="keys">一个或多个列表，不含prefix前辍</param>
        /// <returns></returns>
        public (string key, T value)? BLPopWithKey<T>(int timeout, params string[] keys)
        {
            string[] rkeys = null;
            var tuple = NodesNotSupport(keys, null, (c, k) => c.Value.BLPopBytesWithKey(timeout, rkeys = k));
            if (tuple == null) return null;
            return (rkeys?.Where(b => b == tuple.Item1).First() ?? tuple.Item1, this.DeserializeRedisValueInternal<T>(tuple.Item2));
        }
        /// <summary>
        /// 它是 LPOP 命令的阻塞版本，当给定列表内没有任何元素可供弹出的时候，连接将被 BLPOP 命令阻塞，直到等待超时或发现可弹出元素为止，超时返回null
        /// </summary>
        /// <param name="timeout">超时(秒)</param>
        /// <param name="keys">一个或多个列表，不含prefix前辍</param>
        /// <returns></returns>
        public string BLPop(int timeout, params string[] keys) => NodesNotSupport(keys, null, (c, k) => c.Value.BLPop(timeout, k));
        /// <summary>
        /// 它是 LPOP 命令的阻塞版本，当给定列表内没有任何元素可供弹出的时候，连接将被 BLPOP 命令阻塞，直到等待超时或发现可弹出元素为止，超时返回null
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="timeout">超时(秒)</param>
        /// <param name="keys">一个或多个列表，不含prefix前辍</param>
        /// <returns></returns>
        public T BLPop<T>(int timeout, params string[] keys) => this.DeserializeRedisValueInternal<T>(NodesNotSupport(keys, null, (c, k) => c.Value.BLPopBytes(timeout, k)));
        /// <summary>
        /// 它是 RPOP 命令的阻塞版本，当给定列表内没有任何元素可供弹出的时候，连接将被 BRPOP 命令阻塞，直到等待超时或发现可弹出元素为止，超时返回null
        /// </summary>
        /// <param name="timeout">超时(秒)</param>
        /// <param name="keys">一个或多个列表，不含prefix前辍</param>
        /// <returns></returns>
        public (string key, string value)? BRPopWithKey(int timeout, params string[] keys)
        {
            string[] rkeys = null;
            var tuple = NodesNotSupport(keys, null, (c, k) => c.Value.BRPopWithKey(timeout, rkeys = k));
            if (tuple == null) return null;
            return (rkeys?.Where(b => b == tuple.Item1).First() ?? tuple.Item1, tuple.Item2);
        }
        /// <summary>
        /// 它是 RPOP 命令的阻塞版本，当给定列表内没有任何元素可供弹出的时候，连接将被 BRPOP 命令阻塞，直到等待超时或发现可弹出元素为止，超时返回null
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="timeout">超时(秒)</param>
        /// <param name="keys">一个或多个列表，不含prefix前辍</param>
        /// <returns></returns>
        public (string key, T value)? BRPopWithKey<T>(int timeout, params string[] keys)
        {
            string[] rkeys = null;
            var tuple = NodesNotSupport(keys, null, (c, k) => c.Value.BRPopBytesWithKey(timeout, rkeys = k));
            if (tuple == null) return null;
            return (rkeys?.Where(b => b == tuple.Item1).First() ?? tuple.Item1, this.DeserializeRedisValueInternal<T>(tuple.Item2));
        }
        /// <summary>
        /// 它是 RPOP 命令的阻塞版本，当给定列表内没有任何元素可供弹出的时候，连接将被 BRPOP 命令阻塞，直到等待超时或发现可弹出元素为止，超时返回null
        /// </summary>
        /// <param name="timeout">超时(秒)</param>
        /// <param name="keys">一个或多个列表，不含prefix前辍</param>
        /// <returns></returns>
        public string BRPop(int timeout, params string[] keys) => NodesNotSupport(keys, null, (c, k) => c.Value.BRPop(timeout, k));
        /// <summary>
        /// 它是 RPOP 命令的阻塞版本，当给定列表内没有任何元素可供弹出的时候，连接将被 BRPOP 命令阻塞，直到等待超时或发现可弹出元素为止，超时返回null
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="timeout">超时(秒)</param>
        /// <param name="keys">一个或多个列表，不含prefix前辍</param>
        /// <returns></returns>
        public T BRPop<T>(int timeout, params string[] keys) => this.DeserializeRedisValueInternal<T>(NodesNotSupport(keys, null, (c, k) => c.Value.BRPopBytes(timeout, k)));
        /// <summary>
        /// BRPOPLPUSH 是 RPOPLPUSH 的阻塞版本，当给定列表 source 不为空时， BRPOPLPUSH 的表现和 RPOPLPUSH 一样。
        /// 当列表 source 为空时， BRPOPLPUSH 命令将阻塞连接，直到等待超时，或有另一个客户端对 source 执行 LPUSH 或 RPUSH 命令为止。
        /// </summary>
        /// <param name="source">源key，不含prefix前辍</param>
        /// <param name="destination">目标key，不含prefix前辍</param>
        /// <param name="timeout">超时(秒)</param>
        /// <returns></returns>
        public string BRPopLPush(string source, string destination, int timeout) => NodesNotSupport(new[] { source, destination }, null, (c, k) => c.Value.BRPopLPush(k.First(), k.Last(), timeout));
        /// <summary>
        /// BRPOPLPUSH 是 RPOPLPUSH 的阻塞版本，当给定列表 source 不为空时， BRPOPLPUSH 的表现和 RPOPLPUSH 一样。
        /// 当列表 source 为空时， BRPOPLPUSH 命令将阻塞连接，直到等待超时，或有另一个客户端对 source 执行 LPUSH 或 RPUSH 命令为止。
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="source">源key，不含prefix前辍</param>
        /// <param name="destination">目标key，不含prefix前辍</param>
        /// <param name="timeout">超时(秒)</param>
        /// <returns></returns>
        public T BRPopLPush<T>(string source, string destination, int timeout) => this.DeserializeRedisValueInternal<T>(NodesNotSupport(new[] { source, destination }, null, (c, k) => c.Value.BRPopBytesLPush(k.First(), k.Last(), timeout)));
        /// <summary>
        /// 通过索引获取列表中的元素
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public string LIndex(string key, long index) => ExecuteScalar(key, (c, k) => c.Value.LIndex(k, index));
        /// <summary>
        /// 通过索引获取列表中的元素
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public T LIndex<T>(string key, long index) => this.DeserializeRedisValueInternal<T>(ExecuteScalar(key, (c, k) => c.Value.LIndexBytes(k, index)));
        /// <summary>
        /// 在列表中的元素前面插入元素
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="pivot">列表的元素</param>
        /// <param name="value">新元素</param>
        /// <returns></returns>
        public long LInsertBefore(string key, object pivot, object value)
        {
            var args = this.SerializeRedisValueInternal(value);
            return ExecuteScalar(key, (c, k) => c.Value.LInsert(k, RedisInsert.Before, pivot, args));
        }
        /// <summary>
        /// 在列表中的元素后面插入元素
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="pivot">列表的元素</param>
        /// <param name="value">新元素</param>
        /// <returns></returns>
        public long LInsertAfter(string key, object pivot, object value)
        {
            var args = this.SerializeRedisValueInternal(value);
            return ExecuteScalar(key, (c, k) => c.Value.LInsert(k, RedisInsert.After, pivot, args));
        }
        /// <summary>
        /// 获取列表长度
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public long LLen(string key) => ExecuteScalar(key, (c, k) => c.Value.LLen(k));
        /// <summary>
        /// 移出并获取列表的第一个元素
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public string LPop(string key) => ExecuteScalar(key, (c, k) => c.Value.LPop(k));
        /// <summary>
        /// 移出并获取列表的第一个元素
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public T LPop<T>(string key) => this.DeserializeRedisValueInternal<T>(ExecuteScalar(key, (c, k) => c.Value.LPopBytes(k)));
        /// <summary>
        /// 将一个或多个值插入到列表头部
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">一个或多个值</param>
        /// <returns>执行 LPUSH 命令后，列表的长度</returns>
        public long LPush<T>(string key, params T[] value)
        {
            if (value == null || value.Any() == false) return 0;
            var args = value.Select(z => this.SerializeRedisValueInternal(z)).ToArray();
            return ExecuteScalar(key, (c, k) => c.Value.LPush(k, args));
        }
        /// <summary>
        /// 将一个值插入到已存在的列表头部
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">值</param>
        /// <returns>执行 LPUSHX 命令后，列表的长度。</returns>
        public long LPushX(string key, object value)
        {
            var args = this.SerializeRedisValueInternal(value);
            return ExecuteScalar(key, (c, k) => c.Value.LPushX(k, args));
        }
        /// <summary>
        /// 获取列表指定范围内的元素
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        public string[] LRange(string key, long start, long stop) => ExecuteScalar(key, (c, k) => c.Value.LRange(k, start, stop));
        /// <summary>
        /// 获取列表指定范围内的元素
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        public T[] LRange<T>(string key, long start, long stop) => this.DeserializeRedisValueArrayInternal<T>(ExecuteScalar(key, (c, k) => c.Value.LRangeBytes(k, start, stop)));
        /// <summary>
        /// 根据参数 count 的值，移除列表中与参数 value 相等的元素
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="count">移除的数量，大于0时从表头删除数量count，小于0时从表尾删除数量-count，等于0移除所有</param>
        /// <param name="value">元素</param>
        /// <returns></returns>
        public long LRem(string key, long count, object value)
        {
            var args = this.SerializeRedisValueInternal(value);
            return ExecuteScalar(key, (c, k) => c.Value.LRem(k, count, args));
        }
        /// <summary>
        /// 通过索引设置列表元素的值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="index">索引</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool LSet(string key, long index, object value)
        {
            var args = this.SerializeRedisValueInternal(value);
            return ExecuteScalar(key, (c, k) => c.Value.LSet(k, index, args)) == "OK";
        }
        /// <summary>
        /// 对一个列表进行修剪，让列表只保留指定区间内的元素，不在指定区间之内的元素都将被删除
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        public bool LTrim(string key, long start, long stop) => ExecuteScalar(key, (c, k) => c.Value.LTrim(k, start, stop)) == "OK";
        /// <summary>
        /// 移除并获取列表最后一个元素
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public string RPop(string key) => ExecuteScalar(key, (c, k) => c.Value.RPop(k));
        /// <summary>
        /// 移除并获取列表最后一个元素
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public T RPop<T>(string key) => this.DeserializeRedisValueInternal<T>(ExecuteScalar(key, (c, k) => c.Value.RPopBytes(k)));
        /// <summary>
        /// 将列表 source 中的最后一个元素(尾元素)弹出，并返回给客户端。
        /// 将 source 弹出的元素插入到列表 destination ，作为 destination 列表的的头元素。
        /// </summary>
        /// <param name="source">源key，不含prefix前辍</param>
        /// <param name="destination">目标key，不含prefix前辍</param>
        /// <returns></returns>
        public string RPopLPush(string source, string destination) => NodesNotSupport(new[] { source, destination }, null, (c, k) => c.Value.RPopLPush(k.First(), k.Last()));
        /// <summary>
        /// 将列表 source 中的最后一个元素(尾元素)弹出，并返回给客户端。
        /// 将 source 弹出的元素插入到列表 destination ，作为 destination 列表的的头元素。
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="source">源key，不含prefix前辍</param>
        /// <param name="destination">目标key，不含prefix前辍</param>
        /// <returns></returns>
        public T RPopLPush<T>(string source, string destination) => this.DeserializeRedisValueInternal<T>(NodesNotSupport(new[] { source, destination }, null, (c, k) => c.Value.RPopBytesLPush(k.First(), k.Last())));
        /// <summary>
        /// 在列表中添加一个或多个值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">一个或多个值</param>
        /// <returns>执行 RPUSH 命令后，列表的长度</returns>
        public long RPush<T>(string key, params T[] value)
        {
            if (value == null || value.Any() == false) return 0;
            var args = value.Select(z => this.SerializeRedisValueInternal(z)).ToArray();
            return ExecuteScalar(key, (c, k) => c.Value.RPush(k, args));
        }
        /// <summary>
        /// 为已存在的列表添加值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">一个或多个值</param>
        /// <returns>执行 RPUSHX 命令后，列表的长度</returns>
        public long RPushX(string key, object value)
        {
            var args = this.SerializeRedisValueInternal(value);
            return ExecuteScalar(key, (c, k) => c.Value.RPushX(k, args));
        }
        #endregion


#if !net40

        #region List
        /// <summary>
        /// 通过索引获取列表中的元素
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public Task<string> LIndexAsync(string key, long index) => ExecuteScalarAsync(key, (c, k) => c.Value.LIndexAsync(k, index));
        /// <summary>
        /// 通过索引获取列表中的元素
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        async public Task<T> LIndexAsync<T>(string key, long index) => this.DeserializeRedisValueInternal<T>(await ExecuteScalarAsync(key, (c, k) => c.Value.LIndexBytesAsync(k, index)));
        /// <summary>
        /// 在列表中的元素前面插入元素
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="pivot">列表的元素</param>
        /// <param name="value">新元素</param>
        /// <returns></returns>
        public Task<long> LInsertBeforeAsync(string key, object pivot, object value)
        {
            var args = this.SerializeRedisValueInternal(value);
            return ExecuteScalarAsync(key, (c, k) => c.Value.LInsertAsync(k, RedisInsert.Before, pivot, args));
        }
        /// <summary>
        /// 在列表中的元素后面插入元素
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="pivot">列表的元素</param>
        /// <param name="value">新元素</param>
        /// <returns></returns>
        public Task<long> LInsertAfterAsync(string key, object pivot, object value)
        {
            var args = this.SerializeRedisValueInternal(value);
            return ExecuteScalarAsync(key, (c, k) => c.Value.LInsertAsync(k, RedisInsert.After, pivot, args));
        }
        /// <summary>
        /// 获取列表长度
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public Task<long> LLenAsync(string key) => ExecuteScalarAsync(key, (c, k) => c.Value.LLenAsync(k));
        /// <summary>
        /// 移出并获取列表的第一个元素
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public Task<string> LPopAsync(string key) => ExecuteScalarAsync(key, (c, k) => c.Value.LPopAsync(k));
        /// <summary>
        /// 移出并获取列表的第一个元素
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        async public Task<T> LPopAsync<T>(string key) => this.DeserializeRedisValueInternal<T>(await ExecuteScalarAsync(key, (c, k) => c.Value.LPopBytesAsync(k)));
        /// <summary>
        /// 将一个或多个值插入到列表头部
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">一个或多个值</param>
        /// <returns>执行 LPUSH 命令后，列表的长度</returns>
        async public Task<long> LPushAsync<T>(string key, params T[] value)
        {
            if (value == null || value.Any() == false) return 0;
            var args = value.Select(z => this.SerializeRedisValueInternal(z)).ToArray();
            return await ExecuteScalarAsync(key, (c, k) => c.Value.LPushAsync(k, args));
        }
        /// <summary>
        /// 将一个值插入到已存在的列表头部
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">值</param>
        /// <returns>执行 LPUSHX 命令后，列表的长度。</returns>
        public Task<long> LPushXAsync(string key, object value)
        {
            var args = this.SerializeRedisValueInternal(value);
            return ExecuteScalarAsync(key, (c, k) => c.Value.LPushXAsync(k, args));
        }
        /// <summary>
        /// 获取列表指定范围内的元素
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        public Task<string[]> LRangeAsync(string key, long start, long stop) => ExecuteScalarAsync(key, (c, k) => c.Value.LRangeAsync(k, start, stop));
        /// <summary>
        /// 获取列表指定范围内的元素
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        async public Task<T[]> LRangeAsync<T>(string key, long start, long stop) => this.DeserializeRedisValueArrayInternal<T>(await ExecuteScalarAsync(key, (c, k) => c.Value.LRangeBytesAsync(k, start, stop)));
        /// <summary>
        /// 根据参数 count 的值，移除列表中与参数 value 相等的元素
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="count">移除的数量，大于0时从表头删除数量count，小于0时从表尾删除数量-count，等于0移除所有</param>
        /// <param name="value">元素</param>
        /// <returns></returns>
        public Task<long> LRemAsync(string key, long count, object value)
        {
            var args = this.SerializeRedisValueInternal(value);
            return ExecuteScalarAsync(key, (c, k) => c.Value.LRemAsync(k, count, args));
        }
        /// <summary>
        /// 通过索引设置列表元素的值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="index">索引</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        async public Task<bool> LSetAsync(string key, long index, object value)
        {
            var args = this.SerializeRedisValueInternal(value);
            return await ExecuteScalar(key, (c, k) => c.Value.LSetAsync(k, index, args)) == "OK";
        }
        /// <summary>
        /// 对一个列表进行修剪，让列表只保留指定区间内的元素，不在指定区间之内的元素都将被删除
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        public Task<bool> LTrimAsync(string key, long start, long stop) => ExecuteScalarAsync(key, async (c, k) => await c.Value.LTrimAsync(k, start, stop) == "OK");
        /// <summary>
        /// 移除并获取列表最后一个元素
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public Task<string> RPopAsync(string key) => ExecuteScalarAsync(key, (c, k) => c.Value.RPopAsync(k));
        /// <summary>
        /// 移除并获取列表最后一个元素
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        async public Task<T> RPopAsync<T>(string key) => this.DeserializeRedisValueInternal<T>(await ExecuteScalarAsync(key, (c, k) => c.Value.RPopBytesAsync(k)));
        /// <summary>
        /// 将列表 source 中的最后一个元素(尾元素)弹出，并返回给客户端。
        /// 将 source 弹出的元素插入到列表 destination ，作为 destination 列表的的头元素。
        /// </summary>
        /// <param name="source">源key，不含prefix前辍</param>
        /// <param name="destination">目标key，不含prefix前辍</param>
        /// <returns></returns>
        public Task<string> RPopLPushAsync(string source, string destination) => NodesNotSupportAsync(new[] { source, destination }, null, (c, k) => c.Value.RPopLPushAsync(k.First(), k.Last()));
        /// <summary>
        /// 将列表 source 中的最后一个元素(尾元素)弹出，并返回给客户端。
        /// 将 source 弹出的元素插入到列表 destination ，作为 destination 列表的的头元素。
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="source">源key，不含prefix前辍</param>
        /// <param name="destination">目标key，不含prefix前辍</param>
        /// <returns></returns>
        async public Task<T> RPopLPushAsync<T>(string source, string destination) => this.DeserializeRedisValueInternal<T>(await NodesNotSupportAsync(new[] { source, destination }, null, (c, k) => c.Value.RPopBytesLPushAsync(k.First(), k.Last())));
        /// <summary>
        /// 在列表中添加一个或多个值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">一个或多个值</param>
        /// <returns>执行 RPUSH 命令后，列表的长度</returns>
        async public Task<long> RPushAsync<T>(string key, params T[] value)
        {
            if (value == null || value.Any() == false) return 0;
            var args = value.Select(z => this.SerializeRedisValueInternal(z)).ToArray();
            return await ExecuteScalarAsync(key, (c, k) => c.Value.RPushAsync(k, args));
        }
        /// <summary>
        /// 为已存在的列表添加值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">一个或多个值</param>
        /// <returns>执行 RPUSHX 命令后，列表的长度</returns>
        public Task<long> RPushXAsync(string key, object value)
        {
            var args = this.SerializeRedisValueInternal(value);
            return ExecuteScalar(key, (c, k) => c.Value.RPushXAsync(k, args));
        }
        #endregion


#endif

    }
}