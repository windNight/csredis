using CSRedis.Internal.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
#if !net40
using System.Threading.Tasks;
#endif

namespace CSRedis
{
    public partial class RedisClient
    {
        #region Sync

        /// <summary>
        /// Listen for messages published to channels matching the given patterns
        /// </summary>
        /// <param name="channelPatterns">Patterns to subscribe</param>
        public virtual void PSubscribe(params string[] channelPatterns)
        {
            _subscription.Send(RedisCommands.PSubscribe(channelPatterns));
        }

        /// <summary>
        /// Post a message to a channel
        /// </summary>
        /// <param name="channel">Channel to post message</param>
        /// <param name="message">Message to send</param>
        /// <returns>Number of clients that received the message</returns>
        public virtual long Publish(string channel, string message)
        {
            return Write(RedisCommands.Publish(channel, message));
        }

        /// <summary>
        /// List the currently active channels
        /// </summary>
        /// <param name="pattern">Return only channels matching this pattern</param>
        /// <returns>Array of channel names</returns>
        public virtual string[] PubSubChannels(string pattern = null)
        {
            return Write(RedisCommands.PubSubChannels(pattern));
        }

        /// <summary>
        /// Return the number of subscribers for the specified channels
        /// </summary>
        /// <param name="channels">Channel names</param>
        /// <returns>Array of channel/count tuples</returns>
        public virtual Tuple<string, long>[] PubSubNumSub(params string[] channels)
        {
            return Write(RedisCommands.PubSubNumSub(channels));
        }

        /// <summary>
        /// Return the number of subscriptions to patterns
        /// </summary>
        /// <returns>Number of patterns all clients are subscribed to</returns>
        public virtual long PubSubNumPat()
        {
            return Write(RedisCommands.PubSubNumPat());
        }

        /// <summary>
        /// Stop listening for messages posted to channels matching the given patterns
        /// </summary>
        /// <param name="channelPatterns">Patterns to unsubscribe</param>
        public virtual void PUnsubscribe(params string[] channelPatterns)
        {
            _subscription.Send(RedisCommands.PUnsubscribe(channelPatterns));
        }

        /// <summary>
        /// Listen for messages published to the given channels
        /// </summary>
        /// <param name="channels">Channels to subscribe</param>
        public virtual void Subscribe(params string[] channels)
        {
            _subscription.Send(RedisCommands.Subscribe(channels));
        }

        /// <summary>
        /// Stop listening for messages posted to the given channels
        /// </summary>
        /// <param name="channels">Channels to unsubscribe</param>
        public virtual void Unsubscribe(params string[] channels)
        {
            _subscription.Send(RedisCommands.Unsubscribe(channels));
        }

        #endregion //end Sync

#if !net40

        #region Async

        /// <summary>
        /// Post a message to a channel
        /// </summary>
        /// <param name="channel">Channel to post message</param>
        /// <param name="message">Message to send</param>
        /// <returns>Number of clients that received the message</returns>
        public virtual async Task<long> PublishAsync(string channel, string message)
        {
            return await WriteAsync(RedisCommands.Publish(channel, message));
        }

        /// <summary>
        /// List the currently active channels
        /// </summary>
        /// <param name="pattern">Glob-style channel pattern</param>
        /// <returns>Active channel names</returns>
        public virtual async Task<string[]> PubSubChannelsAsync(string pattern = null)
        {
            return await WriteAsync(RedisCommands.PubSubChannels(pattern));
        }

        /// <summary>
        /// Return the number of subscribers (not counting clients subscribed to patterns) for the specified channels
        /// </summary>
        /// <param name="channels">Channels to query</param>
        /// <returns>Channel names and counts</returns>
        public virtual async Task<Tuple<string, long>[]> PubSubNumSubAsync(params string[] channels)
        {
            return await WriteAsync(RedisCommands.PubSubNumSub(channels));
        }

        /// <summary>
        /// Return the number of subscriptions to patterns
        /// </summary>
        /// <returns>The number of patterns all the clients are subscribed to</returns>
        public virtual async Task<long> PubSubNumPatAsync()
        {
            return await WriteAsync(RedisCommands.PubSubNumPat());
        }

        #endregion//end Async

#endif

    }
}
