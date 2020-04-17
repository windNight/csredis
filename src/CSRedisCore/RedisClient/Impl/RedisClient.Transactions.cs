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
        #region Transactions
        /// <summary>
        /// Discard all commands issued after MULTI
        /// </summary>
        /// <returns>Status code</returns>
        public virtual string Discard()
        {
            string response = _transaction.Abort();
            if (_connector.IsPipelined)
                return _connector.EndPipe()[0].ToString();
            return response;
        }

        /// <summary>
        /// Execute all commands issued after MULTI
        /// </summary>
        /// <returns>Array of output from all transaction commands</returns>
        public virtual object[] Exec()
        {
            return _transaction.Execute();
        }

        /// <summary>
        /// Mark the start of a transaction block
        /// </summary>
        /// <returns>Status code</returns>
        public virtual string Multi()
        {
            return _transaction.Start();
        }

        /// <summary>
        /// Forget about all watched keys
        /// </summary>
        /// <returns>Status code</returns>
        public virtual string Unwatch()
        {
            return Write(RedisCommands.Unwatch());
        }

        /// <summary>
        /// Watch the given keys to determine execution of the MULTI/EXEC block
        /// </summary>
        /// <param name="keys">Keys to watch</param>
        /// <returns>Status code</returns>
        public virtual string Watch(params string[] keys)
        {
            return Write(RedisCommands.Watch(keys));
        }
        #endregion

#if !net40

        #region Transactions
        /// <summary>
        /// Mark the start of a transaction block
        /// </summary>
        /// <returns>Status code</returns>
        public virtual async Task<string> MultiAsync()
        {
            return await _transaction.StartAsync();
        }

        /// <summary>
        /// Discard all commands issued after MULTI
        /// </summary>
        /// <returns>Status code</returns>
        public virtual async Task<string> DiscardAsync()
        {
            return await _transaction.AbortAsync();
        }

        /// <summary>
        /// Execute all commands issued after MULTI
        /// </summary>
        /// <returns>Array of output from all transaction commands</returns>
        public virtual async Task<object[]> ExecAsync()
        {
            return await _transaction.ExecuteAsync();
        }

        /// <summary>
        /// Forget about all watched keys
        /// </summary>
        /// <returns>Status code</returns>
        public virtual async Task<string> UnwatchAsync()
        {
            return await WriteAsync(RedisCommands.Unwatch());
        }

        /// <summary>
        /// Watch the given keys to determine execution of the MULTI/EXEC block
        /// </summary>
        /// <param name="keys">Keys to watch</param>
        /// <returns>Status code</returns>
        public virtual async Task<string> WatchAsync(params string[] keys)
        {
            return await WriteAsync(RedisCommands.Watch(keys));
        }

        #endregion

#endif

    }
}
