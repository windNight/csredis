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
        /// [redis-server 3.2.1] 修改指定key(s) 最后访问时间 若key不存在，不做操作
        /// </summary>
        /// <param name="keys">Keys</param>
        /// <returns></returns>
        public virtual long Touch(params string[] keys)
        {
            return Write(RedisCommands.Touch(keys));
        }

        /// <summary>
        /// [redis-server 4.0.0] Delete a key, 该命令和DEL十分相似：删除指定的key(s),若key不存在则该key被跳过。但是，相比DEL会产生阻塞，该命令会在另一个线程中回收内存，因此它是非阻塞的。 这也是该命令名字的由来：仅将keys从keyspace元数据中删除，真正的删除会在后续异步操作。
        /// </summary>
        /// <param name="keys">Keys to delete</param>
        /// <returns>Number of keys removed</returns>
        public virtual long UnLink(params string[] keys)
        {
            return Write(RedisCommands.UnLink(keys));
        }

        /// <summary>
        /// Delete a key
        /// </summary>
        /// <param name="keys">Keys to delete</param>
        /// <returns>Number of keys removed</returns>
        public virtual long Del(params string[] keys)
        {
            return Write(RedisCommands.Del(keys));
        }

        /// <summary>
        /// Return a serialized version of the value stored at the specified key
        /// </summary>
        /// <param name="key">Key to dump</param>
        /// <returns>Serialized value</returns>
        public virtual byte[] Dump(string key)
        {
            return Write(RedisCommands.Dump(key));
        }

        /// <summary>
        /// Determine if a key exists
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if key exists</returns>
        public virtual bool Exists(string key)
        {
            return Write(RedisCommands.Exists(key));
        }
        public virtual long Exists(string[] keys)
        {
            return Write(RedisCommands.Exists(keys));
        }

        /// <summary>
        /// Set a key's time to live in seconds
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="expiration">Expiration (nearest second)</param>
        /// <returns>True if timeout was set; false if key does not exist or timeout could not be set</returns>
        public virtual bool Expire(string key, TimeSpan expiration)
        {
            return Write(RedisCommands.Expire(key, expiration));
        }

        /// <summary>
        /// Set a key's time to live in seconds
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="seconds">Expiration in seconds</param>
        /// <returns>True if timeout was set; false if key does not exist or timeout could not be set</returns>
        public virtual bool Expire(string key, int seconds)
        {
            return Write(RedisCommands.Expire(key, seconds));
        }

        /// <summary>
        /// Set the expiration for a key (nearest second)
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="expirationDate">Date of expiration, to nearest second</param>
        /// <returns>True if timeout was set; false if key does not exist or timeout could not be set</returns>
        public virtual bool ExpireAt(string key, DateTime expirationDate)
        {
            return Write(RedisCommands.ExpireAt(key, expirationDate));
        }

        /// <summary>
        /// Set the expiration for a key as a UNIX timestamp
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="timestamp">UNIX timestamp</param>
        /// <returns>True if timeout was set; false if key does not exist or timeout could not be set</returns>
        public virtual bool ExpireAt(string key, int timestamp)
        {
            return Write(RedisCommands.ExpireAt(key, timestamp));
        }

        /// <summary>
        /// Find all keys matching the given pattern
        /// </summary>
        /// <param name="pattern">Pattern to match</param>
        /// <returns>Array of keys matching pattern</returns>
        public virtual string[] Keys(string pattern)
        {
            return Write(RedisCommands.Keys(pattern));
        }

        /// <summary>
        /// Atomically transfer a key from a Redis instance to another one
        /// </summary>
        /// <param name="host">Remote Redis host</param>
        /// <param name="port">Remote Redis port</param>
        /// <param name="key">Key to migrate</param>
        /// <param name="destinationDb">Remote database ID</param>
        /// <param name="timeoutMilliseconds">Timeout in milliseconds</param>
        /// <returns>Status message</returns>
        public virtual string Migrate(string host, int port, string key, int destinationDb, int timeoutMilliseconds)
        {
            return Write(RedisCommands.Migrate(host, port, key, destinationDb, timeoutMilliseconds));
        }

        /// <summary>
        /// Atomically transfer a key from a Redis instance to another one
        /// </summary>
        /// <param name="host">Remote Redis host</param>
        /// <param name="port">Remote Redis port</param>
        /// <param name="key">Key to migrate</param>
        /// <param name="destinationDb">Remote database ID</param>
        /// <param name="timeout">Timeout in milliseconds</param>
        /// <returns>Status message</returns>
        public virtual string Migrate(string host, int port, string key, int destinationDb, TimeSpan timeout)
        {
            return Write(RedisCommands.Migrate(host, port, key, destinationDb, timeout));
        }

        /// <summary>
        /// Move a key to another database
        /// </summary>
        /// <param name="key">Key to move</param>
        /// <param name="database">Database destination ID</param>
        /// <returns>True if key was moved</returns>
        public virtual bool Move(string key, int database)
        {
            return Write(RedisCommands.Move(key, database));
        }

        /// <summary>
        /// Get the number of references of the value associated with the specified key
        /// </summary>
        /// <param name="arguments">Subcommand arguments</param>
        /// <returns>The type of internal representation used to store the value at the specified key</returns>
        public virtual string ObjectEncoding(params string[] arguments)
        {
            return Write(RedisCommands.ObjectEncoding(arguments));
        }

        /// <summary>
        /// Inspect the internals of Redis objects
        /// </summary>
        /// <param name="subCommand">Type of Object command to send</param>
        /// <param name="arguments">Subcommand arguments</param>
        /// <returns>Varies depending on subCommand</returns>
        public virtual long? Object(RedisObjectSubCommand subCommand, params string[] arguments)
        {
            return Write(RedisCommands.Object(subCommand, arguments));
        }

        /// <summary>
        /// Remove the expiration from a key
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <returns>True if timeout was removed</returns>
        public virtual bool Persist(string key)
        {
            return Write(RedisCommands.Persist(key));
        }

        /// <summary>
        /// Set a key's time to live in milliseconds
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="expiration">Expiration (nearest millisecond)</param>
        /// <returns>True if timeout was set</returns>
        public virtual bool PExpire(string key, TimeSpan expiration)
        {
            return Write(RedisCommands.PExpire(key, expiration));
        }

        /// <summary>
        /// Set a key's time to live in milliseconds
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="milliseconds">Expiration in milliseconds</param>
        /// <returns>True if timeout was set</returns>
        public virtual bool PExpire(string key, long milliseconds)
        {
            return Write(RedisCommands.PExpire(key, milliseconds));
        }

        /// <summary>
        /// Set the expiration for a key (nearest millisecond)
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="date">Expiration date</param>
        /// <returns>True if timeout was set</returns>
        public virtual bool PExpireAt(string key, DateTime date)
        {
            return Write(RedisCommands.PExpireAt(key, date));
        }

        /// <summary>
        /// Set the expiration for a key as a UNIX timestamp specified in milliseconds
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="timestamp">Expiration timestamp (milliseconds)</param>
        /// <returns>True if timeout was set</returns>
        public virtual bool PExpireAt(string key, long timestamp)
        {
            return Write(RedisCommands.PExpireAt(key, timestamp));
        }

        /// <summary>
        /// Get the time to live for a key in milliseconds
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>Time-to-live in milliseconds</returns>
        public virtual long PTtl(string key)
        {
            return Write(RedisCommands.PTtl(key));
        }

        /// <summary>
        /// Return a random key from the keyspace
        /// </summary>
        /// <returns>A random key</returns>
        public virtual string RandomKey()
        {
            return Write(RedisCommands.RandomKey());
        }

        /// <summary>
        /// Rename a key
        /// </summary>
        /// <param name="key">Key to rename</param>
        /// <param name="newKey">New key name</param>
        /// <returns>Status code</returns>
        public virtual string Rename(string key, string newKey)
        {
            return Write(RedisCommands.Rename(key, newKey));
        }

        /// <summary>
        /// Rename a key, only if the new key does not exist
        /// </summary>
        /// <param name="key">Key to rename</param>
        /// <param name="newKey">New key name</param>
        /// <returns>True if key was renamed</returns>
        public virtual bool RenameNx(string key, string newKey)
        {
            return Write(RedisCommands.RenameNx(key, newKey));
        }

        /// <summary>
        /// Create a key using the provided serialized value, previously obtained using dump
        /// </summary>
        /// <param name="key">Key to restore</param>
        /// <param name="ttlMilliseconds">Time-to-live in milliseconds</param>
        /// <param name="serializedValue">Serialized value from DUMP</param>
        /// <returns>Status code</returns>
        public virtual string Restore(string key, long ttlMilliseconds, byte[] serializedValue)
        {
            return Write(RedisCommands.Restore(key, ttlMilliseconds, serializedValue));
        }

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
        public virtual string[] Sort(string key, long? offset = null, long? count = 0, string by = null, RedisSortDir? dir = null, bool? isAlpha = null, params string[] get)
        {
            return Write(RedisCommands.Sort(key, offset, count, by, dir, isAlpha, get));
        }

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
        public virtual long SortAndStore(string key, string destination, long? offset = null, long? count = 0, string by = null, RedisSortDir? dir = null, bool? isAlpha = false, params string[] get)
        {
            return Write(RedisCommands.SortAndStore(key, destination, offset, count, by, dir, isAlpha, get));
        }

        /// <summary>
        /// Get the time to live for a key
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>Time-to-live in seconds</returns>
        public virtual long Ttl(string key)
        {
            return Write(RedisCommands.Ttl(key));
        }

        /// <summary>
        /// Determine the type stored at key
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>Type of key</returns>
        public virtual string Type(string key)
        {
            return Write(RedisCommands.Type(key));
        }

        /// <summary>
        /// Iterate the set of keys in the currently selected Redis database
        /// </summary>
        /// <param name="cursor">The cursor returned by the server in the previous call, or 0 if this is the first call</param>
        /// <param name="pattern">Glob-style pattern to filter returned elements</param>
        /// <param name="count">Set the maximum number of elements to return</param>
        /// <returns>Updated cursor and result set</returns>
        public virtual RedisScan<string> Scan(long cursor, string pattern = null, long? count = null)
        {
            return Write(RedisCommands.Scan(cursor, pattern, count));
        }
        public virtual RedisScan<byte[]> ScanBytes(long cursor, string pattern = null, long? count = null)
        {
            return Write(RedisCommands.ScanBytes(cursor, pattern, count));
        }

        #endregion //end Sync

