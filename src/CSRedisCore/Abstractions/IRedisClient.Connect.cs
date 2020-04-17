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
    public interface IRedisClientUseConnectCommands
    {
        /// <summary>
        /// Call arbitrary Redis command
        /// </summary>
        /// <param name="command">Command name</param>
        /// <param name="args">Command arguments</param>
        /// <returns>Redis object</returns>
        object Call(string command, params string[] args);

        #region Connection

        /// <summary>
        /// Connect to the remote host
        /// </summary>
        /// <param name="timeout">Connection timeout in milliseconds</param>
        /// <returns>True if connected</returns>
        bool Connect(int timeout);


        /// <summary>
        /// Authenticate to the server
        /// </summary>
        /// <param name="password">Redis server password</param>
        /// <returns>Status message</returns>
        string Auth(string password);

        /// <summary>
        /// Echo the given string
        /// </summary>
        /// <param name="message">Message to echo</param>
        /// <returns>Message</returns>
        string Echo(string message);

        /// <summary>
        /// Ping the server
        /// </summary>
        /// <returns>Status message</returns>
        string Ping();

        /// <summary>
        /// Close the connection
        /// </summary>
        /// <returns>Status message</returns>
        string Quit();

        /// <summary>
        /// Change the selected database for the current connection
        /// </summary>
        /// <param name="index">Zero-based database index</param>
        /// <returns>Status message</returns>
        string Select(int index);
        #endregion

#if !net40

        /// <summary>
        /// Call arbitrary redis command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        Task<object> CallAsync(string command, params string[] args);


        #region Connection
        /// <summary>
        /// Open connection to redis server
        /// </summary>
        /// <returns>True on success</returns>
        Task<bool> ConnectAsync();


        /// <summary>
        /// Authenticate to the server
        /// </summary>
        /// <param name="password">Server password</param>
        /// <returns>Task associated with status message</returns>
        Task<string> AuthAsync(string password);




        /// <summary>
        /// Echo the given string
        /// </summary>
        /// <param name="message">Message to echo</param>
        /// <returns>Task associated with echo response</returns>
        Task<string> EchoAsync(string message);




        /// <summary>
        /// Ping the server
        /// </summary>
        /// <returns>Task associated with status message</returns>
        Task<string> PingAsync();




        /// <summary>
        /// Close the connection
        /// </summary>
        /// <returns>Task associated with status message</returns>
        Task<string> QuitAsync();


        /// <summary>
        /// Change the selected database for the current connection
        /// </summary>
        /// <param name="index">Zero-based database index</param>
        /// <returns>Status message</returns>
        Task<string> SelectAsync(int index);



        #endregion

#endif

    }
}
