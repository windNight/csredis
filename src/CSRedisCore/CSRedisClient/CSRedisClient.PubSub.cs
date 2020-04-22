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

        #region Pub/Sub
        /// <summary>
        /// 用于将信息发送到指定分区节点的频道，最终消息发布格式：1|message
        /// </summary>
        /// <param name="channel">频道名</param>
        /// <param name="message">消息文本</param>
        /// <returns></returns>
        public long Publish(string channel, string message)
        {
            var msgid = HIncrBy("csredisclient:Publish:msgid", channel, 1);
            return ExecuteScalar(channel, (c, k) => c.Value.Publish(channel, $"{msgid}|{message}"));
        }
        /// <summary>
        /// 用于将信息发送到指定分区节点的频道，与 Publish 方法不同，不返回消息id头，即 1|
        /// </summary>
        /// <param name="channel">频道名</param>
        /// <param name="message">消息文本</param>
        /// <returns></returns>
        public long PublishNoneMessageId(string channel, string message) => ExecuteScalar(channel, (c, k) => c.Value.Publish(channel, message));
        /// <summary>
        /// 查看所有订阅频道
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public string[] PubSubChannels(string pattern)
        {
            var ret = new List<string>();
            Nodes.Values.ToList().ForEach(a => ret.AddRange(GetAndExecute(a, c => c.Value.PubSubChannels(pattern))));
            return ret.ToArray();
        }
        /// <summary>
        /// 查看所有模糊订阅端的数量
        /// </summary>
        /// <returns></returns>
        [Obsolete("分区模式下，其他客户端的模糊订阅可能不会返回")]
        public long PubSubNumPat() => GetAndExecute(Nodes.First().Value, c => c.Value.PubSubNumPat());
        /// <summary>
        /// 查看所有订阅端的数量
        /// </summary>
        /// <param name="channels">频道</param>
        /// <returns></returns>
        [Obsolete("分区模式下，其他客户端的订阅可能不会返回")]
        public Dictionary<string, long> PubSubNumSub(params string[] channels) => ExecuteArray(channels, (c, k) =>
        {
            var prefix = (c.Pool as RedisClientPool).Prefix;
            return c.Value.PubSubNumSub(k.Select(z => string.IsNullOrEmpty(prefix) == false && z.StartsWith(prefix) ? z.Substring(prefix.Length) : z).ToArray());
        }).ToDictionary(z => z.Item1, y => y.Item2);
        /// <summary>
        /// 订阅，根据分区规则返回SubscribeObject，Subscribe(("chan1", msg => Console.WriteLine(msg.Body)), ("chan2", msg => Console.WriteLine(msg.Body)))
        /// </summary>
        /// <param name="channels">频道和接收器</param>
        /// <returns>返回可停止订阅的对象</returns>
        public SubscribeObject Subscribe(params (string, Action<SubscribeMessageEventArgs>)[] channels)
        {
            var chans = channels.Select(a => a.Item1).Distinct().ToArray();
            var onmessages = channels.ToDictionary(a => a.Item1, b => b.Item2);

            var rules = new Dictionary<string, List<string>>();
            for (var a = 0; a < chans.Length; a++)
            {
                var rule = NodeRuleRaw(chans[a]);
                if (rules.ContainsKey(rule)) rules[rule].Add(chans[a]);
                else rules.Add(rule, new List<string> { chans[a] });
            }

            List<(string[] keys, Object<RedisClient> conn)> subscrs = new List<(string[] keys, Object<RedisClient> conn)>();
            foreach (var r in rules)
            {
                var pool = Nodes.TryGetValue(r.Key, out var p) ? p : Nodes.First().Value;
                subscrs.Add((r.Value.ToArray(), pool.Get()));
            }

            var so = new SubscribeObject(this, chans, subscrs.ToArray(), onmessages);
            return so;
        }
        public class SubscribeObject : IDisposable
        {
            internal CSRedisClient Redis;
            public string[] Channels { get; }
            public (string[] chans, Object<RedisClient> conn)[] Subscrs { get; }
            internal Dictionary<string, Action<SubscribeMessageEventArgs>> OnMessageDic;
            public bool IsUnsubscribed { get; private set; } = true;

            internal SubscribeObject(CSRedisClient redis, string[] channels, (string[] chans, Object<RedisClient> conn)[] subscrs, Dictionary<string, Action<SubscribeMessageEventArgs>> onMessageDic)
            {
                this.Redis = redis;
                this.Channels = channels;
                this.Subscrs = subscrs;
                this.OnMessageDic = onMessageDic;
                this.IsUnsubscribed = false;

                AppDomain.CurrentDomain.ProcessExit += (s1, e1) =>
                {
                    this.Dispose();
                };
                try
                {
                    Console.CancelKeyPress += (s1, e1) =>
                    {
                        if (e1.Cancel) return;
                        this.Dispose();
                    };
                }
                catch { }

                foreach (var subscr in this.Subscrs)
                {
                    new Thread(Subscribe).Start(subscr);
                }
            }

            private void Subscribe(object state)
            {
                var subscr = ((string[] chans, Object<RedisClient> conn))state;
                var pool = subscr.conn.Pool as RedisClientPool;
                var testCSRedis_Subscribe_Keepalive = "0\r\n";// $"CSRedis_Subscribe_Keepalive{Guid.NewGuid().ToString()}";
                var testKeepalived = true;

                EventHandler<RedisSubscriptionReceivedEventArgs> SubscriptionReceived = (a, b) =>
                {
                    try
                    {
                        if (b.Message.Type == "message" && this.OnMessageDic != null && this.OnMessageDic.TryGetValue(b.Message.Channel, out var action) == true)
                        {
                            var msgidIdx = b.Message.Body.IndexOf('|');
                            if (msgidIdx != -1 && long.TryParse(b.Message.Body.Substring(0, msgidIdx), out var trylong))
                                action(new SubscribeMessageEventArgs
                                {
                                    MessageId = trylong,
                                    Body = b.Message.Body.Substring(msgidIdx + 1),
                                    Channel = b.Message.Channel
                                });
                            else if (b.Message.Body != testCSRedis_Subscribe_Keepalive)
                                action(new SubscribeMessageEventArgs
                                {
                                    MessageId = 0,
                                    Body = b.Message.Body,
                                    Channel = b.Message.Channel
                                });
                            else
                            {
                                testKeepalived = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var bgcolor = Console.BackgroundColor;
                        var forecolor = Console.ForegroundColor;
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($"订阅方法执行出错【{pool.Key}】(channels:{string.Join(",", Channels)})/(chans:{string.Join(",", subscr.chans)})：{ex.Message}\r\n{ex.StackTrace}");
                        Console.BackgroundColor = bgcolor;
                        Console.ForegroundColor = forecolor;
                        Console.WriteLine();
                    }
                };
                subscr.conn.Value.SubscriptionReceived += SubscriptionReceived;

                bool isSubscribeing = false;
                bool isKeepliveReSubscribe = false;
                Timer keeplive = new Timer(state2 =>
                {
                    if (isSubscribeing == false) return;
                    try
                    {
                        foreach (var chan in subscr.chans)
                        {
                            testKeepalived = false;
                            Redis.PublishNoneMessageId(chan, testCSRedis_Subscribe_Keepalive);
                            for (var a = 0; a < 50; a++)
                            {
                                if (isSubscribeing == false) return;
                                Thread.CurrentThread.Join(100);
                                if (testKeepalived) break;
                            }
                            if (testKeepalived == false)
                            {
                                isKeepliveReSubscribe = true;
                                //订阅掉线，重新订阅
                                try { subscr.conn.Value.Unsubscribe(); } catch { }
                                try { subscr.conn.Value.Quit(); } catch { }
                                try { subscr.conn.Value.Socket?.Shutdown(System.Net.Sockets.SocketShutdown.Both); } catch { }
                                break;
                            }
                        }
                    }
                    catch
                    {
                    }
                }, null, 60000, 60000);
                while (IsUnsubscribed == false)
                {
                    try
                    {
                        subscr.conn.Value.Ping();

                        var bgcolor = Console.BackgroundColor;
                        var forecolor = Console.ForegroundColor;
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($"正在订阅【{pool.Key}】(channels:{string.Join(",", Channels)})/(chans:{string.Join(",", subscr.chans)})");
                        Console.BackgroundColor = bgcolor;
                        Console.ForegroundColor = forecolor;
                        Console.WriteLine();

                        isSubscribeing = true;
                        isKeepliveReSubscribe = false;
                        //SetSocketOption KeepAlive 经测试无效，仍然侍丢失频道
                        //subscr.conn.Value.Socket?.SetSocketOption(System.Net.Sockets.SocketOptionLevel.Socket, System.Net.Sockets.SocketOptionName.KeepAlive, 60000);
                        subscr.conn.Value.ReceiveTimeout = 0;
                        subscr.conn.Value.Subscribe(subscr.chans);

                        if (IsUnsubscribed == false)
                        {
                            if (isKeepliveReSubscribe == true)
                                throw new Exception("每60秒检查发现订阅频道丢失");

                            //服务器断开连接 IsConnected == false https://github.com/2881099/csredis/issues/37
                            if (subscr.conn.Value.IsConnected == false)
                                throw new Exception("redis-server 连接已断开");
                        }
                    }
                    catch (Exception ex)
                    {
                        if (IsUnsubscribed) break;

                        var bgcolor = Console.BackgroundColor;
                        var forecolor = Console.ForegroundColor;
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($"订阅出错【{pool.Key}】(channels:{string.Join(",", Channels)})/(chans:{string.Join(",", subscr.chans)})：{ex.Message}，3秒后重连。。。");
                        Console.BackgroundColor = bgcolor;
                        Console.ForegroundColor = forecolor;
                        Console.WriteLine();
                        Thread.CurrentThread.Join(1000 * 3);

                        subscr.conn.ResetValue();
                        subscr.conn.Value.SubscriptionReceived += SubscriptionReceived;
                    }
                }
                subscr.conn.Value.SubscriptionReceived -= SubscriptionReceived;
                isSubscribeing = false;
                isKeepliveReSubscribe = false;
                try { keeplive.Dispose(); } catch { }
            }

            public void Unsubscribe()
            {
                this.Dispose();
            }

            public void Dispose()
            {
                this.IsUnsubscribed = true;
                if (this.Subscrs != null)
                {
                    foreach (var subscr in this.Subscrs)
                    {
                        try { subscr.conn.Value.Unsubscribe(); } catch { }
                        try { subscr.conn.Value.ReceiveTimeout = (subscr.conn.Pool as RedisClientPool)._policy._syncTimeout; } catch { }
                        subscr.conn.Pool.Return(subscr.conn, true);
                    }
                }
            }
        }
        public class SubscribeMessageEventArgs
        {
            /// <summary>
            /// 频道的消息id
            /// </summary>
            public long MessageId { get; set; }
            /// <summary>
            /// 频道
            /// </summary>
            public string Channel { get; set; }
            /// <summary>
            /// 接收到的内容
            /// </summary>
            public string Body { get; set; }
        }
        /// <summary>
        /// 模糊订阅，订阅所有分区节点(同条消息只处理一次），返回SubscribeObject，PSubscribe(new [] { "chan1*", "chan2*" }, msg => Console.WriteLine(msg.Body))
        /// </summary>
        /// <param name="channelPatterns">模糊频道</param>
        /// <param name="pmessage">接收器</param>
        /// <returns>返回可停止模糊订阅的对象</returns>
        public PSubscribeObject PSubscribe(string[] channelPatterns, Action<PSubscribePMessageEventArgs> pmessage)
        {
            var chans = channelPatterns.Distinct().ToArray();

            List<Object<RedisClient>> redisConnections = new List<Object<RedisClient>>();
            foreach (var pool in Nodes)
                redisConnections.Add(pool.Value.Get());

            var so = new PSubscribeObject(this, chans, redisConnections.ToArray(), pmessage);
            return so;
        }
        public class PSubscribeObject : IDisposable
        {
            internal CSRedisClient Redis;
            public string[] Channels { get; }
            internal Action<PSubscribePMessageEventArgs> OnPMessage;
            public Object<RedisClient>[] RedisConnections { get; }
            public bool IsPUnsubscribed { get; private set; } = true;

            internal PSubscribeObject(CSRedisClient redis, string[] channels, Object<RedisClient>[] redisConnections, Action<PSubscribePMessageEventArgs> onPMessage)
            {
                this.Redis = redis;
                this.Channels = channels;
                this.RedisConnections = redisConnections;
                this.OnPMessage = onPMessage;
                this.IsPUnsubscribed = false;

                AppDomain.CurrentDomain.ProcessExit += (s1, e1) =>
                {
                    this.Dispose();
                };
                try
                {
                    Console.CancelKeyPress += (s1, e1) =>
                    {
                        if (e1.Cancel) return;
                        this.Dispose();
                    };
                }
                catch { }

                foreach (var conn in this.RedisConnections)
                {
                    new Thread(PSubscribe).Start(conn);
                }
            }

            private void PSubscribe(object state)
            {
                var conn = (Object<RedisClient>)state;
                var pool = conn.Pool as RedisClientPool;
                var psubscribeKey = string.Join("pSpLiT", Channels);

                EventHandler<RedisSubscriptionReceivedEventArgs> SubscriptionReceived = (a, b) =>
                {
                    try
                    {
                        if (b.Message.Type == "pmessage" && this.OnPMessage != null)
                        {
                            var msgidIdx = b.Message.Body.IndexOf('|');
                            if (msgidIdx != -1 && long.TryParse(b.Message.Body.Substring(0, msgidIdx), out var trylong))
                            {
                                var readed = Redis.Eval($@"
ARGV[1] = redis.call('HGET', KEYS[1], '{b.Message.Channel}')
if ARGV[1] ~= ARGV[2] then
  redis.call('HSET', KEYS[1], '{b.Message.Channel}', ARGV[2])
  return 1
end
return 0", $"CSRedisPSubscribe{psubscribeKey}", "", trylong.ToString());
                                if (readed?.ToString() == "1")
                                    this.OnPMessage(new PSubscribePMessageEventArgs
                                    {
                                        Body = b.Message.Body.Substring(msgidIdx + 1),
                                        Channel = b.Message.Channel,
                                        MessageId = trylong,
                                        Pattern = b.Message.Pattern
                                    });
                                //else
                                //	Console.WriteLine($"消息被处理过：id:{trylong} channel:{b.Message.Channel} pattern:{b.Message.Pattern} body:{b.Message.Body.Substring(msgidIdx + 1)}");
                            }
                            else
                                this.OnPMessage(new PSubscribePMessageEventArgs
                                {
                                    Body = b.Message.Body,
                                    Channel = b.Message.Channel,
                                    MessageId = 0,
                                    Pattern = b.Message.Pattern
                                });
                        }
                    }
                    catch (Exception ex)
                    {
                        var bgcolor = Console.BackgroundColor;
                        var forecolor = Console.ForegroundColor;
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($"模糊订阅出错【{pool.Key}】(channels:{string.Join(",", Channels)})：{ex.Message}\r\n{ex.StackTrace}");
                        Console.BackgroundColor = bgcolor;
                        Console.ForegroundColor = forecolor;
                        Console.WriteLine();

                    }
                };
                conn.Value.SubscriptionReceived += SubscriptionReceived;

                while (true)
                {
                    try
                    {
                        conn.Value.Ping();

                        var bgcolor = Console.BackgroundColor;
                        var forecolor = Console.ForegroundColor;
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($"正在模糊订阅【{pool.Key}】(channels:{string.Join(",", Channels)})");
                        Console.BackgroundColor = bgcolor;
                        Console.ForegroundColor = forecolor;
                        Console.WriteLine();

                        //conn.Value.Socket?.SetSocketOption(System.Net.Sockets.SocketOptionLevel.Socket, System.Net.Sockets.SocketOptionName.KeepAlive, 60000);
                        conn.Value.ReceiveTimeout = 0;
                        conn.Value.PSubscribe(this.Channels);

                        if (IsPUnsubscribed == false)
                        {
                            //服务器断开连接 IsConnected == false https://github.com/2881099/csredis/issues/37
                            if (conn.Value.IsConnected == false)
                                throw new Exception("redis-server 连接已断开");
                        }
                    }
                    catch (Exception ex)
                    {
                        if (IsPUnsubscribed) break;

                        var bgcolor = Console.BackgroundColor;
                        var forecolor = Console.ForegroundColor;
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($"模糊订阅出错【{pool.Key}】(channels:{string.Join(",", Channels)})：{ex.Message}，3秒后重连。。。");
                        Console.BackgroundColor = bgcolor;
                        Console.ForegroundColor = forecolor;
                        Console.WriteLine();
                        Thread.CurrentThread.Join(1000 * 3);

                        conn.ResetValue();
                        conn.Value.SubscriptionReceived += SubscriptionReceived;
                    }
                }
            }

            public void PUnsubscribe()
            {
                this.Dispose();
            }

            public void Dispose()
            {
                this.IsPUnsubscribed = true;
                if (this.RedisConnections != null)
                {
                    foreach (var conn in this.RedisConnections)
                    {
                        try { conn.Value.PUnsubscribe(); } catch { }
                        try { conn.Value.ReceiveTimeout = (conn.Pool as RedisClientPool)._policy._syncTimeout; } catch { }
                        conn.Pool.Return(conn, true);
                    }
                }
            }
        }
        public class PSubscribePMessageEventArgs : SubscribeMessageEventArgs
        {
            /// <summary>
            /// 匹配模式
            /// </summary>
            public string Pattern { get; set; }
        }
        #endregion

        #region 使用列表实现订阅发布 lpush + blpop
        /// <summary>
        /// 使用lpush + blpop订阅端（多端非争抢模式），都可以收到消息
        /// </summary>
        /// <param name="listKey">list key（不含prefix前辍）</param>
        /// <param name="clientId">订阅端标识，若重复则争抢，若唯一必然收到消息</param>
        /// <param name="onMessage">接收消息委托</param>
        /// <returns></returns>
        public SubscribeListBroadcastObject SubscribeListBroadcast(string listKey, string clientId, Action<string> onMessage)
        {
            this.HSetNx($"{listKey}_SubscribeListBroadcast", clientId, 1);
            var subobj = new SubscribeListBroadcastObject
            {
                OnDispose = () =>
                {
                    this.HDel($"{listKey}_SubscribeListBroadcast", clientId);
                }
            };
            //订阅其他端转发的消息
            subobj.SubscribeLists.Add(this.SubscribeList($"{listKey}_{clientId}", onMessage));
            //订阅主消息，接收消息后分发
            subobj.SubscribeLists.Add(this.SubscribeList(new[] { listKey }, (key, msg) =>
            {
                try
                {
                    this.HSetNx($"{listKey}_SubscribeListBroadcast", clientId, 1);
                    if (msg == null) return;

                    var clients = this.HKeys($"{listKey}_SubscribeListBroadcast");
                    var pipe = this.StartPipe();
                    foreach (var c in clients)
                        if (string.Compare(clientId, c, true) != 0) //过滤本端分发
                            pipe.LPush($"{listKey}_{c}", msg);
                    pipe.EndPipe();
                    onMessage?.Invoke(msg);
                }
                catch (ObjectDisposedException)
                {
                }
                catch (Exception ex)
                {
                    var bgcolor = Console.BackgroundColor;
                    var forecolor = Console.ForegroundColor;
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"列表订阅出错(listKey:{listKey})：{ex.Message}");
                    Console.BackgroundColor = bgcolor;
                    Console.ForegroundColor = forecolor;
                    Console.WriteLine();
                }
            }, true));

            AppDomain.CurrentDomain.ProcessExit += (s1, e1) =>
            {
                subobj.Dispose();
            };
            try
            {
                Console.CancelKeyPress += (s1, e1) =>
                {
                    if (e1.Cancel) return;
                    subobj.Dispose();
                };
            }
            catch { }

            return subobj;
        }
        public class SubscribeListBroadcastObject : IDisposable
        {
            internal Action OnDispose;
            internal List<SubscribeListObject> SubscribeLists = new List<SubscribeListObject>();

            public void Dispose()
            {
                try { OnDispose?.Invoke(); } catch (ObjectDisposedException) { }
                foreach (var sub in SubscribeLists) sub.Dispose();
            }
        }
        /// <summary>
        /// 使用lpush + blpop订阅端（多端争抢模式），只有一端收到消息
        /// </summary>
        /// <param name="listKey">list key（不含prefix前辍）</param>
        /// <param name="onMessage">接收消息委托</param>
        /// <returns></returns>
        public SubscribeListObject SubscribeList(string listKey, Action<string> onMessage) => SubscribeList(new[] { listKey }, (k, v) => onMessage(v), false);
        /// <summary>
        /// 使用lpush + blpop订阅端（多端争抢模式），只有一端收到消息
        /// </summary>
        /// <param name="listKeys">支持多个 key（不含prefix前辍）</param>
        /// <param name="onMessage">接收消息委托，参数1：key；参数2：消息体</param>
        /// <returns></returns>
        public SubscribeListObject SubscribeList(string[] listKeys, Action<string, string> onMessage) => SubscribeList(listKeys, onMessage, false);
        private SubscribeListObject SubscribeList(string[] listKeys, Action<string, string> onMessage, bool ignoreEmpty)
        {
            if (listKeys == null || listKeys.Any() == false) throw new ArgumentException("参数 listKey 不可为空");
            var listKeysStr = string.Join(", ", listKeys);
            var isMultiKey = listKeys.Length > 1;
            var subobj = new SubscribeListObject();

            var bgcolor = Console.BackgroundColor;
            var forecolor = Console.ForegroundColor;
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"正在订阅列表(listKey:{listKeysStr})");
            Console.BackgroundColor = bgcolor;
            Console.ForegroundColor = forecolor;
            Console.WriteLine();

            new Thread(() =>
            {
                while (subobj.IsUnsubscribed == false)
                {
                    try
                    {
                        if (isMultiKey)
                        {
                            var msg = this.BLPopWithKey(5, listKeys);
                            if (msg != null)
                                if (!ignoreEmpty || (ignoreEmpty && !string.IsNullOrEmpty(msg.Value.value)))
                                    onMessage?.Invoke(msg.Value.key, msg.Value.value);
                        }
                        else
                        {
                            var msg = this.BLPop(5, listKeys);
                            if (!ignoreEmpty || (ignoreEmpty && !string.IsNullOrEmpty(msg)))
                                onMessage?.Invoke(listKeys[0], msg);
                        }
                    }
                    catch (ObjectDisposedException)
                    {
                    }
                    catch (Exception ex)
                    {
                        bgcolor = Console.BackgroundColor;
                        forecolor = Console.ForegroundColor;
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($"列表订阅出错(listKey:{listKeysStr})：{ex.Message}");
                        Console.BackgroundColor = bgcolor;
                        Console.ForegroundColor = forecolor;
                        Console.WriteLine();

                        Thread.CurrentThread.Join(3000);
                    }
                }
            }).Start();

            AppDomain.CurrentDomain.ProcessExit += (s1, e1) =>
            {
                subobj.Dispose();
            };
            try
            {
                Console.CancelKeyPress += (s1, e1) =>
                {
                    if (e1.Cancel) return;
                    subobj.Dispose();
                };
            }
            catch { }

            return subobj;
        }
        public class SubscribeListObject : IDisposable
        {
            internal List<SubscribeListObject> OtherSubs = new List<SubscribeListObject>();
            public bool IsUnsubscribed { get; set; }

            public void Dispose()
            {
                this.IsUnsubscribed = true;
                foreach (var sub in OtherSubs) sub.Dispose();
            }
        }
        #endregion


#if !net40

        #region Pub/Sub
        /// <summary>
        /// 用于将信息发送到指定分区节点的频道，最终消息发布格式：1|message
        /// </summary>
        /// <param name="channel">频道名</param>
        /// <param name="message">消息文本</param>
        /// <returns></returns>
        public async Task<long> PublishAsync(string channel, string message)
        {
            var msgid = await HIncrByAsync("csredisclient:Publish:msgid", channel, 1);
            return await ExecuteScalarAsync(channel, (c, k) => c.Value.PublishAsync(channel, $"{msgid}|{message}"));
        }
        /// <summary>
        /// 用于将信息发送到指定分区节点的频道，与 Publish 方法不同，不返回消息id头，即 1|
        /// </summary>
        /// <param name="channel">频道名</param>
        /// <param name="message">消息文本</param>
        /// <returns></returns>
        public Task<long> PublishNoneMessageIdAsync(string channel, string message) => ExecuteScalarAsync(channel, (c, k) => c.Value.PublishAsync(channel, message));
        /// <summary>
        /// 查看所有订阅频道
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public async Task<string[]> PubSubChannelsAsync(string pattern)
        {
            var ret = new List<string>();
            foreach (var pool in Nodes.Values)
                ret.AddRange(await GetAndExecuteAsync(pool, c => c.Value.PubSubChannelsAsync(pattern)));
            return ret.ToArray();
        }
        /// <summary>
        /// 查看所有模糊订阅端的数量
        /// </summary>
        /// <returns></returns>
        [Obsolete("分区模式下，其他客户端的模糊订阅可能不会返回")]
        public Task<long> PubSubNumPatAsync() => GetAndExecuteAsync(Nodes.First().Value, c => c.Value.PubSubNumPatAsync());
        /// <summary>
        /// 查看所有订阅端的数量
        /// </summary>
        /// <param name="channels">频道</param>
        /// <returns></returns>
        [Obsolete("分区模式下，其他客户端的订阅可能不会返回")]
        public async Task<Dictionary<string, long>> PubSubNumSubAsync(params string[] channels) => (await ExecuteArrayAsync(channels, (c, k) =>
        {
            var prefix = (c.Pool as RedisClientPool).Prefix;
            return c.Value.PubSubNumSubAsync(k.Select(z => string.IsNullOrEmpty(prefix) == false && z.StartsWith(prefix) ? z.Substring(prefix.Length) : z).ToArray());
        })).ToDictionary(z => z.Item1, y => y.Item2);
        #endregion

#endif

    }
}