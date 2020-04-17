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
    #region 连接命令

    /// <summary>
    /// 打印字符串
    /// </summary>
    /// <param name="nodeKey">分区key</param>
    /// <param name="message">消息</param>
    /// <returns></returns>
    public static string Echo(string nodeKey, string message) => Instance.Echo(nodeKey, message);

    /// <summary>
    /// 打印字符串
    /// </summary>
    /// <param name="message">消息</param>
    /// <returns></returns>
    public static string Echo(string message) => Instance.Echo(message);

    /// <summary>
    /// 查看服务是否运行
    /// </summary>
    /// <param name="nodeKey">分区key</param>
    /// <returns></returns>
    public static bool Ping(string nodeKey) => Instance.Ping(nodeKey);

    /// <summary>
    /// 查看服务是否运行
    /// </summary>
    /// <returns></returns>
    public static bool Ping() => Instance.Ping(); 

    #endregion

#if !net40

    #region 连接命令

    /// <summary>
    /// 打印字符串
    /// </summary>
    /// <param name="nodeKey">分区key</param>
    /// <param name="message">消息</param>
    /// <returns></returns>
    public static Task<string> EchoAsync(string nodeKey, string message) => Instance.EchoAsync(nodeKey, message);

    /// <summary>
    /// 打印字符串
    /// </summary>
    /// <param name="message">消息</param>
    /// <returns></returns>
    public static Task<string> EchoAsync(string message) => Instance.EchoAsync(message);

    /// <summary>
    /// 查看服务是否运行
    /// </summary>
    /// <param name="nodeKey">分区key</param>
    /// <returns></returns>
    public static Task<bool> PingAsync(string nodeKey) => Instance.PingAsync(nodeKey);

    /// <summary>
    /// 查看服务是否运行
    /// </summary>
    /// <returns></returns>
    public static Task<bool> PingAsync() => Instance.PingAsync();
    #endregion

#endif

}
