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

        #region 服务器命令
        /// <summary>
        /// 在所有分区节点上，执行服务器命令
        /// </summary>
        public NodesServerManagerProvider NodesServerManager { get; set; }
        public partial class NodesServerManagerProvider
        {
            private CSRedisClient _csredis;

            public NodesServerManagerProvider(CSRedisClient csredis)
            {
                _csredis = csredis;
            }

            /// <summary>
            /// 异步执行一个 AOF（AppendOnly File） 文件重写操作
            /// </summary>
            /// <returns></returns>
            public (string node, string value)[] BgRewriteAof() => _csredis.Nodes.Values.Select(a => _csredis.GetAndExecute(a, c => (a.Key, c.Value.BgRewriteAof()))).ToArray();
            /// <summary>
            /// 在后台异步保存当前数据库的数据到磁盘
            /// </summary>
            /// <returns></returns>
            public (string node, string value)[] BgSave() => _csredis.Nodes.Values.Select(a => _csredis.GetAndExecute(a, c => (a.Key, c.Value.BgSave()))).ToArray();
            /// <summary>
            /// 关闭客户端连接
            /// </summary>
            /// <param name="ip">ip</param>
            /// <param name="port">端口</param>
            /// <returns></returns>
            public (string node, string value)[] ClientKill(string ip, int port) => _csredis.Nodes.Values.Select(a => _csredis.GetAndExecute(a, c => (a.Key, c.Value.ClientKill(ip, port)))).ToArray();
            /// <summary>
            /// 关闭客户端连接
            /// </summary>
            /// <param name="addr">ip:port</param>
            /// <param name="id">客户唯一标识</param>
            /// <param name="type">类型：normal | slave | pubsub</param>
            /// <param name="skipMe">跳过自己</param>
            /// <returns></returns>
            public (string node, long value)[] ClientKill(string addr = null, string id = null, ClientKillType? type = null, bool? skipMe = null) => _csredis.Nodes.Values.Select(a => _csredis.GetAndExecute(a, c => (a.Key, c.Value.ClientKill(addr, id, type?.ToString(), skipMe)))).ToArray();
            /// <summary>
            /// 获取连接到服务器的客户端连接列表
            /// </summary>
            /// <returns></returns>
            public (string node, string value)[] ClientList() => _csredis.Nodes.Values.Select(a => _csredis.GetAndExecute(a, c => (a.Key, c.Value.ClientList()))).ToArray();
            /// <summary>
            /// 获取连接的名称
            /// </summary>
            /// <returns></returns>
            public (string node, string value)[] ClientGetName() => _csredis.Nodes.Values.Select(a => _csredis.GetAndExecute(a, c => (a.Key, c.Value.ClientGetName()))).ToArray();
            /// <summary>
            /// 在指定时间内终止运行来自客户端的命令
            /// </summary>
            /// <param name="timeout">阻塞时间</param>
            /// <returns></returns>
            public (string node, string value)[] ClientPause(TimeSpan timeout) => _csredis.Nodes.Values.Select(a => _csredis.GetAndExecute(a, c => (a.Key, c.Value.ClientPause(timeout)))).ToArray();
            /// <summary>
            /// 设置当前连接的名称
            /// </summary>
            /// <param name="connectionName">连接名称</param>
            /// <returns></returns>
            public (string node, string value)[] ClientSetName(string connectionName) => _csredis.Nodes.Values.Select(a => _csredis.GetAndExecute(a, c => (a.Key, c.Value.ClientSetName(connectionName)))).ToArray();
            /// <summary>
            /// 返回当前服务器时间
            /// </summary>
            /// <returns></returns>
            public (string node, DateTime value)[] Time() => _csredis.Nodes.Values.Select(a => _csredis.GetAndExecute(a, c => (a.Key, c.Value.Time()))).ToArray();
            /// <summary>
            /// 获取指定配置参数的值
            /// </summary>
            /// <param name="parameter">参数</param>
            /// <returns></returns>
            public (string node, Dictionary<string, string> value)[] ConfigGet(string parameter) => _csredis.Nodes.Values.Select(a => _csredis.GetAndExecute(a, c => (a.Key, c.Value.ConfigGet(parameter).ToDictionary(z => z.Item1, y => y.Item2)))).ToArray();
            /// <summary>
            /// 对启动 Redis 服务器时所指定的 redis.conf 配置文件进行改写
            /// </summary>
            /// <returns></returns>
            public (string node, string value)[] ConfigRewrite() => _csredis.Nodes.Values.Select(a => _csredis.GetAndExecute(a, c => (a.Key, c.Value.ConfigRewrite()))).ToArray();
            /// <summary>
            /// 修改 redis 配置参数，无需重启
            /// </summary>
            /// <param name="parameter">参数</param>
            /// <param name="value">值</param>
            /// <returns></returns>
            public (string node, string value)[] ConfigSet(string parameter, string value) => _csredis.Nodes.Values.Select(a => _csredis.GetAndExecute(a, c => (a.Key, c.Value.ConfigSet(parameter, value)))).ToArray();
            /// <summary>
            /// 重置 INFO 命令中的某些统计数据
            /// </summary>
            /// <returns></returns>
            public (string node, string value)[] ConfigResetStat() => _csredis.Nodes.Values.Select(a => _csredis.GetAndExecute(a, c => (a.Key, c.Value.ConfigResetStat()))).ToArray();
            /// <summary>
            /// 返回当前数据库的 key 的数量
            /// </summary>
            /// <returns></returns>
            public (string node, long value)[] DbSize() => _csredis.Nodes.Values.Select(a => _csredis.GetAndExecute(a, c => (a.Key, c.Value.DbSize()))).ToArray();
            /// <summary>
            /// 让 Redis 服务崩溃
            /// </summary>
            /// <returns></returns>
            public (string node, string value)[] DebugSegFault() => _csredis.Nodes.Values.Select(a => _csredis.GetAndExecute(a, c => (a.Key, c.Value.DebugSegFault()))).ToArray();
            /// <summary>
            /// 删除所有数据库的所有key
            /// </summary>
            /// <returns></returns>
            public (string node, string value)[] FlushAll() => _csredis.Nodes.Values.Select(a => _csredis.GetAndExecute(a, c => (a.Key, c.Value.FlushAll()))).ToArray();
            /// <summary>
            /// 删除当前数据库的所有key
            /// </summary>
            /// <returns></returns>
            public (string node, string value)[] FlushDb() => _csredis.Nodes.Values.Select(a => _csredis.GetAndExecute(a, c => (a.Key, c.Value.FlushDb()))).ToArray();
            /// <summary>
            /// 获取 Redis 服务器的各种信息和统计数值
            /// </summary>
            /// <param name="section">部分(all|default|server|clients|memory|persistence|stats|replication|cpu|commandstats|cluster|keyspace)</param>
            /// <returns></returns>
            public (string node, string value)[] Info(InfoSection? section = null) => _csredis.Nodes.Values.Select(a => _csredis.GetAndExecute(a, c => (a.Key, c.Value.Info(section?.ToString())))).ToArray();
            /// <summary>
            /// 返回最近一次 Redis 成功将数据保存到磁盘上的时间
            /// </summary>
            /// <returns></returns>
            public (string node, DateTime value)[] LastSave() => _csredis.Nodes.Values.Select(a => _csredis.GetAndExecute(a, c => (a.Key, c.Value.LastSave()))).ToArray();
            ///// <summary>
            ///// 实时打印出 Redis 服务器接收到的命令，调试用
            ///// </summary>
            ///// <param name="onReceived">接收命令</param>
            ///// <returns></returns>
            //public (string node, string value)[] Monitor(Action<object, object> onReceived) => _csredis.Nodes.Values.Select(a => _csredis.GetAndExecute(a, c => {
            //	c.Value.MonitorReceived += (s, o) => onReceived?.Invoke(s, o.Message);
            //	return (a.Key, c.Value.Monitor());
            //})).ToArray();
            /// <summary>
            /// 返回主从实例所属的角色
            /// </summary>
            /// <returns></returns>
            public (string node, RedisRole value)[] Role() => _csredis.Nodes.Values.Select(a => _csredis.GetAndExecute(a, c => (a.Key, c.Value.Role()))).ToArray();
            /// <summary>
            /// 同步保存数据到硬盘
            /// </summary>
            /// <returns></returns>
            public (string node, string value)[] Save() => _csredis.Nodes.Values.Select(a => _csredis.GetAndExecute(a, c => (a.Key, c.Value.Save()))).ToArray();
            /// <summary>
            /// 异步保存数据到硬盘，并关闭服务器
            /// </summary>
            /// <param name="isSave">是否保存</param>
            /// <returns></returns>
            public (string node, string value)[] Shutdown(bool isSave = true) => _csredis.Nodes.Values.Select(a => _csredis.GetAndExecute(a, c => (a.Key, c.Value.Shutdown(isSave)))).ToArray();
            /// <summary>
            /// 将服务器转变为指定服务器的从属服务器(slave server)，如果当前服务器已经是某个主服务器(master server)的从属服务器，那么执行 SLAVEOF host port 将使当前服务器停止对旧主服务器的同步，丢弃旧数据集，转而开始对新主服务器进行同步。
            /// </summary>
            /// <param name="host">主机</param>
            /// <param name="port">端口</param>
            /// <returns></returns>
            public (string node, string value)[] SlaveOf(string host, int port) => _csredis.Nodes.Values.Select(a => _csredis.GetAndExecute(a, c => (a.Key, c.Value.SlaveOf(host, port)))).ToArray();
            /// <summary>
            /// 从属服务器执行命令 SLAVEOF NO ONE 将使得这个从属服务器关闭复制功能，并从从属服务器转变回主服务器，原来同步所得的数据集不会被丢弃。
            /// </summary>
            /// <returns></returns>
            public (string node, string value)[] SlaveOfNoOne() => _csredis.Nodes.Values.Select(a => _csredis.GetAndExecute(a, c => (a.Key, c.Value.SlaveOfNoOne()))).ToArray();
            /// <summary>
            /// 管理 redis 的慢日志，按数量获取
            /// </summary>
            /// <param name="count">数量</param>
            /// <returns></returns>
            public (string node, RedisSlowLogEntry[] value)[] SlowLogGet(long? count = null) => _csredis.Nodes.Values.Select(a => _csredis.GetAndExecute(a, c => (a.Key, c.Value.SlowLogGet(count)))).ToArray();
            /// <summary>
            /// 管理 redis 的慢日志，总数量
            /// </summary>
            /// <returns></returns>
            public (string node, long value)[] SlowLogLen() => _csredis.Nodes.Values.Select(a => _csredis.GetAndExecute(a, c => (a.Key, c.Value.SlowLogLen()))).ToArray();
            /// <summary>
            /// 管理 redis 的慢日志，清空
            /// </summary>
            /// <returns></returns>
            public (string node, string value)[] SlowLogReset() => _csredis.Nodes.Values.Select(a => _csredis.GetAndExecute(a, c => (a.Key, c.Value.SlowLogReset()))).ToArray();
            /// <summary>
            /// 用于复制功能(replication)的内部命令
            /// </summary>
            /// <returns></returns>
            public (string node, byte[] value)[] Sync() => _csredis.Nodes.Values.Select(a => _csredis.GetAndExecute(a, c => (a.Key, c.Value.Sync()))).ToArray();
        }

        /// <summary>
        /// 在指定分区节点上，执行服务器命令
        /// </summary>
        /// <param name="node">节点</param>
        /// <returns></returns>
        public NodeServerManagerProvider NodeServerManager(string node) => new NodeServerManagerProvider(this, GetNodeOrThrowNotFound(node));
        public partial class NodeServerManagerProvider
        {
            private CSRedisClient _csredis;
            private RedisClientPool _pool;

            public NodeServerManagerProvider(CSRedisClient csredis, RedisClientPool pool)
            {
                _csredis = csredis;
                _pool = pool;
            }

            /// <summary>
            /// 异步执行一个 AOF（AppendOnly File） 文件重写操作
            /// </summary>
            /// <returns></returns>
            public string BgRewriteAof() => _csredis.GetAndExecute(_pool, c => c.Value.BgRewriteAof());
            /// <summary>
            /// 在后台异步保存当前数据库的数据到磁盘
            /// </summary>
            /// <returns></returns>
            public string BgSave() => _csredis.GetAndExecute(_pool, c => c.Value.BgSave());
            /// <summary>
            /// 关闭客户端连接
            /// </summary>
            /// <param name="ip">ip</param>
            /// <param name="port">端口</param>
            /// <returns></returns>
            public string ClientKill(string ip, int port) => _csredis.GetAndExecute(_pool, c => c.Value.ClientKill(ip, port));
            /// <summary>
            /// 关闭客户端连接
            /// </summary>
            /// <param name="addr">ip:port</param>
            /// <param name="id">客户唯一标识</param>
            /// <param name="type">类型：normal | slave | pubsub</param>
            /// <param name="skipMe">跳过自己</param>
            /// <returns></returns>
            public long ClientKill(string addr = null, string id = null, ClientKillType? type = null, bool? skipMe = null) => _csredis.GetAndExecute(_pool, c => c.Value.ClientKill(addr, id, type?.ToString(), skipMe));
            public enum ClientKillType { normal, slave, pubsub }
            /// <summary>
            /// 获取连接到服务器的客户端连接列表
            /// </summary>
            /// <returns></returns>
            public string ClientList() => _csredis.GetAndExecute(_pool, c => c.Value.ClientList());
            /// <summary>
            /// 获取连接的名称
            /// </summary>
            /// <returns></returns>
            public string ClientGetName() => _csredis.GetAndExecute(_pool, c => c.Value.ClientGetName());
            /// <summary>
            /// 在指定时间内终止运行来自客户端的命令
            /// </summary>
            /// <param name="timeout">阻塞时间</param>
            /// <returns></returns>
            public string ClientPause(TimeSpan timeout) => _csredis.GetAndExecute(_pool, c => c.Value.ClientPause(timeout));
            /// <summary>
            /// 设置当前连接的名称
            /// </summary>
            /// <param name="connectionName">连接名称</param>
            /// <returns></returns>
            public string ClientSetName(string connectionName) => _csredis.GetAndExecute(_pool, c => c.Value.ClientSetName(connectionName));
            /// <summary>
            /// 返回当前服务器时间
            /// </summary>
            /// <returns></returns>
            public DateTime Time() => _csredis.GetAndExecute(_pool, c => c.Value.Time());
            /// <summary>
            /// 获取指定配置参数的值
            /// </summary>
            /// <param name="parameter">参数</param>
            /// <returns></returns>
            public Dictionary<string, string> ConfigGet(string parameter) => _csredis.GetAndExecute(_pool, c => c.Value.ConfigGet(parameter)).ToDictionary(z => z.Item1, y => y.Item2);
            /// <summary>
            /// 对启动 Redis 服务器时所指定的 redis.conf 配置文件进行改写
            /// </summary>
            /// <returns></returns>
            public string ConfigRewrite() => _csredis.GetAndExecute(_pool, c => c.Value.ConfigRewrite());
            /// <summary>
            /// 修改 redis 配置参数，无需重启
            /// </summary>
            /// <param name="parameter">参数</param>
            /// <param name="value">值</param>
            /// <returns></returns>
            public string ConfigSet(string parameter, string value) => _csredis.GetAndExecute(_pool, c => c.Value.ConfigSet(parameter, value));
            /// <summary>
            /// 重置 INFO 命令中的某些统计数据
            /// </summary>
            /// <returns></returns>
            public string ConfigResetStat() => _csredis.GetAndExecute(_pool, c => c.Value.ConfigResetStat());
            /// <summary>
            /// 返回当前数据库的 key 的数量
            /// </summary>
            /// <returns></returns>
            public long DbSize() => _csredis.GetAndExecute(_pool, c => c.Value.DbSize());
            /// <summary>
            /// 让 Redis 服务崩溃
            /// </summary>
            /// <returns></returns>
            public string DebugSegFault() => _csredis.GetAndExecute(_pool, c => c.Value.DebugSegFault());
            /// <summary>
            /// 删除所有数据库的所有key
            /// </summary>
            /// <returns></returns>
            public string FlushAll() => _csredis.GetAndExecute(_pool, c => c.Value.FlushAll());
            /// <summary>
            /// 删除当前数据库的所有key
            /// </summary>
            /// <returns></returns>
            public string FlushDb() => _csredis.GetAndExecute(_pool, c => c.Value.FlushDb());
            /// <summary>
            /// 获取 Redis 服务器的各种信息和统计数值
            /// </summary>
            /// <param name="section">部分(Server | Clients | Memory | Persistence | Stats | Replication | CPU | Keyspace)</param>
            /// <returns></returns>
            public string Info(InfoSection? section = null) => _csredis.GetAndExecute(_pool, c => c.Value.Info(section?.ToString()));
            /// <summary>
            /// 返回最近一次 Redis 成功将数据保存到磁盘上的时间
            /// </summary>
            /// <returns></returns>
            public DateTime LastSave() => _csredis.GetAndExecute(_pool, c => c.Value.LastSave());
            ///// <summary>
            ///// 实时打印出 Redis 服务器接收到的命令，调试用
            ///// </summary>
            ///// <param name="onReceived">接收命令</param>
            ///// <returns></returns>
            //public string Monitor(Action<object, object> onReceived) => _csredis.GetAndExecute(_pool, c => {
            //	c.Value.MonitorReceived += (s, o) => onReceived?.Invoke(s, o.Message);
            //	return c.Value.Monitor();
            //});
            /// <summary>
            /// 返回主从实例所属的角色
            /// </summary>
            /// <returns></returns>
            public RedisRole Role() => _csredis.GetAndExecute(_pool, c => c.Value.Role());
            /// <summary>
            /// 同步保存数据到硬盘
            /// </summary>
            /// <returns></returns>
            public string Save() => _csredis.GetAndExecute(_pool, c => c.Value.Save());
            /// <summary>
            /// 异步保存数据到硬盘，并关闭服务器
            /// </summary>
            /// <param name="isSave">是否保存</param>
            /// <returns></returns>
            public string Shutdown(bool isSave = true) => _csredis.GetAndExecute(_pool, c => c.Value.Shutdown(isSave));
            /// <summary>
            /// 将服务器转变为指定服务器的从属服务器(slave server)，如果当前服务器已经是某个主服务器(master server)的从属服务器，那么执行 SLAVEOF host port 将使当前服务器停止对旧主服务器的同步，丢弃旧数据集，转而开始对新主服务器进行同步。
            /// </summary>
            /// <param name="host">主机</param>
            /// <param name="port">端口</param>
            /// <returns></returns>
            public string SlaveOf(string host, int port) => _csredis.GetAndExecute(_pool, c => c.Value.SlaveOf(host, port));
            /// <summary>
            /// 从属服务器执行命令 SLAVEOF NO ONE 将使得这个从属服务器关闭复制功能，并从从属服务器转变回主服务器，原来同步所得的数据集不会被丢弃。
            /// </summary>
            /// <returns></returns>
            public string SlaveOfNoOne() => _csredis.GetAndExecute(_pool, c => c.Value.SlaveOfNoOne());
            /// <summary>
            /// 管理 redis 的慢日志，按数量获取
            /// </summary>
            /// <param name="count">数量</param>
            /// <returns></returns>
            public RedisSlowLogEntry[] SlowLogGet(long? count = null) => _csredis.GetAndExecute(_pool, c => c.Value.SlowLogGet(count));
            /// <summary>
            /// 管理 redis 的慢日志，总数量
            /// </summary>
            /// <returns></returns>
            public long SlowLogLen() => _csredis.GetAndExecute(_pool, c => c.Value.SlowLogLen());
            /// <summary>
            /// 管理 redis 的慢日志，清空
            /// </summary>
            /// <returns></returns>
            public string SlowLogReset() => _csredis.GetAndExecute(_pool, c => c.Value.SlowLogReset());
            /// <summary>
            /// 用于复制功能(replication)的内部命令
            /// </summary>
            /// <returns></returns>
            public byte[] Sync() => _csredis.GetAndExecute(_pool, c => c.Value.Sync());
        }
        #endregion


