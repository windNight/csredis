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
        #region Sorted Set
        /// <summary>
        /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最高得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。 当返回多个元素时候，得分最高的元素将是第一个元素，然后是分数较低的元素。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public (string member, decimal score)[] ZPopMax(string key, long count) => ExecuteScalar(key, (c, k) => c.Value.ZPopMax(k, count)).Select(a => (a.Item1, a.Item2)).ToArray();
        /// <summary>
        /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最高得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。 当返回多个元素时候，得分最高的元素将是第一个元素，然后是分数较低的元素。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public (T member, decimal score)[] ZPopMax<T>(string key, long count) => this.DeserializeRedisValueTuple1Internal<T, decimal>(ExecuteScalar(key, (c, k) => c.Value.ZPopMaxBytes(k, count)));
        /// <summary>
        /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最低得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。 当返回多个元素时候，得分最低的元素将是第一个元素，然后是分数较高的元素。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public (string member, decimal score)[] ZPopMin(string key, long count) => ExecuteScalar(key, (c, k) => c.Value.ZPopMin(k, count)).Select(a => (a.Item1, a.Item2)).ToArray();
        /// <summary>
        /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最低得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。 当返回多个元素时候，得分最低的元素将是第一个元素，然后是分数较高的元素。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public (T member, decimal score)[] ZPopMin<T>(string key, long count) => this.DeserializeRedisValueTuple1Internal<T, decimal>(ExecuteScalar(key, (c, k) => c.Value.ZPopMinBytes(k, count)));

        /// <summary>
        /// 向有序集合添加一个或多个成员，或者更新已存在成员的分数
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="scoreMembers">一个或多个成员分数</param>
        /// <returns></returns>
        public long ZAdd(string key, params (decimal, object)[] scoreMembers)
        {
            if (scoreMembers == null || scoreMembers.Any() == false) return 0;
            var args = scoreMembers.Select(a => new Tuple<decimal, object>(a.Item1, this.SerializeRedisValueInternal(a.Item2))).ToArray();
            return ExecuteScalar(key, (c, k) => c.Value.ZAdd(k, args));
        }
        /// <summary>
        /// 获取有序集合的成员数量
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public long ZCard(string key) => ExecuteScalar(key, (c, k) => c.Value.ZCard(k));
        /// <summary>
        /// 计算在有序集合中指定区间分数的成员数量
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">分数最小值 decimal.MinValue 1</param>
        /// <param name="max">分数最大值 decimal.MaxValue 10</param>
        /// <returns></returns>
        public long ZCount(string key, decimal min, decimal max) => ExecuteScalar(key, (c, k) => c.Value.ZCount(k, min == decimal.MinValue ? "-inf" : min.ToString(), max == decimal.MaxValue ? "+inf" : max.ToString()));
        /// <summary>
        /// 计算在有序集合中指定区间分数的成员数量
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">分数最小值 -inf (1 1</param>
        /// <param name="max">分数最大值 +inf (10 10</param>
        /// <returns></returns>
        public long ZCount(string key, string min, string max) => ExecuteScalar(key, (c, k) => c.Value.ZCount(k, min, max));
        /// <summary>
        /// 有序集合中对指定成员的分数加上增量 increment
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member">成员</param>
        /// <param name="increment">增量值(默认=1)</param>
        /// <returns></returns>
        public decimal ZIncrBy(string key, object member, decimal increment = 1)
        {
            var args = this.SerializeRedisValueInternal(member);
            return ExecuteScalar(key, (c, k) => c.Value.ZIncrBy(k, increment, args));
        }

        /// <summary>
        /// 计算给定的一个或多个有序集的交集，将结果集存储在新的有序集合 destination 中
        /// </summary>
        /// <param name="destination">新的有序集合，不含prefix前辍</param>
        /// <param name="weights">使用 WEIGHTS 选项，你可以为 每个 给定有序集 分别 指定一个乘法因子。如果没有指定 WEIGHTS 选项，乘法因子默认设置为 1 。</param>
        /// <param name="aggregate">Sum | Min | Max</param>
        /// <param name="keys">一个或多个有序集合，不含prefix前辍</param>
        /// <returns></returns>
        public long ZInterStore(string destination, decimal[] weights, RedisAggregate aggregate, params string[] keys)
        {
            if (keys == null || keys.Length == 0) throw new Exception("keys 参数不可为空");
            if (weights != null && weights.Length != keys.Length) throw new Exception("weights 和 keys 参数长度必须相同");
            return NodesNotSupport(new[] { destination }.Concat(keys).ToArray(), 0, (c, k) => c.Value.ZInterStore(k.First(), weights, aggregate, k.Skip(1).ToArray()));
        }

        /// <summary>
        /// 通过索引区间返回有序集合成指定区间内的成员
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        public string[] ZRange(string key, long start, long stop) => ExecuteScalar(key, (c, k) => c.Value.ZRange(k, start, stop, false));
        /// <summary>
        /// 通过索引区间返回有序集合成指定区间内的成员
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        public T[] ZRange<T>(string key, long start, long stop) => this.DeserializeRedisValueArrayInternal<T>(ExecuteScalar(key, (c, k) => c.Value.ZRangeBytes(k, start, stop, false)));
        /// <summary>
        /// 通过索引区间返回有序集合成指定区间内的成员和分数
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        public (string member, decimal score)[] ZRangeWithScores(string key, long start, long stop) => ExecuteScalar(key, (c, k) => c.Value.ZRangeWithScores(k, start, stop)).Select(a => (a.Item1, a.Item2)).ToArray();
        /// <summary>
        /// 通过索引区间返回有序集合成指定区间内的成员和分数
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        public (T member, decimal score)[] ZRangeWithScores<T>(string key, long start, long stop) => this.DeserializeRedisValueTuple1Internal<T, decimal>(ExecuteScalar(key, (c, k) => c.Value.ZRangeBytesWithScores(k, start, stop)));

        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">分数最小值 decimal.MinValue 1</param>
        /// <param name="max">分数最大值 decimal.MaxValue 10</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        public string[] ZRangeByScore(string key, decimal min, decimal max, long? count = null, long offset = 0) =>
            ExecuteScalar(key, (c, k) => c.Value.ZRangeByScore(k, min == decimal.MinValue ? "-inf" : min.ToString(), max == decimal.MaxValue ? "+inf" : max.ToString(), false, offset, count));
        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">分数最小值 decimal.MinValue 1</param>
        /// <param name="max">分数最大值 decimal.MaxValue 10</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        public T[] ZRangeByScore<T>(string key, decimal min, decimal max, long? count = null, long offset = 0) =>
            this.DeserializeRedisValueArrayInternal<T>(ExecuteScalar(key, (c, k) => c.Value.ZRangeBytesByScore(k, min == decimal.MinValue ? "-inf" : min.ToString(), max == decimal.MaxValue ? "+inf" : max.ToString(), false, offset, count)));
        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">分数最小值 -inf (1 1</param>
        /// <param name="max">分数最大值 +inf (10 10</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        public string[] ZRangeByScore(string key, string min, string max, long? count = null, long offset = 0) =>
            ExecuteScalar(key, (c, k) => c.Value.ZRangeByScore(k, min, max, false, offset, count));
        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">分数最小值 -inf (1 1</param>
        /// <param name="max">分数最大值 +inf (10 10</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        public T[] ZRangeByScore<T>(string key, string min, string max, long? count = null, long offset = 0) =>
            this.DeserializeRedisValueArrayInternal<T>(ExecuteScalar(key, (c, k) => c.Value.ZRangeBytesByScore(k, min, max, false, offset, count)));

        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员和分数
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">分数最小值 decimal.MinValue 1</param>
        /// <param name="max">分数最大值 decimal.MaxValue 10</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        public (string member, decimal score)[] ZRangeByScoreWithScores(string key, decimal min, decimal max, long? count = null, long offset = 0) =>
            ExecuteScalar(key, (c, k) => c.Value.ZRangeByScoreWithScores(k, min == decimal.MinValue ? "-inf" : min.ToString(), max == decimal.MaxValue ? "+inf" : max.ToString(), offset, count)).Select(z => (z.Item1, z.Item2)).ToArray();
        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员和分数
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">分数最小值 decimal.MinValue 1</param>
        /// <param name="max">分数最大值 decimal.MaxValue 10</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        public (T member, decimal score)[] ZRangeByScoreWithScores<T>(string key, decimal min, decimal max, long? count = null, long offset = 0) =>
            this.DeserializeRedisValueTuple1Internal<T, decimal>(ExecuteScalar(key, (c, k) => c.Value.ZRangeBytesByScoreWithScores(k, min == decimal.MinValue ? "-inf" : min.ToString(), max == decimal.MaxValue ? "+inf" : max.ToString(), offset, count)));
        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员和分数
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">分数最小值 -inf (1 1</param>
        /// <param name="max">分数最大值 +inf (10 10</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        public (string member, decimal score)[] ZRangeByScoreWithScores(string key, string min, string max, long? count = null, long offset = 0) =>
            ExecuteScalar(key, (c, k) => c.Value.ZRangeByScoreWithScores(k, min, max, offset, count)).Select(z => (z.Item1, z.Item2)).ToArray();
        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员和分数
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">分数最小值 -inf (1 1</param>
        /// <param name="max">分数最大值 +inf (10 10</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        public (T member, decimal score)[] ZRangeByScoreWithScores<T>(string key, string min, string max, long? count = null, long offset = 0) =>
            this.DeserializeRedisValueTuple1Internal<T, decimal>(ExecuteScalar(key, (c, k) => c.Value.ZRangeBytesByScoreWithScores(k, min, max, offset, count)));

        /// <summary>
        /// 返回有序集合中指定成员的索引
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member">成员</param>
        /// <returns></returns>
        public long? ZRank(string key, object member)
        {
            var args = this.SerializeRedisValueInternal(member);
            return ExecuteScalar(key, (c, k) => c.Value.ZRank(k, args));
        }
        /// <summary>
        /// 移除有序集合中的一个或多个成员
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member">一个或多个成员</param>
        /// <returns></returns>
        public long ZRem<T>(string key, params T[] member)
        {
            if (member == null || member.Any() == false) return 0;
            var args = member.Select(z => this.SerializeRedisValueInternal(z)).ToArray();
            return ExecuteScalar(key, (c, k) => c.Value.ZRem(k, args));
        }
        /// <summary>
        /// 移除有序集合中给定的排名区间的所有成员
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        public long ZRemRangeByRank(string key, long start, long stop) => ExecuteScalar(key, (c, k) => c.Value.ZRemRangeByRank(k, start, stop));
        /// <summary>
        /// 移除有序集合中给定的分数区间的所有成员
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">分数最小值 decimal.MinValue 1</param>
        /// <param name="max">分数最大值 decimal.MaxValue 10</param>
        /// <returns></returns>
        public long ZRemRangeByScore(string key, decimal min, decimal max) => ExecuteScalar(key, (c, k) => c.Value.ZRemRangeByScore(k, min == decimal.MinValue ? "-inf" : min.ToString(), max == decimal.MaxValue ? "+inf" : max.ToString()));
        /// <summary>
        /// 移除有序集合中给定的分数区间的所有成员
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">分数最小值 -inf (1 1</param>
        /// <param name="max">分数最大值 +inf (10 10</param>
        /// <returns></returns>
        public long ZRemRangeByScore(string key, string min, string max) => ExecuteScalar(key, (c, k) => c.Value.ZRemRangeByScore(k, min, max));

        /// <summary>
        /// 返回有序集中指定区间内的成员，通过索引，分数从高到底
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        public string[] ZRevRange(string key, long start, long stop) => ExecuteScalar(key, (c, k) => c.Value.ZRevRange(k, start, stop, false));
        /// <summary>
        /// 返回有序集中指定区间内的成员，通过索引，分数从高到底
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        public T[] ZRevRange<T>(string key, long start, long stop) => this.DeserializeRedisValueArrayInternal<T>(ExecuteScalar(key, (c, k) => c.Value.ZRevRangeBytes(k, start, stop, false)));
        /// <summary>
        /// 返回有序集中指定区间内的成员和分数，通过索引，分数从高到底
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        public (string member, decimal score)[] ZRevRangeWithScores(string key, long start, long stop) => ExecuteScalar(key, (c, k) => c.Value.ZRevRangeWithScores(k, start, stop)).Select(a => (a.Item1, a.Item2)).ToArray();
        /// <summary>
        /// 返回有序集中指定区间内的成员和分数，通过索引，分数从高到底
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        public (T member, decimal score)[] ZRevRangeWithScores<T>(string key, long start, long stop) => this.DeserializeRedisValueTuple1Internal<T, decimal>(ExecuteScalar(key, (c, k) => c.Value.ZRevRangeBytesWithScores(k, start, stop)));

        /// <summary>
        /// 返回有序集中指定分数区间内的成员，分数从高到低排序
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="max">分数最大值 decimal.MaxValue 10</param>
        /// <param name="min">分数最小值 decimal.MinValue 1</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        public string[] ZRevRangeByScore(string key, decimal max, decimal min, long? count = null, long? offset = 0) => ExecuteScalar(key, (c, k) => c.Value.ZRevRangeByScore(k, max == decimal.MaxValue ? "+inf" : max.ToString(), min == decimal.MinValue ? "-inf" : min.ToString(), false, offset, count));
        /// <summary>
        /// 返回有序集中指定分数区间内的成员，分数从高到低排序
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="max">分数最大值 decimal.MaxValue 10</param>
        /// <param name="min">分数最小值 decimal.MinValue 1</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        public T[] ZRevRangeByScore<T>(string key, decimal max, decimal min, long? count = null, long offset = 0) =>
            this.DeserializeRedisValueArrayInternal<T>(ExecuteScalar(key, (c, k) => c.Value.ZRevRangeBytesByScore(k, max == decimal.MaxValue ? "+inf" : max.ToString(), min == decimal.MinValue ? "-inf" : min.ToString(), false, offset, count)));
        /// <summary>
        /// 返回有序集中指定分数区间内的成员，分数从高到低排序
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="max">分数最大值 +inf (10 10</param>
        /// <param name="min">分数最小值 -inf (1 1</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        public string[] ZRevRangeByScore(string key, string max, string min, long? count = null, long? offset = 0) => ExecuteScalar(key, (c, k) => c.Value.ZRevRangeByScore(k, max, min, false, offset, count));
        /// <summary>
        /// 返回有序集中指定分数区间内的成员，分数从高到低排序
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="max">分数最大值 +inf (10 10</param>
        /// <param name="min">分数最小值 -inf (1 1</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        public T[] ZRevRangeByScore<T>(string key, string max, string min, long? count = null, long offset = 0) =>
            this.DeserializeRedisValueArrayInternal<T>(ExecuteScalar(key, (c, k) => c.Value.ZRevRangeBytesByScore(k, max, min, false, offset, count)));

        /// <summary>
        /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="max">分数最大值 decimal.MaxValue 10</param>
        /// <param name="min">分数最小值 decimal.MinValue 1</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        public (string member, decimal score)[] ZRevRangeByScoreWithScores(string key, decimal max, decimal min, long? count = null, long offset = 0) =>
            ExecuteScalar(key, (c, k) => c.Value.ZRevRangeByScoreWithScores(k, max == decimal.MaxValue ? "+inf" : max.ToString(), min == decimal.MinValue ? "-inf" : min.ToString(), offset, count)).Select(z => (z.Item1, z.Item2)).ToArray();
        /// <summary>
        /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="max">分数最大值 decimal.MaxValue 10</param>
        /// <param name="min">分数最小值 decimal.MinValue 1</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        public (T member, decimal score)[] ZRevRangeByScoreWithScores<T>(string key, decimal max, decimal min, long? count = null, long offset = 0) =>
            this.DeserializeRedisValueTuple1Internal<T, decimal>(ExecuteScalar(key, (c, k) => c.Value.ZRevRangeBytesByScoreWithScores(k, max == decimal.MaxValue ? "+inf" : max.ToString(), min == decimal.MinValue ? "-inf" : min.ToString(), offset, count)));
        /// <summary>
        /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="max">分数最大值 +inf (10 10</param>
        /// <param name="min">分数最小值 -inf (1 1</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        public (string member, decimal score)[] ZRevRangeByScoreWithScores(string key, string max, string min, long? count = null, long offset = 0) =>
            ExecuteScalar(key, (c, k) => c.Value.ZRevRangeByScoreWithScores(k, max, min, offset, count)).Select(z => (z.Item1, z.Item2)).ToArray();
        /// <summary>
        /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="max">分数最大值 +inf (10 10</param>
        /// <param name="min">分数最小值 -inf (1 1</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        public (T member, decimal score)[] ZRevRangeByScoreWithScores<T>(string key, string max, string min, long? count = null, long offset = 0) =>
            this.DeserializeRedisValueTuple1Internal<T, decimal>(ExecuteScalar(key, (c, k) => c.Value.ZRevRangeBytesByScoreWithScores(k, max, min, offset, count)));

        /// <summary>
        /// 返回有序集合中指定成员的排名，有序集成员按分数值递减(从大到小)排序
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member">成员</param>
        /// <returns></returns>
        public long? ZRevRank(string key, object member)
        {
            var args = this.SerializeRedisValueInternal(member);
            return ExecuteScalar(key, (c, k) => c.Value.ZRevRank(k, args));
        }
        /// <summary>
        /// 返回有序集中，成员的分数值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member">成员</param>
        /// <returns></returns>
        public decimal? ZScore(string key, object member)
        {
            var args = this.SerializeRedisValueInternal(member);
            return ExecuteScalar(key, (c, k) => c.Value.ZScore(k, args));
        }

        /// <summary>
        /// 计算给定的一个或多个有序集的并集，将结果集存储在新的有序集合 destination 中
        /// </summary>
        /// <param name="destination">新的有序集合，不含prefix前辍</param>
        /// <param name="weights">使用 WEIGHTS 选项，你可以为 每个 给定有序集 分别 指定一个乘法因子。如果没有指定 WEIGHTS 选项，乘法因子默认设置为 1 。</param>
        /// <param name="aggregate">Sum | Min | Max</param>
        /// <param name="keys">一个或多个有序集合，不含prefix前辍</param>
        /// <returns></returns>
        public long ZUnionStore(string destination, decimal[] weights, RedisAggregate aggregate, params string[] keys)
        {
            if (keys == null || keys.Length == 0) throw new Exception("keys 参数不可为空");
            if (weights != null && weights.Length != keys.Length) throw new Exception("weights 和 keys 参数长度必须相同");
            return NodesNotSupport(new[] { destination }.Concat(keys).ToArray(), 0, (c, k) => c.Value.ZUnionStore(k.First(), weights, aggregate, k.Skip(1).ToArray()));
        }

        /// <summary>
        /// 迭代有序集合中的元素
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="cursor">位置</param>
        /// <param name="pattern">模式</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public RedisScan<(string member, decimal score)> ZScan(string key, long cursor, string pattern = null, long? count = null)
        {
            var scan = ExecuteScalar(key, (c, k) => c.Value.ZScan(k, cursor, pattern, count));
            return new RedisScan<(string, decimal)>(scan.Cursor, scan.Items.Select(z => (z.Item1, z.Item2)).ToArray());
        }
        /// <summary>
        /// 迭代有序集合中的元素
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="cursor">位置</param>
        /// <param name="pattern">模式</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public RedisScan<(T member, decimal score)> ZScan<T>(string key, long cursor, string pattern = null, long? count = null)
        {
            var scan = ExecuteScalar(key, (c, k) => c.Value.ZScanBytes(k, cursor, pattern, count));
            return new RedisScan<(T, decimal)>(scan.Cursor, this.DeserializeRedisValueTuple1Internal<T, decimal>(scan.Items));
        }

        /// <summary>
        /// 当有序集合的所有成员都具有相同的分值时，有序集合的元素会根据成员的字典序来进行排序，这个命令可以返回给定的有序集合键 key 中，值介于 min 和 max 之间的成员。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
        /// <param name="max">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        public string[] ZRangeByLex(string key, string min, string max, long? count = null, long offset = 0) =>
            ExecuteScalar(key, (c, k) => c.Value.ZRangeByLex(k, min, max, offset, count));
        /// <summary>
        /// 当有序集合的所有成员都具有相同的分值时，有序集合的元素会根据成员的字典序来进行排序，这个命令可以返回给定的有序集合键 key 中，值介于 min 和 max 之间的成员。
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
        /// <param name="max">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        public T[] ZRangeByLex<T>(string key, string min, string max, long? count = null, long offset = 0) =>
            this.DeserializeRedisValueArrayInternal<T>(ExecuteScalar(key, (c, k) => c.Value.ZRangeBytesByLex(k, min, max, offset, count)));

        /// <summary>
        /// 当有序集合的所有成员都具有相同的分值时，有序集合的元素会根据成员的字典序来进行排序，这个命令可以返回给定的有序集合键 key 中，值介于 min 和 max 之间的成员。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
        /// <param name="max">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
        /// <returns></returns>
        public long ZRemRangeByLex(string key, string min, string max) =>
            ExecuteScalar(key, (c, k) => c.Value.ZRemRangeByLex(k, min, max));
        /// <summary>
        /// 当有序集合的所有成员都具有相同的分值时，有序集合的元素会根据成员的字典序来进行排序，这个命令可以返回给定的有序集合键 key 中，值介于 min 和 max 之间的成员。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
        /// <param name="max">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
        /// <returns></returns>
        public long ZLexCount(string key, string min, string max) =>
            ExecuteScalar(key, (c, k) => c.Value.ZLexCount(k, min, max));
        #endregion


#if !net40

        #region Sorted Set
        /// <summary>
        /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最高得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。 当返回多个元素时候，得分最高的元素将是第一个元素，然后是分数较低的元素。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        async public Task<(string member, decimal score)[]> ZPopMaxAsync(string key, long count) => (await ExecuteScalarAsync(key, (c, k) => c.Value.ZPopMaxAsync(k, count))).Select(a => (a.Item1, a.Item2)).ToArray();
        /// <summary>
        /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最高得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。 当返回多个元素时候，得分最高的元素将是第一个元素，然后是分数较低的元素。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        async public Task<(T member, decimal score)[]> ZPopMaxAsync<T>(string key, long count) => this.DeserializeRedisValueTuple1Internal<T, decimal>(await ExecuteScalarAsync(key, (c, k) => c.Value.ZPopMaxBytesAsync(k, count)));
        /// <summary>
        /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最低得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。 当返回多个元素时候，得分最低的元素将是第一个元素，然后是分数较高的元素。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        async public Task<(string member, decimal score)[]> ZPopMinAsync(string key, long count) => (await ExecuteScalarAsync(key, (c, k) => c.Value.ZPopMinAsync(k, count))).Select(a => (a.Item1, a.Item2)).ToArray();
        /// <summary>
        /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最低得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。 当返回多个元素时候，得分最低的元素将是第一个元素，然后是分数较高的元素。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        async public Task<(T member, decimal score)[]> ZPopMinAsync<T>(string key, long count) => this.DeserializeRedisValueTuple1Internal<T, decimal>(await ExecuteScalarAsync(key, (c, k) => c.Value.ZPopMinBytesAsync(k, count)));

        /// <summary>
        /// 向有序集合添加一个或多个成员，或者更新已存在成员的分数
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="scoreMembers">一个或多个成员分数</param>
        /// <returns></returns>
        async public Task<long> ZAddAsync(string key, params (decimal, object)[] scoreMembers)
        {
            if (scoreMembers == null || scoreMembers.Any() == false) return 0;
            var args = scoreMembers.Select(a => new Tuple<decimal, object>(a.Item1, this.SerializeRedisValueInternal(a.Item2))).ToArray();
            return await ExecuteScalarAsync(key, (c, k) => c.Value.ZAddAsync(k, args));
        }
        /// <summary>
        /// 获取有序集合的成员数量
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public Task<long> ZCardAsync(string key) => ExecuteScalarAsync(key, (c, k) => c.Value.ZCardAsync(k));
        /// <summary>
        /// 计算在有序集合中指定区间分数的成员数量
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">分数最小值 decimal.MinValue 1</param>
        /// <param name="max">分数最大值 decimal.MaxValue 10</param>
        /// <returns></returns>
        public Task<long> ZCountAsync(string key, decimal min, decimal max) => ExecuteScalarAsync(key, (c, k) => c.Value.ZCountAsync(k, min == decimal.MinValue ? "-inf" : min.ToString(), max == decimal.MaxValue ? "+inf" : max.ToString()));
        /// <summary>
        /// 计算在有序集合中指定区间分数的成员数量
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">分数最小值 -inf (1 1</param>
        /// <param name="max">分数最大值 +inf (10 10</param>
        /// <returns></returns>
        public Task<long> ZCountAsync(string key, string min, string max) => ExecuteScalarAsync(key, (c, k) => c.Value.ZCountAsync(k, min, max));
        /// <summary>
        /// 有序集合中对指定成员的分数加上增量 increment
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member">成员</param>
        /// <param name="increment">增量值(默认=1)</param>
        /// <returns></returns>
        public Task<decimal> ZIncrByAsync(string key, string member, decimal increment = 1)
        {
            var args = this.SerializeRedisValueInternal(member);
            return ExecuteScalarAsync(key, (c, k) => c.Value.ZIncrByAsync(k, increment, args));
        }

        /// <summary>
        /// 计算给定的一个或多个有序集的交集，将结果集存储在新的有序集合 destination 中
        /// </summary>
        /// <param name="destination">新的有序集合，不含prefix前辍</param>
        /// <param name="weights">使用 WEIGHTS 选项，你可以为 每个 给定有序集 分别 指定一个乘法因子。如果没有指定 WEIGHTS 选项，乘法因子默认设置为 1 。</param>
        /// <param name="aggregate">Sum | Min | Max</param>
        /// <param name="keys">一个或多个有序集合，不含prefix前辍</param>
        /// <returns></returns>
        public Task<long> ZInterStoreAsync(string destination, decimal[] weights, RedisAggregate aggregate, params string[] keys)
        {
            if (keys == null || keys.Length == 0) throw new Exception("keys 参数不可为空");
            if (weights != null && weights.Length != keys.Length) throw new Exception("weights 和 keys 参数长度必须相同");
            return NodesNotSupportAsync(new[] { destination }.Concat(keys).ToArray(), 0, (c, k) => c.Value.ZInterStoreAsync(k.First(), weights, aggregate, k.Skip(1).ToArray()));
        }

        /// <summary>
        /// 通过索引区间返回有序集合成指定区间内的成员
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        public Task<string[]> ZRangeAsync(string key, long start, long stop) => ExecuteScalarAsync(key, (c, k) => c.Value.ZRangeAsync(k, start, stop, false));
        /// <summary>
        /// 通过索引区间返回有序集合成指定区间内的成员
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        async public Task<T[]> ZRangeAsync<T>(string key, long start, long stop) => this.DeserializeRedisValueArrayInternal<T>(await ExecuteScalarAsync(key, (c, k) => c.Value.ZRangeBytesAsync(k, start, stop, false)));
        /// <summary>
        /// 通过索引区间返回有序集合成指定区间内的成员和分数
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        async public Task<(string member, decimal score)[]> ZRangeWithScoresAsync(string key, long start, long stop) => (await ExecuteScalarAsync(key, (c, k) => c.Value.ZRangeWithScoresAsync(k, start, stop))).Select(a => (a.Item1, a.Item2)).ToArray();
        /// <summary>
        /// 通过索引区间返回有序集合成指定区间内的成员和分数
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        async public Task<(T member, decimal score)[]> ZRangeWithScoresAsync<T>(string key, long start, long stop) => this.DeserializeRedisValueTuple1Internal<T, decimal>(await ExecuteScalarAsync(key, (c, k) => c.Value.ZRangeBytesWithScoresAsync(k, start, stop)));

        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">分数最小值 decimal.MinValue 1</param>
        /// <param name="max">分数最大值 decimal.MaxValue 10</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        public Task<string[]> ZRangeByScoreAsync(string key, decimal min, decimal max, long? count = null, long offset = 0) =>
            ExecuteScalarAsync(key, (c, k) => c.Value.ZRangeByScoreAsync(k, min == decimal.MinValue ? "-inf" : min.ToString(), max == decimal.MaxValue ? "+inf" : max.ToString(), false, offset, count));
        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">分数最小值 decimal.MinValue 1</param>
        /// <param name="max">分数最大值 decimal.MaxValue 10</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        async public Task<T[]> ZRangeByScoreAsync<T>(string key, decimal min, decimal max, long? count = null, long offset = 0) =>
            this.DeserializeRedisValueArrayInternal<T>(await ExecuteScalarAsync(key, (c, k) => c.Value.ZRangeBytesByScoreAsync(k, min == decimal.MinValue ? "-inf" : min.ToString(), max == decimal.MaxValue ? "+inf" : max.ToString(), false, offset, count)));
        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">分数最小值 -inf (1 1</param>
        /// <param name="max">分数最大值 +inf (10 10</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        public Task<string[]> ZRangeByScoreAsync(string key, string min, string max, long? count = null, long offset = 0) =>
            ExecuteScalarAsync(key, (c, k) => c.Value.ZRangeByScoreAsync(k, min, max, false, offset, count));
        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">分数最小值 -inf (1 1</param>
        /// <param name="max">分数最大值 +inf (10 10</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        async public Task<T[]> ZRangeByScoreAsync<T>(string key, string min, string max, long? count = null, long offset = 0) =>
            this.DeserializeRedisValueArrayInternal<T>(await ExecuteScalarAsync(key, (c, k) => c.Value.ZRangeBytesByScoreAsync(k, min, max, false, offset, count)));

        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员和分数
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">分数最小值 decimal.MinValue 1</param>
        /// <param name="max">分数最大值 decimal.MaxValue 10</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        async public Task<(string member, decimal score)[]> ZRangeByScoreWithScoresAsync(string key, decimal min, decimal max, long? count = null, long offset = 0) =>
            (await ExecuteScalarAsync(key, (c, k) => c.Value.ZRangeByScoreWithScoresAsync(k, min == decimal.MinValue ? "-inf" : min.ToString(), max == decimal.MaxValue ? "+inf" : max.ToString(), offset, count))).Select(z => (z.Item1, z.Item2)).ToArray();
        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员和分数
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">分数最小值 decimal.MinValue 1</param>
        /// <param name="max">分数最大值 decimal.MaxValue 10</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        async public Task<(T member, decimal score)[]> ZRangeByScoreWithScoresAsync<T>(string key, decimal min, decimal max, long? count = null, long offset = 0) =>
            this.DeserializeRedisValueTuple1Internal<T, decimal>(await ExecuteScalarAsync(key, (c, k) => c.Value.ZRangeBytesByScoreWithScoresAsync(k, min == decimal.MinValue ? "-inf" : min.ToString(), max == decimal.MaxValue ? "+inf" : max.ToString(), offset, count)));
        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员和分数
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">分数最小值 -inf (1 1</param>
        /// <param name="max">分数最大值 +inf (10 10</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        async public Task<(string member, decimal score)[]> ZRangeByScoreWithScoresAsync(string key, string min, string max, long? count = null, long offset = 0) =>
            (await ExecuteScalarAsync(key, (c, k) => c.Value.ZRangeByScoreWithScoresAsync(k, min, max, offset, count))).Select(z => (z.Item1, z.Item2)).ToArray();
        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员和分数
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">分数最小值 -inf (1 1</param>
        /// <param name="max">分数最大值 +inf (10 10</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        async public Task<(T member, decimal score)[]> ZRangeByScoreWithScoresAsync<T>(string key, string min, string max, long? count = null, long offset = 0) =>
            this.DeserializeRedisValueTuple1Internal<T, decimal>(await ExecuteScalarAsync(key, (c, k) => c.Value.ZRangeBytesByScoreWithScoresAsync(k, min, max, offset, count)));

        /// <summary>
        /// 返回有序集合中指定成员的索引
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member">成员</param>
        /// <returns></returns>
        public Task<long?> ZRankAsync(string key, object member)
        {
            var args = this.SerializeRedisValueInternal(member);
            return ExecuteScalarAsync(key, (c, k) => c.Value.ZRankAsync(k, args));
        }
        /// <summary>
        /// 移除有序集合中的一个或多个成员
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member">一个或多个成员</param>
        /// <returns></returns>
        async public Task<long> ZRemAsync<T>(string key, params T[] member)
        {
            if (member == null || member.Any() == false) return 0;
            var args = member.Select(z => this.SerializeRedisValueInternal(z)).ToArray();
            return await ExecuteScalarAsync(key, (c, k) => c.Value.ZRemAsync(k, args));
        }
        /// <summary>
        /// 移除有序集合中给定的排名区间的所有成员
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        public Task<long> ZRemRangeByRankAsync(string key, long start, long stop) => ExecuteScalarAsync(key, (c, k) => c.Value.ZRemRangeByRankAsync(k, start, stop));
        /// <summary>
        /// 移除有序集合中给定的分数区间的所有成员
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">分数最小值 decimal.MinValue 1</param>
        /// <param name="max">分数最大值 decimal.MaxValue 10</param>
        /// <returns></returns>
        public Task<long> ZRemRangeByScoreAsync(string key, decimal min, decimal max) => ExecuteScalarAsync(key, (c, k) => c.Value.ZRemRangeByScoreAsync(k, min == decimal.MinValue ? "-inf" : min.ToString(), max == decimal.MaxValue ? "+inf" : max.ToString()));
        /// <summary>
        /// 移除有序集合中给定的分数区间的所有成员
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">分数最小值 -inf (1 1</param>
        /// <param name="max">分数最大值 +inf (10 10</param>
        /// <returns></returns>
        public Task<long> ZRemRangeByScoreAsync(string key, string min, string max) => ExecuteScalarAsync(key, (c, k) => c.Value.ZRemRangeByScoreAsync(k, min, max));

        /// <summary>
        /// 返回有序集中指定区间内的成员，通过索引，分数从高到底
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        public Task<string[]> ZRevRangeAsync(string key, long start, long stop) => ExecuteScalarAsync(key, (c, k) => c.Value.ZRevRangeAsync(k, start, stop, false));
        /// <summary>
        /// 返回有序集中指定区间内的成员，通过索引，分数从高到底
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        async public Task<T[]> ZRevRangeAsync<T>(string key, long start, long stop) => this.DeserializeRedisValueArrayInternal<T>(await ExecuteScalarAsync(key, (c, k) => c.Value.ZRevRangeBytesAsync(k, start, stop, false)));
        /// <summary>
        /// 返回有序集中指定区间内的成员和分数，通过索引，分数从高到底
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        async public Task<(string member, decimal score)[]> ZRevRangeWithScoresAsync(string key, long start, long stop) => (await ExecuteScalarAsync(key, (c, k) => c.Value.ZRevRangeWithScoresAsync(k, start, stop))).Select(a => (a.Item1, a.Item2)).ToArray();
        /// <summary>
        /// 返回有序集中指定区间内的成员和分数，通过索引，分数从高到底
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        async public Task<(T member, decimal score)[]> ZRevRangeWithScoresAsync<T>(string key, long start, long stop) => this.DeserializeRedisValueTuple1Internal<T, decimal>(await ExecuteScalarAsync(key, (c, k) => c.Value.ZRevRangeBytesWithScoresAsync(k, start, stop)));

        /// <summary>
        /// 返回有序集中指定分数区间内的成员，分数从高到低排序
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="max">分数最大值 decimal.MaxValue 10</param>
        /// <param name="min">分数最小值 decimal.MinValue 1</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        public Task<string[]> ZRevRangeByScoreAsync(string key, decimal max, decimal min, long? count = null, long? offset = 0) => ExecuteScalarAsync(key, (c, k) => c.Value.ZRevRangeByScoreAsync(k, max == decimal.MaxValue ? "+inf" : max.ToString(), min == decimal.MinValue ? "-inf" : min.ToString(), false, offset, count));
        /// <summary>
        /// 返回有序集中指定分数区间内的成员，分数从高到低排序
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="max">分数最大值 decimal.MaxValue 10</param>
        /// <param name="min">分数最小值 decimal.MinValue 1</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        async public Task<T[]> ZRevRangeByScoreAsync<T>(string key, decimal max, decimal min, long? count = null, long offset = 0) =>
            this.DeserializeRedisValueArrayInternal<T>(await ExecuteScalarAsync(key, (c, k) => c.Value.ZRevRangeBytesByScoreAsync(k, max == decimal.MaxValue ? "+inf" : max.ToString(), min == decimal.MinValue ? "-inf" : min.ToString(), false, offset, count)));
        /// <summary>
        /// 返回有序集中指定分数区间内的成员，分数从高到低排序
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="max">分数最大值 +inf (10 10</param>
        /// <param name="min">分数最小值 -inf (1 1</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        public Task<string[]> ZRevRangeByScoreAsync(string key, string max, string min, long? count = null, long? offset = 0) => ExecuteScalarAsync(key, (c, k) => c.Value.ZRevRangeByScoreAsync(k, max, min, false, offset, count));
        /// <summary>
        /// 返回有序集中指定分数区间内的成员，分数从高到低排序
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="max">分数最大值 +inf (10 10</param>
        /// <param name="min">分数最小值 -inf (1 1</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        async public Task<T[]> ZRevRangeByScoreAsync<T>(string key, string max, string min, long? count = null, long offset = 0) =>
            this.DeserializeRedisValueArrayInternal<T>(await ExecuteScalarAsync(key, (c, k) => c.Value.ZRevRangeBytesByScoreAsync(k, max, min, false, offset, count)));

        /// <summary>
        /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="max">分数最大值 decimal.MaxValue 10</param>
        /// <param name="min">分数最小值 decimal.MinValue 1</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        async public Task<(string member, decimal score)[]> ZRevRangeByScoreWithScoresAsync(string key, decimal max, decimal min, long? count = null, long offset = 0) =>
            (await ExecuteScalarAsync(key, (c, k) => c.Value.ZRevRangeByScoreWithScoresAsync(k, max == decimal.MaxValue ? "+inf" : max.ToString(), min == decimal.MinValue ? "-inf" : min.ToString(), offset, count))).Select(z => (z.Item1, z.Item2)).ToArray();
        /// <summary>
        /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="max">分数最大值 decimal.MaxValue 10</param>
        /// <param name="min">分数最小值 decimal.MinValue 1</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        async public Task<(T member, decimal score)[]> ZRevRangeByScoreWithScoresAsync<T>(string key, decimal max, decimal min, long? count = null, long offset = 0) =>
            this.DeserializeRedisValueTuple1Internal<T, decimal>(await ExecuteScalarAsync(key, (c, k) => c.Value.ZRevRangeBytesByScoreWithScoresAsync(k, max == decimal.MaxValue ? "+inf" : max.ToString(), min == decimal.MinValue ? "-inf" : min.ToString(), offset, count)));
        /// <summary>
        /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="max">分数最大值 +inf (10 10</param>
        /// <param name="min">分数最小值 -inf (1 1</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        async public Task<(string member, decimal score)[]> ZRevRangeByScoreWithScoresAsync(string key, string max, string min, long? count = null, long offset = 0) =>
            (await ExecuteScalarAsync(key, (c, k) => c.Value.ZRevRangeByScoreWithScoresAsync(k, max, min, offset, count))).Select(z => (z.Item1, z.Item2)).ToArray();
        /// <summary>
        /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="max">分数最大值 +inf (10 10</param>
        /// <param name="min">分数最小值 -inf (1 1</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        async public Task<(T member, decimal score)[]> ZRevRangeByScoreWithScoresAsync<T>(string key, string max, string min, long? count = null, long offset = 0) =>
            this.DeserializeRedisValueTuple1Internal<T, decimal>(await ExecuteScalarAsync(key, (c, k) => c.Value.ZRevRangeBytesByScoreWithScoresAsync(k, max, min, offset, count)));

        /// <summary>
        /// 返回有序集合中指定成员的排名，有序集成员按分数值递减(从大到小)排序
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member">成员</param>
        /// <returns></returns>
        public Task<long?> ZRevRankAsync(string key, object member)
        {
            var args = this.SerializeRedisValueInternal(member);
            return ExecuteScalarAsync(key, (c, k) => c.Value.ZRevRankAsync(k, args));
        }
        /// <summary>
        /// 返回有序集中，成员的分数值
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member">成员</param>
        /// <returns></returns>
        public Task<decimal?> ZScoreAsync(string key, object member)
        {
            var args = this.SerializeRedisValueInternal(member);
            return ExecuteScalarAsync(key, (c, k) => c.Value.ZScoreAsync(k, args));
        }

        /// <summary>
        /// 计算给定的一个或多个有序集的并集，将结果集存储在新的有序集合 destination 中
        /// </summary>
        /// <param name="destination">新的有序集合，不含prefix前辍</param>
        /// <param name="weights">使用 WEIGHTS 选项，你可以为 每个 给定有序集 分别 指定一个乘法因子。如果没有指定 WEIGHTS 选项，乘法因子默认设置为 1 。</param>
        /// <param name="aggregate">Sum | Min | Max</param>
        /// <param name="keys">一个或多个有序集合，不含prefix前辍</param>
        /// <returns></returns>
        public Task<long> ZUnionStoreAsync(string destination, decimal[] weights, RedisAggregate aggregate, params string[] keys)
        {
            if (keys == null || keys.Length == 0) throw new Exception("keys 参数不可为空");
            if (weights != null && weights.Length != keys.Length) throw new Exception("weights 和 keys 参数长度必须相同");
            return NodesNotSupportAsync(new[] { destination }.Concat(keys).ToArray(), 0, (c, k) => c.Value.ZUnionStoreAsync(k.First(), weights, aggregate, k.Skip(1).ToArray()));
        }

        /// <summary>
        /// 迭代有序集合中的元素
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="cursor">位置</param>
        /// <param name="pattern">模式</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        async public Task<RedisScan<(string member, decimal score)>> ZScanAsync(string key, long cursor, string pattern = null, long? count = null)
        {
            var scan = await ExecuteScalarAsync(key, (c, k) => c.Value.ZScanAsync(k, cursor, pattern, count));
            return new RedisScan<(string, decimal)>(scan.Cursor, scan.Items.Select(z => (z.Item1, z.Item2)).ToArray());
        }
        /// <summary>
        /// 迭代有序集合中的元素
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="cursor">位置</param>
        /// <param name="pattern">模式</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        async public Task<RedisScan<(T member, decimal score)>> ZScanAsync<T>(string key, long cursor, string pattern = null, long? count = null)
        {
            var scan = await ExecuteScalarAsync(key, (c, k) => c.Value.ZScanBytesAsync(k, cursor, pattern, count));
            return new RedisScan<(T, decimal)>(scan.Cursor, this.DeserializeRedisValueTuple1Internal<T, decimal>(scan.Items));
        }

        /// <summary>
        /// 当有序集合的所有成员都具有相同的分值时，有序集合的元素会根据成员的字典序来进行排序，这个命令可以返回给定的有序集合键 key 中，值介于 min 和 max 之间的成员。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
        /// <param name="max">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        public Task<string[]> ZRangeByLexAsync(string key, string min, string max, long? count = null, long offset = 0) =>
            ExecuteScalarAsync(key, (c, k) => c.Value.ZRangeByLexAsync(k, min, max, offset, count));
        /// <summary>
        /// 当有序集合的所有成员都具有相同的分值时，有序集合的元素会根据成员的字典序来进行排序，这个命令可以返回给定的有序集合键 key 中，值介于 min 和 max 之间的成员。
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
        /// <param name="max">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        async public Task<T[]> ZRangeByLexAsync<T>(string key, string min, string max, long? count = null, long offset = 0) =>
            this.DeserializeRedisValueArrayInternal<T>(await ExecuteScalarAsync(key, (c, k) => c.Value.ZRangeBytesByLexAsync(k, min, max, offset, count)));

        /// <summary>
        /// 当有序集合的所有成员都具有相同的分值时，有序集合的元素会根据成员的字典序来进行排序，这个命令可以返回给定的有序集合键 key 中，值介于 min 和 max 之间的成员。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
        /// <param name="max">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
        /// <returns></returns>
        public Task<long> ZRemRangeByLexAsync(string key, string min, string max) =>
            ExecuteScalarAsync(key, (c, k) => c.Value.ZRemRangeByLexAsync(k, min, max));
        /// <summary>
        /// 当有序集合的所有成员都具有相同的分值时，有序集合的元素会根据成员的字典序来进行排序，这个命令可以返回给定的有序集合键 key 中，值介于 min 和 max 之间的成员。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="min">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
        /// <param name="max">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
        /// <returns></returns>
        public Task<long> ZLexCountAsync(string key, string min, string max) =>
            ExecuteScalarAsync(key, (c, k) => c.Value.ZLexCountAsync(k, min, max));
        #endregion


#endif

    }
}