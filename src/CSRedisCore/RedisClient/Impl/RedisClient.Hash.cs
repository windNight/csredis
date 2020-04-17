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
        /// [redis-server 3.2.0] 返回hash指定field的value的字符串长度，如果hash或者field不存在，返回0.
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="field">Field</param>
        /// <returns></returns>
        public virtual long HStrLen(string key, string field)
        {
            return Write(RedisCommands.HStrLen(key, field));
        }

        /// <summary>
        /// Delete one or more hash fields
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="fields">Fields to delete</param>
        /// <returns>Number of fields removed from hash</returns>
        public virtual long HDel(string key, params string[] fields)
        {
            return Write(RedisCommands.HDel(key, fields));
        }

        /// <summary>
        /// Determine if a hash field exists
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="field">Field to check</param>
        /// <returns>True if hash field exists</returns>
        public virtual bool HExists(string key, string field)
        {
            return Write(RedisCommands.HExists(key, field));
        }

        /// <summary>
        /// Get the value of a hash field
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="field">Field to get</param>
        /// <returns>Value of hash field</returns>
        public virtual string HGet(string key, string field)
        {
            return Write(RedisCommands.HGet(key, field));
        }
        public virtual byte[] HGetBytes(string key, string field)
        {
            return Write(RedisCommands.HGetBytes(key, field));
        }

        /// <summary>
        /// Get all the fields and values in a hash
        /// </summary>
        /// <typeparam name="T">Object to map hash</typeparam>
        /// <param name="key">Hash key</param>
        /// <returns>Strongly typed object mapped from hash</returns>
        public virtual T HGetAll<T>(string key)
            where T : class
        {
            return Write(RedisCommands.HGetAll<T>(key));
        }

        /// <summary>
        /// Get all the fields and values in a hash
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <returns>Dictionary mapped from string</returns>
        public virtual Dictionary<string, string> HGetAll(string key)
        {
            return Write(RedisCommands.HGetAll(key));
        }
        public virtual Dictionary<string, byte[]> HGetAllBytes(string key)
        {
            return Write(RedisCommands.HGetAllBytes(key));
        }

        /// <summary>
        /// Increment the integer value of a hash field by the given number
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="field">Field to increment</param>
        /// <param name="increment">Increment value</param>
        /// <returns>Value of field after increment</returns>
        public virtual long HIncrBy(string key, string field, long increment)
        {
            return Write(RedisCommands.HIncrBy(key, field, increment));
        }

        /// <summary>
        /// Increment the float value of a hash field by the given number
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="field">Field to increment</param>
        /// <param name="increment">Increment value</param>
        /// <returns>Value of field after increment</returns>
        public virtual decimal HIncrByFloat(string key, string field, decimal increment)
        {
            return Write(RedisCommands.HIncrByFloat(key, field, increment));
        }

        /// <summary>
        /// Get all the fields in a hash
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <returns>All hash field names</returns>
        public virtual string[] HKeys(string key)
        {
            return Write(RedisCommands.HKeys(key));
        }

        /// <summary>
        /// Get the number of fields in a hash
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <returns>Number of fields in hash</returns>
        public virtual long HLen(string key)
        {
            return Write(RedisCommands.HLen(key));
        }

        /// <summary>
        /// Get the values of all the given hash fields
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="fields">Fields to return</param>
        /// <returns>Values of given fields</returns>
        public virtual string[] HMGet(string key, params string[] fields)
        {
            return Write(RedisCommands.HMGet(key, fields));
        }
        public virtual byte[][] HMGetBytes(string key, params string[] fields)
        {
            return Write(RedisCommands.HMGetBytes(key, fields));
        }

        /// <summary>
        /// Set multiple hash fields to multiple values
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="dict">Dictionary mapping of hash</param>
        /// <returns>Status code</returns>
        public virtual string HMSet(string key, Dictionary<string, object> dict)
        {
            return Write(RedisCommands.HMSet(key, dict));
        }

        /// <summary>
        /// Set multiple hash fields to multiple values
        /// </summary>
        /// <typeparam name="T">Type of object to map hash</typeparam>
        /// <param name="key">Hash key</param>
        /// <param name="obj">Object mapping of hash</param>
        /// <returns>Status code</returns>
        public virtual string HMSet<T>(string key, T obj)
            where T : class
        {
            return Write(RedisCommands.HMSet<T>(key, obj));
        }

        /// <summary>
        /// Set multiple hash fields to multiple values
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="keyValues">Array of [key,value,key,value,..]</param>
        /// <returns>Status code</returns>
        public virtual string HMSet(string key, params object[] keyValues)
        {
            return Write(RedisCommands.HMSet(key, keyValues));
        }

        /// <summary>
        /// Set the value of a hash field
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="field">Hash field to set</param>
        /// <param name="value">Value to set</param>
        /// <returns>True if field is new</returns>
        public virtual bool HSet(string key, string field, object value)
        {
            return Write(RedisCommands.HSet(key, field, value));
        }

        /// <summary>
        /// Set the value of a hash field, only if the field does not exist
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="field">Hash field to set</param>
        /// <param name="value">Value to set</param>
        /// <returns>True if field was set to value</returns>
        public virtual bool HSetNx(string key, string field, object value)
        {
            return Write(RedisCommands.HSetNx(key, field, value));
        }

        /// <summary>
        /// Get all the values in a hash
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <returns>Array of all values in hash</returns>
        public virtual string[] HVals(string key)
        {
            return Write(RedisCommands.HVals(key));
        }
        public virtual byte[][] HValsBytes(string key)
        {
            return Write(RedisCommands.HValsBytes(key));
        }

        /// <summary>
        /// Iterate the keys and values of a hash field
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="cursor">The cursor returned by the server in the previous call, or 0 if this is the first call</param>
        /// <param name="pattern">Glob-style pattern to filter returned elements</param>
        /// <param name="count">Maximum number of elements to return</param>
        /// <returns>Updated cursor and result set</returns>
        public virtual RedisScan<Tuple<string, string>> HScan(string key, long cursor, string pattern = null, long? count = null)
        {
            return Write(RedisCommands.HScan(key, cursor, pattern, count));
        }
        public virtual RedisScan<Tuple<string, byte[]>> HScanBytes(string key, long cursor, string pattern = null, long? count = null)
        {
            return Write(RedisCommands.HScanBytes(key, cursor, pattern, count));
        }

        #endregion //end Sync

