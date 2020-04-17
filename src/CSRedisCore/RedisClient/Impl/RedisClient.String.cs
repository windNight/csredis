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
        /// Append a value to a key
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="value">Value to append to key</param>
        /// <returns>Length of string after append</returns>
        public virtual long Append(string key, object value)
        {
            return Write(RedisCommands.Append(key, value));
        }

        /// <summary>
        /// Count set bits in a string
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <param name="start">Start offset</param>
        /// <param name="end">Stop offset</param>
        /// <returns>Number of bits set to 1</returns>
        public virtual long BitCount(string key, long? start = null, long? end = null)
        {
            return Write(RedisCommands.BitCount(key, start, end));
        }

        /// <summary>
        /// Perform bitwise operations between strings
        /// </summary>
        /// <param name="operation">Bit command to execute</param>
        /// <param name="destKey">Store result in destination key</param>
        /// <param name="keys">Keys to operate</param>
        /// <returns>Size of string stored in the destination key</returns>
        public virtual long BitOp(RedisBitOp operation, string destKey, params string[] keys)
        {
            return Write(RedisCommands.BitOp(operation, destKey, keys));
        }

        /// <summary>
        /// Find first bit set or clear in a string
        /// </summary>
        /// <param name="key">Key to examine</param>
        /// <param name="bit">Bit value (1 or 0)</param>
        /// <param name="start">Examine string at specified byte offset</param>
        /// <param name="end">Examine string to specified byte offset</param>
        /// <returns>Position of the first bit set to the specified value</returns>
        public virtual long BitPos(string key, bool bit, long? start = null, long? end = null)
        {
            return Write(RedisCommands.BitPos(key, bit, start, end));
        }

        /// <summary>
        /// Decrement the integer value of a key by one
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <returns>Value of key after decrement</returns>
        public virtual long Decr(string key)
        {
            return Write(RedisCommands.Decr(key));
        }

        /// <summary>
        /// Decrement the integer value of a key by the given number
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="decrement">Decrement value</param>
        /// <returns>Value of key after decrement</returns>
        public virtual long DecrBy(string key, long decrement)
        {
            return Write(RedisCommands.DecrBy(key, decrement));
        }

        /// <summary>
        /// Get the value of a key
        /// </summary>
        /// <param name="key">Key to lookup</param>
        /// <returns>Value of key</returns>
        public virtual string Get(string key)
        {
            return Write(RedisCommands.Get(key));
        }
        public virtual byte[] GetBytes(string key)
        {
            return Write(RedisCommands.GetBytes(key));
        }

        /// <summary>
        /// Returns the bit value at offset in the string value stored at key
        /// </summary>
        /// <param name="key">Key to lookup</param>
        /// <param name="offset">Offset of key to check</param>
        /// <returns>Bit value stored at offset</returns>
        public virtual bool GetBit(string key, uint offset)
        {
            return Write(RedisCommands.GetBit(key, offset));
        }

        /// <summary>
        /// Get a substring of the string stored at a key
        /// </summary>
        /// <param name="key">Key to lookup</param>
        /// <param name="start">Start offset</param>
        /// <param name="end">End offset</param>
        /// <returns>Substring in the specified range</returns>
        public virtual string GetRange(string key, long start, long end)
        {
            return Write(RedisCommands.GetRange(key, start, end));
        }
        public virtual byte[] GetRangeBytes(string key, long start, long end)
        {
            return Write(RedisCommands.GetRangeBytes(key, start, end));
        }

        /// <summary>
        /// Set the string value of a key and return its old value
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="value">Value to set</param>
        /// <returns>Old value stored at key, or null if key did not exist</returns>
        public virtual string GetSet(string key, object value)
        {
            return Write(RedisCommands.GetSet(key, value));
        }
        public virtual byte[] GetSetBytes(string key, object value)
        {
            return Write(RedisCommands.GetSetBytes(key, value));
        }

        /// <summary>
        /// Increment the integer value of a key by one
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <returns>Value of key after increment</returns>
        public virtual long Incr(string key)
        {
            return Write(RedisCommands.Incr(key));
        }

        /// <summary>
        /// Increment the integer value of a key by the given amount
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="increment">Increment amount</param>
        /// <returns>Value of key after increment</returns>
        public virtual long IncrBy(string key, long increment)
        {
            return Write(RedisCommands.IncrBy(key, increment));
        }

        /// <summary>
        /// Increment the float value of a key by the given amount
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="increment">Increment amount</param>
        /// <returns>Value of key after increment</returns>
        public virtual decimal IncrByFloat(string key, decimal increment)
        {
            return Write(RedisCommands.IncrByFloat(key, increment));
        }

        /// <summary>
        /// Get the values of all the given keys
        /// </summary>
        /// <param name="keys">Keys to lookup</param>
        /// <returns>Array of values at the specified keys</returns>
        public virtual string[] MGet(params string[] keys)
        {
            return Write(RedisCommands.MGet(keys));
        }
        public virtual byte[][] MGetBytes(params string[] keys)
        {
            return Write(RedisCommands.MGetBytes(keys));
        }
        /// <summary>
        /// Set multiple keys to multiple values
        /// </summary>
        /// <param name="keyValues">Key values to set</param>
        /// <returns>Status code</returns>
        public virtual string MSet(params Tuple<string, object>[] keyValues)
        {
            return Write(RedisCommands.MSet(keyValues));
        }

        /// <summary>
        /// Set multiple keys to multiple values
        /// </summary>
        /// <param name="keyValues">Key values to set [k1, v1, k2, v2, ..]</param>
        /// <returns>Status code</returns>
        public virtual string MSet(params object[] keyValues)
        {
            return Write(RedisCommands.MSet(keyValues));
        }

        /// <summary>
        /// Set multiple keys to multiple values, only if none of the keys exist
        /// </summary>
        /// <param name="keyValues">Key values to set</param>
        /// <returns>True if all keys were set</returns>
        public virtual bool MSetNx(params Tuple<string, object>[] keyValues)
        {
            return Write(RedisCommands.MSetNx(keyValues));
        }

        /// <summary>
        /// Set multiple keys to multiple values, only if none of the keys exist
        /// </summary>
        /// <param name="keyValues">Key values to set [k1, v1, k2, v2, ..]</param>
        /// <returns>True if all keys were set</returns>
        public virtual bool MSetNx(params object[] keyValues)
        {
            return Write(RedisCommands.MSetNx(keyValues));
        }

        /// <summary>
        /// Set the value and expiration in milliseconds of a key
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="milliseconds">Expiration in milliseconds</param>
        /// <param name="value">Value to set</param>
        /// <returns>Status code</returns>
        public virtual string PSetEx(string key, long milliseconds, object value)
        {
            return Write(RedisCommands.PSetEx(key, milliseconds, value));
        }

        /// <summary>
        /// Set the string value of a key
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="value">Value to set</param>
        /// <returns>Status code</returns>
        public virtual string Set(string key, object value)
        {
            return Write(RedisCommands.Set(key, value));
        }

        /// <summary>
        /// Set the string value of a key with atomic expiration and existence condition
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="value">Value to set</param>
        /// <param name="expiration">Set expiration to nearest millisecond</param>
        /// <param name="condition">Set key if existence condition</param>
        /// <returns>Status code, or null if condition not met</returns>
        public virtual string Set(string key, object value, TimeSpan expiration, RedisExistence? condition = null)
        {
            return Write(RedisCommands.Set(key, value, expiration, condition));
        }

        /// <summary>
        /// Set the string value of a key with atomic expiration and existence condition
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="value">Value to set</param>
        /// <param name="expirationSeconds">Set expiration to nearest second</param>
        /// <param name="condition">Set key if existence condition</param>
        /// <returns>Status code, or null if condition not met</returns>
        public virtual string Set(string key, object value, int? expirationSeconds = null, RedisExistence? condition = null)
        {
            return Write(RedisCommands.Set(key, value, expirationSeconds, condition));
        }

        /// <summary>
        /// Set the string value of a key with atomic expiration and existence condition
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="value">Value to set</param>
        /// <param name="expirationMilliseconds">Set expiration to nearest millisecond</param>
        /// <param name="condition">Set key if existence condition</param>
        /// <returns>Status code, or null if condition not met</returns>
        public virtual string Set(string key, object value, long? expirationMilliseconds = null, RedisExistence? condition = null)
        {
            return Write(RedisCommands.Set(key, value, expirationMilliseconds, condition));
        }

        /// <summary>
        /// Sets or clears the bit at offset in the string value stored at key
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="offset">Modify key at offset</param>
        /// <param name="value">Value to set (on or off)</param>
        /// <returns>Original bit stored at offset</returns>
        public virtual bool SetBit(string key, uint offset, bool value)
        {
            return Write(RedisCommands.SetBit(key, offset, value));
        }

        /// <summary>
        /// Set the value and expiration of a key
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="seconds">Expiration in seconds</param>
        /// <param name="value">Value to set</param>
        /// <returns>Status code</returns>
        public virtual string SetEx(string key, long seconds, object value)
        {
            return Write(RedisCommands.SetEx(key, seconds, value));
        }

        /// <summary>
        /// Set the value of a key, only if the key does not exist
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="value">Value to set</param>
        /// <returns>True if key was set</returns>
        public virtual bool SetNx(string key, object value)
        {
            return Write(RedisCommands.SetNx(key, value));
        }

        /// <summary>
        /// Overwrite part of a string at key starting at the specified offset
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="offset">Start offset</param>
        /// <param name="value">Value to write at offset</param>
        /// <returns>Length of string after operation</returns>
        public virtual long SetRange(string key, uint offset, object value)
        {
            return Write(RedisCommands.SetRange(key, offset, value));
        }

        /// <summary>
        /// Get the length of the value stored in a key
        /// </summary>
        /// <param name="key">Key to lookup</param>
        /// <returns>Length of string at key</returns>
        public virtual long StrLen(string key)
        {
            return Write(RedisCommands.StrLen(key));
        }

        #endregion //end Sync

