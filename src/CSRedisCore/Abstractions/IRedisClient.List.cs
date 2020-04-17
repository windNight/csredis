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
    public interface IRedisClientUseList
    {
        #region Lists
        /// <summary>
        /// Remove and get the first element and key in a list, or block until one is available
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <param name="keys">List keys</param>
        /// <returns>List key and list value</returns>
        Tuple<string, string> BLPopWithKey(int timeout, params string[] keys);
        Tuple<string, byte[]> BLPopBytesWithKey(int timeout, params string[] keys);


        /// <summary>
        /// Remove and get the first element and key in a list, or block until one is available
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <param name="keys">List keys</param>
        /// <returns>List key and list value</returns>
        Tuple<string, string> BLPopWithKey(TimeSpan timeout, params string[] keys);
        Tuple<string, byte[]> BLPopBytesWithKey(TimeSpan timeout, params string[] keys);


        /// <summary>
        /// Remove and get the first element value in a list, or block until one is available
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <param name="keys">List keys</param>
        /// <returns>List value</returns>
        string BLPop(int timeout, params string[] keys);
        byte[] BLPopBytes(int timeout, params string[] keys);


        /// <summary>
        /// Remove and get the first element value in a list, or block until one is available
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <param name="keys">List keys</param>
        /// <returns>List value</returns>
        string BLPop(TimeSpan timeout, params string[] keys);
        byte[] BLPopBytes(TimeSpan timeout, params string[] keys);


        /// <summary>
        /// Remove and get the last element and key in a list, or block until one is available
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <param name="keys">List keys</param>
        /// <returns>List key and list value</returns>
        Tuple<string, string> BRPopWithKey(int timeout, params string[] keys);
        Tuple<string, byte[]> BRPopBytesWithKey(int timeout, params string[] keys);


        /// <summary>
        /// Remove and get the last element and key in a list, or block until one is available
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <param name="keys">List keys</param>
        /// <returns>List key and list value</returns>
        Tuple<string, string> BRPopWithKey(TimeSpan timeout, params string[] keys);
        Tuple<string, byte[]> BRPopBytesWithKey(TimeSpan timeout, params string[] keys);

        /// <summary>
        /// Remove and get the last element value in a list, or block until one is available
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <param name="keys">List value</param>
        /// <returns></returns>
        string BRPop(int timeout, params string[] keys);
        byte[] BRPopBytes(int timeout, params string[] keys);

        /// <summary>
        /// Remove and get the last element value in a list, or block until one is available
        /// </summary>
        /// <param name="timeout">Timeout in seconds</param>
        /// <param name="keys">List keys</param>
        /// <returns>List value</returns>
        string BRPop(TimeSpan timeout, params string[] keys);
        byte[] BRPopBytes(TimeSpan timeout, params string[] keys);

        /// <summary>
        /// Pop a value from a list, push it to another list and return it; or block until one is available
        /// </summary>
        /// <param name="source">Source list key</param>
        /// <param name="destination">Destination key</param>
        /// <param name="timeout">Timeout in seconds</param>
        /// <returns>Element popped</returns>
        string BRPopLPush(string source, string destination, int timeout);
        byte[] BRPopBytesLPush(string source, string destination, int timeout);

        /// <summary>
        /// Pop a value from a list, push it to another list and return it; or block until one is available
        /// </summary>
        /// <param name="source">Source list key</param>
        /// <param name="destination">Destination key</param>
        /// <param name="timeout">Timeout in seconds</param>
        /// <returns>Element popped</returns>
        string BRPopLPush(string source, string destination, TimeSpan timeout);
        byte[] BRPopBytesLPush(string source, string destination, TimeSpan timeout);

        /// <summary>
        /// Get an element from a list by its index
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="index">Zero-based index of item to return</param>
        /// <returns>Element at index</returns>
        string LIndex(string key, long index);
        byte[] LIndexBytes(string key, long index);

        /// <summary>
        /// Insert an element before or after another element in a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="insertType">Relative position</param>
        /// <param name="pivot">Relative element</param>
        /// <param name="value">Element to insert</param>
        /// <returns>Length of list after insert or -1 if pivot not found</returns>
        long LInsert(string key, RedisInsert insertType, object pivot, object value);


        /// <summary>
        /// Get the length of a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <returns>Length of list at key</returns>
        long LLen(string key);


        /// <summary>
        /// Remove and get the first element in a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <returns>First element in list</returns>
        string LPop(string key);
        byte[] LPopBytes(string key);

        /// <summary>
        /// Prepend one or multiple values to a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="values">Values to push</param>
        /// <returns>Length of list after push</returns>
        long LPush(string key, params object[] values);

        /// <summary>
        /// Prepend a value to a list, only if the list exists
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="value">Value to push</param>
        /// <returns>Length of list after push</returns>
        long LPushX(string key, object value);


        /// <summary>
        /// Get a range of elements from a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <returns>List of elements in range</returns>
        string[] LRange(string key, long start, long stop);
        byte[][] LRangeBytes(string key, long start, long stop);

        /// <summary>
        /// Remove elements from a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="count">&gt;0: remove N elements from head to tail; &lt;0: remove N elements from tail to head; =0: remove all elements</param>
        /// <param name="value">Remove elements equal to value</param>
        /// <returns>Number of removed elements</returns>
        long LRem(string key, long count, object value);


        /// <summary>
        /// Set the value of an element in a list by its index
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="index">List index to modify</param>
        /// <param name="value">New element value</param>
        /// <returns>Status code</returns>
        string LSet(string key, long index, object value);

        /// <summary>
        /// Trim a list to the specified range
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="start">Zero-based start index</param>
        /// <param name="stop">Zero-based stop index</param>
        /// <returns>Status code</returns>
        string LTrim(string key, long start, long stop);

        /// <summary>
        /// Remove and get the last elment in a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <returns>Value of last list element</returns>
        string RPop(string key);
        byte[] RPopBytes(string key);

        /// <summary>
        /// Remove the last elment in a list, append it to another list and return it
        /// </summary>
        /// <param name="source">List source key</param>
        /// <param name="destination">Destination key</param>
        /// <returns>Element being popped and pushed</returns>
        string RPopLPush(string source, string destination);
        byte[] RPopBytesLPush(string source, string destination);

        /// <summary>
        /// Append one or multiple values to a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="values">Values to push</param>
        /// <returns>Length of list after push</returns>
        long RPush(string key, params object[] values);

        /// <summary>
        /// Append a value to a list, only if the list exists
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="value">Value to push</param>
        /// <returns>Length of list after push</returns>
        long RPushX(string key, object value);
        #endregion

#if !net40

        #region Lists
        /// <summary>
        /// Get an element from a list by its index
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="index">Zero-based index of item to return</param>
        /// <returns>Element at index</returns>
        Task<string> LIndexAsync(string key, long index);
        Task<byte[]> LIndexBytesAsync(string key, long index);



        /// <summary>
        /// Insert an element before or after another element in a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="insertType">Relative position</param>
        /// <param name="pivot">Relative element</param>
        /// <param name="value">Element to insert</param>
        /// <returns>Length of list after insert or -1 if pivot not found</returns>
        Task<long> LInsertAsync(string key, RedisInsert insertType, object pivot, object value);




        /// <summary>
        /// Get the length of a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <returns>Length of list at key</returns>
        Task<long> LLenAsync(string key);




        /// <summary>
        /// Remove and get the first element in a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <returns>First element in list</returns>
        Task<string> LPopAsync(string key);
        Task<byte[]> LPopBytesAsync(string key);



        /// <summary>
        /// Prepend one or multiple values to a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="values">Values to push</param>
        /// <returns>Length of list after push</returns>
        Task<long> LPushAsync(string key, params object[] values);




        /// <summary>
        /// Prepend a value to a list, only if the list exists
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="value">Value to push</param>
        /// <returns>Length of list after push</returns>
        Task<long> LPushXAsync(string key, object value);




        /// <summary>
        /// Get a range of elements from a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <returns>List of elements in range</returns>
        Task<string[]> LRangeAsync(string key, long start, long stop);
        Task<byte[][]> LRangeBytesAsync(string key, long start, long stop);



        /// <summary>
        /// Remove elements from a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="count">&gt;0: remove N elements from head to tail; &lt;0: remove N elements from tail to head; =0: remove all elements</param>
        /// <param name="value">Remove elements equal to value</param>
        /// <returns>Number of removed elements</returns>
        Task<long> LRemAsync(string key, long count, object value);




        /// <summary>
        /// Set the value of an element in a list by its index
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="index">List index to modify</param>
        /// <param name="value">New element value</param>
        /// <returns>Status code</returns>
        Task<string> LSetAsync(string key, long index, object value);




        /// <summary>
        /// Trim a list to the specified range
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="start">Zero-based start index</param>
        /// <param name="stop">Zero-based stop index</param>
        /// <returns>Status code</returns>
        Task<string> LTrimAsync(string key, long start, long stop);




        /// <summary>
        /// Remove and get the last elment in a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <returns>Value of last list element</returns>
        Task<string> RPopAsync(string key);
        Task<byte[]> RPopBytesAsync(string key);



        /// <summary>
        /// Remove the last elment in a list, append it to another list and return it
        /// </summary>
        /// <param name="source">List source key</param>
        /// <param name="destination">Destination key</param>
        /// <returns>Element being popped and pushed</returns>
        Task<string> RPopLPushAsync(string source, string destination);
        Task<byte[]> RPopBytesLPushAsync(string source, string destination);



        /// <summary>
        /// Append one or multiple values to a list
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="values">Values to push</param>
        /// <returns>Length of list after push</returns>
        Task<long> RPushAsync(string key, params object[] values);


        /// <summary>
        /// Append a value to a list, only if the list exists
        /// </summary>
        /// <param name="key">List key</param>
        /// <param name="value">Value to push</param>
        /// <returns>Length of list after push</returns>
        Task<long> RPushXAsync(string key, object value);



        #endregion

#endif

    }
}
