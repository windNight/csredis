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

        /// <summary>
        /// 开启分布式锁，若超时返回null
        /// </summary>
        /// <param name="name">锁名称</param>
        /// <param name="timeoutSeconds">超时（秒）</param>
        /// <param name="autoDelay">自动延长锁超时时间，看门狗线程的超时时间为timeoutSeconds/2 ， 在看门狗线程超时时间时自动延长锁的时间为timeoutSeconds。除非程序意外退出，否则永不超时。</param>
        /// <returns></returns>
        public CSRedisClientLock Lock(string name, int timeoutSeconds, bool autoDelay = true)
        {
            name = $"CSRedisClientLock:{name}";
            var startTime = DateTime.Now;
            while (DateTime.Now.Subtract(startTime).TotalSeconds < timeoutSeconds)
            {
                var value = Guid.NewGuid().ToString();
                if (this.Set(name, value, timeoutSeconds, RedisExistence.Nx) == true)
                {
                    return new CSRedisClientLock(this, name, value, timeoutSeconds, autoDelay);
                }
                Thread.CurrentThread.Join(3);
            }
            return null;
        }
        /// <summary>
        /// 开启分布式锁，若超时返回null
        /// </summary>
        /// <param name="name">锁名称</param>
        /// <param name="timeoutMiSeconds">超时（毫秒）</param>
        /// <param name="autoDelay">自动延长锁超时时间，看门狗线程的超时时间为timeoutSeconds/2 ， 在看门狗线程超时时间时自动延长锁的时间为timeoutSeconds。除非程序意外退出，否则永不超时。</param>
        /// <returns></returns>
        public CSRedisClientLock Lock(string name, long timeoutMiSeconds, bool autoDelay = true)
        {
            name = $"CSRedisClientLock:{name}";
            var startTime = DateTime.Now;
            while (DateTime.Now.Subtract(startTime).TotalMilliseconds < timeoutMiSeconds)
            {
                var value = Guid.NewGuid().ToString();
                var ts = TimeSpan.FromMilliseconds(timeoutMiSeconds);
                if (this.Set(name, value, ts, RedisExistence.Nx) == true)
                {
                    return new CSRedisClientLock(this, name, value, (int)timeoutMiSeconds / 1000, autoDelay);
                }
                Thread.CurrentThread.Join(3);
            }
            return null;
        }

        public bool UnLock(string name)
        {
            var key = $"CSRedisClientLock:{name}";
            return Del(key) > 0;
        }


#if !net40




#endif

    }
    public class CSRedisClientLock : IDisposable
    {

        CSRedisClient _client;
        string _name;
        string _value;
        int _timeoutSeconds;
        Timer _autoDelayTimer;

        internal CSRedisClientLock(CSRedisClient rds, string name, string value, int timeoutSeconds, bool autoDelay)
        {
            _client = rds;
            _name = name;
            _value = value;
            _timeoutSeconds = timeoutSeconds;
            if (autoDelay)
            {
                var milliseconds = _timeoutSeconds * 1000 / 2;
                _autoDelayTimer = new Timer(state2 => Delay(milliseconds), null, milliseconds, milliseconds);
            }
        }

        /// <summary>
        /// 延长锁时间，锁在占用期内操作时返回true，若因锁超时被其他使用者占用则返回false
        /// </summary>
        /// <param name="milliseconds">延长的毫秒数</param>
        /// <returns>成功/失败</returns>
        public bool Delay(int milliseconds)
        {
            var ret = _client.Eval(@"local gva = redis.call('GET', KEYS[1])
if gva == ARGV[1] then
  local ttlva = redis.call('PTTL', KEYS[1])
  redis.call('PEXPIRE', KEYS[1], ARGV[2] + ttlva)
  return 1
end
return 0", _name, _value, milliseconds)?.ToString() == "1";
            if (ret == false) _autoDelayTimer?.Dispose(); //未知情况，关闭定时器
            return ret;
        }

        /// <summary>
        /// 释放分布式锁
        /// </summary>
        /// <returns>成功/失败</returns>
        public bool Unlock()
        {
            _autoDelayTimer?.Dispose();
            return _client.Eval(@"local gva = redis.call('GET', KEYS[1])
if gva == ARGV[1] then
  redis.call('DEL', KEYS[1])
  return 1
end
return 0", _name, _value)?.ToString() == "1";
        }

        public void Dispose() => this.Unlock();
    }
}