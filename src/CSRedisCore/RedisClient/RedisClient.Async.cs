
using System.Collections.Concurrent;
#if !net40
using CSRedis.Internal.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSRedis
{
    public partial class RedisClient
    {
        /// <summary>
        /// Open connection to redis server
        /// </summary>
        /// <returns>True on success</returns>
        public virtual async Task<bool> ConnectAsync()
        {
            return await _connector.ConnectAsync();
        }

        /// <summary>
        /// Call arbitrary redis command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual async Task<object> CallAsync(string command, params string[] args)
        {
            return await WriteAsync(RedisCommands.Call(command, args));
        }

        internal ConcurrentQueue<TaskCompletionSource<object>> _asyncPipe;

        async Task<T> WriteAsync<T>(RedisCommand<T> command)
        {
            if (_transaction.Active)
                return await _transaction.WriteAsync(command);
            else if (_asyncPipe != null)
            {
                var tsc = new TaskCompletionSource<object>();
                _asyncPipe.Enqueue(tsc);

                _connector.Pipeline.Write(command);

                var ret = await tsc.Task;
                return (T)ret;
            }
            else
                return await _connector.CallAsync(command);
        } 

    }
}
#endif