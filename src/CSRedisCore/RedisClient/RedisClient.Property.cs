using System.Globalization;
using System.Threading;
using CSRedis.Internal;
using System;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CSRedis.Internal.IO;
using System.Net;
using System.Net.Sockets;


namespace CSRedis
{
    /// <summary>
    /// Represents a client connection to a Redis server instance
    /// </summary>
    public partial class RedisClient
    {
        const int DefaultPort = 6379;
        const bool DefaultSSL = false;
        const int DefaultConcurrency = 1000;
        const int DefaultBufferSize = 10240;
        readonly RedisConnector _connector;
        readonly RedisTransaction _transaction;
        readonly SubscriptionListener _subscription;
        readonly MonitorListener _monitor;
        bool _streaming;

        internal Socket Socket => (_connector?._redisSocket as RedisSocket)?._socket;

        /// <summary>
        /// Occurs when a subscription message is received
        /// </summary>
        public event EventHandler<RedisSubscriptionReceivedEventArgs> SubscriptionReceived;

        /// <summary>
        /// Occurs when a subscription channel is added or removed
        /// </summary>
        public event EventHandler<RedisSubscriptionChangedEventArgs> SubscriptionChanged;

        /// <summary>
        /// Occurs when a transaction command is acknowledged by the server
        /// </summary>
        public event EventHandler<RedisTransactionQueuedEventArgs> TransactionQueued;

        /// <summary>
        /// Occurs when a monitor message is received
        /// </summary>
        public event EventHandler<RedisMonitorEventArgs> MonitorReceived;

        /// <summary>
        /// Occurs when the connection has sucessfully reconnected
        /// </summary>
        public event EventHandler Connected;


        /// <summary>
        /// Get the Redis server hostname
        /// </summary>
        public string Host { get { return GetHost(); } }

        /// <summary>
        /// Get the Redis server port
        /// </summary>
        public int Port { get { return GetPort(); } }

        /// <summary>
        /// Get a value indicating whether the Redis client is connected to the server
        /// </summary>
        public bool IsConnected { get { return _connector.IsConnected; } }

        /// <summary>
        /// Get or set the string encoding used to communicate with the server
        /// </summary>
        public Encoding Encoding
        {
            get { return _connector.Encoding; }
            set { _connector.Encoding = value; }
        }

        /// <summary>
        /// Get or set the connection read timeout (milliseconds)
        /// </summary>
        public int ReceiveTimeout
        {
            get { return _connector.ReceiveTimeout; }
            set { _connector.ReceiveTimeout = value; }
        }

        /// <summary>
        /// Get or set the connection send timeout (milliseconds)
        /// </summary>
        public int SendTimeout
        {
            get { return _connector.SendTimeout; }
            set { _connector.SendTimeout = value; }
        }

        /// <summary>
        /// Get or set the number of times to attempt a reconnect after a connection fails
        /// </summary>
        public int ReconnectAttempts
        {
            get { return _connector.ReconnectAttempts; }
            set { _connector.ReconnectAttempts = value; }
        }

        /// <summary>
        /// Get or set the amount of time (milliseconds) to wait between reconnect attempts
        /// </summary>
        public int ReconnectWait
        {
            get { return _connector.ReconnectWait; }
            set { _connector.ReconnectWait = value; }
        }

    }
}
