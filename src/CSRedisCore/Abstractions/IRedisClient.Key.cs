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
    public interface IRedisClientUseKeyCommands
    {
        #region Keys
        /// <summary>
        /// Delete a key
        /// </summary>
        /// <param name="keys">Keys to delete</param>
        /// <returns>Number of keys removed</returns>
        long Del(params string[] keys);


        /// <summary>
        /// Return a serialized version of the value stored at the specified key
        /// </summary>
        /// <param name="key">Key to dump</param>
        /// <returns>Serialized value</returns>
        byte[] Dump(string key);


        /// <summary>
        /// Determine if a key exists
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if key exists</returns>
        bool Exists(string key);


        /// <summary>
        /// Set a key's time to live in seconds
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="expiration">Expiration (nearest second);</param>
        /// <returns>True if timeout was set; false if key does not exist or timeout could not be set</returns>
        bool Expire(string key, TimeSpan expiration);


        /// <summary>
        /// Set a key's time to live in seconds
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="seconds">Expiration in seconds</param>
        /// <returns>True if timeout was set; false if key does not exist or timeout could not be set</returns>
        bool Expire(string key, int seconds);


        /// <summary>
        /// Set the expiration for a key (nearest second);
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="expirationDate">Date of expiration, to nearest second</param>
        /// <returns>True if timeout was set; false if key does not exist or timeout could not be set</returns>
        bool ExpireAt(string key, DateTime expirationDate);


        /// <summary>
        /// Set the expiration for a key as a UNIX timestamp
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="timestamp">UNIX timestamp</param>
        /// <returns>True if timeout was set; false if key does not exist or timeout could not be set</returns>
        bool ExpireAt(string key, int timestamp);


        /// <summary>
        /// Find all keys matching the given pattern
        /// </summary>
        /// <param name="pattern">Pattern to match</param>
        /// <returns>Array of keys matching pattern</returns>
        string[] Keys(string pattern);


        /// <summary>
        /// Atomically transfer a key from a Redis instance to another one
        /// </summary>
        /// <param name="host">Remote Redis host</param>
        /// <param name="port">Remote Redis port</param>
        /// <param name="key">Key to migrate</param>
        /// <param name="destinationDb">Remote database ID</param>
        /// <param name="timeoutMilliseconds">Timeout in milliseconds</param>
        /// <returns>Status message</returns>
        string Migrate(string host, int port, string key, int destinationDb, int timeoutMilliseconds);


        /// <summary>
        /// Atomically transfer a key from a Redis instance to another one
        /// </summary>
        /// <param name="host">Remote Redis host</param>
        /// <param name="port">Remote Redis port</param>
        /// <param name="key">Key to migrate</param>
        /// <param name="destinationDb">Remote database ID</param>
        /// <param name="timeout">Timeout in milliseconds</param>
        /// <returns>Status message</returns>
        string Migrate(string host, int port, string key, int destinationDb, TimeSpan timeout);


        /// <summary>
        /// Move a key to another database
        /// </summary>
        /// <param name="key">Key to move</param>
        /// <param name="database">Database destination ID</param>
        /// <returns>True if key was moved</returns>
        bool Move(string key, int database);


        /// <summary>
        /// Get the number of references of the value associated with the specified key
        /// </summary>
        /// <param name="arguments">Subcommand arguments</param>
        /// <returns>The type of internal representation used to store the value at the specified key</returns>
        string ObjectEncoding(params string[] arguments);


        /// <summary>
        /// Inspect the internals of Redis objects
        /// </summary>
        /// <param name="subCommand">Type of Object command to send</param>
        /// <param name="arguments">Subcommand arguments</param>
        /// <returns>Varies depending on subCommand</returns>
        long? Object(RedisObjectSubCommand subCommand, params string[] arguments);


        /// <summary>
        /// Remove the expiration from a key
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <returns>True if timeout was removed</returns>
        bool Persist(string key);


        /// <summary>
        /// Set a key's time to live in milliseconds
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="expiration">Expiration (nearest millisecond);</param>
        /// <returns>True if timeout was set</returns>
        bool PExpire(string key, TimeSpan expiration);


        /// <summary>
        /// Set a key's time to live in milliseconds
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="milliseconds">Expiration in milliseconds</param>
        /// <returns>True if timeout was set</returns>
        bool PExpire(string key, long milliseconds);


        /// <summary>
        /// Set the expiration for a key (nearest millisecond);
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="date">Expiration date</param>
        /// <returns>True if timeout was set</returns>
        bool PExpireAt(string key, DateTime date);


        /// <summary>
        /// Set the expiration for a key as a UNIX timestamp specified in milliseconds
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="timestamp">Expiration timestamp (milliseconds);</param>
        /// <returns>True if timeout was set</returns>
        bool PExpireAt(string key, long timestamp);

        /// <summary>
        /// Get the time to live for a key in milliseconds
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>Time-to-live in milliseconds</returns>
        long PTtl(string key);


        /// <summary>
        /// Return a random key from the keyspace
        /// </summary>
        /// <returns>A random key</returns>
        string RandomKey();


        /// <summary>
        /// Rename a key
        /// </summary>
        /// <param name="key">Key to rename</param>
        /// <param name="newKey">New key name</param>
        /// <returns>Status code</returns>
        string Rename(string key, string newKey);


        /// <summary>
        /// Rename a key, only if the new key does not exist
        /// </summary>
        /// <param name="key">Key to rename</param>
        /// <param name="newKey">New key name</param>
        /// <returns>True if key was renamed</returns>
        bool RenameNx(string key, string newKey);


        /// <summary>
        /// Create a key using the provided serialized value, previously obtained using dump
        /// </summary>
        /// <param name="key">Key to restore</param>
        /// <param name="ttlMilliseconds">Time-to-live in milliseconds</param>
        /// <param name="serializedValue">Serialized value from DUMP</param>
        /// <returns>Status code</returns>
        string Restore(string key, long ttlMilliseconds, byte[] serializedValue);

        /// <summary>
        /// Sort the elements in a list, set or sorted set
        /// </summary>
        /// <param name="key">Key to sort</param>
        /// <param name="offset">Number of elements to skip</param>
        /// <param name="count">Number of elements to return</param>
        /// <param name="by">Sort by external key</param>
        /// <param name="dir">Sort direction</param>
        /// <param name="isAlpha">Sort lexicographically</param>
        /// <param name="get">Retrieve external keys</param>
        /// <returns>The sorted list</returns>
        string[] Sort(string key, long? offset = null, long? count = null, string by = null, RedisSortDir? dir = null, bool? isAlpha = null, params string[] get);


        /// <summary>
        /// Sort the elements in a list, set or sorted set, then store the result in a new list
        /// </summary>
        /// <param name="key">Key to sort</param>
        /// <param name="destination">Destination key name of stored sort</param>
        /// <param name="offset">Number of elements to skip</param>
        /// <param name="count">Number of elements to return</param>
        /// <param name="by">Sort by external key</param>
        /// <param name="dir">Sort direction</param>
        /// <param name="isAlpha">Sort lexicographically</param>
        /// <param name="get">Retrieve external keys</param>
        /// <returns>Number of elements stored</returns>
        long SortAndStore(string key, string destination, long? offset = null, long? count = null, string by = null, RedisSortDir? dir = null, bool? isAlpha = false, params string[] get);


        /// <summary>
        /// Get the time to live for a key
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>Time-to-live in seconds</returns>
        long Ttl(string key);


        /// <summary>
        /// Determine the type stored at key
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>Type of key</returns>
        string Type(string key);


        /// <summary>
        /// Iterate the set of keys in the currently selected Redis database
        /// </summary>
        /// <param name="cursor">The cursor returned by the server in the previous call, or 0 if this is the first call</param>
        /// <param name="pattern">Glob-style pattern to filter returned elements</param>
        /// <param name="count">Set the maximum number of elements to return</param>
        /// <returns>Updated cursor and result set</returns>
        RedisScan<string> Scan(long cursor, string pattern = null, long? count = null);
        RedisScan<byte[]> ScanBytes(long cursor, string pattern = null, long? count = null);

        #endregion

#if !net40

        #region Keys
        /// <summary>
        /// Delete a key
        /// </summary>
        /// <param name="keys">Keys to delete</param>
        /// <returns></returns>
        Task<long> DelAsync(params string[] keys);




        /// <summary>
        /// Return a serialized version of the value stored at the specified key
        /// </summary>
        /// <param name="key">Key to dump</param>
        /// <returns></returns>
        Task<byte[]> DumpAsync(string key);




        /// <summary>
        /// Determine if a key exists
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string key);




        /// <summary>
        /// Set a key's time to live in seconds
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="expiration">Expiration (nearest second)</param>
        /// <returns></returns>
        Task<bool> ExpireAsync(string key, int expiration);




        /// <summary>
        /// Set a key's time to live in seconds
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="expiration">Expiration in seconds</param>
        /// <returns></returns>
        Task<bool> ExpireAsync(string key, TimeSpan expiration);




        /// <summary>
        /// Set the expiration for a key (nearest second);
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="expirationDate">Date of expiration, to nearest second</param>
        /// <returns></returns>
        Task<bool> ExpireAtAsync(string key, DateTime expirationDate);




        /// <summary>
        /// Set the expiration for a key as a UNIX timestamp
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        Task<bool> ExpireAtAsync(string key, int timestamp);




        /// <summary>
        /// Find all keys matching the given pattern
        /// </summary>
        /// <param name="pattern">Pattern to match</param>
        /// <returns></returns>
        Task<string[]> KeysAsync(string pattern);




        /// <summary>
        /// Atomically transfer a key from a Redis instance to another one
        /// </summary>
        /// <param name="host">Remote Redis host</param>
        /// <param name="port">Remote Redis port</param>
        /// <param name="key">Key to migrate</param>
        /// <param name="destinationDb">Remote database ID</param>
        /// <param name="timeout">Timeout in milliseconds</param>
        /// <returns></returns>
        Task<string> MigrateAsync(string host, int port, string key, int destinationDb, int timeout);




        /// <summary>
        /// Atomically transfer a key from a Redis instance to another one
        /// </summary>
        /// <param name="host">Remote Redis host</param>
        /// <param name="port">Remote Redis port</param>
        /// <param name="key">Key to migrate</param>
        /// <param name="destinationDb">Remote database ID</param>
        /// <param name="timeout">Timeout in milliseconds</param>
        /// <returns></returns>
        Task<string> MigrateAsync(string host, int port, string key, int destinationDb, TimeSpan timeout);




        /// <summary>
        /// Move a key to another database
        /// </summary>
        /// <param name="key">Key to move</param>
        /// <param name="database">Database destination ID</param>
        /// <returns></returns>
        Task<bool> MoveAsync(string key, int database);




        /// <summary>
        /// Get the number of references of the value associated with the specified key
        /// </summary>
        /// <param name="arguments">Subcommand arguments</param>
        /// <returns>The type of internal representation used to store the value at the specified key</returns>
        Task<string> ObjectEncodingAsync(params string[] arguments);




        /// <summary>
        /// Inspect the internals of Redis objects
        /// </summary>
        /// <param name="subCommand">Type of Object command to send</param>
        /// <param name="arguments">Subcommand arguments</param>
        /// <returns>Varies depending on subCommand</returns>
        Task<long?> ObjectAsync(RedisObjectSubCommand subCommand, params string[] arguments);




        /// <summary>
        /// Remove the expiration from a key
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <returns></returns>
        Task<bool> PersistAsync(string key);




        /// <summary>
        /// Set a key's time to live in milliseconds
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="expiration">Expiration (nearest millisecond)</param>
        /// <returns></returns>
        Task<bool> PExpireAsync(string key, TimeSpan expiration);




        /// <summary>
        /// Set a key's time to live in milliseconds
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="milliseconds">Expiration in milliseconds</param>
        /// <returns></returns>
        Task<bool> PExpireAsync(string key, long milliseconds);




        /// <summary>
        /// Set the expiration for a key (nearest millisecond);
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="date">Expiration date</param>
        /// <returns></returns>
        Task<bool> PExpireAtAsync(string key, DateTime date);




        /// <summary>
        /// Set the expiration for a key as a UNIX timestamp specified in milliseconds
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="timestamp">Expiration timestamp (milliseconds)</param>
        /// <returns></returns>
        Task<bool> PExpireAtAsync(string key, long timestamp);




        /// <summary>
        /// Get the time to live for a key in milliseconds
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns></returns>
        Task<long> PTtlAsync(string key);




        /// <summary>
        /// Return a random key from the keyspace
        /// </summary>
        /// <returns></returns>
        Task<string> RandomKeyAsync();




        /// <summary>
        /// Rename a key
        /// </summary>
        /// <param name="key">Key to rename</param>
        /// <param name="newKey">New key name</param>
        /// <returns></returns>
        Task<string> RenameAsync(string key, string newKey);




        /// <summary>
        /// Rename a key, only if the new key does not exist
        /// </summary>
        /// <param name="key">Key to rename</param>
        /// <param name="newKey">New key name</param>
        /// <returns></returns>
        Task<bool> RenameNxAsync(string key, string newKey);




        /// <summary>
        /// Create a key using the provided serialized value, previously obtained using dump
        /// </summary>
        /// <param name="key">Key to restore</param>
        /// <param name="ttlMilliseconds">Time-to-live in milliseconds</param>
        /// <param name="serializedValue">Serialized value from DUMP</param>
        /// <returns></returns>
        Task<string> RestoreAsync(string key, long ttlMilliseconds, byte[] serializedValue);




        /// <summary>
        /// Sort the elements in a list, set or sorted set
        /// </summary>
        /// <param name="key">Key to sort</param>
        /// <param name="offset">Number of elements to skip</param>
        /// <param name="count">Number of elements to return</param>
        /// <param name="by">Sort by external key</param>
        /// <param name="dir">Sort direction</param>
        /// <param name="isAlpha">Sort lexicographically</param>
        /// <param name="get">Retrieve external keys</param>
        /// <returns></returns>
        Task<string[]> SortAsync(string key, long? offset = null, long? count = null, string by = null, RedisSortDir? dir = null, bool? isAlpha = null, params string[] get);




        /// <summary>
        /// Sort the elements in a list, set or sorted set, then store the result in a new list
        /// </summary>
        /// <param name="key">Key to sort</param>
        /// <param name="destination">Destination key name of stored sort</param>
        /// <param name="offset">Number of elements to skip</param>
        /// <param name="count">Number of elements to return</param>
        /// <param name="by">Sort by external key</param>
        /// <param name="dir">Sort direction</param>
        /// <param name="isAlpha">Sort lexicographically</param>
        /// <param name="get">Retrieve external keys</param>
        /// <returns></returns>
        Task<long> SortAndStoreAsync(string key, string destination, long? offset = null, long? count = null, string by = null, RedisSortDir? dir = null, bool? isAlpha = null, params string[] get);




        /// <summary>
        /// Get the time to live for a key
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns></returns>
        Task<long> TtlAsync(string key);




        /// <summary>
        /// Determine the type stored at key
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns></returns>
        Task<string> TypeAsync(string key);


        /// <summary>
        /// Iterate the set of keys in the currently selected Redis database
        /// </summary>
        /// <param name="cursor">The cursor returned by the server in the previous call, or 0 if this is the first call</param>
        /// <param name="pattern">Glob-style pattern to filter returned elements</param>
        /// <param name="count">Set the maximum number of elements to return</param>
        /// <returns>Updated cursor and result set</returns>
        Task<RedisScan<string>> ScanAsync(long cursor, string pattern = null, long? count = null);
        Task<RedisScan<byte[]>> ScanBytesAsync(long cursor, string pattern = null, long? count = null);

        #endregion

#endif

    }
}