#if !net40

        #region Async

        /// <summary>
        /// [redis-server 3.2.1] 修改指定key(s) 最后访问时间 若key不存在，不做操作
        /// </summary>
        /// <param name="keys">Keys</param>
        /// <returns></returns>
        public virtual async Task<long> TouchAsync(params string[] keys)
        {
            return await WriteAsync(RedisCommands.Touch(keys));
        }

        /// <summary>
        /// [redis-server 4.0.0] Delete a key, 该命令和DEL十分相似：删除指定的key(s),若key不存在则该key被跳过。但是，相比DEL会产生阻塞，该命令会在另一个线程中回收内存，因此它是非阻塞的。 这也是该命令名字的由来：仅将keys从keyspace元数据中删除，真正的删除会在后续异步操作。
        /// </summary>
        /// <param name="keys">Keys to delete</param>
        /// <returns>Number of keys removed</returns>
        public virtual async Task<long> UnLinkAsync(params string[] keys)
        {
            return await WriteAsync(RedisCommands.UnLink(keys));
        }

        /// <summary>
        /// Delete a key
        /// </summary>
        /// <param name="keys">Keys to delete</param>
        /// <returns></returns>
        public virtual async Task<long> DelAsync(params string[] keys)
        {
            return await WriteAsync(RedisCommands.Del(keys));
        }

        /// <summary>
        /// Return a serialized version of the value stored at the specified key
        /// </summary>
        /// <param name="key">Key to dump</param>
        /// <returns></returns>
        public virtual async Task<byte[]> DumpAsync(string key)
        {
            return await WriteAsync(RedisCommands.Dump(key));
        }

        /// <summary>
        /// Determine if a key exists
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns></returns>
        public virtual async Task<bool> ExistsAsync(string key)
        {
            return await WriteAsync(RedisCommands.Exists(key));
        }

        public virtual async Task<long> ExistsAsync(string[] keys)
        {
            return await WriteAsync(RedisCommands.Exists(keys));
        }

        /// <summary>
        /// Set a key's time to live in seconds
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="expiration">Expiration (nearest second)</param>
        /// <returns></returns>
        public virtual async Task<bool> ExpireAsync(string key, int expiration)
        {
            return await WriteAsync(RedisCommands.Expire(key, expiration));
        }

        /// <summary>
        /// Set a key's time to live in seconds
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="expiration">Expiration in seconds</param>
        /// <returns></returns>
        public virtual async Task<bool> ExpireAsync(string key, TimeSpan expiration)
        {
            return await WriteAsync(RedisCommands.Expire(key, expiration));
        }

        /// <summary>
        /// Set the expiration for a key (nearest second)
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="expirationDate">Date of expiration, to nearest second</param>
        /// <returns></returns>
        public virtual async Task<bool> ExpireAtAsync(string key, DateTime expirationDate)
        {
            return await WriteAsync(RedisCommands.ExpireAt(key, expirationDate));
        }

        /// <summary>
        /// Set the expiration for a key as a UNIX timestamp
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public virtual async Task<bool> ExpireAtAsync(string key, int timestamp)
        {
            return await WriteAsync(RedisCommands.ExpireAt(key, timestamp));
        }

        /// <summary>
        /// Find all keys matching the given pattern
        /// </summary>
        /// <param name="pattern">Pattern to match</param>
        /// <returns></returns>
        public virtual async Task<string[]> KeysAsync(string pattern)
        {
            return await WriteAsync(RedisCommands.Keys(pattern));
        }

        /// <summary>
        /// Atomically transfer a key from a Redis instance to another one
        /// </summary>
        /// <param name="host">Remote Redis host</param>
        /// <param name="port">Remote Redis port</param>
        /// <param name="key">Key to migrate</param>
        /// <param name="destinationDb">Remote database ID</param>
        /// <param name="timeout">Timeout in milliseconds</param>
        /// <returns></returns>
        public virtual async Task<string> MigrateAsync(string host, int port, string key, int destinationDb, int timeout)
        {
            return await WriteAsync(RedisCommands.Migrate(host, port, key, destinationDb, timeout));
        }

        /// <summary>
        /// Atomically transfer a key from a Redis instance to another one
        /// </summary>
        /// <param name="host">Remote Redis host</param>
        /// <param name="port">Remote Redis port</param>
        /// <param name="key">Key to migrate</param>
        /// <param name="destinationDb">Remote database ID</param>
        /// <param name="timeout">Timeout in milliseconds</param>
        /// <returns></returns>
        public virtual async Task<string> MigrateAsync(string host, int port, string key, int destinationDb, TimeSpan timeout)
        {
            return await WriteAsync(RedisCommands.Migrate(host, port, key, destinationDb, timeout));
        }

        /// <summary>
        /// Move a key to another database
        /// </summary>
        /// <param name="key">Key to move</param>
        /// <param name="database">Database destination ID</param>
        /// <returns></returns>
        public virtual async Task<bool> MoveAsync(string key, int database)
        {
            return await WriteAsync(RedisCommands.Move(key, database));
        }

        /// <summary>
        /// Get the number of references of the value associated with the specified key
        /// </summary>
        /// <param name="arguments">Subcommand arguments</param>
        /// <returns>The type of internal representation used to store the value at the specified key</returns>
        public virtual async Task<string> ObjectEncodingAsync(params string[] arguments)
        {
            return await WriteAsync(RedisCommands.ObjectEncoding(arguments));
        }

        /// <summary>
        /// Inspect the internals of Redis objects
        /// </summary>
        /// <param name="subCommand">Type of Object command to send</param>
        /// <param name="arguments">Subcommand arguments</param>
        /// <returns>Varies depending on subCommand</returns>
        public virtual async Task<long?> ObjectAsync(RedisObjectSubCommand subCommand, params string[] arguments)
        {
            return await WriteAsync(RedisCommands.Object(subCommand, arguments));
        }

        /// <summary>
        /// Remove the expiration from a key
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <returns></returns>
        public virtual async Task<bool> PersistAsync(string key)
        {
            return await WriteAsync(RedisCommands.Persist(key));
        }

        /// <summary>
        /// Set a key's time to live in milliseconds
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="expiration">Expiration (nearest millisecond)</param>
        /// <returns></returns>
        public virtual async Task<bool> PExpireAsync(string key, TimeSpan expiration)
        {
            return await WriteAsync(RedisCommands.PExpire(key, expiration));
        }

        /// <summary>
        /// Set a key's time to live in milliseconds
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="milliseconds">Expiration in milliseconds</param>
        /// <returns></returns>
        public virtual async Task<bool> PExpireAsync(string key, long milliseconds)
        {
            return await WriteAsync(RedisCommands.PExpire(key, milliseconds));
        }

        /// <summary>
        /// Set the expiration for a key (nearest millisecond)
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="date">Expiration date</param>
        /// <returns></returns>
        public virtual async Task<bool> PExpireAtAsync(string key, DateTime date)
        {
            return await WriteAsync(RedisCommands.PExpireAt(key, date));
        }

        /// <summary>
        /// Set the expiration for a key as a UNIX timestamp specified in milliseconds
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="timestamp">Expiration timestamp (milliseconds)</param>
        /// <returns></returns>
        public virtual async Task<bool> PExpireAtAsync(string key, long timestamp)
        {
            return await WriteAsync(RedisCommands.PExpireAt(key, timestamp));
        }

        /// <summary>
        /// Get the time to live for a key in milliseconds
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns></returns>
        public virtual async Task<long> PTtlAsync(string key)
        {
            return await WriteAsync(RedisCommands.PTtl(key));
        }

        /// <summary>
        /// Return a random key from the keyspace
        /// </summary>
        /// <returns></returns>
        public virtual async Task<string> RandomKeyAsync()
        {
            return await WriteAsync(RedisCommands.RandomKey());
        }

        /// <summary>
        /// Rename a key
        /// </summary>
        /// <param name="key">Key to rename</param>
        /// <param name="newKey">New key name</param>
        /// <returns></returns>
        public virtual async Task<string> RenameAsync(string key, string newKey)
        {
            return await WriteAsync(RedisCommands.Rename(key, newKey));
        }

        /// <summary>
        /// Rename a key, only if the new key does not exist
        /// </summary>
        /// <param name="key">Key to rename</param>
        /// <param name="newKey">New key name</param>
        /// <returns></returns>
        public virtual async Task<bool> RenameNxAsync(string key, string newKey)
        {
            return await WriteAsync(RedisCommands.RenameNx(key, newKey));
        }

        /// <summary>
        /// Create a key using the provided serialized value, previously obtained using dump
        /// </summary>
        /// <param name="key">Key to restore</param>
        /// <param name="ttlMilliseconds">Time-to-live in milliseconds</param>
        /// <param name="serializedValue">Serialized value from DUMP</param>
        /// <returns></returns>
        public virtual async Task<string> RestoreAsync(string key, long ttlMilliseconds, byte[] serializedValue)
        {
            return await WriteAsync(RedisCommands.Restore(key, ttlMilliseconds, serializedValue));
        }

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
        public virtual async Task<string[]> SortAsync(string key, long? offset = null, long? count = null, string by = null, RedisSortDir? dir = null, bool? isAlpha = null, params string[] get)
        {
            return await WriteAsync(RedisCommands.Sort(key, offset, count, by, dir, isAlpha, get));
        }

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
        public virtual async Task<long> SortAndStoreAsync(string key, string destination, long? offset = null, long? count = null, string by = null, RedisSortDir? dir = null, bool? isAlpha = null, params string[] get)
        {
            return await WriteAsync(RedisCommands.SortAndStore(key, destination, offset, count, by, dir, isAlpha, get));
        }

        /// <summary>
        /// Get the time to live for a key
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns></returns>
        public virtual async Task<long> TtlAsync(string key)
        {
            return await WriteAsync(RedisCommands.Ttl(key));
        }

        /// <summary>
        /// Determine the type stored at key
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns></returns>
        public virtual async Task<string> TypeAsync(string key)
        {
            return await WriteAsync(RedisCommands.Type(key));
        }

        /// <summary>
        /// Iterate the set of keys in the currently selected Redis database
        /// </summary>
        /// <param name="cursor">The cursor returned by the server in the previous call, or 0 if this is the first call</param>
        /// <param name="pattern">Glob-style pattern to filter returned elements</param>
        /// <param name="count">Set the maximum number of elements to return</param>
        /// <returns>Updated cursor and result set</returns>
        public virtual async Task<RedisScan<string>> ScanAsync(long cursor, string pattern = null, long? count = null)
        {
            return await WriteAsync(RedisCommands.Scan(cursor, pattern, count));
        }

        public virtual async Task<RedisScan<byte[]>> ScanBytesAsync(long cursor, string pattern = null, long? count = null)
        {
            return await WriteAsync(RedisCommands.ScanBytes(cursor, pattern, count));
        }

        #endregion //end Async

#endif

    }
}
