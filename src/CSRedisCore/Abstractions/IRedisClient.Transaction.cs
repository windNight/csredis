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
    public interface IRedisClientUseTransaction
    {
        #region Transactions
        /// <summary>
        /// Discard all commands issued after MULTI
        /// </summary>
        /// <returns>Status code</returns>
        string Discard();




        /// <summary>
        /// Execute all commands issued after MULTI
        /// </summary>
        /// <returns>Array of output from all transaction commands</returns>
        object[] Exec();




        /// <summary>
        /// Mark the start of a transaction block
        /// </summary>
        /// <returns>Status code</returns>
        string Multi();




        /// <summary>
        /// Forget about all watched keys
        /// </summary>
        /// <returns>Status code</returns>
        string Unwatch();




        /// <summary>
        /// Watch the given keys to determine execution of the MULTI/EXEC block
        /// </summary>
        /// <param name="keys">Keys to watch</param>
        /// <returns>Status code</returns>
        string Watch(params string[] keys);



        #endregion


#if !net40

        #region Transactions
        /// <summary>
        /// Mark the start of a transaction block
        /// </summary>
        /// <returns>Status code</returns>
        Task<string> MultiAsync();




        /// <summary>
        /// Discard all commands issued after MULTI
        /// </summary>
        /// <returns>Status code</returns>
        Task<string> DiscardAsync();




        /// <summary>
        /// Execute all commands issued after MULTI
        /// </summary>
        /// <returns>Array of output from all transaction commands</returns>
        Task<object[]> ExecAsync();




        /// <summary>
        /// Forget about all watched keys
        /// </summary>
        /// <returns>Status code</returns>
        Task<string> UnwatchAsync();




        /// <summary>
        /// Watch the given keys to determine execution of the MULTI/EXEC block
        /// </summary>
        /// <param name="keys">Keys to watch</param>
        /// <returns>Status code</returns>
        Task<string> WatchAsync(params string[] keys);



        #endregion 

#endif

    }
}
