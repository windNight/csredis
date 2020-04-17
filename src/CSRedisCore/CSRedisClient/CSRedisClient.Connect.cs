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

        #region 连接命令

        /// <summary>
        /// 验证密码是否正确
        /// </summary>
        /// <param name="nodeKey">分区key</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [Obsolete("不建议手工执行，连接池自己管理最佳")]
        private bool Auth(string nodeKey, string password) => GetAndExecute(GetNodeOrThrowNotFound(nodeKey), c => c.Value.Auth(password)) == "OK";
        /// <summary>
        /// 打印字符串
        /// </summary>
        /// <param name="nodeKey">分区key</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public string Echo(string nodeKey, string message) => GetAndExecute(GetNodeOrThrowNotFound(nodeKey), c => c.Value.Echo(message));
        /// <summary>
        /// 打印字符串
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public string Echo(string message) => GetAndExecute(Nodes.First().Value, c => c.Value.Echo(message));
        /// <summary>
        /// 查看服务是否运行
        /// </summary>
        /// <param name="nodeKey">分区key</param>
        /// <returns></returns>
        public bool Ping(string nodeKey) => GetAndExecute(GetNodeOrThrowNotFound(nodeKey), c => c.Value.Ping()) == "PONG";
        /// <summary>
        /// 查看服务是否运行
        /// </summary>
        /// <returns></returns>
        public bool Ping() => GetAndExecute(Nodes.First().Value, c => c.Value.Ping()) == "PONG";
        /// <summary>
        /// 关闭当前连接
        /// </summary>
        /// <param name="nodeKey">分区key</param>
        /// <returns></returns>
        [Obsolete("不建议手工执行，连接池自己管理最佳")]
        private bool Quit(string nodeKey) => GetAndExecute(GetNodeOrThrowNotFound(nodeKey), c => c.Value.Quit()) == "OK";
        /// <summary>
        /// 切换到指定的数据库
        /// </summary>
        /// <param name="nodeKey">分区key</param>
        /// <param name="index">数据库</param>
        /// <returns></returns>
        [Obsolete("不建议手工执行，连接池所有连接应该指向同一数据库，若手工修改将导致数据的不一致")]
        private bool Select(string nodeKey, int index) => GetAndExecute(GetNodeOrThrowNotFound(nodeKey), c => c.Value.Select(index)) == "OK";

        #endregion

#if !net40

        #region 连接命令
        /// <summary>
        /// 验证密码是否正确
        /// </summary>
        /// <param name="nodeKey">分区key</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [Obsolete("不建议手工执行，连接池自己管理最佳")]
        private Task<bool> AuthAsync(string nodeKey, string password) => GetAndExecuteAsync(GetNodeOrThrowNotFound(nodeKey), async c => await c.Value.AuthAsync(password) == "OK");
        /// <summary>
        /// 打印字符串
        /// </summary>
        /// <param name="nodeKey">分区key</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public Task<string> EchoAsync(string nodeKey, string message) => GetAndExecuteAsync(GetNodeOrThrowNotFound(nodeKey), c => c.Value.EchoAsync(message));
        /// <summary>
        /// 打印字符串
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public Task<string> EchoAsync(string message) => GetAndExecuteAsync(Nodes.First().Value, c => c.Value.EchoAsync(message));
        /// <summary>
        /// 查看服务是否运行
        /// </summary>
        /// <param name="nodeKey">分区key</param>
        /// <returns></returns>
        public Task<bool> PingAsync(string nodeKey) => GetAndExecuteAsync(GetNodeOrThrowNotFound(nodeKey), async c => await c.Value.PingAsync() == "PONG");
        /// <summary>
        /// 查看服务是否运行
        /// </summary>
        /// <returns></returns>
        public Task<bool> PingAsync() => GetAndExecuteAsync(Nodes.First().Value, async c => await c.Value.PingAsync() == "PONG");
        /// <summary>
        /// 关闭当前连接
        /// </summary>
        /// <param name="nodeKey">分区key</param>
        /// <returns></returns>
        [Obsolete("不建议手工执行，连接池自己管理最佳")]
        private Task<bool> QuitAsync(string nodeKey) => GetAndExecuteAsync(GetNodeOrThrowNotFound(nodeKey), async c => await c.Value.QuitAsync() == "OK");
        /// <summary>
        /// 切换到指定的数据库
        /// </summary>
        /// <param name="nodeKey">分区key</param>
        /// <param name="index">数据库</param>
        /// <returns></returns>
        [Obsolete("不建议手工执行，连接池所有连接应该指向同一数据库，若手工修改将导致数据的不一致")]
        private Task<bool> SelectAsync(string nodeKey, int index) => GetAndExecuteAsync(GetNodeOrThrowNotFound(nodeKey), async c => await c.Value.SelectAsync(index) == "OK");

        #endregion

#endif

    }
}