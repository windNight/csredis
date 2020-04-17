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
    /// <summary>
    /// 开启分布式锁，若超时返回null
    /// </summary>
    /// <param name="name">锁名称</param>
    /// <param name="timeoutSeconds">超时（秒）</param>
    /// <param name="autoDelay">自动延长锁超时时间，看门狗线程的超时时间为timeoutSeconds/2 ， 在看门狗线程超时时间时自动延长锁的时间为timeoutSeconds。除非程序意外退出，否则永不超时。</param>
    /// <returns></returns>
    public static CSRedisClientLock Lock(string name, int timeoutSeconds, bool autoDelay = true) => Instance.Lock(name, timeoutSeconds);

    /// <summary>
    /// 开启分布式锁，若超时返回null
    /// </summary>
    /// <param name="name">锁名称</param>
    /// <param name="timeoutMiSeconds">超时（毫秒）</param>
    /// <param name="autoDelay">自动延长锁超时时间，看门狗线程的超时时间为timeoutSeconds/2 ， 在看门狗线程超时时间时自动延长锁的时间为timeoutSeconds。除非程序意外退出，否则永不超时。</param>
    /// <returns></returns>
    public static CSRedisClientLock Lock(string name, long timeoutMiSeconds, bool autoDelay = true) => Instance.Lock(name, timeoutMiSeconds);

    public static bool UnLock(string name) => Instance.UnLock(name);


}
