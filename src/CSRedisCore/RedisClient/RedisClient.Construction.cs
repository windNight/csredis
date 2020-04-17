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
        /// <summary>
        /// Create a new RedisClient using default port and encoding
        /// </summary>
        /// <param name="host">Redis server hostname</param>
        public RedisClient(string host)
            : this(host, DefaultPort)
        { }

        /// <summary>
        /// Create a new RedisClient
        /// </summary>
        /// <param name="host">Redis server hostname</param>
        /// <param name="port">Redis server port</param>
        public RedisClient(string host, int port)
            : this(host, port, DefaultSSL)
        { }

        /// <summary>
        /// Create a new RedisClient
        /// </summary>
        /// <param name="host">Redis server hostname</param>
        /// <param name="port">Redis server port</param>
        /// <param name="ssl">Set to true if remote Redis server expects SSL</param>
        public RedisClient(string host, int port, bool ssl)
            : this(host, port, ssl, DefaultConcurrency, DefaultBufferSize)
        { }

        /// <summary>
        /// Create a new RedisClient
        /// </summary>
        /// <param name="endpoint">Redis server</param>
        public RedisClient(EndPoint endpoint)
            : this(endpoint, DefaultSSL)
        { }

        /// <summary>
        /// Create a new RedisClient
        /// </summary>
        /// <param name="endpoint">Redis server</param>
        /// <param name="ssl">Set to true if remote Redis server expects SSL</param>
        public RedisClient(EndPoint endpoint, bool ssl)
            : this(endpoint, ssl, DefaultConcurrency, DefaultBufferSize)
        { }

        /// <summary>
        /// Create a new RedisClient with specific async concurrency settings
        /// </summary>
        /// <param name="host">Redis server hostname</param>
        /// <param name="port">Redis server port</param>
        /// <param name="asyncConcurrency">Max concurrent threads (default 1000)</param>
        /// <param name="asyncBufferSize">Async thread buffer size (default 10240 bytes)</param>
        public RedisClient(string host, int port, int asyncConcurrency, int asyncBufferSize)
            : this(host, port, DefaultSSL, asyncConcurrency, asyncBufferSize)
        { }

        /// <summary>
        /// Create a new RedisClient with specific async concurrency settings
        /// </summary>
        /// <param name="host">Redis server hostname</param>
        /// <param name="port">Redis server port</param>
        /// <param name="ssl">Set to true if remote Redis server expects SSL</param>
        /// <param name="asyncConcurrency">Max concurrent threads (default 1000)</param>
        /// <param name="asyncBufferSize">Async thread buffer size (default 10240 bytes)</param>
        public RedisClient(string host, int port, bool ssl, int asyncConcurrency, int asyncBufferSize)
            : this(new DnsEndPoint(host, port), ssl, asyncConcurrency, asyncBufferSize)
        { }

        /// <summary>
        /// Create a new RedisClient with specific async concurrency settings
        /// </summary>
        /// <param name="endpoint">Redis server</param>
        /// <param name="asyncConcurrency">Max concurrent threads (default 1000)</param>
        /// <param name="asyncBufferSize">Async thread buffer size (default 10240 bytes)</param>
        public RedisClient(EndPoint endpoint, int asyncConcurrency, int asyncBufferSize)
            : this(endpoint, DefaultSSL, asyncConcurrency, asyncBufferSize)
        { }

        /// <summary>
        /// Create a new RedisClient with specific async concurrency settings
        /// </summary>
        /// <param name="endpoint">Redis server</param>
        /// <param name="ssl">Set to true if remote Redis server expects SSL</param>
        /// <param name="asyncConcurrency">Max concurrent threads (default 1000)</param>
        /// <param name="asyncBufferSize">Async thread buffer size (default 10240 bytes)</param>
        public RedisClient(EndPoint endpoint, bool ssl, int asyncConcurrency, int asyncBufferSize)
            : this(new RedisSocket(ssl), endpoint, asyncConcurrency, asyncBufferSize)
        { }

        internal RedisClient(IRedisSocket socket, EndPoint endpoint)
            : this(socket, endpoint, DefaultConcurrency, DefaultBufferSize)
        { }

        internal RedisClient(IRedisSocket socket, EndPoint endpoint, int asyncConcurrency, int asyncBufferSize)
        {
            // use invariant culture - we have to set it explicitly for every thread we create to 
            // prevent any floating-point problems (mostly because of number formats in non en-US cultures).
            //CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture; 这行会影响 string.Compare 结果

            _connector = new RedisConnector(endpoint, socket, asyncConcurrency, asyncBufferSize);
            _transaction = new RedisTransaction(_connector);
            _subscription = new SubscriptionListener(_connector);
            _monitor = new MonitorListener(_connector);

            _subscription.MessageReceived += OnSubscriptionReceived;
            _subscription.Changed += OnSubscriptionChanged;
            _monitor.MonitorReceived += OnMonitorReceived;
            _connector.Connected += OnConnectionConnected;
            _transaction.TransactionQueued += OnTransactionQueued;
        }

    }
}
