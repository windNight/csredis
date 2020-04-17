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
        /// Authenticate to the server
        /// </summary>
        /// <param name="password">Redis server password</param>
        /// <returns>Status message</returns>
        public virtual string Auth(string password)
        {
            return Write(RedisCommands.Auth(password));
        }

        /// <summary>
        /// Echo the given string
        /// </summary>
        /// <param name="message">Message to echo</param>
        /// <returns>Message</returns>
        public virtual string Echo(string message)
        {
            return Write(RedisCommands.Echo(message));
        }

        /// <summary>
        /// Ping the server
        /// </summary>
        /// <returns>Status message</returns>
        public virtual string Ping()
        {
            return Write(RedisCommands.Ping());
        }

        /// <summary>
        /// Close the connection
        /// </summary>
        /// <returns>Status message</returns>
        public virtual string Quit()
        {
            string response = Write(RedisCommands.Quit());
            _connector.Dispose();
            return response;
        }

        /// <summary>
        /// Change the selected database for the current connection
        /// </summary>
        /// <param name="index">Zero-based database index</param>
        /// <returns>Status message</returns>
        public virtual string Select(int index)
        {
            return Write(RedisCommands.Select(index));
        }

        #endregion //end Sync

#if !net40
        #region Async
        /// <summary>
        /// Authenticate to the server
        /// </summary>
        /// <param name="password">Server password</param>
        /// <returns>Task associated with status message</returns>
        public virtual async Task<string> AuthAsync(string password)
        {
            return await WriteAsync(RedisCommands.Auth(password));
        }

        /// <summary>
        /// Echo the given string
        /// </summary>
        /// <param name="message">Message to echo</param>
        /// <returns>Task associated with echo response</returns>
        public virtual async Task<string> EchoAsync(string message)
        {
            return await WriteAsync(RedisCommands.Echo(message));
        }

        /// <summary>
        /// Ping the server
        /// </summary>
        /// <returns>Task associated with status message</returns>
        public virtual async Task<string> PingAsync()
        {
            return await WriteAsync(RedisCommands.Ping());
        }

        /// <summary>
        /// Close the connection
        /// </summary>
        /// <returns>Task associated with status message</returns>
        public virtual async Task<string> QuitAsync()
        {
            return await WriteAsync(RedisCommands.Quit())
                .ContinueWith<string>(t =>
                {
                    _connector.Dispose();
                    return t.Result;
                });
        }

        /// <summary>
        /// Change the selected database for the current connection
        /// </summary>
        /// <param name="index">Zero-based database index</param>
        /// <returns>Status message</returns>
        public virtual async Task<string> SelectAsync(int index)
        {
            return await WriteAsync(RedisCommands.Select(index));
        }

        #endregion //end async
#endif

    }
}
