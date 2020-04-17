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
    #region Sorted Set

    /// <summary>
    /// 向有序集合添加一个或多个成员，或者更新已存在成员的分数
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="scoreMembers">一个或多个成员分数</param>
    /// <returns></returns>
    public static long ZAdd(string key, params (decimal, object)[] scoreMembers) => Instance.ZAdd(key, scoreMembers);
    /// <summary>
    /// 获取有序集合的成员数量
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static long ZCard(string key) => Instance.ZCard(key);
    /// <summary>
    /// 计算在有序集合中指定区间分数的成员数量
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">分数最小值 decimal.MinValue 1</param>
    /// <param name="max">分数最大值 decimal.MaxValue 10</param>
    /// <returns></returns>
    public static long ZCount(string key, decimal min, decimal max) => Instance.ZCount(key, min, max);
    /// <summary>
    /// 计算在有序集合中指定区间分数的成员数量
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">分数最小值 -inf (1 1</param>
    /// <param name="max">分数最大值 +inf (10 10</param>
    /// <returns></returns>
    public static long ZCount(string key, string min, string max) => Instance.ZCount(key, min, max);
    /// <summary>
    /// 有序集合中对指定成员的分数加上增量 increment
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="member">成员</param>
    /// <param name="increment">增量值(默认=1)</param>
    /// <returns></returns>
    public static decimal ZIncrBy(string key, string member, decimal increment = 1) => Instance.ZIncrBy(key, member, increment);

    /// <summary>
    /// 计算给定的一个或多个有序集的交集，将结果集存储在新的有序集合 destination 中
    /// </summary>
    /// <param name="destination">新的有序集合，不含prefix前辍</param>
    /// <param name="weights">使用 WEIGHTS 选项，你可以为 每个 给定有序集 分别 指定一个乘法因子。如果没有指定 WEIGHTS 选项，乘法因子默认设置为 1 。</param>
    /// <param name="aggregate">Sum | Min | Max</param>
    /// <param name="keys">一个或多个有序集合，不含prefix前辍</param>
    /// <returns></returns>
    public static long ZInterStore(string destination, decimal[] weights, RedisAggregate aggregate, params string[] keys) => Instance.ZInterStore(destination, weights, aggregate, keys);

    /// <summary>
    /// 通过索引区间返回有序集合成指定区间内的成员
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <returns></returns>
    public static string[] ZRange(string key, long start, long stop) => Instance.ZRange(key, start, stop);
    /// <summary>
    /// 通过索引区间返回有序集合成指定区间内的成员
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <returns></returns>
    public static T[] ZRange<T>(string key, long start, long stop) => Instance.ZRange<T>(key, start, stop);
    /// <summary>
    /// 通过索引区间返回有序集合成指定区间内的成员和分数
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <returns></returns>
    public static (string member, decimal score)[] ZRangeWithScores(string key, long start, long stop) => Instance.ZRangeWithScores(key, start, stop);
    /// <summary>
    /// 通过索引区间返回有序集合成指定区间内的成员和分数
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <returns></returns>
    public static (T member, decimal score)[] ZRangeWithScores<T>(string key, long start, long stop) => Instance.ZRangeWithScores<T>(key, start, stop);

    /// <summary>
    /// 通过分数返回有序集合指定区间内的成员
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">分数最小值 decimal.MinValue 1</param>
    /// <param name="max">分数最大值 decimal.MaxValue 10</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static string[] ZRangeByScore(string key, decimal min, decimal max, long? limit = null, long offset = 0) =>
        Instance.ZRangeByScore(key, min, max, limit, offset);
    /// <summary>
    /// 通过分数返回有序集合指定区间内的成员
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">分数最小值 decimal.MinValue 1</param>
    /// <param name="max">分数最大值 decimal.MaxValue 10</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static T[] ZRangeByScore<T>(string key, decimal min, decimal max, long? limit = null, long offset = 0) =>
        Instance.ZRangeByScore<T>(key, min, max, limit, offset);
    /// <summary>
    /// 通过分数返回有序集合指定区间内的成员
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">分数最小值 -inf (1 1</param>
    /// <param name="max">分数最大值 +inf (10 10</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static string[] ZRangeByScore(string key, string min, string max, long? limit = null, long offset = 0) =>
        Instance.ZRangeByScore(key, min, max, limit, offset);
    /// <summary>
    /// 通过分数返回有序集合指定区间内的成员
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">分数最小值 -inf (1 1</param>
    /// <param name="max">分数最大值 +inf (10 10</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static T[] ZRangeByScore<T>(string key, string min, string max, long? limit = null, long offset = 0) =>
        Instance.ZRangeByScore<T>(key, min, max, limit, offset);

    /// <summary>
    /// 通过分数返回有序集合指定区间内的成员和分数
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">分数最小值 decimal.MinValue 1</param>
    /// <param name="max">分数最大值 decimal.MaxValue 10</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static (string member, decimal score)[] ZRangeByScoreWithScores(string key, decimal min, decimal max, long? limit = null, long offset = 0) =>
        Instance.ZRangeByScoreWithScores(key, min, max, limit, offset);
    /// <summary>
    /// 通过分数返回有序集合指定区间内的成员和分数
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">分数最小值 decimal.MinValue 1</param>
    /// <param name="max">分数最大值 decimal.MaxValue 10</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static (T member, decimal score)[] ZRangeByScoreWithScores<T>(string key, decimal min, decimal max, long? limit = null, long offset = 0) =>
        Instance.ZRangeByScoreWithScores<T>(key, min, max, limit, offset);
    /// <summary>
    /// 通过分数返回有序集合指定区间内的成员和分数
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">分数最小值 -inf (1 1</param>
    /// <param name="max">分数最大值 +inf (10 10</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static (string member, decimal score)[] ZRangeByScoreWithScores(string key, string min, string max, long? limit = null, long offset = 0) =>
        Instance.ZRangeByScoreWithScores(key, min, max, limit, offset);
    /// <summary>
    /// 通过分数返回有序集合指定区间内的成员和分数
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">分数最小值 -inf (1 1</param>
    /// <param name="max">分数最大值 +inf (10 10</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static (T member, decimal score)[] ZRangeByScoreWithScores<T>(string key, string min, string max, long? limit = null, long offset = 0) =>
           Instance.ZRangeByScoreWithScores<T>(key, min, max, limit, offset);

    /// <summary>
    /// 返回有序集合中指定成员的索引
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="member">成员</param>
    /// <returns></returns>
    public static long? ZRank(string key, object member) => Instance.ZRank(key, member);
    /// <summary>
    /// 移除有序集合中的一个或多个成员
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="member">一个或多个成员</param>
    /// <returns></returns>
    public static long ZRem<T>(string key, params T[] member) => Instance.ZRem(key, member);
    /// <summary>
    /// 移除有序集合中给定的排名区间的所有成员
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <returns></returns>
    public static long ZRemRangeByRank(string key, long start, long stop) => Instance.ZRemRangeByRank(key, start, stop);
    /// <summary>
    /// 移除有序集合中给定的分数区间的所有成员
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">分数最小值 decimal.MinValue 1</param>
    /// <param name="max">分数最大值 decimal.MaxValue 10</param>
    /// <returns></returns>
    public static long ZRemRangeByScore(string key, decimal min, decimal max) => Instance.ZRemRangeByScore(key, min, max);
    /// <summary>
    /// 移除有序集合中给定的分数区间的所有成员
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">分数最小值 -inf (1 1</param>
    /// <param name="max">分数最大值 +inf (10 10</param>
    /// <returns></returns>
    public static long ZRemRangeByScore(string key, string min, string max) => Instance.ZRemRangeByScore(key, min, max);

    /// <summary>
    /// 返回有序集中指定区间内的成员，通过索引，分数从高到底
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <returns></returns>
    public static string[] ZRevRange(string key, long start, long stop) => Instance.ZRevRange(key, start, stop);
    /// <summary>
    /// 返回有序集中指定区间内的成员，通过索引，分数从高到底
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <returns></returns>
    public static T[] ZRevRange<T>(string key, long start, long stop) => Instance.ZRevRange<T>(key, start, stop);
    /// <summary>
    /// 返回有序集中指定区间内的成员和分数，通过索引，分数从高到底
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <returns></returns>
    public static (string member, decimal score)[] ZRevRangeWithScores(string key, long start, long stop) => Instance.ZRevRangeWithScores(key, start, stop);
    /// <summary>
    /// 返回有序集中指定区间内的成员和分数，通过索引，分数从高到底
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <returns></returns>
    public static (T member, decimal score)[] ZRevRangeWithScores<T>(string key, long start, long stop) => Instance.ZRevRangeWithScores<T>(key, start, stop);

    /// <summary>
    /// 返回有序集中指定分数区间内的成员，分数从高到低排序
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="max">分数最大值 decimal.MaxValue 10</param>
    /// <param name="min">分数最小值 decimal.MinValue 1</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static string[] ZRevRangeByScore(string key, decimal max, decimal min, long? limit = null, long? offset = 0) => Instance.ZRevRangeByScore(key, max, min, limit, offset);
    /// <summary>
    /// 返回有序集中指定分数区间内的成员，分数从高到低排序
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="max">分数最大值 decimal.MaxValue 10</param>
    /// <param name="min">分数最小值 decimal.MinValue 1</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static T[] ZRevRangeByScore<T>(string key, decimal max, decimal min, long? limit = null, long offset = 0) =>
        Instance.ZRevRangeByScore<T>(key, max, min, limit, offset);
    /// <summary>
    /// 返回有序集中指定分数区间内的成员，分数从高到低排序
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="max">分数最大值 +inf (10 10</param>
    /// <param name="min">分数最小值 -inf (1 1</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static string[] ZRevRangeByScore(string key, string max, string min, long? limit = null, long? offset = 0) => Instance.ZRevRangeByScore(key, max, min, limit, offset);
    /// <summary>
    /// 返回有序集中指定分数区间内的成员，分数从高到低排序
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="max">分数最大值 +inf (10 10</param>
    /// <param name="min">分数最小值 -inf (1 1</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static T[] ZRevRangeByScore<T>(string key, string max, string min, long? limit = null, long offset = 0) =>
           Instance.ZRevRangeByScore<T>(key, max, min, limit, offset);

    /// <summary>
    /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="max">分数最大值 decimal.MaxValue 10</param>
    /// <param name="min">分数最小值 decimal.MinValue 1</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static (string member, decimal score)[] ZRevRangeByScoreWithScores(string key, decimal max, decimal min, long? limit = null, long offset = 0) =>
         Instance.ZRevRangeByScoreWithScores(key, max, min, limit, offset);
    /// <summary>
    /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="max">分数最大值 decimal.MaxValue 10</param>
    /// <param name="min">分数最小值 decimal.MinValue 1</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static (T member, decimal score)[] ZRevRangeByScoreWithScores<T>(string key, decimal max, decimal min, long? limit = null, long offset = 0) =>
           Instance.ZRevRangeByScoreWithScores<T>(key, max, min, limit, offset);
    /// <summary>
    /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="max">分数最大值 +inf (10 10</param>
    /// <param name="min">分数最小值 -inf (1 1</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static (string member, decimal score)[] ZRevRangeByScoreWithScores(string key, string max, string min, long? limit = null, long offset = 0) =>
           Instance.ZRevRangeByScoreWithScores(key, max, min, limit, offset);
    /// <summary>
    /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="max">分数最大值 +inf (10 10</param>
    /// <param name="min">分数最小值 -inf (1 1</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static (T member, decimal score)[] ZRevRangeByScoreWithScores<T>(string key, string max, string min, long? limit = null, long offset = 0) =>
        Instance.ZRevRangeByScoreWithScores<T>(key, max, min, limit, offset);

    /// <summary>
    /// 返回有序集合中指定成员的排名，有序集成员按分数值递减(从大到小)排序
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="member">成员</param>
    /// <returns></returns>
    public static long? ZRevRank(string key, object member) => Instance.ZRevRank(key, member);
    /// <summary>
    /// 返回有序集中，成员的分数值
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="member">成员</param>
    /// <returns></returns>
    public static decimal? ZScore(string key, object member) => Instance.ZScore(key, member);

    /// <summary>
    /// 计算给定的一个或多个有序集的并集，将结果集存储在新的有序集合 destination 中
    /// </summary>
    /// <param name="destination">新的有序集合，不含prefix前辍</param>
    /// <param name="weights">使用 WEIGHTS 选项，你可以为 每个 给定有序集 分别 指定一个乘法因子。如果没有指定 WEIGHTS 选项，乘法因子默认设置为 1 。</param>
    /// <param name="aggregate">Sum | Min | Max</param>
    /// <param name="keys">一个或多个有序集合，不含prefix前辍</param>
    /// <returns></returns>
    public static long ZUnionStore(string destination, decimal[] weights, RedisAggregate aggregate, params string[] keys) => Instance.ZUnionStore(destination, weights, aggregate, keys);

    /// <summary>
    /// 迭代有序集合中的元素
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="cursor">位置</param>
    /// <param name="pattern">模式</param>
    /// <param name="count">数量</param>
    /// <returns></returns>
    public static RedisScan<(string member, decimal score)> ZScan(string key, long cursor, string pattern = null, long? count = null) =>
           Instance.ZScan(key, cursor, pattern, count);
    /// <summary>
    /// 迭代有序集合中的元素
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="cursor">位置</param>
    /// <param name="pattern">模式</param>
    /// <param name="count">数量</param>
    /// <returns></returns>
    public static RedisScan<(T member, decimal score)> ZScan<T>(string key, long cursor, string pattern = null, long? count = null) =>
           Instance.ZScan<T>(key, cursor, pattern, count);

    /// <summary>
    /// 当有序集合的所有成员都具有相同的分值时，有序集合的元素会根据成员的字典序来进行排序，这个命令可以返回给定的有序集合键 key 中，值介于 min 和 max 之间的成员。
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
    /// <param name="max">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static string[] ZRangeByLex(string key, string min, string max, long? limit = null, long offset = 0) =>
           Instance.ZRangeByLex(key, min, max, limit, offset);
    /// <summary>
    /// 当有序集合的所有成员都具有相同的分值时，有序集合的元素会根据成员的字典序来进行排序，这个命令可以返回给定的有序集合键 key 中，值介于 min 和 max 之间的成员。
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
    /// <param name="max">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static T[] ZRangeByLex<T>(string key, string min, string max, long? limit = null, long offset = 0) =>
        Instance.ZRangeByLex<T>(key, min, max, limit, offset);

    /// <summary>
    /// 当有序集合的所有成员都具有相同的分值时，有序集合的元素会根据成员的字典序来进行排序，这个命令可以返回给定的有序集合键 key 中，值介于 min 和 max 之间的成员。
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
    /// <param name="max">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
    /// <returns></returns>
    public static long ZRemRangeByLex(string key, string min, string max) =>
           Instance.ZRemRangeByLex(key, min, max);
    /// <summary>
    /// 当有序集合的所有成员都具有相同的分值时，有序集合的元素会根据成员的字典序来进行排序，这个命令可以返回给定的有序集合键 key 中，值介于 min 和 max 之间的成员。
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
    /// <param name="max">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
    /// <returns></returns>
    public static long ZLexCount(string key, string min, string max) =>
           Instance.ZLexCount(key, min, max);

    /// <summary>
    /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最高得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。 当返回多个元素时候，得分最高的元素将是第一个元素，然后是分数较低的元素。
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="count">数量</param>
    /// <returns></returns>
    public static (string member, decimal score)[] ZPopMax(string key, long count) =>
        Instance.ZPopMax(key, count);

    /// <summary>
    /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最高得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。 当返回多个元素时候，得分最高的元素将是第一个元素，然后是分数较低的元素。
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="count">数量</param>
    /// <returns></returns>
    public static (T member, decimal score)[] ZPopMax<T>(string key, long count) =>
        Instance.ZPopMax<T>(key, count);

    /// <summary>
    /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最低得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。 当返回多个元素时候，得分最低的元素将是第一个元素，然后是分数较高的元素。
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="count">数量</param>
    /// <returns></returns>
    public static (string member, decimal score)[] ZPopMin(string key, long count) =>
        Instance.ZPopMin(key, count);

    /// <summary>
    /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最低得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。 当返回多个元素时候，得分最低的元素将是第一个元素，然后是分数较高的元素。
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="count">数量</param>
    /// <returns></returns>
    public static (T member, decimal score)[] ZPopMin<T>(string key, long count) =>
        Instance.ZPopMin<T>(key, count);

    #endregion

#if !net40

    #region Sorted Set
    /// <summary>
    /// 向有序集合添加一个或多个成员，或者更新已存在成员的分数
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="scoreMembers">一个或多个成员分数</param>
    /// <returns></returns>
    public static Task<long> ZAddAsync(string key, params (decimal, object)[] scoreMembers) => Instance.ZAddAsync(key, scoreMembers);
    /// <summary>
    /// 获取有序集合的成员数量
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<long> ZCardAsync(string key) => Instance.ZCardAsync(key);
    /// <summary>
    /// 计算在有序集合中指定区间分数的成员数量
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">分数最小值 decimal.MinValue 1</param>
    /// <param name="max">分数最大值 decimal.MaxValue 10</param>
    /// <returns></returns>
    public static Task<long> ZCountAsync(string key, decimal min, decimal max) => Instance.ZCountAsync(key, min, max);
    /// <summary>
    /// 计算在有序集合中指定区间分数的成员数量
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">分数最小值 -inf (1 1</param>
    /// <param name="max">分数最大值 +inf (10 10</param>
    /// <returns></returns>
    public static Task<long> ZCountAsync(string key, string min, string max) => Instance.ZCountAsync(key, min, max);
    /// <summary>
    /// 有序集合中对指定成员的分数加上增量 increment
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="member">成员</param>
    /// <param name="increment">增量值(默认=1)</param>
    /// <returns></returns>
    public static Task<decimal> ZIncrByAsync(string key, string member, decimal increment = 1) => Instance.ZIncrByAsync(key, member, increment);

    /// <summary>
    /// 计算给定的一个或多个有序集的交集，将结果集存储在新的有序集合 destination 中
    /// </summary>
    /// <param name="destination">新的有序集合，不含prefix前辍</param>
    /// <param name="weights">使用 WEIGHTS 选项，你可以为 每个 给定有序集 分别 指定一个乘法因子。如果没有指定 WEIGHTS 选项，乘法因子默认设置为 1 。</param>
    /// <param name="aggregate">Sum | Min | Max</param>
    /// <param name="keys">一个或多个有序集合，不含prefix前辍</param>
    /// <returns></returns>
    public static Task<long> ZInterStoreAsync(string destination, decimal[] weights, RedisAggregate aggregate, params string[] keys) => Instance.ZInterStoreAsync(destination, weights, aggregate, keys);

    /// <summary>
    /// 通过索引区间返回有序集合成指定区间内的成员
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <returns></returns>
    public static Task<string[]> ZRangeAsync(string key, long start, long stop) => Instance.ZRangeAsync(key, start, stop);
    /// <summary>
    /// 通过索引区间返回有序集合成指定区间内的成员
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <returns></returns>
    public static Task<T[]> ZRangeAsync<T>(string key, long start, long stop) => Instance.ZRangeAsync<T>(key, start, stop);
    /// <summary>
    /// 通过索引区间返回有序集合成指定区间内的成员和分数
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <returns></returns>
    public static Task<(string member, decimal score)[]> ZRangeWithScoresAsync(string key, long start, long stop) => Instance.ZRangeWithScoresAsync(key, start, stop);
    /// <summary>
    /// 通过索引区间返回有序集合成指定区间内的成员和分数
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <returns></returns>
    public static Task<(T member, decimal score)[]> ZRangeWithScoresAsync<T>(string key, long start, long stop) => Instance.ZRangeWithScoresAsync<T>(key, start, stop);

    /// <summary>
    /// 通过分数返回有序集合指定区间内的成员
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">分数最小值 decimal.MinValue 1</param>
    /// <param name="max">分数最大值 decimal.MaxValue 10</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static Task<string[]> ZRangeByScoreAsync(string key, decimal min, decimal max, long? limit = null, long offset = 0) => Instance.ZRangeByScoreAsync(key, min, max, limit, offset);
    /// <summary>
    /// 通过分数返回有序集合指定区间内的成员
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">分数最小值 decimal.MinValue 1</param>
    /// <param name="max">分数最大值 decimal.MaxValue 10</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static Task<T[]> ZRangeByScoreAsync<T>(string key, decimal min, decimal max, long? limit = null, long offset = 0) => Instance.ZRangeByScoreAsync<T>(key, min, max, limit, offset);
    /// <summary>
    /// 通过分数返回有序集合指定区间内的成员
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">分数最小值 -inf (1 1</param>
    /// <param name="max">分数最大值 +inf (10 10</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static Task<string[]> ZRangeByScoreAsync(string key, string min, string max, long? limit = null, long offset = 0) => Instance.ZRangeByScoreAsync(key, min, max, limit, offset);
    /// <summary>
    /// 通过分数返回有序集合指定区间内的成员
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">分数最小值 -inf (1 1</param>
    /// <param name="max">分数最大值 +inf (10 10</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static Task<T[]> ZRangeByScoreAsync<T>(string key, string min, string max, long? limit = null, long offset = 0) => Instance.ZRangeByScoreAsync<T>(key, min, max, limit, offset);

    /// <summary>
    /// 通过分数返回有序集合指定区间内的成员和分数
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">分数最小值 decimal.MinValue 1</param>
    /// <param name="max">分数最大值 decimal.MaxValue 10</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static Task<(string member, decimal score)[]> ZRangeByScoreWithScoresAsync(string key, decimal min, decimal max, long? limit = null, long offset = 0) =>
           Instance.ZRangeByScoreWithScoresAsync(key, min, max, limit, offset);
    /// <summary>
    /// 通过分数返回有序集合指定区间内的成员和分数
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">分数最小值 decimal.MinValue 1</param>
    /// <param name="max">分数最大值 decimal.MaxValue 10</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static Task<(T member, decimal score)[]> ZRangeByScoreWithScoresAsync<T>(string key, decimal min, decimal max, long? limit = null, long offset = 0) =>
           Instance.ZRangeByScoreWithScoresAsync<T>(key, min, max, limit, offset);
    /// <summary>
    /// 通过分数返回有序集合指定区间内的成员和分数
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">分数最小值 -inf (1 1</param>
    /// <param name="max">分数最大值 +inf (10 10</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static Task<(string member, decimal score)[]> ZRangeByScoreWithScoresAsync(string key, string min, string max, long? limit = null, long offset = 0) =>
           Instance.ZRangeByScoreWithScoresAsync(key, min, max, limit, offset);
    /// <summary>
    /// 通过分数返回有序集合指定区间内的成员和分数
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">分数最小值 -inf (1 1</param>
    /// <param name="max">分数最大值 +inf (10 10</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static Task<(T member, decimal score)[]> ZRangeByScoreWithScoresAsync<T>(string key, string min, string max, long? limit = null, long offset = 0) =>
           Instance.ZRangeByScoreWithScoresAsync<T>(key, min, max, limit, offset);

    /// <summary>
    /// 返回有序集合中指定成员的索引
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="member">成员</param>
    /// <returns></returns>
    public static Task<long?> ZRankAsync(string key, object member) => Instance.ZRankAsync(key, member);
    /// <summary>
    /// 移除有序集合中的一个或多个成员
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="member">一个或多个成员</param>
    /// <returns></returns>
    public static Task<long> ZRemAsync<T>(string key, params T[] member) => Instance.ZRemAsync(key, member);
    /// <summary>
    /// 移除有序集合中给定的排名区间的所有成员
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <returns></returns>
    public static Task<long> ZRemRangeByRankAsync(string key, long start, long stop) => Instance.ZRemRangeByRankAsync(key, start, stop);
    /// <summary>
    /// 移除有序集合中给定的分数区间的所有成员
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">分数最小值 decimal.MinValue 1</param>
    /// <param name="max">分数最大值 decimal.MaxValue 10</param>
    /// <returns></returns>
    public static Task<long> ZRemRangeByScoreAsync(string key, decimal min, decimal max) => Instance.ZRemRangeByScoreAsync(key, min, max);
    /// <summary>
    /// 移除有序集合中给定的分数区间的所有成员
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">分数最小值 -inf (1 1</param>
    /// <param name="max">分数最大值 +inf (10 10</param>
    /// <returns></returns>
    public static Task<long> ZRemRangeByScoreAsync(string key, string min, string max) => Instance.ZRemRangeByScoreAsync(key, min, max);

    /// <summary>
    /// 返回有序集中指定区间内的成员，通过索引，分数从高到底
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <returns></returns>
    public static Task<string[]> ZRevRangeAsync(string key, long start, long stop) => Instance.ZRevRangeAsync(key, start, stop);
    /// <summary>
    /// 返回有序集中指定区间内的成员，通过索引，分数从高到底
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <returns></returns>
    public static Task<T[]> ZRevRangeAsync<T>(string key, long start, long stop) => Instance.ZRevRangeAsync<T>(key, start, stop);
    /// <summary>
    /// 返回有序集中指定区间内的成员和分数，通过索引，分数从高到底
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <returns></returns>
    public static Task<(string member, decimal score)[]> ZRevRangeWithScoresAsync(string key, long start, long stop) => Instance.ZRevRangeWithScoresAsync(key, start, stop);
    /// <summary>
    /// 返回有序集中指定区间内的成员和分数，通过索引，分数从高到底
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
    /// <returns></returns>
    public static Task<(T member, decimal score)[]> ZRevRangeWithScoresAsync<T>(string key, long start, long stop) => Instance.ZRevRangeWithScoresAsync<T>(key, start, stop);

    /// <summary>
    /// 返回有序集中指定分数区间内的成员，分数从高到低排序
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="max">分数最大值 decimal.MaxValue 10</param>
    /// <param name="min">分数最小值 decimal.MinValue 1</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static Task<string[]> ZRevRangeByScoreAsync(string key, decimal max, decimal min, long? limit = null, long? offset = 0) => Instance.ZRevRangeByScoreAsync(key, max, min, limit, offset);
    /// <summary>
    /// 返回有序集中指定分数区间内的成员，分数从高到低排序
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="max">分数最大值 decimal.MaxValue 10</param>
    /// <param name="min">分数最小值 decimal.MinValue 1</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static Task<T[]> ZRevRangeByScoreAsync<T>(string key, decimal max, decimal min, long? limit = null, long offset = 0) => Instance.ZRevRangeByScoreAsync<T>(key, max, min, limit, offset);
    /// <summary>
    /// 返回有序集中指定分数区间内的成员，分数从高到低排序
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="max">分数最大值 +inf (10 10</param>
    /// <param name="min">分数最小值 -inf (1 1</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static Task<string[]> ZRevRangeByScoreAsync(string key, string max, string min, long? limit = null, long? offset = 0) => Instance.ZRevRangeByScoreAsync(key, max, min, limit, offset);
    /// <summary>
    /// 返回有序集中指定分数区间内的成员，分数从高到低排序
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="max">分数最大值 +inf (10 10</param>
    /// <param name="min">分数最小值 -inf (1 1</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static Task<T[]> ZRevRangeByScoreAsync<T>(string key, string max, string min, long? limit = null, long offset = 0) => Instance.ZRevRangeByScoreAsync<T>(key, max, min, limit, offset);

    /// <summary>
    /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="max">分数最大值 decimal.MaxValue 10</param>
    /// <param name="min">分数最小值 decimal.MinValue 1</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static Task<(string member, decimal score)[]> ZRevRangeByScoreWithScoresAsync(string key, decimal max, decimal min, long? limit = null, long offset = 0) =>
           Instance.ZRevRangeByScoreWithScoresAsync(key, max, min, limit, offset);
    /// <summary>
    /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="max">分数最大值 decimal.MaxValue 10</param>
    /// <param name="min">分数最小值 decimal.MinValue 1</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static Task<(T member, decimal score)[]> ZRevRangeByScoreWithScoresAsync<T>(string key, decimal max, decimal min, long? limit = null, long offset = 0) =>
           Instance.ZRevRangeByScoreWithScoresAsync<T>(key, max, min, limit, offset);
    /// <summary>
    /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="max">分数最大值 +inf (10 10</param>
    /// <param name="min">分数最小值 -inf (1 1</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static Task<(string member, decimal score)[]> ZRevRangeByScoreWithScoresAsync(string key, string max, string min, long? limit = null, long offset = 0) =>
           Instance.ZRevRangeByScoreWithScoresAsync(key, max, min, limit, offset);
    /// <summary>
    /// 返回有序集中指定分数区间内的成员和分数，分数从高到低排序
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="max">分数最大值 +inf (10 10</param>
    /// <param name="min">分数最小值 -inf (1 1</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static Task<(T member, decimal score)[]> ZRevRangeByScoreWithScoresAsync<T>(string key, string max, string min, long? limit = null, long offset = 0) =>
           Instance.ZRevRangeByScoreWithScoresAsync<T>(key, max, min, limit, offset);

    /// <summary>
    /// 返回有序集合中指定成员的排名，有序集成员按分数值递减(从大到小)排序
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="member">成员</param>
    /// <returns></returns>
    public static Task<long?> ZRevRankAsync(string key, object member) => Instance.ZRevRankAsync(key, member);
    /// <summary>
    /// 返回有序集中，成员的分数值
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="member">成员</param>
    /// <returns></returns>
    public static Task<decimal?> ZScoreAsync(string key, object member) => Instance.ZScoreAsync(key, member);

    /// <summary>
    /// 计算给定的一个或多个有序集的并集，将结果集存储在新的有序集合 destination 中
    /// </summary>
    /// <param name="destination">新的有序集合，不含prefix前辍</param>
    /// <param name="weights">使用 WEIGHTS 选项，你可以为 每个 给定有序集 分别 指定一个乘法因子。如果没有指定 WEIGHTS 选项，乘法因子默认设置为 1 。</param>
    /// <param name="aggregate">Sum | Min | Max</param>
    /// <param name="keys">一个或多个有序集合，不含prefix前辍</param>
    /// <returns></returns>
    public static Task<long> ZUnionStoreAsync(string destination, decimal[] weights, RedisAggregate aggregate, params string[] keys) => Instance.ZUnionStoreAsync(destination, weights, aggregate, keys);

    /// <summary>
    /// 迭代有序集合中的元素
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="cursor">位置</param>
    /// <param name="pattern">模式</param>
    /// <param name="count">数量</param>
    /// <returns></returns>
    public static Task<RedisScan<(string member, decimal score)>> ZScanAsync(string key, long cursor, string pattern = null, long? count = null) =>
           Instance.ZScanAsync(key, cursor, pattern, count);
    /// <summary>
    /// 迭代有序集合中的元素
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="cursor">位置</param>
    /// <param name="pattern">模式</param>
    /// <param name="count">数量</param>
    /// <returns></returns>
    public static Task<RedisScan<(T member, decimal score)>> ZScanAsync<T>(string key, long cursor, string pattern = null, long? count = null) =>
           Instance.ZScanAsync<T>(key, cursor, pattern, count);

    /// <summary>
    /// 当有序集合的所有成员都具有相同的分值时，有序集合的元素会根据成员的字典序来进行排序，这个命令可以返回给定的有序集合键 key 中，值介于 min 和 max 之间的成员。
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
    /// <param name="max">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static Task<string[]> ZRangeByLexAsync(string key, string min, string max, long? limit = null, long offset = 0) =>
           Instance.ZRangeByLexAsync(key, min, max, limit, offset);
    /// <summary>
    /// 当有序集合的所有成员都具有相同的分值时，有序集合的元素会根据成员的字典序来进行排序，这个命令可以返回给定的有序集合键 key 中，值介于 min 和 max 之间的成员。
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
    /// <param name="max">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
    /// <param name="limit">返回多少成员</param>
    /// <param name="offset">返回条件偏移位置</param>
    /// <returns></returns>
    public static Task<T[]> ZRangeByLexAsync<T>(string key, string min, string max, long? limit = null, long offset = 0) =>
           Instance.ZRangeByLexAsync<T>(key, min, max, limit, offset);

    /// <summary>
    /// 当有序集合的所有成员都具有相同的分值时，有序集合的元素会根据成员的字典序来进行排序，这个命令可以返回给定的有序集合键 key 中，值介于 min 和 max 之间的成员。
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
    /// <param name="max">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
    /// <returns></returns>
    public static Task<long> ZRemRangeByLexAsync(string key, string min, string max) =>
           Instance.ZRemRangeByLexAsync(key, min, max);
    /// <summary>
    /// 当有序集合的所有成员都具有相同的分值时，有序集合的元素会根据成员的字典序来进行排序，这个命令可以返回给定的有序集合键 key 中，值介于 min 和 max 之间的成员。
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="min">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
    /// <param name="max">'(' 表示包含在范围，'[' 表示不包含在范围，'+' 正无穷大，'-' 负无限。 ZRANGEBYLEX zset - + ，命令将返回有序集合中的所有元素</param>
    /// <returns></returns>
    public static Task<long> ZLexCountAsync(string key, string min, string max) =>
           Instance.ZLexCountAsync(key, min, max);

    /// <summary>
    /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最高得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。 当返回多个元素时候，得分最高的元素将是第一个元素，然后是分数较低的元素。
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="count">数量</param>
    /// <returns></returns>
    public static Task<(string member, decimal score)[]> ZPopMaxAsync(string key, long count) =>
        Instance.ZPopMaxAsync(key, count);

    /// <summary>
    /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最高得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。 当返回多个元素时候，得分最高的元素将是第一个元素，然后是分数较低的元素。
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="count">数量</param>
    /// <returns></returns>
    public static Task<(T member, decimal score)[]> ZPopMaxAsync<T>(string key, long count) =>
        Instance.ZPopMaxAsync<T>(key, count);

    /// <summary>
    /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最低得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。 当返回多个元素时候，得分最低的元素将是第一个元素，然后是分数较高的元素。
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="count">数量</param>
    /// <returns></returns>
    public static Task<(string member, decimal score)[]> ZPopMinAsync(string key, long count) =>
        Instance.ZPopMinAsync(key, count);

    /// <summary>
    /// [redis-server 5.0.0] 删除并返回有序集合key中的最多count个具有最低得分的成员。如未指定，count的默认值为1。指定一个大于有序集合的基数的count不会产生错误。 当返回多个元素时候，得分最低的元素将是第一个元素，然后是分数较高的元素。
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="count">数量</param>
    /// <returns></returns>
    public static Task<(T member, decimal score)[]> ZPopMinAsync<T>(string key, long count) =>
        Instance.ZPopMinAsync<T>(key, count);

    #endregion

#endif

}
