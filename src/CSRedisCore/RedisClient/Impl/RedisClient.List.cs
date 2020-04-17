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
        /// Remove and get the first element and key in a list, or block until one is available
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <param name="keys">List keys</param>
        /// <returns>List key and list value</returns>
        public virtual Tuple<string, string> BLPopWithKey(int timeout, params string[] keys)
        {
            return Write(RedisCommands.BLPopWithKey(timeout, keys));
        }
        public virtual Tuple<string, byte[]> BLPopBytesWithKey(int timeout, params string[] keys)
        {
            return Write(RedisCommands.BLPopBytesWithKey(timeout, keys));
        }

        /// <summary>
        /// Remove and get the first element and key in a list, or block until one is available
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <param name="keys">List keys</param>
        /// <returns>List key and list value</returns>
        public virtual Tuple<string, string> BLPopWithKey(TimeSpan timeout, params string[] keys)
        {
            return Write(RedisCommands.BLPopWithKey(timeout, keys));
        }
        public virtual Tuple<string, byte[]> BLPopBytesWithKey(TimeSpan timeout, params string[] keys)
        {
            return Write(RedisCommands.BLPopBytesWithKey(timeout, keys));
        }

        /// <summary>
        /// Remove and get the first element value in a list, or block until one is available
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <param name="keys">List keys</param>
        /// <returns>List value</returns>
        public virtual string BLPop(int timeout, params string[] keys)
        {
            return Write(RedisCommands.BLPop(timeout, keys));
        }
        public virtual byte[] BLPopBytes(int timeout, params string[] keys)
        {
            return Write(RedisCommands.BLPopBytes(timeout, keys));
        }

        /// <summary>
        /// Remove and get the first element value in a list, or block until one is available
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <param name="keys">List keys</param>
        /// <returns>List value</returns>
        public virtual string BLPop(TimeSpan timeout, params string[] keys)
        {
            return Write(RedisCommands.BLPop(timeout, keys));
        }
        public virtual byte[] BLPopBytes(TimeSpan timeout, params string[] keys)
        {
            return Write(RedisCommands.BLPopBytes(timeout, keys));
        }

        /// <summary>
        /// Remove and get the last element and key in a list, or block until one is available
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <param name="keys">List keys</param>
        /// <returns>List key and list value</returns>
        public virtual Tuple<string, string> BRPopWithKey(int timeout, params string[] keys)
        {
            return Write(RedisCommands.BRPopWithKey(timeout, keys));
        }
        public virtual Tuple<string, byte[]> BRPopBytesWithKey(int timeout, params string[] keys)
        {
            return Write(RedisCommands.BRPopBytesWithKey(timeout, keys));
        }

        /// <summary>
        /// Remove and get the last element and key in a list, or block until one is available
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <param name="keys">List keys</param>
        /// <returns>List key and list value</returns>
        public virtual Tuple<string, string> BRPopWithKey(TimeSpan timeout, params string[] keys)
        {
            return Write(RedisCommands.BRPopWithKey(timeout, keys));
        }
        public virtual Tuple<string, byte[]> BRPopBytesWithKey(TimeSpan timeout, params string[] keys)
        {
            return Write(RedisCommands.BRPopBytesWithKey(timeout, keys));
        }

        /// <summary>
        /// Remove and get the last element value in a list, or block until one is available
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <param name="keys">List value</param>
        /// <returns></returns>
        public virtual string BRPop(int timeout, params string[] keys)
        {
            return Write(RedisCommands.BRPop(timeout, keys));
        }
        public virtual byte[] BRPopBytes(int timeout, params string[] keys)
        {
            return Write(RedisCommands.BRPopBytes(timeout, keys));
        }

        /// <summary>
        /// Remove and get the last element value in a list, or block until one is available
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <param name="keys">List keys</param>
        /// <returns>List value</returns>
        public virtual string BRPop(TimeSpan timeout, params string[] keys)
        {
            return Write(RedisCommands.BRPop(timeout, keys));
        }
        public virtual byte[] BRPopBytes(TimeSpan timeout, params string[] keys)
        {
            return Write(RedisCommands.BRPopBytes(timeout, keys));
        }

        /// <summary>
        /// Pop a value from a list, push it to another list and return it; or block until one is available
        /// </summary>
        /// <param name="source">Source list key</param>
        /// <param name="destination">Destination key</param>
        /// <param name="timeout">Timeout in seconds</param>
        /// <returns>Element popped</returns>
        public virtual string BRPopLPush(string source, string destination, int timeout)
        {
            return Write(RedisCommands.BRPopLPush(source, destination, timeout));
        }
        public virtual byte[] BRPopBytesLPush(string source, string destination, int timeout)
        {
            return Write(RedisCommands.BRPopBytesLPush(source, destination, timeout));
        }

        /// <summary>
        /// Pop a value from a list, push it to another list and return it; or block until one is available
        /// </summary>
        /// <param name="source">Source list key</param>
        /// <param name="destination">Destination key</param>
        /// <param name="timeout">Timeout in seconds</param>
        /// <returns>Element popped</returns>
        public virtual string BRPopLPush(string source, string destination, TimeSpan timeout)
        {
            return Write(RedisCommands.BRPopLPush(source, destination, timeout));
        }
        public virtual byte[] BRPopBytesLPush(string source, string destination, TimeSpan timeout)
        {
            return Write(RedisCommands.BRPopBytesLPush(source, destination, timeout));
        }

        /// <summary>
        /// Get an element from a list by its index
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="index">Zero-based index of item to return</param>
        /// <returns>Element at index</returns>
        public virtual string LIndex(string key, long index)
        {
            return Write(RedisCommands.LIndex(key, index));
        }
        public virtual byte[] LIndexBytes(string key, long index)
        {
            return Write(RedisCommands.LIndexBytes(key, index));
        }

        /// <summary>
        /// Insert an element before or after another element in a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="insertType">Relative position</param>
        /// <param name="pivot">Relative element</param>
        /// <param name="value">Element to insert</param>
        /// <returns>Length of list after insert or -1 if pivot not found</returns>
        public virtual long LInsert(string key, RedisInsert insertType, object pivot, object value)
        {
            return Write(RedisCommands.LInsert(key, insertType, pivot, value));
        }

        /// <summary>
        /// Get the length of a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <returns>Length of list at key</returns>
        public virtual long LLen(string key)
        {
            return Write(RedisCommands.LLen(key));
        }

        /// <summary>
        /// Remove and get the first element in a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <returns>First element in list</returns>
        public virtual string LPop(string key)
        {
            return Write(RedisCommands.LPop(key));
        }
        public virtual byte[] LPopBytes(string key)
        {
            return Write(RedisCommands.LPopBytes(key));
        }

        /// <summary>
        /// Prepend one or multiple values to a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="values">Values to push</param>
        /// <returns>Length of list after push</returns>
        public virtual long LPush(string key, params object[] values)
        {
            return Write(RedisCommands.LPush(key, values));
        }

        /// <summary>
        /// Prepend a value to a list, only if the list exists
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="value">Value to push</param>
        /// <returns>Length of list after push</returns>
        public virtual long LPushX(string key, object value)
        {
            return Write(RedisCommands.LPushX(key, value));
        }

        /// <summary>
        /// Get a range of elements from a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <returns>List of elements in range</returns>
        public virtual string[] LRange(string key, long start, long stop)
        {
            return Write(RedisCommands.LRange(key, start, stop));
        }
        public virtual byte[][] LRangeBytes(string key, long start, long stop)
        {
            return Write(RedisCommands.LRangeBytes(key, start, stop));
        }

        /// <summary>
        /// Remove elements from a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="count">&gt;0: remove N elements from head to tail; &lt;0: remove N elements from tail to head; =0: remove all elements</param>
        /// <param name="value">Remove elements equal to value</param>
        /// <returns>Number of removed elements</returns>
        public virtual long LRem(string key, long count, object value)
        {
            return Write(RedisCommands.LRem(key, count, value));
        }

        /// <summary>
        /// Set the value of an element in a list by its index
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="index">List index to modify</param>
        /// <param name="value">New element value</param>
        /// <returns>Status code</returns>
        public virtual string LSet(string key, long index, object value)
        {
            return Write(RedisCommands.LSet(key, index, value));
        }

        /// <summary>
        /// Trim a list to the specified range
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="start">Zero-based start index</param>
        /// <param name="stop">Zero-based stop index</param>
        /// <returns>Status code</returns>
        public virtual string LTrim(string key, long start, long stop)
        {
            return Write(RedisCommands.LTrim(key, start, stop));
        }

        /// <summary>
        /// Remove and get the last elment in a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <returns>Value of last list element</returns>
        public virtual string RPop(string key)
        {
            return Write(RedisCommands.RPop(key));
        }
        public virtual byte[] RPopBytes(string key)
        {
            return Write(RedisCommands.RPopBytes(key));
        }

        /// <summary>
        /// Remove the last elment in a list, append it to another list and return it
        /// </summary>
        /// <param name="source">List source key</param>
        /// <param name="destination">Destination key</param>
        /// <returns>Element being popped and pushed</returns>
        public virtual string RPopLPush(string source, string destination)
        {
            return Write(RedisCommands.RPopLPush(source, destination));
        }
        public virtual byte[] RPopBytesLPush(string source, string destination)
        {
            return Write(RedisCommands.RPopBytesLPush(source, destination));
        }

        /// <summary>
        /// Append one or multiple values to a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="values">Values to push</param>
        /// <returns>Length of list after push</returns>
        public virtual long RPush(string key, params object[] values)
        {
            return Write(RedisCommands.RPush(key, values));
        }

        /// <summary>
        /// Append a value to a list, only if the list exists
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="value">Value to push</param>
        /// <returns>Length of list after push</returns>
        public virtual long RPushX(string key, object value)
        {
            return Write(RedisCommands.RPushX(key, value));
        }

        #endregion //end Sync


