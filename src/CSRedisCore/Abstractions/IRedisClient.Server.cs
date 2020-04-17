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
    public interface IRedisClientUseServerCommands
    {

        #region Server
        /// <summary>
        /// Asyncronously rewrite the append-only file
        /// </summary>
        /// <returns>Status code</returns>
        string BgRewriteAof();




        /// <summary>
        /// Asynchronously save the dataset to disk
        /// </summary>
        /// <returns>Status code</returns>
        string BgSave();




        /// <summary>
        /// Kill the connection of a client
        /// </summary>
        /// <param name="ip">Client IP returned from CLIENT LIST</param>
        /// <param name="port">Client port returned from CLIENT LIST</param>
        /// <returns>Status code</returns>
        string ClientKill(string ip, int port);




        /// <summary>
        /// Kill the connection of a client
        /// </summary>
        /// <param name="addr">client's ip:port</param>
        /// <param name="id">client's unique ID</param>
        /// <param name="type">client type (normal|slave|pubsub);</param>
        /// <param name="skipMe">do not kill the calling client</param>
        /// <returns>Nummber of clients killed</returns>
        long ClientKill(string addr = null, string id = null, string type = null, bool? skipMe = null);




        /// <summary>
        /// Get the list of client connections
        /// </summary>
        /// <returns>Formatted string of clients</returns>
        string ClientList();




        /// <summary>
        /// Suspend all Redis clients for the specified amount of time
        /// </summary>
        /// <param name="milliseconds">Time to pause in milliseconds</param>
        /// <returns>Status code</returns>
        string ClientPause(int milliseconds);




        /// <summary>
        /// Suspend all Redis clients for the specified amount of time
        /// </summary>
        /// <param name="timeout">Time to pause</param>
        /// <returns>Status code</returns>
        string ClientPause(TimeSpan timeout);




        /// <summary>
        /// Get the current connection name
        /// </summary>
        /// <returns>Connection name</returns>
        string ClientGetName();




        /// <summary>
        /// Set the current connection name
        /// </summary>
        /// <param name="connectionName">Name of connection (no spaces);</param>
        /// <returns>Status code</returns>
        string ClientSetName(string connectionName);




        /// <summary>
        /// Get the value of a configuration paramter
        /// </summary>
        /// <param name="parameter">Configuration parameter to lookup</param>
        /// <returns>Configuration value</returns>
        Tuple<string, string>[] ConfigGet(string parameter);




        /// <summary>
        /// Reset the stats returned by INFO
        /// </summary>
        /// <returns>Status code</returns>
        string ConfigResetStat();




        /// <summary>
        /// Rewrite the redis.conf file the server was started with, applying the minimal changes needed to make it reflect current configuration
        /// </summary>
        /// <returns>Status code</returns>
        string ConfigRewrite();




        /// <summary>
        /// Set a configuration parameter to the given value
        /// </summary>
        /// <param name="parameter">Parameter to set</param>
        /// <param name="value">Value to set</param>
        /// <returns>Status code</returns>
        string ConfigSet(string parameter, string value);




        /// <summary>
        ///
        /// </summary>
        /// <returns>Number of keys</returns>
        long DbSize();




        /// <summary>
        /// Make the server crash :(
        /// </summary>
        /// <returns>Status code</returns>
        string DebugSegFault();




        /// <summary>
        /// Remove all keys from all databases
        /// </summary>
        /// <returns>Status code</returns>
        string FlushAll();




        /// <summary>
        /// Remove all keys from the current database
        /// </summary>
        /// <returns>Status code</returns>
        string FlushDb();




        /// <summary>
        /// Get information and statistics about the server
        /// </summary>
        /// <param name="section">all|default|server|clients|memory|persistence|stats|replication|cpu|commandstats|cluster|keyspace</param>
        /// <returns>Formatted string</returns>
        string Info(string section = null);




        /// <summary>
        /// Get the timestamp of the last successful save to disk
        /// </summary>
        /// <returns>Date of last save</returns>
        DateTime LastSave();




        /// <summary>
        /// Listen for all requests received by the server in real time
        /// </summary>
        /// <returns>Status code</returns>
        string Monitor();




        /// <summary>
        /// Get role information for the current Redis instance
        /// </summary>
        /// <returns>RedisMasterRole|RedisSlaveRole|RedisSentinelRole</returns>
        RedisRole Role();




        /// <summary>
        /// Syncronously save the dataset to disk
        /// </summary>
        /// <returns>Status code</returns>
        string Save();




        /// <summary>
        /// Syncronously save the dataset to disk an then shut down the server
        /// </summary>
        /// <param name="save">Force a DB saving operation even if no save points are configured</param>
        /// <returns>Status code</returns>
        string Shutdown(bool? save = null);




        /// <summary>
        /// Make the server a slave of another instance or promote it as master
        /// </summary>
        /// <param name="host">Master host</param>
        /// <param name="port">master port</param>
        /// <returns>Status code</returns>
        string SlaveOf(string host, int port);




        /// <summary>
        /// Turn off replication, turning the Redis server into a master
        /// </summary>
        /// <returns>Status code</returns>
        string SlaveOfNoOne();




        /// <summary>
        /// Get latest entries from the slow log
        /// </summary>
        /// <param name="count">Limit entries returned</param>
        /// <returns>Slow log entries</returns>
        RedisSlowLogEntry[] SlowLogGet(long? count = null);




        /// <summary>
        /// Get the length of the slow log
        /// </summary>
        /// <returns>Slow log length</returns>
        long SlowLogLen();




        /// <summary>
        /// Reset the slow log
        /// </summary>
        /// <returns>Status code</returns>
        string SlowLogReset();




        /// <summary>
        /// Internal command used for replication
        /// </summary>
        /// <returns>Byte array of Redis sync data</returns>
        byte[] Sync();




        /// <summary>
        ///
        /// </summary>
        /// <returns>Server time</returns>
        DateTime Time();



        #endregion

#if !net40

        #region Server
        /// <summary>
        /// Asyncronously rewrite the append-only file
        /// </summary>
        /// <returns>Status code</returns>
        Task<string> BgRewriteAofAsync();




        /// <summary>
        /// Asynchronously save the dataset to disk
        /// </summary>
        /// <returns>Status code</returns>
        Task<string> BgSaveAsync();




        /// <summary>
        /// Get the current connection name
        /// </summary>
        /// <returns>Connection name</returns>
        Task<string> ClientGetNameAsync();




        /// <summary>
        /// Kill the connection of a client
        /// </summary>
        /// <param name="ip">Client IP returned from CLIENT LIST</param>
        /// <param name="port">Client port returned from CLIENT LIST</param>
        /// <returns>Status code</returns>
        Task<string> ClientKillAsync(string ip, int port);




        /// <summary>
        /// Kill the connection of a client
        /// </summary>
        /// <param name="addr">Client address</param>
        /// <param name="id">Client ID</param>
        /// <param name="type">Client type</param>
        /// <param name="skipMe">Set to true to skip calling client</param>
        /// <returns>The number of clients killed</returns>
        Task<long> ClientKillAsync(string addr = null, string id = null, string type = null, bool? skipMe = null);




        /// <summary>
        /// Get the list of client connections
        /// </summary>
        /// <returns>Formatted string of clients</returns>
        Task<string> ClientListAsync();




        /// <summary>
        /// Suspend all the Redis clients for the specified amount of time 
        /// </summary>
        /// <param name="milliseconds">Time in milliseconds to suspend</param>
        /// <returns>Status code</returns>
        Task<string> ClientPauseAsync(int milliseconds);




        /// <summary>
        /// Suspend all the Redis clients for the specified amount of time 
        /// </summary>
        /// <param name="timeout">Time to suspend</param>
        /// <returns>Status code</returns>
        Task<string> ClientPauseAsync(TimeSpan timeout);




        /// <summary>
        /// Set the current connection name
        /// </summary>
        /// <param name="connectionName">Name of connection (no spaces)</param>
        /// <returns>Status code</returns>
        Task<string> ClientSetNameAsync(string connectionName);




        /// <summary>
        /// Get the value of a configuration paramter
        /// </summary>
        /// <param name="parameter">Configuration parameter to lookup</param>
        /// <returns>Configuration value</returns>
        Task<Tuple<string, string>[]> ConfigGetAsync(string parameter);




        /// <summary>
        /// Reset the stats returned by INFO
        /// </summary>
        /// <returns>Status code</returns>
        Task<string> ConfigResetStatAsync();




        /// <summary>
        /// Rewrites the redis.conf file
        /// </summary>
        /// <returns>Status code</returns>
        Task<string> ConfigRewriteAsync();




        /// <summary>
        /// Set a configuration parameter to the given value
        /// </summary>
        /// <param name="parameter">Parameter to set</param>
        /// <param name="value">Value to set</param>
        /// <returns>Status code</returns>
        Task<string> ConfigSetAsync(string parameter, string value);




        /// <summary>
        /// Return the number of keys in the selected database
        /// </summary>
        /// <returns>Number of keys</returns>
        Task<long> DbSizeAsync();




        /// <summary>
        /// Make the server crash :(
        /// </summary>
        /// <returns>Status code</returns>
        Task<string> DebugSegFaultAsync();




        /// <summary>
        /// Remove all keys from all databases
        /// </summary>
        /// <returns>Status code</returns>
        Task<string> FlushAllAsync();




        /// <summary>
        /// Remove all keys from the current database
        /// </summary>
        /// <returns>Status code</returns>
        Task<string> FlushDbAsync();




        /// <summary>
        /// Get information and statistics about the server
        /// </summary>
        /// <param name="section">all|default|server|clients|memory|persistence|stats|replication|cpu|commandstats|cluster|keyspace</param>
        /// <returns>Formatted string</returns>
        Task<string> InfoAsync(string section = null);




        /// <summary>
        /// Get the timestamp of the last successful save to disk
        /// </summary>
        /// <returns>Date of last save</returns>
        Task<DateTime> LastSaveAsync();




        /// <summary>
        /// Provide information on the role of a Redis instance in the context of replication
        /// </summary>
        /// <returns>Role information</returns>
        Task<RedisRole> RoleAsync();




        /// <summary>
        /// Syncronously save the dataset to disk
        /// </summary>
        /// <returns>Status code</returns>
        Task<string> SaveAsync();




        /// <summary>
        /// Syncronously save the dataset to disk an then shut down the server
        /// </summary>
        /// <param name="save">Force a DB saving operation even if no save points are configured</param>
        /// <returns>Status code</returns>
        Task<string> ShutdownAsync(bool? save = null);




        /// <summary>
        /// Make the server a slave of another instance or promote it as master
        /// </summary>
        /// <param name="host">Master host</param>
        /// <param name="port">master port</param>
        /// <returns>Status code</returns>
        Task<string> SlaveOfAsync(string host, int port);




        /// <summary>
        /// Turn off replication, turning the Redis server into a master
        /// </summary>
        /// <returns>Status code</returns>
        Task<string> SlaveOfNoOneAsync();




        /// <summary>
        /// Get latest entries from the slow log
        /// </summary>
        /// <param name="count">Limit entries returned</param>
        /// <returns>Slow log entries</returns>
        Task<RedisSlowLogEntry[]> SlowLogGetAsync(long? count = null);




        /// <summary>
        /// Get the length of the slow log
        /// </summary>
        /// <returns>Slow log length</returns>
        Task<long> SlowLogLenAsync();




        /// <summary>
        /// Reset the slow log
        /// </summary>
        /// <returns>Status code</returns>
        Task<string> SlowLogResetAsync();




        /// <summary>
        /// Internal command used for replication
        /// </summary>
        /// <returns>Byte array of Redis sync data</returns>
        Task<byte[]> SyncAsync();




        /// <summary>
        /// Return the current server time
        /// </summary>
        /// <returns>Server time</returns>
        Task<DateTime> TimeAsync();



        #endregion

#endif

    }
}
