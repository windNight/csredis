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

        #region Stream redis-server 5.0
        /// <summary>
        /// XACK命令用于从流的消费者组的待处理条目列表（简称PEL）中删除一条或多条消息。 当一条消息交付到某个消费者时，它将被存储在PEL中等待处理， 这通常出现在作为调用XREADGROUP命令的副作用，或者一个消费者通过调用XCLAIM命令接管消息的时候。 待处理消息被交付到某些消费者，但是服务器尚不确定它是否至少被处理了一次。 因此对新调用XREADGROUP来获取消费者的消息历史记录（比如用0作为ID）将返回此类消息。 类似地，待处理的消息将由检查PEL的XPENDING命令列出。
        /// <para></para>
        /// 一旦消费者成功地处理完一条消息，它应该调用XACK，这样这个消息就不会被再次处理， 且作为一个副作用，关于此消息的PEL条目也会被清除，从Redis服务器释放内存。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="group">组</param>
        /// <param name="id">消息id</param>
        /// <returns></returns>
        public long XAck(string key, string group, string id) => ExecuteScalar(key, (c, k) => c.Value.XAck(k, group, id));

        /// <summary>
        /// 将指定的流条目追加到指定key的流中。 如果key不存在，作为运行这个命令的副作用，将使用流的条目自动创建key。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="fieldValues">键值对数组</param>
        /// <returns></returns>
        public string XAdd(string key, params (string, string)[] fieldValues) => XAdd(key, 0, "*", fieldValues);
        /// <summary>
        /// 将指定的流条目追加到指定key的流中。 如果key不存在，作为运行这个命令的副作用，将使用流的条目自动创建key。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="id">消息id，如果指定的id参数是字符*（星号ASCII字符），XADD命令会自动为您生成一个唯一的ID。 但是，也可以指定一个良好格式的ID，以便新的条目以指定的ID准确存储</param>
        /// <param name="fieldValues">键值对数组</param>
        /// <returns></returns>
        public string XAdd(string key, string id = "*", params (string, string)[] fieldValues) => XAdd(key, 0, id, fieldValues);
        /// <summary>
        /// 将指定的流条目追加到指定key的流中。 如果key不存在，作为运行这个命令的副作用，将使用流的条目自动创建key。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="maxLen">上限流，当小于0时~</param>
        /// <param name="id">消息id，如果指定的id参数是字符*（星号ASCII字符），XADD命令会自动为您生成一个唯一的ID。 但是，也可以指定一个良好格式的ID，以便新的条目以指定的ID准确存储</param>
        /// <param name="fieldValues">键值对数组</param>
        /// <returns></returns>
        public string XAdd(string key, long maxLen, string id = "*", params (string, string)[] fieldValues) => ExecuteScalar(key, (c, k) => c.Value.XAdd(k, maxLen, id, fieldValues));

        /// <summary>
        /// 在流的消费者组上下文中，此命令改变待处理消息的所有权
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="group">组</param>
        /// <param name="consumer">消费者</param>
        /// <param name="minIdleTime">耗秒</param>
        /// <param name="id">消息id</param>
        /// <returns></returns>
        public (string id, string[] items)[] XClaim(string key, string group, string consumer, long minIdleTime, params string[] id) =>
            ExecuteScalar(key, (c, k) => c.Value.XClaim(k, group, consumer, minIdleTime, id));
        /// <summary>
        /// 在流的消费者组上下文中，此命令改变待处理消息的所有权
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="group">组</param>
        /// <param name="consumer">消费者</param>
        /// <param name="minIdleTime">耗秒</param>
        /// <param name="id">消息id</param>
        /// <param name="idle">耗秒, 设置消息的空闲时间（自最后一次交付到目前的时间）。如果没有指定IDLE，则假设IDLE值为0，即时间计数被重置，因为消息现在有新的所有者来尝试处理它。</param>
        /// <param name="retryCount">将重试计数器设置为指定的值。这个计数器在每一次消息被交付的时候递增。</param>
        /// <param name="force">在待处理条目列表（PEL）中创建待处理消息条目，即使某些指定的ID尚未在分配给不同客户端的待处理条目列表（PEL）中。但是消息必须存在于流中，否则不存在的消息ID将会被忽略。</param>
        /// <returns></returns>
        public (string id, string[] items)[] XClaim(string key, string group, string consumer, long minIdleTime, string[] id, long idle, long retryCount, bool force) =>
            ExecuteScalar(key, (c, k) => c.Value.XClaim(k, group, consumer, minIdleTime, id, idle, retryCount, force));

        /// <summary>
        /// 在流的消费者组上下文中，此命令改变待处理消息的所有权
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="group">组</param>
        /// <param name="consumer">消费者</param>
        /// <param name="minIdleTime">耗秒</param>
        /// <param name="id">消息id</param>
        /// <returns>只返回消息id</returns>
        public string[] XClaimJustId(string key, string group, string consumer, long minIdleTime, params string[] id) =>
            ExecuteScalar(key, (c, k) => c.Value.XClaimJustId(k, group, consumer, minIdleTime, id));
        /// <summary>
        /// 在流的消费者组上下文中，此命令改变待处理消息的所有权
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="group">组</param>
        /// <param name="consumer">消费者</param>
        /// <param name="minIdleTime">耗秒</param>
        /// <param name="id">消息id</param>
        /// <param name="idle">耗秒, 设置消息的空闲时间（自最后一次交付到目前的时间）。如果没有指定IDLE，则假设IDLE值为0，即时间计数被重置，因为消息现在有新的所有者来尝试处理它。</param>
        /// <param name="retryCount">将重试计数器设置为指定的值。这个计数器在每一次消息被交付的时候递增。</param>
        /// <param name="force">在待处理条目列表（PEL）中创建待处理消息条目，即使某些指定的ID尚未在分配给不同客户端的待处理条目列表（PEL）中。但是消息必须存在于流中，否则不存在的消息ID将会被忽略。</param>
        /// <returns>只返回消息id</returns>
        public string[] XClaimJustId(string key, string group, string consumer, long minIdleTime, string[] id, long idle, long retryCount, bool force) =>
            ExecuteScalar(key, (c, k) => c.Value.XClaimJustId(k, group, consumer, minIdleTime, id, idle, retryCount, force));

        /// <summary>
        /// 从指定流中移除指定的条目，并返回成功删除的条目的数量，在传递的ID不存在的情况下， 返回的数量可能与传递的ID数量不同。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="id">消息id</param>
        /// <returns></returns>
        public long XDel(string key, params string[] id) => ExecuteScalar(key, (c, k) => c.Value.XDel(k, id));

        /// <summary>
        /// 创建一个新的消费者组
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="group">组名</param>
        /// <param name="id">特殊的ID ‘$’（这表示：流中最后一项的ID）。在这种情况下，从该消费者组获取数据的消费者只能看到到达流的新元素。但如果你希望消费者组获取整个流的历史记录，使用0作为消费者组的开始ID。</param>
        /// <param name="MkStream">create the empty stream if it does not exist.</param>
        /// <returns>如果指定的消费者组已经存在，则该命令将返回-BUSYGROUP错误。</returns>
        public string XGroupCreate(string key, string group, string id = "$", bool MkStream = false) => ExecuteScalar(key, (c, k) => c.Value.XGroupCreate(k, group, id, MkStream));
        /// <summary>
        /// 设置要传递的下一条消息。 通常情况下，在消费者创建时设置下一个ID，作为XGROUP CREATE的最后一个参数。 但是使用这种形式，可以在以后修改下一个ID，而无需再次删除和创建使用者组。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="group">组名</param>
        /// <param name="id">特殊的ID ‘$’（这表示：流中最后一项的ID）。在这种情况下，从该消费者组获取数据的消费者只能看到到达流的新元素。但如果你希望消费者组获取整个流的历史记录，使用0作为消费者组的开始ID。</param>
        /// <returns></returns>
        public string XGroupSetId(string key, string group, string id = "$") => ExecuteScalar(key, (c, k) => c.Value.XGroupSetId(k, group, id));
        /// <summary>
        /// 销毁消费者组，即使存在活动的消费者和待处理消息，消费者组也将被销毁，因此请确保仅在真正需要时才调用此命令。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="group">组名</param>
        /// <returns></returns>
        public bool XGroupDestroy(string key, string group) => ExecuteScalar(key, (c, k) => c.Value.XGroupDestroy(k, group));
        /// <summary>
        /// 仅从消费者组中移除给定的消费者
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="group">组名</param>
        /// <param name="consumer">消费者</param>
        /// <returns></returns>
        public bool XGroupDelConsumer(string key, string group, string consumer) => ExecuteScalar(key, (c, k) => c.Value.XGroupDelConsumer(k, group, consumer));

        /// <summary>
        /// 返回有关存储在特定键的流的一般信息
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public (long length, long radixTreeKeys, long radixTreeNodes, long groups, string lastGeneratedId, (string id, string[] items) firstEntry, (string id, string[] items) lastEntry) XInfoStream(string key) =>
            ExecuteScalar(key, (c, k) => c.Value.XInfoStream(k));
        /// <summary>
        /// 获得与流关联的所有消费者组数据，该命令显示该组中已知的消费者数量，以及该组中的待处理消息（已传递但尚未确认）数量
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public (string name, long consumers, long pending, string lastDeliveredId)[] XInfoGroups(string key) =>
            ExecuteScalar(key, (c, k) => c.Value.XInfoGroups(k));
        /// <summary>
        /// 取得指定消费者组中的消费者列表，返回每个消息者的空闲毫秒时间（最后一个字段）以及消费者名称和待处理消息数量
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="group"></param>
        /// <returns></returns>
        public (string name, long pending, long idle)[] XInfoConsumers(string key, string group) =>
            ExecuteScalar(key, (c, k) => c.Value.XInfoConsumers(k, group));

        /// <summary>
        /// 返回流中的条目数。如果指定的key不存在，则此命令返回0，就好像该流为空。 但是请注意，与其他的Redis类型不同，零长度流是可能的，所以你应该调用TYPE 或者 EXISTS 来检查一个key是否存在。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <returns></returns>
        public long XLen(string key) => ExecuteScalar(key, (c, k) => c.Value.XLen(k));

        /// <summary>
        /// XPENDING命令是检查待处理消息列表的接口，因此它是一个非常重要的命令，用于观察和了解消费者组正在发生的事情：哪些客户端是活跃的，哪些消息在等待消费，或者查看是否有空闲的消息。此外，该命令与XCLAIM一起使用，用于实现长时间故障的消费者的恢复，因此不处理某些消息：不同的消费者可以认领该消息并继续处理。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="group"></param>
        /// <returns></returns>
        public (long count, string minId, string maxId, (string consumer, long count)[] pendings) XPending(string key, string group) =>
            ExecuteScalar(key, (c, k) => c.Value.XPending(k, group));
        /// <summary>
        /// XPENDING命令是检查待处理消息列表的接口，因此它是一个非常重要的命令，用于观察和了解消费者组正在发生的事情：哪些客户端是活跃的，哪些消息在等待消费，或者查看是否有空闲的消息。此外，该命令与XCLAIM一起使用，用于实现长时间故障的消费者的恢复，因此不处理某些消息：不同的消费者可以认领该消息并继续处理。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="group"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="count"></param>
        /// <param name="consumer"></param>
        /// <returns></returns>
        public (string id, string consumer, long idle, long transferTimes)[] XPending(string key, string group, string start, string end, long count, string consumer = null) =>
            ExecuteScalar(key, (c, k) => c.Value.XPending(k, group, start, end, count, consumer = null));

        /// <summary>
        /// 返回流中满足给定ID范围的条目。范围由最小和最大ID指定。所有ID在指定的两个ID之间或与其中一个ID相等（闭合区间）的条目将会被返回。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="start">开始id，特殊：- 和 +</param>
        /// <param name="end">结束id，特殊：- 和 +</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public (string id, string[] items)[] XRange(string key, string start, string end, long count = 1) =>
            ExecuteScalar(key, (c, k) => c.Value.XRange(k, start, end, count));
        /// <summary>
        /// 与XRANGE完全相同，但显著的区别是以相反的顺序返回条目，并以相反的顺序获取开始-结束参数：在XREVRANGE中，你需要先指定结束ID，再指定开始ID，该命令就会从结束ID侧开始生成两个ID之间（或完全相同）的所有元素。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="end">结束id，特殊：- 和 +</param>
        /// <param name="start">开始id，特殊：- 和 +</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public (string id, string[] items)[] XRevRange(string key, string end, string start, long count = 1) =>
            ExecuteScalar(key, (c, k) => c.Value.XRevRange(k, end, start, count));

        /// <summary>
        /// 从一个或者多个流中读取数据，仅返回ID大于调用者报告的最后接收ID的条目。此命令有一个阻塞选项，用于等待可用的项目，类似于BRPOP或者BZPOPMIN等等。
        /// </summary>
        /// <param name="count">数量</param>
        /// <param name="block">阻塞选项，毫秒</param>
        /// <param name="streams">(key,id) 数组</param>
        /// <returns></returns>
        public (string key, (string id, string[] items)[] data)[] XRead(long count, long block, params (string key, string id)[] streams) =>
            NodesNotSupport(streams.Select(a => a.key).ToArray(), null, (c, k) => c.Value.XRead(count, block, streams.Select((a, i) => (k[i], a.id)).ToArray()));
        /// <summary>
        /// XREADGROUP命令是XREAD命令的特殊版本，支持消费者组。
        /// </summary>
        /// <param name="group">组</param>
        /// <param name="consumer">消费者</param>
        /// <param name="count">数量</param>
        /// <param name="block">阻塞选项，毫秒</param>
        /// <param name="streams">(key,id) 数组</param>
        /// <returns></returns>
        public (string key, (string id, string[] items)[] data)[] XReadGroup(string group, string consumer, long count, long block, params (string key, string id)[] streams) =>
            NodesNotSupport(streams.Select(a => a.key).ToArray(), null, (c, k) => c.Value.XReadGroup(group, consumer, count, block, streams.Select((a, i) => (k[i], a.id)).ToArray()));

        /// <summary>
        /// XTRIM将流裁剪为指定数量的项目，如有需要，将驱逐旧的项目（ID较小的项目）。此命令被设想为接受多种修整策略，但目前只实现了一种，即MAXLEN，并且与XADD中的MAXLEN选项完全相同。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="maxLen">上限流，当小于0时~</param>
        /// <returns></returns>
        public long XTrim(string key, long maxLen) => ExecuteScalar(key, (c, k) => c.Value.XTrim(k, maxLen));
       
        #endregion


#if !net40
         
#endif

    }
}