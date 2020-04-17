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
        #region HyperLogLog
        /// <summary>
        /// 添加指定元素到 HyperLogLog
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="elements">元素</param>
        /// <returns></returns>
        public bool PfAdd<T>(string key, params T[] elements)
        {
            if (elements == null || elements.Any() == false) return false;
            var args = elements.Select(z => this.SerializeRedisValueInternal(z)).ToArray();
            return ExecuteScalar(key, (c, k) => c.Value.PfAdd(k, args));
        }
        /// <summary>
        /// 返回给定 HyperLogLog 的基数估算值
        /// </summary>
        /// <param name="keys">不含prefix前辍</param>
        /// <returns></returns>
        [Obsolete("分区模式下，若keys分散在多个分区节点时，将报错")]
        public long PfCount(params string[] keys) => NodesNotSupport(keys, 0, (c, k) => c.Value.PfCount(k));
        /// <summary>
        /// 将多个 HyperLogLog 合并为一个 HyperLogLog
        /// </summary>
        /// <param name="destKey">新的 HyperLogLog，不含prefix前辍</param>
        /// <param name="sourceKeys">源 HyperLogLog，不含prefix前辍</param>
        /// <returns></returns>
        [Obsolete("分区模式下，若keys分散在多个分区节点时，将报错")]
        public bool PfMerge(string destKey, params string[] sourceKeys) => NodesNotSupport(new[] { destKey }.Concat(sourceKeys).ToArray(), false, (c, k) => c.Value.PfMerge(k.First(), k.Skip(1).ToArray()) == "OK");
        #endregion


#if !net40

        #region HyperLogLog
        /// <summary>
        /// 添加指定元素到 HyperLogLog
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="elements">元素</param>
        /// <returns></returns>
        async public Task<bool> PfAddAsync<T>(string key, params T[] elements)
        {
            if (elements == null || elements.Any() == false) return false;
            var args = elements.Select(z => this.SerializeRedisValueInternal(z)).ToArray();
            return await ExecuteScalarAsync(key, (c, k) => c.Value.PfAddAsync(k, args));
        }
        /// <summary>
        /// 返回给定 HyperLogLog 的基数估算值
        /// </summary>
        /// <param name="keys">不含prefix前辍</param>
        /// <returns></returns>
        [Obsolete("分区模式下，若keys分散在多个分区节点时，将报错")]
        public Task<long> PfCountAsync(params string[] keys) => NodesNotSupportAsync(keys, 0, (c, k) => c.Value.PfCountAsync(k));
        /// <summary>
        /// 将多个 HyperLogLog 合并为一个 HyperLogLog
        /// </summary>
        /// <param name="destKey">新的 HyperLogLog，不含prefix前辍</param>
        /// <param name="sourceKeys">源 HyperLogLog，不含prefix前辍</param>
        /// <returns></returns>
        [Obsolete("分区模式下，若keys分散在多个分区节点时，将报错")]
        public Task<bool> PfMergeAsync(string destKey, params string[] sourceKeys) => NodesNotSupportAsync(new[] { destKey }.Concat(sourceKeys).ToArray(), false, async (c, k) => await c.Value.PfMergeAsync(k.First(), k.Skip(1).ToArray()) == "OK");
        #endregion


#endif

    }
}