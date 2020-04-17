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

    #region HyperLogLog

    /// <summary>
    /// 添加指定元素到 HyperLogLog
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="elements">元素</param>
    /// <returns></returns>
    public static bool PfAdd<T>(string key, params T[] elements) => Instance.PfAdd(key, elements);

    /// <summary>
    /// 返回给定 HyperLogLog 的基数估算值
    /// </summary>
    /// <param name="keys">不含prefix前辍</param>
    /// <returns></returns>
    [Obsolete("分区模式下，若keys分散在多个分区节点时，将报错")]
    public static long PfCount(params string[] keys) => Instance.PfCount(keys);

    /// <summary>
    /// 将多个 HyperLogLog 合并为一个 HyperLogLog
    /// </summary>
    /// <param name="destKey">新的 HyperLogLog，不含prefix前辍</param>
    /// <param name="sourceKeys">源 HyperLogLog，不含prefix前辍</param>
    /// <returns></returns>
    [Obsolete("分区模式下，若keys分散在多个分区节点时，将报错")]
    public static bool PfMerge(string destKey, params string[] sourceKeys) => Instance.PfMerge(destKey, sourceKeys);

    #endregion

#if !net40

    #region HyperLogLog

    /// <summary>
    /// 添加指定元素到 HyperLogLog
    /// </summary>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="elements">元素</param>
    /// <returns></returns>
    public static Task<bool> PfAddAsync<T>(string key, params T[] elements) => Instance.PfAddAsync(key, elements);

    /// <summary>
    /// 返回给定 HyperLogLog 的基数估算值
    /// </summary>
    /// <param name="keys">不含prefix前辍</param>
    /// <returns></returns>
    [Obsolete("分区模式下，若keys分散在多个分区节点时，将报错")]
    public static Task<long> PfCountAsync(params string[] keys) => Instance.PfCountAsync(keys);

    /// <summary>
    /// 将多个 HyperLogLog 合并为一个 HyperLogLog
    /// </summary>
    /// <param name="destKey">新的 HyperLogLog，不含prefix前辍</param>
    /// <param name="sourceKeys">源 HyperLogLog，不含prefix前辍</param>
    /// <returns></returns>
    [Obsolete("分区模式下，若keys分散在多个分区节点时，将报错")]
    public static Task<bool> PfMergeAsync(string destKey, params string[] sourceKeys) => Instance.PfMergeAsync(destKey, sourceKeys);
    #endregion

#endif

}
