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
    public partial class RedisClient : IRedisClient//Sync, IRedisClientAsync
    { 

        /// <summary>
        /// Begin buffered pipeline mode (calls return immediately; use EndPipe() to execute batch)
        /// </summary>
        public void StartPipe()
        {
            _connector.BeginPipe();
        }

        /// <summary>
        /// Begin buffered pipeline mode within the context of a transaction (calls return immediately; use EndPipe() to excute batch)
        /// </summary>
        public void StartPipeTransaction()
        {
            _connector.BeginPipe();
            Multi();
        }

        /// <summary>
        /// Execute pipeline commands
        /// </summary>
        /// <returns>Array of batched command results</returns>
        public object[] EndPipe()
        {
            if (_transaction.Active)
                return _transaction.Execute();
            else
                return _connector.EndPipe();
        }

        /// <summary>
        /// Stream a BULK reply from the server using default buffer size
        /// </summary>
        /// <typeparam name="T">Response type</typeparam>
        /// <param name="destination">Destination stream</param>
        /// <param name="func">Client command to execute (BULK reply only)</param>
        public void StreamTo<T>(Stream destination, Func<IRedisClient, T> func)
        {
            StreamTo(destination, DefaultBufferSize, func);
        }

        /// <summary>
        /// Stream a BULK reply from the server
        /// </summary>
        /// <typeparam name="T">Response type</typeparam>
        /// <param name="destination">Destination stream</param>
        /// <param name="bufferSize">Size of buffer used to write server response</param>
        /// <param name="func">Client command to execute (BULK reply only)</param>
        public void StreamTo<T>(Stream destination, int bufferSize, Func<IRedisClient, T> func)
        {
            _streaming = true;
            func(this);
            _streaming = false;
            _connector.Read(destination, bufferSize);
        }

        /// <summary>
        /// Dispose all resources used by the current RedisClient
        /// </summary>
        public void Dispose()
        {
            _connector.Dispose();
        }

        void OnMonitorReceived(object sender, RedisMonitorEventArgs obj)
        {
            if (MonitorReceived != null)
                MonitorReceived(this, obj);
        }

        void OnSubscriptionReceived(object sender, RedisSubscriptionReceivedEventArgs args)
        {
            if (SubscriptionReceived != null)
                SubscriptionReceived(this, args);
        }

        void OnSubscriptionChanged(object sender, RedisSubscriptionChangedEventArgs args)
        {
            if (SubscriptionChanged != null)
                SubscriptionChanged(this, args);
        }

        void OnConnectionConnected(object sender, EventArgs args)
        {
            if (Connected != null)
                Connected(this, args);
        }

        void OnTransactionQueued(object sender, RedisTransactionQueuedEventArgs args)
        {
            if (TransactionQueued != null)
                TransactionQueued(this, args);
        }

        string GetHost()
        {
            if (_connector.EndPoint is IPEndPoint)
                return (_connector.EndPoint as IPEndPoint).Address.ToString();
            else if (_connector.EndPoint is DnsEndPoint)
                return (_connector.EndPoint as DnsEndPoint).Host;
            else
                return null;
        }

        int GetPort()
        {
            if (_connector.EndPoint is IPEndPoint)
                return (_connector.EndPoint as IPEndPoint).Port;
            else if (_connector.EndPoint is DnsEndPoint)
                return (_connector.EndPoint as DnsEndPoint).Port;
            else
                return -1;
        }
    }
}
