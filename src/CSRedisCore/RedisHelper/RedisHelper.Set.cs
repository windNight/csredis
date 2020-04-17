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

    #region Set

    /// <summary>
    /// 向集合添加一个或多个成员
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="members">一个或多个成员</param>
    /// <returns></returns>
    public static long SAdd<T>(string key, params T[] members) => Instance.SAdd(key, members);
    /// <summary>
    /// 获取集合的成员数
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static long SCard(string key) => Instance.SCard(key);
    /// <summary>
    /// 返回给定所有集合的差集
    /// </summary>
    /// <param name="keys">不含prefix前辍</param>
    /// <returns></returns>
    public static string[] SDiff(params string[] keys) => Instance.SDiff(keys);
    /// <summary>
    /// 返回给定所有集合的差集
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="keys">不含prefix前辍</param>
    /// <returns></returns>
    public static T[] SDiff<T>(params string[] keys) => Instance.SDiff<T>(keys);
    /// <summary>
    /// 返回给定所有集合的差集并存储在 destination 中
    /// </summary>
    /// <param name="destination">新的无序集合，不含prefix前辍</param>
    /// <param name="keys">一个或多个无序集合，不含prefix前辍</param>
    /// <returns></returns>
    public static long SDiffStore(string destination, params string[] keys) => Instance.SDiffStore(destination, keys);
    /// <summary>
    /// 返回给定所有集合的交集
    /// </summary>
    /// <param name="keys">不含prefix前辍</param>
    /// <returns></returns>
    public static string[] SInter(params string[] keys) => Instance.SInter(keys);
    /// <summary>
    /// 返回给定所有集合的交集
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="keys">不含prefix前辍</param>
    /// <returns></returns>
    public static T[] SInter<T>(params string[] keys) => Instance.SInter<T>(keys);
    /// <summary>
    /// 返回给定所有集合的交集并存储在 destination 中
    /// </summary>
    /// <param name="destination">新的无序集合，不含prefix前辍</param>
    /// <param name="keys">一个或多个无序集合，不含prefix前辍</param>
    /// <returns></returns>
    public static long SInterStore(string destination, params string[] keys) => Instance.SInterStore(destination, keys);
    /// <summary>
    /// 判断 member 元素是否是集合 key 的成员
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="member">成员</param>
    /// <returns></returns>
    public static bool SIsMember(string key, object member) => Instance.SIsMember(key, member);
    /// <summary>
    /// 返回集合中的所有成员
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static string[] SMembers(string key) => Instance.SMembers(key);
    /// <summary>
    /// 返回集合中的所有成员
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static T[] SMembers<T>(string key) => Instance.SMembers<T>(key);
    /// <summary>
    /// 将 member 元素从 source 集合移动到 destination 集合
    /// </summary>
    /// <param name="source">无序集合key，不含prefix前辍</param>
    /// <param name="destination">目标无序集合key，不含prefix前辍</param>
    /// <param name="member">成员</param>
    /// <returns></returns>
    public static bool SMove(string source, string destination, object member) => Instance.SMove(source, destination, member);
    /// <summary>
    /// 移除并返回集合中的一个随机元素
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static string SPop(string key) => Instance.SPop(key);
    /// <summary>
    /// 移除并返回集合中的一个随机元素
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static T SPop<T>(string key) => Instance.SPop<T>(key);
    /// <summary>
    /// [redis-server 3.2] 移除并返回集合中的一个或多个随机元素
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="count">移除并返回的个数</param>
    /// <returns></returns>
    public static string[] SPop(string key, long count) => Instance.SPop(key, count);
    /// <summary>
    /// [redis-server 3.2] 移除并返回集合中的一个或多个随机元素
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="count">移除并返回的个数</param>
    /// <returns></returns>
    public static T[] SPop<T>(string key, long count) => Instance.SPop<T>(key, count);
    /// <summary>
    /// 返回集合中的一个随机元素
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static string SRandMember(string key) => Instance.SRandMember(key);
    /// <summary>
    /// 返回集合中的一个随机元素
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static T SRandMember<T>(string key) => Instance.SRandMember<T>(key);
    /// <summary>
    /// 返回集合中一个或多个随机数的元素
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="count">返回个数</param>
    /// <returns></returns>
    public static string[] SRandMembers(string key, int count = 1) => Instance.SRandMembers(key, count);
    /// <summary>
    /// 返回集合中一个或多个随机数的元素
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="count">返回个数</param>
    /// <returns></returns>
    public static T[] SRandMembers<T>(string key, int count = 1) => Instance.SRandMembers<T>(key, count);
    /// <summary>
    /// 移除集合中一个或多个成员
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="members">一个或多个成员</param>
    /// <returns></returns>
    public static long SRem<T>(string key, params T[] members) => Instance.SRem(key, members);
    /// <summary>
    /// 返回所有给定集合的并集
    /// </summary>
    /// <param name="keys">不含prefix前辍</param>
    /// <returns></returns>
    public static string[] SUnion(params string[] keys) => Instance.SUnion(keys);
    /// <summary>
    /// 返回所有给定集合的并集
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="keys">不含prefix前辍</param>
    /// <returns></returns>
    public static T[] SUnion<T>(params string[] keys) => Instance.SUnion<T>(keys);
    /// <summary>
    /// 所有给定集合的并集存储在 destination 集合中
    /// </summary>
    /// <param name="destination">新的无序集合，不含prefix前辍</param>
    /// <param name="keys">一个或多个无序集合，不含prefix前辍</param>
    /// <returns></returns>
    public static long SUnionStore(string destination, params string[] keys) => Instance.SUnionStore(destination, keys);
    /// <summary>
    /// 迭代集合中的元素
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="cursor">位置</param>
    /// <param name="pattern">模式</param>
    /// <param name="count">数量</param>
    /// <returns></returns>
    public static RedisScan<string> SScan(string key, long cursor, string pattern = null, long? count = null) =>
        Instance.SScan(key, cursor, pattern, count);
    /// <summary>
    /// 迭代集合中的元素
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="cursor">位置</param>
    /// <param name="pattern">模式</param>
    /// <param name="count">数量</param>
    /// <returns></returns>
    public static RedisScan<T> SScan<T>(string key, long cursor, string pattern = null, long? count = null) =>
           Instance.SScan<T>(key, cursor, pattern, count);
    
    #endregion