#if !net40


        #region 服务器命令
        public partial class NodesServerManagerProvider
        {
            private async Task<(string node, T value)[]> NodesInternalAsync<T>(Func<Object<RedisClient>, Task<T>> handleAsync)
            {
                var ret = new List<(string, T)>();
                foreach (var pool in _csredis.Nodes.Values)
                    ret.Add((pool.Key, await _csredis.GetAndExecuteAsync(pool, c => handleAsync(c))));
                return ret.ToArray();
            }

            /// <summary>
            /// 异步执行一个 AOF（AppendOnly File） 文件重写操作
            /// </summary>
            /// <returns></returns>
            public Task<(string node, string value)[]> BgRewriteAofAsync() => NodesInternalAsync(c => c.Value.BgRewriteAofAsync());
            /// <summary>
            /// 在后台异步保存当前数据库的数据到磁盘
            /// </summary>
            /// <returns></returns>
            public Task<(string node, string value)[]> BgSaveAsync() => NodesInternalAsync(c => c.Value.BgSaveAsync());
            /// <summary>
            /// 关闭客户端连接
            /// </summary>
            /// <param name="ip">ip</param>
            /// <param name="port">端口</param>
            /// <returns></returns>
            public Task<(string node, string value)[]> ClientKillAsync(string ip, int port) => NodesInternalAsync(c => c.Value.ClientKillAsync(ip, port));
            /// <summary>
            /// 关闭客户端连接
            /// </summary>
            /// <param name="addr">ip:port</param>
            /// <param name="id">客户唯一标识</param>
            /// <param name="type">类型：normal | slave | pubsub</param>
            /// <param name="skipMe">跳过自己</param>
            /// <returns></returns>
            public Task<(string node, long value)[]> ClientKillAsync(string addr = null, string id = null, ClientKillType? type = null, bool? skipMe = null) => NodesInternalAsync(c => c.Value.ClientKillAsync(addr, id, type?.ToString(), skipMe));
            /// <summary>
            /// 获取连接到服务器的客户端连接列表
            /// </summary>
            /// <returns></returns>
            public Task<(string node, string value)[]> ClientListAsync() => NodesInternalAsync(c => c.Value.ClientListAsync());
            /// <summary>
            /// 获取连接的名称
            /// </summary>
            /// <returns></returns>
            public Task<(string node, string value)[]> ClientGetNameAsync() => NodesInternalAsync(c => c.Value.ClientGetNameAsync());
            /// <summary>
            /// 在指定时间内终止运行来自客户端的命令
            /// </summary>
            /// <param name="timeout">阻塞时间</param>
            /// <returns></returns>
            public Task<(string node, string value)[]> ClientPauseAsync(TimeSpan timeout) => NodesInternalAsync(c => c.Value.ClientPauseAsync(timeout));
            /// <summary>
            /// 设置当前连接的名称
            /// </summary>
            /// <param name="connectionName">连接名称</param>
            /// <returns></returns>
            public Task<(string node, string value)[]> ClientSetNameAsync(string connectionName) => NodesInternalAsync(c => c.Value.ClientSetNameAsync(connectionName));
            /// <summary>
            /// 返回当前服务器时间
            /// </summary>
            /// <returns></returns>
            public Task<(string node, DateTime value)[]> TimeAsync() => NodesInternalAsync(c => c.Value.TimeAsync());
            /// <summary>
            /// 获取指定配置参数的值
            /// </summary>
            /// <param name="parameter">参数</param>
            /// <returns></returns>
            public Task<(string node, Dictionary<string, string> value)[]> ConfigGetAsync(string parameter) => NodesInternalAsync(async c => (await c.Value.ConfigGetAsync(parameter)).ToDictionary(z => z.Item1, y => y.Item2));
            /// <summary>
            /// 对启动 Redis 服务器时所指定的 redis.conf 配置文件进行改写
            /// </summary>
            /// <returns></returns>
            public Task<(string node, string value)[]> ConfigRewriteAsync() => NodesInternalAsync(c => c.Value.ConfigRewriteAsync());
            /// <summary>
            /// 修改 redis 配置参数，无需重启
            /// </summary>
            /// <param name="parameter">参数</param>
            /// <param name="value">值</param>
            /// <returns></returns>
            public Task<(string node, string value)[]> ConfigSetAsync(string parameter, string value) => NodesInternalAsync(c => c.Value.ConfigSetAsync(parameter, value));
            /// <summary>
            /// 重置 INFO 命令中的某些统计数据
            /// </summary>
            /// <returns></returns>
            public Task<(string node, string value)[]> ConfigResetStatAsync() => NodesInternalAsync(c => c.Value.ConfigResetStatAsync());
            /// <summary>
            /// 返回当前数据库的 key 的数量
            /// </summary>
            /// <returns></returns>
            public Task<(string node, long value)[]> DbSizeAsync() => NodesInternalAsync(c => c.Value.DbSizeAsync());
            /// <summary>
            /// 让 Redis 服务崩溃
            /// </summary>
            /// <returns></returns>
            public Task<(string node, string value)[]> DebugSegFaultAsync() => NodesInternalAsync(c => c.Value.DebugSegFaultAsync());
            /// <summary>
            /// 删除所有数据库的所有key
            /// </summary>
            /// <returns></returns>
            public Task<(string node, string value)[]> FlushAllAsync() => NodesInternalAsync(c => c.Value.FlushAllAsync());
            /// <summary>
            /// 删除当前数据库的所有key
            /// </summary>
            /// <returns></returns>
            public Task<(string node, string value)[]> FlushDbAsync() => NodesInternalAsync(c => c.Value.FlushDbAsync());
            /// <summary>
            /// 获取 Redis 服务器的各种信息和统计数值
            /// </summary>
            /// <param name="section">部分(all|default|server|clients|memory|persistence|stats|replication|cpu|commandstats|cluster|keyspace)</param>
            /// <returns></returns>
            public Task<(string node, string value)[]> InfoAsync(InfoSection? section = null) => NodesInternalAsync(c => c.Value.InfoAsync(section?.ToString()));
            /// <summary>
            /// 返回最近一次 Redis 成功将数据保存到磁盘上的时间
            /// </summary>
            /// <returns></returns>
            public Task<(string node, DateTime value)[]> LastSaveAsync() => NodesInternalAsync(c => c.Value.LastSaveAsync());
            /// <summary>
            /// 返回主从实例所属的角色
            /// </summary>
            /// <returns></returns>
            public Task<(string node, RedisRole value)[]> RoleAsync() => NodesInternalAsync(c => c.Value.RoleAsync());
            /// <summary>
            /// 同步保存数据到硬盘
            /// </summary>
            /// <returns></returns>
            public Task<(string node, string value)[]> SaveAsync() => NodesInternalAsync(c => c.Value.SaveAsync());
            /// <summary>
            /// 异步保存数据到硬盘，并关闭服务器
            /// </summary>
            /// <param name="isSave">是否保存</param>
            /// <returns></returns>
            public Task<(string node, string value)[]> ShutdownAsync(bool isSave = true) => NodesInternalAsync(c => c.Value.ShutdownAsync(isSave));
            /// <summary>
            /// 将服务器转变为指定服务器的从属服务器(slave server)，如果当前服务器已经是某个主服务器(master server)的从属服务器，那么执行 SLAVEOF host port 将使当前服务器停止对旧主服务器的同步，丢弃旧数据集，转而开始对新主服务器进行同步。
            /// </summary>
            /// <param name="host">主机</param>
            /// <param name="port">端口</param>
            /// <returns></returns>
            public Task<(string node, string value)[]> SlaveOfAsync(string host, int port) => NodesInternalAsync(c => c.Value.SlaveOfAsync(host, port));
            /// <summary>
            /// 从属服务器执行命令 SLAVEOF NO ONE 将使得这个从属服务器关闭复制功能，并从从属服务器转变回主服务器，原来同步所得的数据集不会被丢弃。
            /// </summary>
            /// <returns></returns>
            public Task<(string node, string value)[]> SlaveOfNoOneAsync() => NodesInternalAsync(c => c.Value.SlaveOfNoOneAsync());
            /// <summary>
            /// 管理 redis 的慢日志，按数量获取
            /// </summary>
            /// <param name="count">数量</param>
            /// <returns></returns>
            public Task<(string node, RedisSlowLogEntry[] value)[]> SlowLogGetAsync(long? count = null) => NodesInternalAsync(c => c.Value.SlowLogGetAsync(count));
            /// <summary>
            /// 管理 redis 的慢日志，总数量
            /// </summary>
            /// <returns></returns>
            public Task<(string node, long value)[]> SlowLogLenAsync() => NodesInternalAsync(c => c.Value.SlowLogLenAsync());
            /// <summary>
            /// 管理 redis 的慢日志，清空
            /// </summary>
            /// <returns></returns>
            public Task<(string node, string value)[]> SlowLogResetAsync() => NodesInternalAsync(c => c.Value.SlowLogResetAsync());
            /// <summary>
            /// 用于复制功能(replication)的内部命令
            /// </summary>
            /// <returns></returns>
            public Task<(string node, byte[] value)[]> SyncAsync() => NodesInternalAsync(c => c.Value.SyncAsync());
        }

        public partial class NodeServerManagerProvider
        {

            /// <summary>
            /// 异步执行一个 AOF（AppendOnly File） 文件重写操作
            /// </summary>
            /// <returns></returns>
            public Task<string> BgRewriteAofAsync() => _csredis.GetAndExecute(_pool, c => c.Value.BgRewriteAofAsync());
            /// <summary>
            /// 在后台异步保存当前数据库的数据到磁盘
            /// </summary>
            /// <returns></returns>
            public Task<string> BgSaveAsync() => _csredis.GetAndExecute(_pool, c => c.Value.BgSaveAsync());
            /// <summary>
            /// 关闭客户端连接
            /// </summary>
            /// <param name="ip">ip</param>
            /// <param name="port">端口</param>
            /// <returns></returns>
            public Task<string> ClientKillAsync(string ip, int port) => _csredis.GetAndExecute(_pool, c => c.Value.ClientKillAsync(ip, port));
            /// <summary>
            /// 关闭客户端连接
            /// </summary>
            /// <param name="addr">ip:port</param>
            /// <param name="id">客户唯一标识</param>
            /// <param name="type">类型：normal | slave | pubsub</param>
            /// <param name="skipMe">跳过自己</param>
            /// <returns></returns>
            public Task<long> ClientKillAsync(string addr = null, string id = null, ClientKillType? type = null, bool? skipMe = null) => _csredis.GetAndExecute(_pool, c => c.Value.ClientKillAsync(addr, id, type?.ToString(), skipMe));
            /// <summary>
            /// 获取连接到服务器的客户端连接列表
            /// </summary>
            /// <returns></returns>
            public Task<string> ClientListAsync() => _csredis.GetAndExecute(_pool, c => c.Value.ClientListAsync());
            /// <summary>
            /// 获取连接的名称
            /// </summary>
            /// <returns></returns>
            public Task<string> ClientGetNameAsync() => _csredis.GetAndExecute(_pool, c => c.Value.ClientGetNameAsync());
            /// <summary>
            /// 在指定时间内终止运行来自客户端的命令
            /// </summary>
            /// <param name="timeout">阻塞时间</param>
            /// <returns></returns>
            public Task<string> ClientPauseAsync(TimeSpan timeout) => _csredis.GetAndExecute(_pool, c => c.Value.ClientPauseAsync(timeout));
            /// <summary>
            /// 设置当前连接的名称
            /// </summary>
            /// <param name="connectionName">连接名称</param>
            /// <returns></returns>
            public Task<string> ClientSetNameAsync(string connectionName) => _csredis.GetAndExecute(_pool, c => c.Value.ClientSetNameAsync(connectionName));
            /// <summary>
            /// 返回当前服务器时间
            /// </summary>
            /// <returns></returns>
            public Task<DateTime> TimeAsync() => _csredis.GetAndExecute(_pool, c => c.Value.TimeAsync());
            /// <summary>
            /// 获取指定配置参数的值
            /// </summary>
            /// <param name="parameter">参数</param>
            /// <returns></returns>
            public async Task<Dictionary<string, string>> ConfigGetAsync(string parameter) => (await _csredis.GetAndExecute(_pool, c => c.Value.ConfigGetAsync(parameter))).ToDictionary(z => z.Item1, y => y.Item2);
            /// <summary>
            /// 对启动 Redis 服务器时所指定的 redis.conf 配置文件进行改写
            /// </summary>
            /// <returns></returns>
            public Task<string> ConfigRewriteAsync() => _csredis.GetAndExecute(_pool, c => c.Value.ConfigRewriteAsync());
            /// <summary>
            /// 修改 redis 配置参数，无需重启
            /// </summary>
            /// <param name="parameter">参数</param>
            /// <param name="value">值</param>
            /// <returns></returns>
            public Task<string> ConfigSetAsync(string parameter, string value) => _csredis.GetAndExecute(_pool, c => c.Value.ConfigSetAsync(parameter, value));
            /// <summary>
            /// 重置 INFO 命令中的某些统计数据
            /// </summary>
            /// <returns></returns>
            public Task<string> ConfigResetStatAsync() => _csredis.GetAndExecute(_pool, c => c.Value.ConfigResetStatAsync());
            /// <summary>
            /// 返回当前数据库的 key 的数量
            /// </summary>
            /// <returns></returns>
            public Task<long> DbSizeAsync() => _csredis.GetAndExecute(_pool, c => c.Value.DbSizeAsync());
            /// <summary>
            /// 让 Redis 服务崩溃
            /// </summary>
            /// <returns></returns>
            public Task<string> DebugSegFaultAsync() => _csredis.GetAndExecute(_pool, c => c.Value.DebugSegFaultAsync());
            /// <summary>
            /// 删除所有数据库的所有key
            /// </summary>
            /// <returns></returns>
            public Task<string> FlushAllAsync() => _csredis.GetAndExecute(_pool, c => c.Value.FlushAllAsync());
            /// <summary>
            /// 删除当前数据库的所有key
            /// </summary>
            /// <returns></returns>
            public Task<string> FlushDbAsync() => _csredis.GetAndExecute(_pool, c => c.Value.FlushDbAsync());
            /// <summary>
            /// 获取 Redis 服务器的各种信息和统计数值
            /// </summary>
            /// <param name="section">部分(Server | Clients | Memory | Persistence | Stats | Replication | CPU | Keyspace)</param>
            /// <returns></returns>
            public Task<string> InfoAsync(InfoSection? section = null) => _csredis.GetAndExecute(_pool, c => c.Value.InfoAsync(section?.ToString()));
            /// <summary>
            /// 返回最近一次 Redis 成功将数据保存到磁盘上的时间
            /// </summary>
            /// <returns></returns>
            public Task<DateTime> LastSaveAsync() => _csredis.GetAndExecute(_pool, c => c.Value.LastSaveAsync());
            /// <summary>
            /// 返回主从实例所属的角色
            /// </summary>
            /// <returns></returns>
            public Task<RedisRole> RoleAsync() => _csredis.GetAndExecute(_pool, c => c.Value.RoleAsync());
            /// <summary>
            /// 同步保存数据到硬盘
            /// </summary>
            /// <returns></returns>
            public Task<string> SaveAsync() => _csredis.GetAndExecute(_pool, c => c.Value.SaveAsync());
            /// <summary>
            /// 异步保存数据到硬盘，并关闭服务器
            /// </summary>
            /// <param name="isSave">是否保存</param>
            /// <returns></returns>
            public Task<string> ShutdownAsync(bool isSave = true) => _csredis.GetAndExecute(_pool, c => c.Value.ShutdownAsync(isSave));
            /// <summary>
            /// 将服务器转变为指定服务器的从属服务器(slave server)，如果当前服务器已经是某个主服务器(master server)的从属服务器，那么执行 SLAVEOF host port 将使当前服务器停止对旧主服务器的同步，丢弃旧数据集，转而开始对新主服务器进行同步。
            /// </summary>
            /// <param name="host">主机</param>
            /// <param name="port">端口</param>
            /// <returns></returns>
            public Task<string> SlaveOfAsync(string host, int port) => _csredis.GetAndExecute(_pool, c => c.Value.SlaveOfAsync(host, port));
            /// <summary>
            /// 从属服务器执行命令 SLAVEOF NO ONE 将使得这个从属服务器关闭复制功能，并从从属服务器转变回主服务器，原来同步所得的数据集不会被丢弃。
            /// </summary>
            /// <returns></returns>
            public Task<string> SlaveOfNoOneAsync() => _csredis.GetAndExecute(_pool, c => c.Value.SlaveOfNoOneAsync());
            /// <summary>
            /// 管理 redis 的慢日志，按数量获取
            /// </summary>
            /// <param name="count">数量</param>
            /// <returns></returns>
            public Task<RedisSlowLogEntry[]> SlowLogGetAsync(long? count = null) => _csredis.GetAndExecute(_pool, c => c.Value.SlowLogGetAsync(count));
            /// <summary>
            /// 管理 redis 的慢日志，总数量
            /// </summary>
            /// <returns></returns>
            public Task<long> SlowLogLenAsync() => _csredis.GetAndExecute(_pool, c => c.Value.SlowLogLenAsync());
            /// <summary>
            /// 管理 redis 的慢日志，清空
            /// </summary>
            /// <returns></returns>
            public Task<string> SlowLogResetAsync() => _csredis.GetAndExecute(_pool, c => c.Value.SlowLogResetAsync());
            /// <summary>
            /// 用于复制功能(replication)的内部命令
            /// </summary>
            /// <returns></returns>
            public Task<byte[]> SyncAsync() => _csredis.GetAndExecute(_pool, c => c.Value.SyncAsync());
        }

        #endregion

#endif

    }
}