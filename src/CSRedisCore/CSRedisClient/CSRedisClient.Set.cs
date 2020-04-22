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

        #region Set
        /// <summary>
        /// 向集合添加一个或多个成员
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="members">一个或多个成员</param>
        /// <returns></returns>
        public long SAdd<T>(string key, params T[] members)
        {
            if (members == null || members.Any() == false) return 0;
            var args = members.Select(z => this.SerializeRedisValueInternal(z)).ToArray();
            return ExecuteScalar(key, (c, k) => c.Value.SAdd(k, args));
        }
        /// <summary>
        /// 获取集合的成员数
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public long SCard(string key) => ExecuteScalar(key, (c, k) => c.Value.SCard(k));
        /// <summary>
        /// 返回给定所有集合的差集
        /// </summary>
        /// <param name="keys">不含prefix前辍</param>
        /// <returns></returns>
        public string[] SDiff(params string[] keys) => NodesNotSupport(keys, new string[0], (c, k) => c.Value.SDiff(k));
        /// <summary>
        /// 返回给定所有集合的差集
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="keys">不含prefix前辍</param>
        /// <returns></returns>
        public T[] SDiff<T>(params string[] keys) => this.DeserializeRedisValueArrayInternal<T>(NodesNotSupport(keys, new byte[0][], (c, k) => c.Value.SDiffBytes(k)));
        /// <summary>
        /// 返回给定所有集合的差集并存储在 destination 中
        /// </summary>
        /// <param name="destination">新的无序集合，不含prefix前辍</param>
        /// <param name="keys">一个或多个无序集合，不含prefix前辍</param>
        /// <returns></returns>
        public long SDiffStore(string destination, params string[] keys) => NodesNotSupport(new[] { destination }.Concat(keys).ToArray(), 0, (c, k) => c.Value.SDiffStore(k.First(), k.Skip(1).ToArray()));
        /// <summary>
        /// 返回给定所有集合的交集
        /// </summary>
        /// <param name="keys">不含prefix前辍</param>
        /// <returns></returns>
        public string[] SInter(params string[] keys) => NodesNotSupport(keys, new string[0], (c, k) => c.Value.SInter(k));
        /// <summary>
        /// 返回给定所有集合的交集
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="keys">不含prefix前辍</param>
        /// <returns></returns>
        public T[] SInter<T>(params string[] keys) => this.DeserializeRedisValueArrayInternal<T>(NodesNotSupport(keys, new byte[0][], (c, k) => c.Value.SInterBytes(k)));
        /// <summary>
        /// 返回给定所有集合的交集并存储在 destination 中
        /// </summary>
        /// <param name="destination">新的无序集合，不含prefix前辍</param>
        /// <param name="keys">一个或多个无序集合，不含prefix前辍</param>
        /// <returns></returns>
        public long SInterStore(string destination, params string[] keys) => NodesNotSupport(new[] { destination }.Concat(keys).ToArray(), 0, (c, k) => c.Value.SInterStore(k.First(), k.Skip(1).ToArray()));
        /// <summary>
        /// 判断 member 元素是否是集合 key 的成员
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member">成员</param>
        /// <returns></returns>
        public bool SIsMember(string key, object member)
        {
            var args = this.SerializeRedisValueInternal(member);
            return ExecuteScalar(key, (c, k) => c.Value.SIsMember(k, args));
        }
        /// <summary>
        /// 返回集合中的所有成员
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public string[] SMembers(string key) => ExecuteScalar(key, (c, k) => c.Value.SMembers(k));
        /// <summary>
        /// 返回集合中的所有成员
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public T[] SMembers<T>(string key) => this.DeserializeRedisValueArrayInternal<T>(ExecuteScalar(key, (c, k) => c.Value.SMembersBytes(k)));
        /// <summary>
        /// 将 member 元素从 source 集合移动到 destination 集合
        /// </summary>
        /// <param name="source">无序集合key，不含prefix前辍</param>
        /// <param name="destination">目标无序集合key，不含prefix前辍</param>
        /// <param name="member">成员</param>
        /// <returns></returns>
        public bool SMove(string source, string destination, object member)
        {
            string rule = string.Empty;
            if (Nodes.Count > 1)
            {
                var rule1 = NodeRuleRaw(source);
                var rule2 = NodeRuleRaw(destination);
                if (rule1 != rule2)
                {
                    if (SRem(source, member) <= 0) return false;
                    return SAdd(destination, member) > 0;
                }
                rule = rule1;
            }
            var pool = Nodes.TryGetValue(rule, out var b) ? b : Nodes.First().Value;
            var key1 = string.Concat(pool.Prefix, source);
            var key2 = string.Concat(pool.Prefix, destination);
            var args = this.SerializeRedisValueInternal(member);
            return GetAndExecute(pool, conn => conn.Value.SMove(key1, key2, args));
        }
        /// <summary>
        /// 移除并返回集合中的一个随机元素
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public string SPop(string key) => ExecuteScalar(key, (c, k) => c.Value.SPop(k));
        /// <summary>
        /// 移除并返回集合中的一个随机元素
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public T SPop<T>(string key) => this.DeserializeRedisValueInternal<T>(ExecuteScalar(key, (c, k) => c.Value.SPopBytes(k)));
        /// <summary>
        /// [redis-server 3.2] 移除并返回集合中的一个或多个随机元素
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="count">移除并返回的个数</param>
        /// <returns></returns>
        public string[] SPop(string key, long count) => ExecuteScalar(key, (c, k) => c.Value.SPop(k, count));
        /// <summary>
        /// [redis-server 3.2] 移除并返回集合中的一个或多个随机元素
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="count">移除并返回的个数</param>
        /// <returns></returns>
        public T[] SPop<T>(string key, long count) => this.DeserializeRedisValueArrayInternal<T>(ExecuteScalar(key, (c, k) => c.Value.SPopBytes(k, count)));
        /// <summary>
        /// 返回集合中的一个随机元素
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public string SRandMember(string key) => ExecuteScalar(key, (c, k) => c.Value.SRandMember(k));
        /// <summary>
        /// 返回集合中的一个随机元素
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public T SRandMember<T>(string key) => this.DeserializeRedisValueInternal<T>(ExecuteScalar(key, (c, k) => c.Value.SRandMemberBytes(k)));
        /// <summary>
        /// 返回集合中一个或多个随机数的元素
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="count">返回个数</param>
        /// <returns></returns>
        public string[] SRandMembers(string key, int count = 1) => ExecuteScalar(key, (c, k) => c.Value.SRandMembers(k, count));
        /// <summary>
        /// 返回集合中一个或多个随机数的元素
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="count">返回个数</param>
        /// <returns></returns>
        public T[] SRandMembers<T>(string key, int count = 1) => this.DeserializeRedisValueArrayInternal<T>(ExecuteScalar(key, (c, k) => c.Value.SRandMembersBytes(k, count)));
        /// <summary>
        /// 移除集合中一个或多个成员
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="members">一个或多个成员</param>
        /// <returns></returns>
        public long SRem<T>(string key, params T[] members)
        {
            if (members == null || members.Any() == false) return 0;
            var args = members.Select(z => this.SerializeRedisValueInternal(z)).ToArray();
            return ExecuteScalar(key, (c, k) => c.Value.SRem(k, args));
        }
        /// <summary>
        /// 返回所有给定集合的并集
        /// </summary>
        /// <param name="keys">不含prefix前辍</param>
        /// <returns></returns>
        public string[] SUnion(params string[] keys) => NodesNotSupport(keys, new string[0], (c, k) => c.Value.SUnion(k));
        /// <summary>
        /// 返回所有给定集合的并集
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="keys">不含prefix前辍</param>
        /// <returns></returns>
        public T[] SUnion<T>(params string[] keys) => this.DeserializeRedisValueArrayInternal<T>(NodesNotSupport(keys, new byte[0][], (c, k) => c.Value.SUnionBytes(k)));
        /// <summary>
        /// 所有给定集合的并集存储在 destination 集合中
        /// </summary>
        /// <param name="destination">新的无序集合，不含prefix前辍</param>
        /// <param name="keys">一个或多个无序集合，不含prefix前辍</param>
        /// <returns></returns>
        public long SUnionStore(string destination, params string[] keys) => NodesNotSupport(new[] { destination }.Concat(keys).ToArray(), 0, (c, k) => c.Value.SUnionStore(k.First(), k.Skip(1).ToArray()));
        /// <summary>
        /// 迭代集合中的元素
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="cursor">位置</param>
        /// <param name="pattern">模式</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public RedisScan<string> SScan(string key, long cursor, string pattern = null, long? count = null) => ExecuteScalar(key, (c, k) => c.Value.SScan(k, cursor, pattern, count));
        /// <summary>
        /// 迭代集合中的元素
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="cursor">位置</param>
        /// <param name="pattern">模式</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public RedisScan<T> SScan<T>(string key, long cursor, string pattern = null, long? count = null)
        {
            var scan = ExecuteScalar(key, (c, k) => c.Value.SScanBytes(k, cursor, pattern, count));
            return new RedisScan<T>(scan.Cursor, this.DeserializeRedisValueArrayInternal<T>(scan.Items));
        }
        #endregion

