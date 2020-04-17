using CSRedis;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;

public abstract class RedisHelper : RedisHelper<RedisHelper> { }

public abstract partial class RedisHelper<TMark>
{

    /// <summary>
    /// 永不过期
    /// </summary>
	public static readonly int NeverExpired = -1;
    internal static ThreadLocal<Random> rnd = new ThreadLocal<Random>();
    /// <summary>
    /// 随机秒（防止所有key同一时间过期，雪崩）
    /// </summary>
    /// <param name="minTimeoutSeconds">最小秒数</param>
    /// <param name="maxTimeoutSeconds">最大秒数</param>
    /// <returns></returns>
    public static int RandomExpired(int minTimeoutSeconds, int maxTimeoutSeconds) => rnd.Value.Next(minTimeoutSeconds, maxTimeoutSeconds);

    private static CSRedisClient _instance;
    /// <summary>
    /// CSRedisClient 静态实例，使用前请初始化
    /// RedisHelper.Initialization(new CSRedis.CSRedisClient(\"127.0.0.1:6379,pass=123,defaultDatabase=13,poolsize=50,ssl=false,writeBuffer=10240,prefix=key前辍\"))
    /// </summary>
    public static CSRedisClient Instance
    {
        get
        {
            if (_instance == null) throw new Exception("使用前请初始化 RedisHelper.Initialization(new CSRedis.CSRedisClient(\"127.0.0.1:6379,pass=123,defaultDatabase=13,poolsize=50,ssl=false,writeBuffer=10240,prefix=key前辍\"));");
            return _instance;
        }
    }
    public static ConcurrentDictionary<string, RedisClientPool> Nodes => Instance.Nodes;

    /// <summary>
    /// 初始化csredis静态访问类
    /// RedisHelper.Initialization(new CSRedis.CSRedisClient(\"127.0.0.1:6379,pass=123,defaultDatabase=13,poolsize=50,ssl=false,writeBuffer=10240,prefix=key前辍\"))
    /// </summary>
    /// <param name="csredis"></param>
    public static void Initialization(CSRedisClient csredis)
    {
        _instance = csredis;
    }
     
    /// <summary>
    /// 创建管道传输
    /// </summary>
    /// <param name="handler"></param>
    /// <returns></returns>
    public static object[] StartPipe(Action<CSRedisClientPipe<string>> handler) => Instance.StartPipe(handler);

    /// <summary>
    /// 创建管道传输，打包提交如：RedisHelper.StartPipe().Set("a", "1").HSet("b", "f", "2").EndPipe();
    /// </summary>
    /// <returns></returns>
    public static CSRedisClientPipe<string> StartPipe() => Instance.StartPipe();

    #region 服务器命令
    /// <summary>
    /// 在所有分区节点上，执行服务器命令
    /// </summary>
    public static CSRedisClient.NodesServerManagerProvider NodesServerManager => Instance.NodesServerManager;
    /// <summary>
    /// 在指定分区节点上，执行服务器命令
    /// </summary>
    /// <param name="node">节点</param>
    /// <returns></returns>
    public static CSRedisClient.NodeServerManagerProvider NodeServerManager(string node) => Instance.NodeServerManager(node);
    #endregion 
}