#if !net40

        #region Async

        /// <summary>
        /// Append a value to a key
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="value">Value to append to key</param>
        /// <returns>Length of string after append</returns>
        public virtual async Task<long> AppendAsync(string key, object value)
        {
            return await WriteAsync(RedisCommands.Append(key, value));
        }

        /// <summary>
        /// Count set bits in a string
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <param name="start">Start offset</param>
        /// <param name="end">Stop offset</param>
        /// <returns>Number of bits set to 1</returns>
        public virtual async Task<long> BitCountAsync(string key, long? start = null, long? end = null)
        {
            return await WriteAsync(RedisCommands.BitCount(key, start, end));
        }

        /// <summary>
        /// Perform bitwise operations between strings
        /// </summary>
        /// <param name="operation">Bit command to execute</param>
        /// <param name="destKey">Store result in destination key</param>
        /// <param name="keys">Keys to operate</param>
        /// <returns>Size of string stored in the destination key</returns>
        public virtual async Task<long> BitOpAsync(RedisBitOp operation, string destKey, params string[] keys)
        {
            return await WriteAsync(RedisCommands.BitOp(operation, destKey, keys));
        }

        /// <summary>
        /// Find first bit set or clear in a string
        /// </summary>
        /// <param name="key">Key to examine</param>
        /// <param name="bit">Bit value (1 or 0)</param>
        /// <param name="start">Examine string at specified byte offset</param>
        /// <param name="end">Examine string to specified byte offset</param>
        /// <returns>Position of the first bit set to the specified value</returns>
        public virtual async Task<long> BitPosAsync(string key, bool bit, long? start = null, long? end = null)
        {
            return await WriteAsync(RedisCommands.BitPos(key, bit, start, end));
        }

        /// <summary>
        /// Decrement the integer value of a key by one
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <returns>Value of key after decrement</returns>
        public virtual async Task<long> DecrAsync(string key)
        {
            return await WriteAsync(RedisCommands.Decr(key));
        }

        /// <summary>
        /// Decrement the integer value of a key by the given number
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="decrement">Decrement value</param>
        /// <returns>Value of key after decrement</returns>
        public virtual async Task<long> DecrByAsync(string key, long decrement)
        {
            return await WriteAsync(RedisCommands.DecrBy(key, decrement));
        }

        /// <summary>
        /// Get the value of a key
        /// </summary>
        /// <param name="key">Key to lookup</param>
        /// <returns>Value of key</returns>
        public virtual async Task<string> GetAsync(string key)
        {
            return await WriteAsync(RedisCommands.Get(key));
        }
        public virtual async Task<byte[]> GetBytesAsync(string key)
        {
            return await WriteAsync(RedisCommands.GetBytes(key));
        }

        /// <summary>
        /// Returns the bit value at offset in the string value stored at key
        /// </summary>
        /// <param name="key">Key to lookup</param>
        /// <param name="offset">Offset of key to check</param>
        /// <returns>Bit value stored at offset</returns>
        public virtual async Task<bool> GetBitAsync(string key, uint offset)
        {
            return await WriteAsync(RedisCommands.GetBit(key, offset));
        }

        /// <summary>
        /// Get a substring of the string stored at a key
        /// </summary>
        /// <param name="key">Key to lookup</param>
        /// <param name="start">Start offset</param>
        /// <param name="end">End offset</param>
        /// <returns>Substring in the specified range</returns>
        public virtual async Task<string> GetRangeAsync(string key, long start, long end)
        {
            return await WriteAsync(RedisCommands.GetRange(key, start, end));
        }
        public virtual async Task<byte[]> GetRangeBytesAsync(string key, long start, long end)
        {
            return await WriteAsync(RedisCommands.GetRangeBytes(key, start, end));
        }

        /// <summary>
        /// Set the string value of a key and return its old value
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="value">Value to set</param>
        /// <returns>Old value stored at key, or null if key did not exist</returns>
        public virtual async Task<string> GetSetAsync(string key, object value)
        {
            return await WriteAsync(RedisCommands.GetSet(key, value));
        }
        public virtual async Task<byte[]> GetSetBytesAsync(string key, object value)
        {
            return await WriteAsync(RedisCommands.GetSetBytes(key, value));
        }

        /// <summary>
        /// Increment the integer value of a key by one
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <returns>Value of key after increment</returns>
        public virtual async Task<long> IncrAsync(string key)
        {
            return await WriteAsync(RedisCommands.Incr(key));
        }

        /// <summary>
        /// Increment the integer value of a key by the given amount
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="increment">Increment amount</param>
        /// <returns>Value of key after increment</returns>
        public virtual async Task<long> IncrByAsync(string key, long increment)
        {
            return await WriteAsync(RedisCommands.IncrBy(key, increment));
        }

        /// <summary>
        /// Increment the float value of a key by the given amount
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="increment">Increment amount</param>
        /// <returns>Value of key after increment</returns>
        public virtual async Task<decimal> IncrByFloatAsync(string key, decimal increment)
        {
            return await WriteAsync(RedisCommands.IncrByFloat(key, increment));
        }

        /// <summary>
        /// Get the values of all the given keys
        /// </summary>
        /// <param name="keys">Keys to lookup</param>
        /// <returns>Array of values at the specified keys</returns>
        public virtual async Task<string[]> MGetAsync(params string[] keys)
        {
            return await WriteAsync(RedisCommands.MGet(keys));
        }
        public virtual async Task<byte[][]> MGetBytesAsync(params string[] keys)
        {
            return await WriteAsync(RedisCommands.MGetBytes(keys));
        }

        /// <summary>
        /// Set multiple keys to multiple values
        /// </summary>
        /// <param name="keyValues">Key values to set</param>
        /// <returns>Status code</returns>
        public virtual async Task<string> MSetAsync(params Tuple<string, object>[] keyValues)
        {
            return await WriteAsync(RedisCommands.MSet(keyValues));
        }

        /// <summary>
        /// Set multiple keys to multiple values
        /// </summary>
        /// <param name="keyValues">Key values to set [k1, v1, k2, v2, ..]</param>
        /// <returns>Status code</returns>
        public virtual async Task<string> MSetAsync(params object[] keyValues)
        {
            return await WriteAsync(RedisCommands.MSet(keyValues));
        }

        /// <summary>
        /// Set multiple keys to multiple values, only if none of the keys exist
        /// </summary>
        /// <param name="keyValues">Key values to set</param>
        /// <returns>True if all keys were set</returns>
        public virtual async Task<bool> MSetNxAsync(params Tuple<string, object>[] keyValues)
        {
            return await WriteAsync(RedisCommands.MSetNx(keyValues));
        }

        /// <summary>
        /// Set multiple keys to multiple values, only if none of the keys exist
        /// </summary>
        /// <param name="keyValues">Key values to set [k1, v1, k2, v2, ..]</param>
        /// <returns>True if all keys were set</returns>
        public virtual async Task<bool> MSetNxAsync(params object[] keyValues)
        {
            return await WriteAsync(RedisCommands.MSetNx(keyValues));
        }

        /// <summary>
        /// Set the value and expiration in milliseconds of a key
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="milliseconds">Expiration in milliseconds</param>
        /// <param name="value">Value to set</param>
        /// <returns>Status code</returns>
        public virtual async Task<string> PSetExAsync(string key, long milliseconds, object value)
        {
            return await WriteAsync(RedisCommands.PSetEx(key, milliseconds, value));
        }

        /// <summary>
        /// Set the string value of a key
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="value">Value to set</param>
        /// <returns>Status code</returns>
        public virtual async Task<string> SetAsync(string key, object value)
        {
            return await WriteAsync(RedisCommands.Set(key, value));
        }

        /// <summary>
        /// Set the string value of a key with atomic expiration and existence condition
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="value">Value to set</param>
        /// <param name="expiration">Set expiration to nearest millisecond</param>
        /// <param name="condition">Set key if existence condition</param>
        /// <returns>Status code, or null if condition not met</returns>
        public virtual async Task<string> SetAsync(string key, object value, TimeSpan expiration, RedisExistence? condition = null)
        {
            return await WriteAsync(RedisCommands.Set(key, value, expiration, condition));
        }

        /// <summary>
        /// Set the string value of a key with atomic expiration and existence condition
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="value">Value to set</param>
        /// <param name="expirationSeconds">Set expiration to nearest second</param>
        /// <param name="condition">Set key if existence condition</param>
        /// <returns>Status code, or null if condition not met</returns>
        public virtual async Task<string> SetAsync(string key, object value, int? expirationSeconds = null, RedisExistence? condition = null)
        {
            return await WriteAsync(RedisCommands.Set(key, value, expirationSeconds, condition));
        }

        /// <summary>
        /// Set the string value of a key with atomic expiration and existence condition
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="value">Value to set</param>
        /// <param name="expirationMilliseconds">Set expiration to nearest millisecond</param>
        /// <param name="condition">Set key if existence condition</param>
        /// <returns>Status code, or null if condition not met</returns>
        public virtual async Task<string> SetAsync(string key, object value, long? expirationMilliseconds = null, RedisExistence? condition = null)
        {
            return await WriteAsync(RedisCommands.Set(key, value, expirationMilliseconds, condition));
        }

        /// <summary>
        /// Sets or clears the bit at offset in the string value stored at key
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="offset">Modify key at offset</param>
        /// <param name="value">Value to set (on or off)</param>
        /// <returns>Original bit stored at offset</returns>
        public virtual async Task<bool> SetBitAsync(string key, uint offset, bool value)
        {
            return await WriteAsync(RedisCommands.SetBit(key, offset, value));
        }

        /// <summary>
        /// Set the value and expiration of a key
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="seconds">Expiration in seconds</param>
        /// <param name="value">Value to set</param>
        /// <returns>Status code</returns>
        public virtual async Task<string> SetExAsync(string key, long seconds, object value)
        {
            return await WriteAsync(RedisCommands.SetEx(key, seconds, value));
        }

        /// <summary>
        /// Set the value of a key, only if the key does not exist
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="value">Value to set</param>
        /// <returns>True if key was set</returns>
        public virtual async Task<bool> SetNxAsync(string key, object value)
        {
            return await WriteAsync(RedisCommands.SetNx(key, value));
        }

        /// <summary>
        /// Overwrite part of a string at key starting at the specified offset
        /// </summary>
        /// <param name="key">Key to modify</param>
        /// <param name="offset">Start offset</param>
        /// <param name="value">Value to write at offset</param>
        /// <returns>Length of string after operation</returns>
        public virtual async Task<long> SetRangeAsync(string key, uint offset, object value)
        {
            return await WriteAsync(RedisCommands.SetRange(key, offset, value));
        }

        /// <summary>
        /// Get the length of the value stored in a key
        /// </summary>
        /// <param name="key">Key to lookup</param>
        /// <returns>Length of string at key</returns>
        public virtual async Task<long> StrLenAsync(string key)
        {
            return await WriteAsync(RedisCommands.StrLen(key));
        }

        #endregion //end Async

#endif

    }
}