#if !net40

        #region Async

        /// <summary>
        /// Get an element from a list by its index
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="index">Zero-based index of item to return</param>
        /// <returns>Element at index</returns>
        public virtual async Task<string> LIndexAsync(string key, long index)
        {
            return await WriteAsync(RedisCommands.LIndex(key, index));
        }

        public virtual async Task<byte[]> LIndexBytesAsync(string key, long index)
        {
            return await WriteAsync(RedisCommands.LIndexBytes(key, index));
        }

        /// <summary>
        /// Insert an element before or after another element in a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="insertType">Relative position</param>
        /// <param name="pivot">Relative element</param>
        /// <param name="value">Element to insert</param>
        /// <returns>Length of list after insert or -1 if pivot not found</returns>
        public virtual async Task<long> LInsertAsync(string key, RedisInsert insertType, object pivot, object value)
        {
            return await WriteAsync(RedisCommands.LInsert(key, insertType, pivot, value));
        }

        /// <summary>
        /// Get the length of a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <returns>Length of list at key</returns>
        public virtual async Task<long> LLenAsync(string key)
        {
            return await WriteAsync(RedisCommands.LLen(key));
        }

        /// <summary>
        /// Remove and get the first element in a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <returns>First element in list</returns>
        public virtual async Task<string> LPopAsync(string key)
        {
            return await WriteAsync(RedisCommands.LPop(key));
        }

        public virtual async Task<byte[]> LPopBytesAsync(string key)
        {
            return await WriteAsync(RedisCommands.LPopBytes(key));
        }

        /// <summary>
        /// Prepend one or multiple values to a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="values">Values to push</param>
        /// <returns>Length of list after push</returns>
        public virtual async Task<long> LPushAsync(string key, params object[] values)
        {
            return await WriteAsync(RedisCommands.LPush(key, values));
        }

        /// <summary>
        /// Prepend a value to a list, only if the list exists
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="value">Value to push</param>
        /// <returns>Length of list after push</returns>
        public virtual async Task<long> LPushXAsync(string key, object value)
        {
            return await WriteAsync(RedisCommands.LPushX(key, value));
        }

        /// <summary>
        /// Get a range of elements from a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <returns>List of elements in range</returns>
        public virtual async Task<string[]> LRangeAsync(string key, long start, long stop)
        {
            return await WriteAsync(RedisCommands.LRange(key, start, stop));
        }

        public virtual async Task<byte[][]> LRangeBytesAsync(string key, long start, long stop)
        {
            return await WriteAsync(RedisCommands.LRangeBytes(key, start, stop));
        }

        /// <summary>
        /// Remove elements from a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="count">&gt;0: remove N elements from head to tail; &lt;0: remove N elements from tail to head; =0: remove all elements</param>
        /// <param name="value">Remove elements equal to value</param>
        /// <returns>Number of removed elements</returns>
        public virtual async Task<long> LRemAsync(string key, long count, object value)
        {
            return await WriteAsync(RedisCommands.LRem(key, count, value));
        }

        /// <summary>
        /// Set the value of an element in a list by its index
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="index">List index to modify</param>
        /// <param name="value">New element value</param>
        /// <returns>Status code</returns>
        public virtual async Task<string> LSetAsync(string key, long index, object value)
        {
            return await WriteAsync(RedisCommands.LSet(key, index, value));
        }

        /// <summary>
        /// Trim a list to the specified range
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="start">Zero-based start index</param>
        /// <param name="stop">Zero-based stop index</param>
        /// <returns>Status code</returns>
        public virtual async Task<string> LTrimAsync(string key, long start, long stop)
        {
            return await WriteAsync(RedisCommands.LTrim(key, start, stop));
        }

        /// <summary>
        /// Remove and get the last elment in a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <returns>Value of last list element</returns>
        public virtual async Task<string> RPopAsync(string key)
        {
            return await WriteAsync(RedisCommands.RPop(key));
        }

        public virtual async Task<byte[]> RPopBytesAsync(string key)
        {
            return await WriteAsync(RedisCommands.RPopBytes(key));
        }

        /// <summary>
        /// Remove the last elment in a list, append it to another list and return it
        /// </summary>
        /// <param name="source">List source key</param>
        /// <param name="destination">Destination key</param>
        /// <returns>Element being popped and pushed</returns>
        public virtual async Task<string> RPopLPushAsync(string source, string destination)
        {
            return await WriteAsync(RedisCommands.RPopLPush(source, destination));
        }

        public virtual async Task<byte[]> RPopBytesLPushAsync(string source, string destination)
        {
            return await WriteAsync(RedisCommands.RPopBytesLPush(source, destination));
        }

        /// <summary>
        /// Append one or multiple values to a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="values">Values to push</param>
        /// <returns>Length of list after push</returns>
        public virtual async Task<long> RPushAsync(string key, params object[] values)
        {
            return await WriteAsync(RedisCommands.RPush(key, values));
        }

        /// <summary>
        /// Append a value to a list, only if the list exists
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="value">Value to push</param>
        /// <returns>Length of list after push</returns>
        public virtual async Task<long> RPushXAsync(string key, object value)
        {
            return await WriteAsync(RedisCommands.RPushX(key, value));
        }
        #endregion //Async
#endif


    }
}
