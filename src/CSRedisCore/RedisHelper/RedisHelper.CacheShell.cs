﻿using System;
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
    #region 缓存壳
    /// <summary>
    /// 缓存壳
    /// </summary>
    /// <typeparam name="T">缓存类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="timeoutSeconds">缓存秒数</param>
    /// <param name="getData">获取源数据的函数</param>
    /// <returns></returns>
    public static T CacheShell<T>(string key, int timeoutSeconds, Func<T> getData) => Instance.CacheShell(key, timeoutSeconds, getData);
    /// <summary>
    /// 缓存壳(哈希表)
    /// </summary>
    /// <typeparam name="T">缓存类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="field">字段</param>
    /// <param name="timeoutSeconds">缓存秒数</param>
    /// <param name="getData">获取源数据的函数</param>
    /// <returns></returns>
    public static T CacheShell<T>(string key, string field, int timeoutSeconds, Func<T> getData) => Instance.CacheShell(key, field, timeoutSeconds, getData);

    /// <summary>
    /// 缓存壳(哈希表)，将 fields 每个元素存储到单独的缓存片，实现最大化复用
    /// </summary>
    /// <typeparam name="T">缓存类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="fields">字段</param>
    /// <param name="timeoutSeconds">缓存秒数</param>
    /// <param name="getData">获取源数据的函数，输入参数是没有缓存的 fields，返回值应该是 (field, value)[]</param>
    /// <returns></returns>
    public static (string key, T value)[] CacheShell<T>(string key, string[] fields, int timeoutSeconds, Func<string[], (string, T)[]> getData) => Instance.CacheShell(key, fields, timeoutSeconds, getData);
    #endregion

#if !net40

    #region 缓存壳
    /// <summary>
    /// 缓存壳
    /// </summary>
    /// <typeparam name="T">缓存类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="timeoutSeconds">缓存秒数</param>
    /// <param name="getDataAsync">获取源数据的函数</param>
    /// <returns></returns>
    public static Task<T> CacheShellAsync<T>(string key, int timeoutSeconds, Func<Task<T>> getDataAsync) => Instance.CacheShellAsync(key, timeoutSeconds, getDataAsync);
    /// <summary>
    /// 缓存壳(哈希表)
    /// </summary>
    /// <typeparam name="T">缓存类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="field">字段</param>
    /// <param name="timeoutSeconds">缓存秒数</param>
    /// <param name="getDataAsync">获取源数据的函数</param>
    /// <returns></returns>
    public static Task<T> CacheShellAsync<T>(string key, string field, int timeoutSeconds, Func<Task<T>> getDataAsync) => Instance.CacheShellAsync(key, field, timeoutSeconds, getDataAsync);
    /// <summary>
    /// 缓存壳(哈希表)，将 fields 每个元素存储到单独的缓存片，实现最大化复用
    /// </summary>
    /// <typeparam name="T">缓存类型</typeparam>
    /// <param name="key">不含prefix前辍</param>
    /// <param name="fields">字段</param>
    /// <param name="timeoutSeconds">缓存秒数</param>
    /// <param name="getDataAsync">获取源数据的函数，输入参数是没有缓存的 fields，返回值应该是 (field, value)[]</param>
    /// <returns></returns>
    public static Task<(string key, T value)[]> CacheShellAsync<T>(string key, string[] fields, int timeoutSeconds, Func<string[], Task<(string, T)[]>> getDataAsync) => Instance.CacheShellAsync(key, fields, timeoutSeconds, getDataAsync);
    #endregion

#endif

}
