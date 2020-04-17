using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if !net40
using System.Threading.Tasks;
#endif

namespace CSRedis
{
    /// <summary>
    /// Interface for syncronous RedisClient methods
    /// </summary>
    public interface IRedisClientUsePubSub
    {

        #region Pub/Sub
        /// <summary>
        /// Listen for messages published to channels matching the given patterns
        /// </summary>
        /// <param name="channelPatterns">Patterns to subscribe</param>
        void PSubscribe(params string[] channelPatterns);



        /// <summary>
        /// Post a message to a channel
        /// </summary>
        /// <param name="channel">Channel to post message</param>
        /// <param name="message">Message to send</param>
        /// <returns>Number of clients that received the message</returns>
        long Publish(string channel, string message);




        /// <summary>
        /// List the currently active channels
        /// </summary>
        /// <param name="pattern">Return only channels matching this pattern</param>
        /// <returns>Array of channel names</returns>
        string[] PubSubChannels(string pattern = null);




        /// <summary>
        ///
        /// </summary>
        /// <param name="channels">Channel names</param>
        /// <returns>Array of channel/count tuples</returns>
        Tuple<string, long>[] PubSubNumSub(params string[] channels);




        /// <summary>
        ///
        /// </summary>
        /// <returns>Number of patterns all clients are subscribed to</returns>
        long PubSubNumPat();




        /// <summary>
        /// Stop listening for messages posted to channels matching the given patterns
        /// </summary>
        /// <param name="channelPatterns">Patterns to unsubscribe</param>
        void PUnsubscribe(params string[] channelPatterns);



        /// <summary>
        /// Listen for messages published to the given channels
        /// </summary>
        /// <param name="channels">Channels to subscribe</param>
        void Subscribe(params string[] channels);



        /// <summary>
        /// Stop listening for messages posted to the given channels
        /// </summary>
        /// <param name="channels">Channels to unsubscribe</param>
        void Unsubscribe(params string[] channels);



        #endregion

#if !net40

        #region Pub/Sub
        /// <summary>
        /// Post a message to a channel
        /// </summary>
        /// <param name="channel">Channel to post message</param>
        /// <param name="message">Message to send</param>
        /// <returns>Number of clients that received the message</returns>
        Task<long> PublishAsync(string channel, string message);




        /// <summary>
        /// List the currently active channels
        /// </summary>
        /// <param name="pattern">Glob-style channel pattern</param>
        /// <returns>Active channel names</returns>
        Task<string[]> PubSubChannelsAsync(string pattern = null);




        /// <summary>
        /// Return the number of subscribers (not counting clients subscribed to patterns) for the specified channels
        /// </summary>
        /// <param name="channels">Channels to query</param>
        /// <returns>Channel names and counts</returns>
        Task<Tuple<string, long>[]> PubSubNumSubAsync(params string[] channels);




        /// <summary>
        /// Return the number of subscriptions to patterns
        /// </summary>
        /// <returns>The number of patterns all the clients are subscribed to</returns>
        Task<long> PubSubNumPatAsync();



        #endregion

#endif

    }
}
