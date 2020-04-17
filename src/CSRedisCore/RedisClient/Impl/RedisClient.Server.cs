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
        #region Server

        /// <summary>
        /// Asyncronously rewrite the append-only file
        /// </summary>
        /// <returns>Status code</returns>
        public virtual string BgRewriteAof()
        {
            return Write(RedisCommands.BgRewriteAof());
        }

        /// <summary>
        /// Asynchronously save the dataset to disk
        /// </summary>
        /// <returns>Status code</returns>
        public virtual string BgSave()
        {
            return Write(RedisCommands.BgSave());
        }

        /// <summary>
        /// Kill the connection of a client
        /// </summary>
        /// <param name="ip">Client IP returned from CLIENT LIST</param>
        /// <param name="port">Client port returned from CLIENT LIST</param>
        /// <returns>Status code</returns>
        public virtual string ClientKill(string ip, int port)
        {
            return Write(RedisCommands.ClientKill(ip, port));
        }

        /// <summary>
        /// Kill the connection of a client
        /// </summary>
        /// <param name="addr">client's ip:port</param>
        /// <param name="id">client's unique ID</param>
        /// <param name="type">client type (normal|slave|pubsub)</param>
        /// <param name="skipMe">do not kill the calling client</param>
        /// <returns>Nummber of clients killed</returns>
        public virtual long ClientKill(string addr = null, string id = null, string type = null, bool? skipMe = null)
        {
            return Write(RedisCommands.ClientKill(addr, id, type, skipMe));
        }

        /// <summary>
        /// Get the list of client connections
        /// </summary>
        /// <returns>Formatted string of clients</returns>
        public virtual string ClientList()
        {
            return Write(RedisCommands.ClientList());
        }

        /// <summary>
        /// Suspend all Redis clients for the specified amount of time
        /// </summary>
        /// <param name="milliseconds">Time to pause in milliseconds</param>
        /// <returns>Status code</returns>
        public virtual string ClientPause(int milliseconds)
        {
            return Write(RedisCommands.ClientPause(milliseconds));
        }

        /// <summary>
        /// Suspend all Redis clients for the specified amount of time
        /// </summary>
        /// <param name="timeout">Time to pause</param>
        /// <returns>Status code</returns>
        public virtual string ClientPause(TimeSpan timeout)
        {
            return Write(RedisCommands.ClientPause(timeout));
        }

        /// <summary>
        /// Get the current connection name
        /// </summary>
        /// <returns>Connection name</returns>
        public virtual string ClientGetName()
        {
            return Write(RedisCommands.ClientGetName());
        }

        /// <summary>
        /// Set the current connection name
        /// </summary>
        /// <param name="connectionName">Name of connection (no spaces)</param>
        /// <returns>Status code</returns>
        public virtual string ClientSetName(string connectionName)
        {
            return Write(RedisCommands.ClientSetName(connectionName));
        }

        /// <summary>
        /// Get the value of a configuration paramter
        /// </summary>
        /// <param name="parameter">Configuration parameter to lookup</param>
        /// <returns>Configuration value</returns>
        public virtual Tuple<string, string>[] ConfigGet(string parameter)
        {
            return Write(RedisCommands.ConfigGet(parameter));
        }

        /// <summary>
        /// Reset the stats returned by INFO
        /// </summary>
        /// <returns>Status code</returns>
        public virtual string ConfigResetStat()
        {
            return Write(RedisCommands.ConfigResetStat());
        }

        /// <summary>
        /// Rewrite the redis.conf file the server was started with, applying the minimal changes needed to make it reflect current configuration
        /// </summary>
        /// <returns>Status code</returns>
        public virtual string ConfigRewrite()
        {
            return Write(RedisCommands.ConfigRewrite());
        }

        /// <summary>
        /// Set a configuration parameter to the given value
        /// </summary>
        /// <param name="parameter">Parameter to set</param>
        /// <param name="value">Value to set</param>
        /// <returns>Status code</returns>
        public virtual string ConfigSet(string parameter, string value)
        {
            return Write(RedisCommands.ConfigSet(parameter, value));
        }

        /// <summary>
        /// Return the number of keys in the selected database
        /// </summary>
        /// <returns>Number of keys</returns>
        public virtual long DbSize()
        {
            return Write(RedisCommands.DbSize());
        }

        /// <summary>
        /// Make the server crash :(
        /// </summary>
        /// <returns>Status code</returns>
        public virtual string DebugSegFault()
        {
            return Write(RedisCommands.DebugSegFault());
        }

        /// <summary>
        /// Remove all keys from all databases
        /// </summary>
        /// <returns>Status code</returns>
        public virtual string FlushAll()
        {
            return Write(RedisCommands.FlushAll());
        }

        /// <summary>
        /// Remove all keys from the current database
        /// </summary>
        /// <returns>Status code</returns>
        public virtual string FlushDb()
        {
            return Write(RedisCommands.FlushDb());
        }

        /// <summary>
        /// Get information and statistics about the server
        /// </summary>
        /// <param name="section">all|default|server|clients|memory|persistence|stats|replication|cpu|commandstats|cluster|keyspace</param>
        /// <returns>Formatted string</returns>
        public virtual string Info(string section = null)
        {
            return Write(RedisCommands.Info(section));
        }

        /// <summary>
        /// Get the timestamp of the last successful save to disk
        /// </summary>
        /// <returns>Date of last save</returns>
        public virtual DateTime LastSave()
        {
            return Write(RedisCommands.LastSave());
        }

        /// <summary>
        /// Listen for all requests received by the server in real time
        /// </summary>
        /// <returns>Status code</returns>
        public virtual string Monitor()
        {
            return _monitor.Start();
        }

        /// <summary>
        /// Get role information for the current Redis instance
        /// </summary>
        /// <returns>RedisMasterRole|RedisSlaveRole|RedisSentinelRole</returns>
        public virtual RedisRole Role()
        {
            return Write(RedisCommands.Role());
        }

        /// <summary>
        /// Syncronously save the dataset to disk
        /// </summary>
        /// <returns>Status code</returns>
        public virtual string Save()
        {
            return Write(RedisCommands.Save());
        }

        /// <summary>
        /// Syncronously save the dataset to disk an then shut down the server
        /// </summary>
        /// <param name="save">Force a DB saving operation even if no save points are configured</param>
        /// <returns>Status code</returns>
        public virtual string Shutdown(bool? save = null)
        {
            return Write(RedisCommands.Shutdown(save));
        }

        /// <summary>
        /// Make the server a slave of another instance or promote it as master
        /// </summary>
        /// <param name="host">Master host</param>
        /// <param name="port">master port</param>
        /// <returns>Status code</returns>
        public virtual string SlaveOf(string host, int port)
        {
            return Write(RedisCommands.SlaveOf(host, port));
        }

        /// <summary>
        /// Turn off replication, turning the Redis server into a master
        /// </summary>
        /// <returns>Status code</returns>
        public virtual string SlaveOfNoOne()
        {
            return Write(RedisCommands.SlaveOfNoOne());
        }

        /// <summary>
        /// Get latest entries from the slow log
        /// </summary>
        /// <param name="count">Limit entries returned</param>
        /// <returns>Slow log entries</returns>
        public virtual RedisSlowLogEntry[] SlowLogGet(long? count = null)
        {
            return Write(RedisCommands.SlowLogGet(count));
        }

        /// <summary>
        /// Get the length of the slow log
        /// </summary>
        /// <returns>Slow log length</returns>
        public virtual long SlowLogLen()
        {
            return Write(RedisCommands.SlowLogLen());
        }

        /// <summary>
        /// Reset the slow log
        /// </summary>
        /// <returns>Status code</returns>
        public virtual string SlowLogReset()
        {
            return Write(RedisCommands.SlowLogReset());
        }

        /// <summary>
        /// Internal command used for replication
        /// </summary>
        /// <returns>Byte array of Redis sync data</returns>
        public virtual byte[] Sync()
        {
            return Write(RedisCommands.Sync());
        }

        /// <summary>
        /// Return the current server time
        /// </summary>
        /// <returns>Server time</returns>
        public virtual DateTime Time()
        {
            return Write(RedisCommands.Time());
        }

        #endregion