#if !net40

        #region Async

        /// <summary>
        /// [redis-server 3.2.0] 返回hash指定field的value的字符串长度，如果hash或者field不存在，返回0.
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="field">Field</param>
        /// <returns></returns>
        public virtual async Task<long> HStrLenAsync(string key, string field)
        {
            return await WriteAsync(RedisCommands.HStrLen(key, field));
        }

        /// <summary>
        /// Delete one or more hash fields
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="fields">Fields to delete</param>
        /// <returns>Number of fields removed from hash</returns>
        public virtual async Task<long> HDelAsync(string key, params string[] fields)
        {
            return await WriteAsync(RedisCommands.HDel(key, fields));
        }

        /// <summary>
        /// Determine if a hash field exists
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="field">Field to check</param>
        /// <returns>True if hash field exists</returns>
        public virtual async Task<bool> HExistsAsync(string key, string field)
        {
            return await WriteAsync(RedisCommands.HExists(key, field));
        }

        /// <summary>
        /// Get the value of a hash field
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="field">Field to get</param>
        /// <returns>Value of hash field</returns>
        public virtual async Task<string> HGetAsync(string key, string field)
        {
            return await WriteAsync(RedisCommands.HGet(key, field));
        }

        public virtual async Task<byte[]> HGetBytesAsync(string key, string field)
        {
            return await WriteAsync(RedisCommands.HGetBytes(key, field));
        }

        /// <summary>
        /// Get all the fields and values in a hash
        /// </summary>
        /// <typeparam name="T">Object to map hash</typeparam>
        /// <param name="key">Hash key</param>
        /// <returns>Strongly typed object mapped from hash</returns>
        public virtual async Task<T> HGetAllAsync<T>(string key)
            where T : class
        {
            return await WriteAsync(RedisCommands.HGetAll<T>(key));
        }

        /// <summary>
        /// Get all the fields and values in a hash
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <returns>Dictionary mapped from string</returns>
        public virtual async Task<Dictionary<string, string>> HGetAllAsync(string key)
        {
            return await WriteAsync(RedisCommands.HGetAll(key));
        }

        public virtual async Task<Dictionary<string, byte[]>> HGetAllBytesAsync(string key)
        {
            return await WriteAsync(RedisCommands.HGetAllBytes(key));
        }

        /// <summary>
        /// Increment the integer value of a hash field by the given number
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="field">Field to increment</param>
        /// <param name="increment">Increment value</param>
        /// <returns>Value of field after increment</returns>
        public virtual async Task<long> HIncrByAsync(string key, string field, long increment)
        {
            return await WriteAsync(RedisCommands.HIncrBy(key, field, increment));
        }

        /// <summary>
        /// Increment the float value of a hash field by the given number
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="field">Field to increment</param>
        /// <param name="increment">Increment value</param>
        /// <returns>Value of field after increment</returns>
        public virtual async Task<decimal> HIncrByFloatAsync(string key, string field, decimal increment)
        {
            return await WriteAsync(RedisCommands.HIncrByFloat(key, field, increment));
        }

        /// <summary>
        /// Get all the fields in a hash
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <returns>All hash field names</returns>
        public virtual async Task<string[]> HKeysAsync(string key)
        {
            return await WriteAsync(RedisCommands.HKeys(key));
        }

        /// <summary>
        /// Get the number of fields in a hash
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <returns>Number of fields in hash</returns>
        public virtual async Task<long> HLenAsync(string key)
        {
            return await WriteAsync(RedisCommands.HLen(key));
        }

        /// <summary>
        /// Get the values of all the given hash fields
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="fields">Fields to return</param>
        /// <returns>Values of given fields</returns>
        public virtual async Task<string[]> HMGetAsync(string key, params string[] fields)
        {
            return await WriteAsync(RedisCommands.HMGet(key, fields));
        }

        public virtual async Task<byte[][]> HMGetBytesAsync(string key, params string[] fields)
        {
            return await WriteAsync(RedisCommands.HMGetBytes(key, fields));
        }

        /// <summary>
        /// Set multiple hash fields to multiple values
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="dict">Dictionary mapping of hash</param>
        /// <returns>Status code</returns>
        public virtual async Task<string> HMSetAsync(string key, Dictionary<string, object> dict)
        {
            return await WriteAsync(RedisCommands.HMSet(key, dict));
        }

        /// <summary>
        /// Set multiple hash fields to multiple values
        /// </summary>
        /// <typeparam name="T">Type of object to map hash</typeparam>
        /// <param name="key">Hash key</param>
        /// <param name="obj">Object mapping of hash</param>
        /// <returns>Status code</returns>
        public virtual async Task<string> HMSetAsync<T>(string key, T obj)
            where T : class
        {
            return await WriteAsync(RedisCommands.HMSet<T>(key, obj));
        }

        /// <summary>
        /// Set multiple hash fields to multiple values
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="keyValues">Array of [key,value,key,value,..]</param>
        /// <returns>Status code</returns>
        public virtual async Task<string> HMSetAsync(string key, params object[] keyValues)
        {
            return await WriteAsync(RedisCommands.HMSet(key, keyValues));
        }

        /// <summary>
        /// Set the value of a hash field
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="field">Hash field to set</param>
        /// <param name="value">Value to set</param>
        /// <returns>True if field is new</returns>
        public virtual async Task<bool> HSetAsync(string key, string field, object value)
        {
            return await WriteAsync(RedisCommands.HSet(key, field, value));
        }

        /// <summary>
        /// Set the value of a hash field, only if the field does not exist
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="field">Hash field to set</param>
        /// <param name="value">Value to set</param>
        /// <returns>True if field was set to value</returns>
        public virtual async Task<bool> HSetNxAsync(string key, string field, object value)
        {
            return await WriteAsync(RedisCommands.HSetNx(key, field, value));
        }

        /// <summary>
        /// Get all the values in a hash
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <returns>Array of all values in hash</returns>
        public virtual async Task<string[]> HValsAsync(string key)
        {
            return await WriteAsync(RedisCommands.HVals(key));
        }

        public virtual async Task<byte[][]> HValsBytesAsync(string key)
        {
            return await WriteAsync(RedisCommands.HValsBytes(key));
        }

        /// <summary>
        /// Iterate the keys and values of a hash field
        /// </summary>
        /// <param name="key">Hash key</param>
        /// <param name="cursor">The cursor returned by the server in the previous call, or 0 if this is the first call</param>
        /// <param name="pattern">Glob-style pattern to filter returned elements</param>
        /// <param name="count">Maximum number of elements to return</param>
        /// <returns>Updated cursor and result set</returns>
        public virtual async Task<RedisScan<Tuple<string, string>>> HScanAsync(string key, long cursor, string pattern = null, long? count = null)
        {
            return await WriteAsync(RedisCommands.HScan(key, cursor, pattern, count));
        }

        public virtual async Task<RedisScan<Tuple<string, byte[]>>> HScanBytesAsync(string key, long cursor, string pattern = null, long? count = null)
        {
            return await WriteAsync(RedisCommands.HScanBytes(key, cursor, pattern, count));
        }

        #endregion//end Async
#endif
    }
}