#if !net40

        #region Set
        /// <summary>
        /// 向集合添加一个或多个成员
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="members">一个或多个成员</param>
        /// <returns></returns>
        public async Task<long> SAddAsync<T>(string key, params T[] members)
        {
            if (members == null || members.Any() == false) return 0;
            var args = members.Select(z => this.SerializeRedisValueInternal(z)).ToArray();
            return await ExecuteScalarAsync(key, (c, k) => c.Value.SAddAsync(k, args));
        }
        /// <summary>
        /// 获取集合的成员数
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public Task<long> SCardAsync(string key) => ExecuteScalarAsync(key, (c, k) => c.Value.SCardAsync(k));
        /// <summary>
        /// 返回给定所有集合的差集
        /// </summary>
        /// <param name="keys">不含prefix前辍</param>
        /// <returns></returns>
        public Task<string[]> SDiffAsync(params string[] keys) => NodesNotSupportAsync(keys, new string[0], (c, k) => c.Value.SDiffAsync(k));
        /// <summary>
        /// 返回给定所有集合的差集
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="keys">不含prefix前辍</param>
        /// <returns></returns>
        public async Task<T[]> SDiffAsync<T>(params string[] keys) => this.DeserializeRedisValueArrayInternal<T>(await NodesNotSupportAsync(keys, new byte[0][], (c, k) => c.Value.SDiffBytesAsync(k)));
        /// <summary>
        /// 返回给定所有集合的差集并存储在 destination 中
        /// </summary>
        /// <param name="destination">新的无序集合，不含prefix前辍</param>
        /// <param name="keys">一个或多个无序集合，不含prefix前辍</param>
        /// <returns></returns>
        public Task<long> SDiffStoreAsync(string destination, params string[] keys) => NodesNotSupportAsync(new[] { destination }.Concat(keys).ToArray(), 0, (c, k) => c.Value.SDiffStoreAsync(k.First(), k.Skip(1).ToArray()));
        /// <summary>
        /// 返回给定所有集合的交集
        /// </summary>
        /// <param name="keys">不含prefix前辍</param>
        /// <returns></returns>
        public Task<string[]> SInterAsync(params string[] keys) => NodesNotSupportAsync(keys, new string[0], (c, k) => c.Value.SInterAsync(k));
        /// <summary>
        /// 返回给定所有集合的交集
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="keys">不含prefix前辍</param>
        /// <returns></returns>
        public async Task<T[]> SInterAsync<T>(params string[] keys) => this.DeserializeRedisValueArrayInternal<T>(await NodesNotSupportAsync(keys, new byte[0][], (c, k) => c.Value.SInterBytesAsync(k)));
        /// <summary>
        /// 返回给定所有集合的交集并存储在 destination 中
        /// </summary>
        /// <param name="destination">新的无序集合，不含prefix前辍</param>
        /// <param name="keys">一个或多个无序集合，不含prefix前辍</param>
        /// <returns></returns>
        public Task<long> SInterStoreAsync(string destination, params string[] keys) => NodesNotSupportAsync(new[] { destination }.Concat(keys).ToArray(), 0, (c, k) => c.Value.SInterStoreAsync(k.First(), k.Skip(1).ToArray()));
        /// <summary>
        /// 判断 member 元素是否是集合 key 的成员
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member">成员</param>
        /// <returns></returns>
        public Task<bool> SIsMemberAsync(string key, object member)
        {
            var args = this.SerializeRedisValueInternal(member);
            return ExecuteScalarAsync(key, (c, k) => c.Value.SIsMemberAsync(k, args));
        }
        /// <summary>
        /// 返回集合中的所有成员
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public Task<string[]> SMembersAsync(string key) => ExecuteScalarAsync(key, (c, k) => c.Value.SMembersAsync(k));
        /// <summary>
        /// 返回集合中的所有成员
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public async Task<T[]> SMembersAsync<T>(string key) => this.DeserializeRedisValueArrayInternal<T>(await ExecuteScalarAsync(key, (c, k) => c.Value.SMembersBytesAsync(k)));
        /// <summary>
        /// 将 member 元素从 source 集合移动到 destination 集合
        /// </summary>
        /// <param name="source">无序集合key，不含prefix前辍</param>
        /// <param name="destination">目标无序集合key，不含prefix前辍</param>
        /// <param name="member">成员</param>
        /// <returns></returns>
        public async Task<bool> SMoveAsync(string source, string destination, object member)
        {
            string rule = string.Empty;
            if (Nodes.Count > 1)
            {
                var rule1 = NodeRuleRaw(source);
                var rule2 = NodeRuleRaw(destination);
                if (rule1 != rule2)
                {
                    if (await SRemAsync(source, member) <= 0) return false;
                    return await SAddAsync(destination, member) > 0;
                }
                rule = rule1;
            }
            var pool = Nodes.TryGetValue(rule, out var b) ? b : Nodes.First().Value;
            var key1 = string.Concat(pool.Prefix, source);
            var key2 = string.Concat(pool.Prefix, destination);
            var args = this.SerializeRedisValueInternal(member);
            return await GetAndExecuteAsync(pool, conn => conn.Value.SMoveAsync(key1, key2, args));
        }
        /// <summary>
        /// 移除并返回集合中的一个随机元素
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public Task<string> SPopAsync(string key) => ExecuteScalarAsync(key, (c, k) => c.Value.SPopAsync(k));
        /// <summary>
        /// 移除并返回集合中的一个随机元素
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public async Task<T> SPopAsync<T>(string key) => this.DeserializeRedisValueInternal<T>(await ExecuteScalarAsync(key, (c, k) => c.Value.SPopBytesAsync(k)));
        /// <summary>
        /// [redis-server 3.2] 移除并返回集合中的一个或多个随机元素
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="count">移除并返回的个数</param>
        /// <returns></returns>
        public Task<string[]> SPopAsync(string key, long count) => ExecuteScalarAsync(key, (c, k) => c.Value.SPopAsync(k, count));
        /// <summary>
        /// [redis-server 3.2] 移除并返回集合中的一个或多个随机元素
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="count">移除并返回的个数</param>
        /// <returns></returns>
        public async Task<T[]> SPopAsync<T>(string key, long count) => this.DeserializeRedisValueArrayInternal<T>(await ExecuteScalarAsync(key, (c, k) => c.Value.SPopBytesAsync(k, count)));
        /// <summary>
        /// 返回集合中的一个随机元素
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public Task<string> SRandMemberAsync(string key) => ExecuteScalarAsync(key, (c, k) => c.Value.SRandMemberAsync(k));
        /// <summary>
        /// 返回集合中的一个随机元素
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public async Task<T> SRandMemberAsync<T>(string key) => this.DeserializeRedisValueInternal<T>(await ExecuteScalarAsync(key, (c, k) => c.Value.SRandMemberBytesAsync(k)));
        /// <summary>
        /// 返回集合中一个或多个随机数的元素
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="count">返回个数</param>
        /// <returns></returns>
        public Task<string[]> SRandMembersAsync(string key, int count = 1) => ExecuteScalarAsync(key, (c, k) => c.Value.SRandMembersAsync(k, count));
        /// <summary>
        /// 返回集合中一个或多个随机数的元素
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="count">返回个数</param>
        /// <returns></returns>
        public async Task<T[]> SRandMembersAsync<T>(string key, int count = 1) => this.DeserializeRedisValueArrayInternal<T>(await ExecuteScalarAsync(key, (c, k) => c.Value.SRandMembersBytesAsync(k, count)));
        /// <summary>
        /// 移除集合中一个或多个成员
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="members">一个或多个成员</param>
        /// <returns></returns>
        public async Task<long> SRemAsync<T>(string key, params T[] members)
        {
            if (members == null || members.Any() == false) return 0;
            var args = members.Select(z => this.SerializeRedisValueInternal(z)).ToArray();
            return await ExecuteScalarAsync(key, (c, k) => c.Value.SRemAsync(k, args));
        }
        /// <summary>
        /// 返回所有给定集合的并集
        /// </summary>
        /// <param name="keys">不含prefix前辍</param>
        /// <returns></returns>
        public Task<string[]> SUnionAsync(params string[] keys) => NodesNotSupportAsync(keys, new string[0], (c, k) => c.Value.SUnionAsync(k));
        /// <summary>
        /// 返回所有给定集合的并集
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="keys">不含prefix前辍</param>
        /// <returns></returns>
        public async Task<T[]> SUnionAsync<T>(params string[] keys) => this.DeserializeRedisValueArrayInternal<T>(await NodesNotSupportAsync(keys, new byte[0][], (c, k) => c.Value.SUnionBytesAsync(k)));
        /// <summary>
        /// 所有给定集合的并集存储在 destination 集合中
        /// </summary>
        /// <param name="destination">新的无序集合，不含prefix前辍</param>
        /// <param name="keys">一个或多个无序集合，不含prefix前辍</param>
        /// <returns></returns>
        public Task<long> SUnionStoreAsync(string destination, params string[] keys) => NodesNotSupportAsync(new[] { destination }.Concat(keys).ToArray(), 0, (c, k) => c.Value.SUnionStoreAsync(k.First(), k.Skip(1).ToArray()));
        /// <summary>
        /// 迭代集合中的元素
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="cursor">位置</param>
        /// <param name="pattern">模式</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public Task<RedisScan<string>> SScanAsync(string key, long cursor, string pattern = null, long? count = null) => ExecuteScalarAsync(key, (c, k) => c.Value.SScanAsync(k, cursor, pattern, count));
        /// <summary>
        /// 迭代集合中的元素
        /// </summary>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="cursor">位置</param>
        /// <param name="pattern">模式</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public async Task<RedisScan<T>> SScanAsync<T>(string key, long cursor, string pattern = null, long? count = null)
        {
            var scan = await ExecuteScalarAsync(key, (c, k) => c.Value.SScanBytesAsync(k, cursor, pattern, count));
            return new RedisScan<T>(scan.Cursor, this.DeserializeRedisValueArrayInternal<T>(scan.Items));
        }
        #endregion


#endif

    }
}