#if !net40

        #region Server

        /// <summary>
        /// Asyncronously rewrite the append-only file
        /// </summary>
        /// <returns>Status code</returns>
        public virtual async Task<string> BgRewriteAofAsync()
        {
            return await WriteAsync(RedisCommands.BgRewriteAof());
        }

        /// <summary>
        /// Asynchronously save the dataset to disk
        /// </summary>
        /// <returns>Status code</returns>
        public virtual async Task<string> BgSaveAsync()
        {
            return await WriteAsync(RedisCommands.BgSave());
        }

        /// <summary>
        /// Get the current connection name
        /// </summary>
        /// <returns>Connection name</returns>
        public virtual async Task<string> ClientGetNameAsync()
        {
            return await WriteAsync(RedisCommands.ClientGetName());
        }

        /// <summary>
        /// Kill the connection of a client
        /// </summary>
        /// <param name="ip">Client IP returned from CLIENT LIST</param>
        /// <param name="port">Client port returned from CLIENT LIST</param>
        /// <returns>Status code</returns>
        public virtual async Task<string> ClientKillAsync(string ip, int port)
        {
            return await WriteAsync(RedisCommands.ClientKill(ip, port));
        }

        /// <summary>
        /// Kill the connection of a client
        /// </summary>
        /// <param name="addr">Client address</param>
        /// <param name="id">Client ID</param>
        /// <param name="type">Client type</param>
        /// <param name="skipMe">Set to true to skip calling client</param>
        /// <returns>The number of clients killed</returns>
        public virtual async Task<long> ClientKillAsync(string addr = null, string id = null, string type = null, bool? skipMe = null)
        {
            return await WriteAsync(RedisCommands.ClientKill(addr, id, type, skipMe));
        }

        /// <summary>
        /// Get the list of client connections
        /// </summary>
        /// <returns>Formatted string of clients</returns>
        public virtual async Task<string> ClientListAsync()
        {
            return await WriteAsync(RedisCommands.ClientList());
        }

        /// <summary>
        /// Suspend all the Redis clients for the specified amount of time 
        /// </summary>
        /// <param name="milliseconds">Time in milliseconds to suspend</param>
        /// <returns>Status code</returns>
        public virtual async Task<string> ClientPauseAsync(int milliseconds)
        {
            return await WriteAsync(RedisCommands.ClientPause(milliseconds));
        }

        /// <summary>
        /// Suspend all the Redis clients for the specified amount of time 
        /// </summary>
        /// <param name="timeout">Time to suspend</param>
        /// <returns>Status code</returns>
        public virtual async Task<string> ClientPauseAsync(TimeSpan timeout)
        {
            return await WriteAsync(RedisCommands.ClientPause(timeout));
        }

        /// <summary>
        /// Set the current connection name
        /// </summary>
        /// <param name="connectionName">Name of connection (no spaces)</param>
        /// <returns>Status code</returns>
        public virtual async Task<string> ClientSetNameAsync(string connectionName)
        {
            return await WriteAsync(RedisCommands.ClientSetName(connectionName));
        }

        /// <summary>
        /// Get the value of a configuration paramter
        /// </summary>
        /// <param name="parameter">Configuration parameter to lookup</param>
        /// <returns>Configuration value</returns>
        public virtual async Task<Tuple<string, string>[]> ConfigGetAsync(string parameter)
        {
            return await WriteAsync(RedisCommands.ConfigGet(parameter));
        }

        /// <summary>
        /// Reset the stats returned by INFO
        /// </summary>
        /// <returns>Status code</returns>
        public virtual async Task<string> ConfigResetStatAsync()
        {
            return await WriteAsync(RedisCommands.ConfigResetStat());
        }

        /// <summary>
        /// Rewrites the redis.conf file
        /// </summary>
        /// <returns>Status code</returns>
        public virtual async Task<string> ConfigRewriteAsync()
        {
            return await WriteAsync(RedisCommands.ConfigRewrite());
        }

        /// <summary>
        /// Set a configuration parameter to the given value
        /// </summary>
        /// <param name="parameter">Parameter to set</param>
        /// <param name="value">Value to set</param>
        /// <returns>Status code</returns>
        public virtual async Task<string> ConfigSetAsync(string parameter, string value)
        {
            return await WriteAsync(RedisCommands.ConfigSet(parameter, value));
        }

        /// <summary>
        /// Return the number of keys in the selected database
        /// </summary>
        /// <returns>Number of keys</returns>
        public virtual async Task<long> DbSizeAsync()
        {
            return await WriteAsync(RedisCommands.DbSize());
        }

        /// <summary>
        /// Make the server crash :(
        /// </summary>
        /// <returns>Status code</returns>
        public virtual async Task<string> DebugSegFaultAsync()
        {
            return await WriteAsync(RedisCommands.DebugSegFault());
        }

        /// <summary>
        /// Remove all keys from all databases
        /// </summary>
        /// <returns>Status code</returns>
        public virtual async Task<string> FlushAllAsync()
        {
            return await WriteAsync(RedisCommands.FlushAll());
        }

        /// <summary>
        /// Remove all keys from the current database
        /// </summary>
        /// <returns>Status code</returns>
        public virtual async Task<string> FlushDbAsync()
        {
            return await WriteAsync(RedisCommands.FlushDb());
        }

        /// <summary>
        /// Get information and statistics about the server
        /// </summary>
        /// <param name="section">all|default|server|clients|memory|persistence|stats|replication|cpu|commandstats|cluster|keyspace</param>
        /// <returns>Formatted string</returns>
        public virtual async Task<string> InfoAsync(string section = null)
        {
            return await WriteAsync(RedisCommands.Info());
        }

        /// <summary>
        /// Get the timestamp of the last successful save to disk
        /// </summary>
        /// <returns>Date of last save</returns>
        public virtual async Task<DateTime> LastSaveAsync()
        {
            return await WriteAsync(RedisCommands.LastSave());
        }

        /// <summary>
        /// Provide information on the role of a Redis instance in the context of replication
        /// </summary>
        /// <returns>Role information</returns>
        public virtual async Task<RedisRole> RoleAsync()
        {
            return await WriteAsync(RedisCommands.Role());
        }

        /// <summary>
        /// Syncronously save the dataset to disk
        /// </summary>
        /// <returns>Status code</returns>
        public virtual async Task<string> SaveAsync()
        {
            return await WriteAsync(RedisCommands.Save());
        }

        /// <summary>
        /// Syncronously save the dataset to disk an then shut down the server
        /// </summary>
        /// <param name="save">Force a DB saving operation even if no save points are configured</param>
        /// <returns>Status code</returns>
        public virtual async Task<string> ShutdownAsync(bool? save = null)
        {
            return await WriteAsync(RedisCommands.Shutdown());
        }

        /// <summary>
        /// Make the server a slave of another instance or promote it as master
        /// </summary>
        /// <param name="host">Master host</param>
        /// <param name="port">master port</param>
        /// <returns>Status code</returns>
        public virtual async Task<string> SlaveOfAsync(string host, int port)
        {
            return await WriteAsync(RedisCommands.SlaveOf(host, port));
        }

        /// <summary>
        /// Turn off replication, turning the Redis server into a master
        /// </summary>
        /// <returns>Status code</returns>
        public virtual async Task<string> SlaveOfNoOneAsync()
        {
            return await WriteAsync(RedisCommands.SlaveOfNoOne());
        }

        /// <summary>
        /// Get latest entries from the slow log
        /// </summary>
        /// <param name="count">Limit entries returned</param>
        /// <returns>Slow log entries</returns>
        public virtual async Task<RedisSlowLogEntry[]> SlowLogGetAsync(long? count = null)
        {
            return await WriteAsync(RedisCommands.SlowLogGet(count));
        }

        /// <summary>
        /// Get the length of the slow log
        /// </summary>
        /// <returns>Slow log length</returns>
        public virtual async Task<long> SlowLogLenAsync()
        {
            return await WriteAsync(RedisCommands.SlowLogLen());
        }

        /// <summary>
        /// Reset the slow log
        /// </summary>
        /// <returns>Status code</returns>
        public virtual async Task<string> SlowLogResetAsync()
        {
            return await WriteAsync(RedisCommands.SlowLogReset());
        }

        /// <summary>
        /// Internal command used for replication
        /// </summary>
        /// <returns>Byte array of Redis sync data</returns>
        public virtual async Task<byte[]> SyncAsync()
        {
            return await WriteAsync(RedisCommands.Sync());
        }

        /// <summary>
        /// Return the current server time
        /// </summary>
        /// <returns>Server time</returns>
        public virtual async Task<DateTime> TimeAsync()
        {
            return await WriteAsync(RedisCommands.Time());
        }

        #endregion

#endif

    }
}