#if !net40

    #region Set
    /// <summary>
    /// 向集合添加一个或多个成员
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="members">一个或多个成员</param>
    /// <returns></returns>
    public static Task<long> SAddAsync<T>(string key, params T[] members) => Instance.SAddAsync(key, members);
    /// <summary>
    /// 获取集合的成员数
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<long> SCardAsync(string key) => Instance.SCardAsync(key);
    /// <summary>
    /// 返回给定所有集合的差集
    /// </summary>
    /// <param name="keys">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<string[]> SDiffAsync(params string[] keys) => Instance.SDiffAsync(keys);
    /// <summary>
    /// 返回给定所有集合的差集
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="keys">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<T[]> SDiffAsync<T>(params string[] keys) => Instance.SDiffAsync<T>(keys);
    /// <summary>
    /// 返回给定所有集合的差集并存储在 destination 中
    /// </summary>
    /// <param name="destination">新的无序集合，不含prefix前辍</param>
    /// <param name="keys">一个或多个无序集合，不含prefix前辍</param>
    /// <returns></returns>
    public static Task<long> SDiffStoreAsync(string destination, params string[] keys) => Instance.SDiffStoreAsync(destination, keys);
    /// <summary>
    /// 返回给定所有集合的交集
    /// </summary>
    /// <param name="keys">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<string[]> SInterAsync(params string[] keys) => Instance.SInterAsync(keys);
    /// <summary>
    /// 返回给定所有集合的交集
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="keys">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<T[]> SInterAsync<T>(params string[] keys) => Instance.SInterAsync<T>(keys);
    /// <summary>
    /// 返回给定所有集合的交集并存储在 destination 中
    /// </summary>
    /// <param name="destination">新的无序集合，不含prefix前辍</param>
    /// <param name="keys">一个或多个无序集合，不含prefix前辍</param>
    /// <returns></returns>
    public static Task<long> SInterStoreAsync(string destination, params string[] keys) => Instance.SInterStoreAsync(destination, keys);
    /// <summary>
    /// 判断 member 元素是否是集合 key 的成员
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="member">成员</param>
    /// <returns></returns>
    public static Task<bool> SIsMemberAsync(string key, object member) => Instance.SIsMemberAsync(key, member);
    /// <summary>
    /// 返回集合中的所有成员
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<string[]> SMembersAsync(string key) => Instance.SMembersAsync(key);
    /// <summary>
    /// 返回集合中的所有成员
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<T[]> SMembersAsync<T>(string key) => Instance.SMembersAsync<T>(key);
    /// <summary>
    /// 将 member 元素从 source 集合移动到 destination 集合
    /// </summary>
    /// <param name="source">无序集合key，不含prefix前辍</param>
    /// <param name="destination">目标无序集合key，不含prefix前辍</param>
    /// <param name="member">成员</param>
    /// <returns></returns>
    public static Task<bool> SMoveAsync(string source, string destination, object member) => Instance.SMoveAsync(source, destination, member);
    /// <summary>
    /// 移除并返回集合中的一个随机元素
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<string> SPopAsync(string key) => Instance.SPopAsync(key);
    /// <summary>
    /// 移除并返回集合中的一个随机元素
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<T> SPopAsync<T>(string key) => Instance.SPopAsync<T>(key);
    /// <summary>
    /// [redis-server 3.2] 移除并返回集合中的一个或多个随机元素
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="count">移除并返回的个数</param>
    /// <returns></returns>
    public static Task<string[]> SPopAsync(string key, long count) => Instance.SPopAsync(key, count);
    /// <summary>
    /// [redis-server 3.2] 移除并返回集合中的一个或多个随机元素
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="count">移除并返回的个数</param>
    /// <returns></returns>
    public static Task<T[]> SPopAsync<T>(string key, long count) => Instance.SPopAsync<T>(key, count);
    /// <summary>
    /// 返回集合中的一个随机元素
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<string> SRandMemberAsync(string key) => Instance.SRandMemberAsync(key);
    /// <summary>
    /// 返回集合中的一个随机元素
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<T> SRandMemberAsync<T>(string key) => Instance.SRandMemberAsync<T>(key);
    /// <summary>
    /// 返回集合中一个或多个随机数的元素
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="count">返回个数</param>
    /// <returns></returns>
    public static Task<string[]> SRandMembersAsync(string key, int count = 1) => Instance.SRandMembersAsync(key, count);
    /// <summary>
    /// 返回集合中一个或多个随机数的元素
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="count">返回个数</param>
    /// <returns></returns>
    public static Task<T[]> SRandMembersAsync<T>(string key, int count = 1) => Instance.SRandMembersAsync<T>(key, count);
    /// <summary>
    /// 移除集合中一个或多个成员
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="members">一个或多个成员</param>
    /// <returns></returns>
    public static Task<long> SRemAsync<T>(string key, params T[] members) => Instance.SRemAsync(key, members);
    /// <summary>
    /// 返回所有给定集合的并集
    /// </summary>
    /// <param name="keys">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<string[]> SUnionAsync(params string[] keys) => Instance.SUnionAsync(keys);
    /// <summary>
    /// 返回所有给定集合的并集
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="keys">不含prefix前辍</param>
    /// <returns></returns>
    public static Task<T[]> SUnionAsync<T>(params string[] keys) => Instance.SUnionAsync<T>(keys);
    /// <summary>
    /// 所有给定集合的并集存储在 destination 集合中
    /// </summary>
    /// <param name="destination">新的无序集合，不含prefix前辍</param>
    /// <param name="keys">一个或多个无序集合，不含prefix前辍</param>
    /// <returns></returns>
    public static Task<long> SUnionStoreAsync(string destination, params string[] keys) => Instance.SUnionStoreAsync(destination, keys);
    /// <summary>
    /// 迭代集合中的元素
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="cursor">位置</param>
    /// <param name="pattern">模式</param>
    /// <param name="count">数量</param>
    /// <returns></returns>
    public static Task<RedisScan<string>> SScanAsync(string key, long cursor, string pattern = null, long? count = null) => Instance.SScanAsync(key, cursor, pattern, count);
    /// <summary>
    /// 迭代集合中的元素
    /// </summary>
    /// <typeparam name="T">byte[] 或其他类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="cursor">位置</param>
    /// <param name="pattern">模式</param>
    /// <param name="count">数量</param>
    /// <returns></returns>
    public static Task<RedisScan<T>> SScanAsync<T>(string key, long cursor, string pattern = null, long? count = null) => Instance.SScanAsync<T>(key, cursor, pattern, count);
    #endregion

#endif

}
