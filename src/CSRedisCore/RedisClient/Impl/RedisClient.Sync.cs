using CSRedis.Internal.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSRedis
{
    public partial class RedisClient
    {
        /// <summary>
        /// Connect to the remote host
        /// </summary>
        /// <param name="timeout">Connection timeout in milliseconds</param>
        /// <returns>True if connected</returns>
        public virtual bool Connect(int timeout)
        {
            return _connector.Connect(timeout);
        }

        /// <summary>
        /// Call arbitrary Redis command
        /// </summary>
        /// <param name="command">Command name</param>
        /// <param name="args">Command arguments</param>
        /// <returns>Redis object</returns>
        public virtual object Call(string command, params string[] args)
        {
            return Write(RedisCommands.Call(command, args));
        }

        T Write<T>(RedisCommand<T> command)
        {
            if (_transaction.Active)
                return _transaction.Write(command);
            else if (_monitor.Listening)
                return default(T);
            else if (_streaming)
            {
                _connector.Write(command);
                return default(T);
            }
            else
                return _connector.Call(command);
        }

        #region Streams 5.0
        public virtual long XAck(string key, string group, params string[] id) => Write(RedisCommands.XAck(key, group, id));

        public virtual string XAdd(string key, long maxLen, string id = "*", params (string, string)[] fieldValues) => Write(RedisCommands.XAdd(key, maxLen, id, fieldValues));

        public virtual (string id, string[] items)[] XClaim(string key, string group, string consumer, long minIdleTime, params string[] id) =>
            Write(RedisCommands.XClaim(key, group, consumer, minIdleTime, id));
        public virtual (string id, string[] items)[] XClaim(string key, string group, string consumer, long minIdleTime, string[] id, long idle, long retryCount, bool force) =>
            Write(RedisCommands.XClaim(key, group, consumer, minIdleTime, id, idle, retryCount, force));

        public virtual string[] XClaimJustId(string key, string group, string consumer, long minIdleTime, params string[] id) =>
            Write(RedisCommands.XClaimJustId(key, group, consumer, minIdleTime, id));
        public virtual string[] XClaimJustId(string key, string group, string consumer, long minIdleTime, string[] id, long idle, long retryCount, bool force) =>
            Write(RedisCommands.XClaimJustId(key, group, consumer, minIdleTime, id, idle, retryCount, force));

        public virtual long XDel(string key, params string[] id) => Write(RedisCommands.XDel(key, id));

        public virtual string XGroupCreate(string key, string group, string id = "$", bool MkStream = false) => Write(RedisCommands.XGroup.Create(key, group, id, MkStream));
        public virtual string XGroupSetId(string key, string group, string id = "$") => Write(RedisCommands.XGroup.SetId(key, group, id));
        public virtual bool XGroupDestroy(string key, string group) => Write(RedisCommands.XGroup.Destroy(key, group));
        public virtual bool XGroupDelConsumer(string key, string group, string consumer) => Write(RedisCommands.XGroup.DelConsumer(key, group, consumer));

        public virtual (long length, long radixTreeKeys, long radixTreeNodes, long groups, string lastGeneratedId, (string id, string[] items) firstEntry, (string id, string[] items) lastEntry) XInfoStream(string key) => Write(RedisCommands.XInfoStream(key));
        public virtual (string name, long consumers, long pending, string lastDeliveredId)[] XInfoGroups(string key) => Write(RedisCommands.XInfoGroups(key));
        public virtual (string name, long pending, long idle)[] XInfoConsumers(string key, string group) => Write(RedisCommands.XInfoConsumers(key, group));

        public virtual long XLen(string key) => Write(RedisCommands.XLen(key));

        public virtual (long count, string minId, string maxId, (string consumer, long count)[] pendings) XPending(string key, string group) => Write(RedisCommands.XPending(key, group));
        public virtual (string id, string consumer, long millisecond, long transferTimes)[] XPending(string key, string group, string start, string end, long count, string consumer = null) => Write(RedisCommands.XPending(key, group, start, end, count, consumer));

        public virtual (string id, string[] items)[] XRange(string key, string start, string end, long count = 1) => Write(RedisCommands.XRange(key, start, end, count));
        public virtual (string id, string[] items)[] XRevRange(string key, string end, string start, long count = 1) => Write(RedisCommands.XRevRange(key, end, start, count));

        public virtual (string key, (string id, string[] items)[] data)[] XRead(long count, long block, params (string key, string id)[] streams) =>
            Write(RedisCommands.XRead(count, block, streams));
        public virtual (string key, (string id, string[] items)[] data)[] XReadGroup(string group, string consumer, long count, long block, params (string key, string id)[] streams) =>
            Write(RedisCommands.XReadGroup(group, consumer, count, block, streams));

        public virtual long XTrim(string key, long maxLen) => Write(RedisCommands.XTrim(key, maxLen));
        #endregion

    }
}
