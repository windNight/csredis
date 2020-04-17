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
        /// Add one or more members to a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="members">Members to add to set</param>
        /// <returns>Number of elements added to set</returns>
        public virtual long SAdd(string key, params object[] members)
        {
            return Write(RedisCommands.SAdd(key, members));
        }

        /// <summary>
        /// Get the number of members in a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <returns>Number of elements in set</returns>
        public virtual long SCard(string key)
        {
            return Write(RedisCommands.SCard(key));
        }

        /// <summary>
        /// Subtract multiple sets
        /// </summary>
        /// <param name="keys">Set keys to subtract</param>
        /// <returns>Array of elements in resulting set</returns>
        public virtual string[] SDiff(params string[] keys)
        {
            return Write(RedisCommands.SDiff(keys));
        }
        public virtual byte[][] SDiffBytes(params string[] keys)
        {
            return Write(RedisCommands.SDiffBytes(keys));
        }

        /// <summary>
        /// Subtract multiple sets and store the resulting set in a key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="keys">Set keys to subtract</param>
        /// <returns>Number of elements in the resulting set</returns>
        public virtual long SDiffStore(string destination, params string[] keys)
        {
            return Write(RedisCommands.SDiffStore(destination, keys));
        }

        /// <summary>
        /// Intersect multiple sets
        /// </summary>
        /// <param name="keys">Set keys to intersect</param>
        /// <returns>Array of elements in resulting set</returns>
        public virtual string[] SInter(params string[] keys)
        {
            return Write(RedisCommands.SInter(keys));
        }
        public virtual byte[][] SInterBytes(params string[] keys)
        {
            return Write(RedisCommands.SInterBytes(keys));
        }

        /// <summary>
        /// Intersect multiple sets and store the resulting set in a key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="keys">Set keys to intersect</param>
        /// <returns>Number of elements in resulting set</returns>
        public virtual long SInterStore(string destination, params string[] keys)
        {
            return Write(RedisCommands.SInterStore(destination, keys));
        }

        /// <summary>
        /// Determine if a given value is a member of a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="member">Member to lookup</param>
        /// <returns>True if member exists in set</returns>
        public virtual bool SIsMember(string key, object member)
        {
            return Write(RedisCommands.SIsMember(key, member));
        }

        /// <summary>
        /// Get all the members in a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <returns>All elements in the set</returns>
        public virtual string[] SMembers(string key)
        {
            return Write(RedisCommands.SMembers(key));
        }
        public virtual byte[][] SMembersBytes(string key)
        {
            return Write(RedisCommands.SMembersBytes(key));
        }

        /// <summary>
        /// Move a member from one set to another
        /// </summary>
        /// <param name="source">Source key</param>
        /// <param name="destination">Destination key</param>
        /// <param name="member">Member to move</param>
        /// <returns>True if element was moved</returns>
        public virtual bool SMove(string source, string destination, object member)
        {
            return Write(RedisCommands.SMove(source, destination, member));
        }

        /// <summary>
        /// Remove and return a random member from a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <returns>The removed element</returns>
        public virtual string SPop(string key)
        {
            return Write(RedisCommands.SPop(key));
        }
        public virtual byte[] SPopBytes(string key)
        {
            return Write(RedisCommands.SPopBytes(key));
        }

        /// <summary>
        /// Remove and return one or more random member from a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="count">Number of elements to remove and return</param>
        /// <returns>The removed elements</returns>
        public virtual string[] SPop(string key, long count)
        {
            return Write(RedisCommands.SPop(key, count));
        }
        public virtual byte[][] SPopBytes(string key, long count)
        {
            return Write(RedisCommands.SPopBytes(key, count));
        }

        /// <summary>
        /// Get a random member from a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <returns>One random element from set</returns>
        public virtual string SRandMember(string key)
        {
            return Write(RedisCommands.SRandMember(key));
        }
        public virtual byte[] SRandMemberBytes(string key)
        {
            return Write(RedisCommands.SRandMemberBytes(key));
        }

        /// <summary>
        /// Get one or more random members from a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>One or more random elements from set</returns>
        public virtual string[] SRandMembers(string key, long count)
        {
            return Write(RedisCommands.SRandMembers(key, count));
        }
        public virtual byte[][] SRandMembersBytes(string key, long count)
        {
            return Write(RedisCommands.SRandMembersBytes(key, count));
        }

        /// <summary>
        /// Remove one or more members from a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="members">Set members to remove</param>
        /// <returns>Number of elements removed from set</returns>
        public virtual long SRem(string key, params object[] members)
        {
            return Write(RedisCommands.SRem(key, members));
        }

        /// <summary>
        /// Add multiple sets
        /// </summary>
        /// <param name="keys">Set keys to union</param>
        /// <returns>Array of elements in resulting set</returns>
        public virtual string[] SUnion(params string[] keys)
        {
            return Write(RedisCommands.SUnion(keys));
        }
        public virtual byte[][] SUnionBytes(params string[] keys)
        {
            return Write(RedisCommands.SUnionBytes(keys));
        }

        /// <summary>
        /// Add multiple sets and store the resulting set in a key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="keys">Set keys to union</param>
        /// <returns>Number of elements in resulting set</returns>
        public virtual long SUnionStore(string destination, params string[] keys)
        {
            return Write(RedisCommands.SUnionStore(destination, keys));
        }

        /// <summary>
        /// Iterate the elements of a set field
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="cursor">The cursor returned by the server in the previous call, or 0 if this is the first call</param>
        /// <param name="pattern">Glob-style pattern to filter returned elements</param>
        /// <param name="count">Maximum number of elements to return</param>
        /// <returns>Updated cursor and result set</returns>
        public virtual RedisScan<string> SScan(string key, long cursor, string pattern = null, long? count = null)
        {
            return Write(RedisCommands.SScan(key, cursor, pattern, count));
        }
        public virtual RedisScan<byte[]> SScanBytes(string key, long cursor, string pattern = null, long? count = null)
        {
            return Write(RedisCommands.SScanBytes(key, cursor, pattern, count));
        }

        #endregion //end Sync

#if !net40

        #region Async

        /// <summary>
        /// Add one or more members to a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="members">Members to add to set</param>
        /// <returns>Number of elements added to set</returns>
        public virtual async Task<long> SAddAsync(string key, params object[] members)
        {
            return await WriteAsync(RedisCommands.SAdd(key, members));
        }

        /// <summary>
        /// Get the number of members in a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <returns>Number of elements in set</returns>
        public virtual async Task<long> SCardAsync(string key)
        {
            return await WriteAsync(RedisCommands.SCard(key));
        }

        /// <summary>
        /// Subtract multiple sets
        /// </summary>
        /// <param name="keys">Set keys to subtract</param>
        /// <returns>Array of elements in resulting set</returns>
        public virtual async Task<string[]> SDiffAsync(params string[] keys)
        {
            return await WriteAsync(RedisCommands.SDiff(keys));
        }

        public virtual async Task<byte[][]> SDiffBytesAsync(params string[] keys)
        {
            return await WriteAsync(RedisCommands.SDiffBytes(keys));
        }

        /// <summary>
        /// Subtract multiple sets and store the resulting set in a key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="keys">Set keys to subtract</param>
        /// <returns>Number of elements in the resulting set</returns>
        public virtual async Task<long> SDiffStoreAsync(string destination, params string[] keys)
        {
            return await WriteAsync(RedisCommands.SDiffStore(destination, keys));
        }

        /// <summary>
        /// Intersect multiple sets
        /// </summary>
        /// <param name="keys">Set keys to intersect</param>
        /// <returns>Array of elements in resulting set</returns>
        public virtual async Task<string[]> SInterAsync(params string[] keys)
        {
            return await WriteAsync(RedisCommands.SInter(keys));
        }

        public virtual async Task<byte[][]> SInterBytesAsync(params string[] keys)
        {
            return await WriteAsync(RedisCommands.SInterBytes(keys));
        }

        /// <summary>
        /// Intersect multiple sets and store the resulting set in a key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="keys">Set keys to intersect</param>
        /// <returns>Number of elements in resulting set</returns>
        public virtual async Task<long> SInterStoreAsync(string destination, params string[] keys)
        {
            return await WriteAsync(RedisCommands.SInterStore(destination, keys));
        }

        /// <summary>
        /// Determine if a given value is a member of a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="member">Member to lookup</param>
        /// <returns>True if member exists in set</returns>
        public virtual async Task<bool> SIsMemberAsync(string key, object member)
        {
            return await WriteAsync(RedisCommands.SIsMember(key, member));
        }

        /// <summary>
        /// Get all the members in a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <returns>All elements in the set</returns>
        public virtual async Task<string[]> SMembersAsync(string key)
        {
            return await WriteAsync(RedisCommands.SMembers(key));
        }

        public virtual async Task<byte[][]> SMembersBytesAsync(string key)
        {
            return await WriteAsync(RedisCommands.SMembersBytes(key));
        }

        /// <summary>
        /// Move a member from one set to another
        /// </summary>
        /// <param name="source">Source key</param>
        /// <param name="destination">Destination key</param>
        /// <param name="member">Member to move</param>
        /// <returns>True if element was moved</returns>
        public virtual async Task<bool> SMoveAsync(string source, string destination, object member)
        {
            return await WriteAsync(RedisCommands.SMove(source, destination, member));
        }

        /// <summary>
        /// Remove and return a random member from a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <returns>The removed element</returns>
        public virtual async Task<string> SPopAsync(string key)
        {
            return await WriteAsync(RedisCommands.SPop(key));
        }

        public virtual async Task<byte[]> SPopBytesAsync(string key)
        {
            return await WriteAsync(RedisCommands.SPopBytes(key));
        }

        /// <summary>
        /// Remove and return one or more random members from a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="count">Number of elements to remove and return</param>
        /// <returns></returns>
        public virtual async Task<string[]> SPopAsync(string key, long count)
        {
            return await WriteAsync(RedisCommands.SPop(key, count));
        }

        public virtual async Task<byte[][]> SPopBytesAsync(string key, long count)
        {
            return await WriteAsync(RedisCommands.SPopBytes(key, count));
        }

        /// <summary>
        /// Get a random member from a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <returns>One random element from set</returns>
        public virtual async Task<string> SRandMemberAsync(string key)
        {
            return await WriteAsync(RedisCommands.SRandMember(key));
        }

        public virtual async Task<byte[]> SRandMemberBytesAsync(string key)
        {
            return await WriteAsync(RedisCommands.SRandMemberBytes(key));
        }

        /// <summary>
        /// Get one or more random members from a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>One or more random elements from set</returns>
        public virtual async Task<string[]> SRandMembersAsync(string key, long count)
        {
            return await WriteAsync(RedisCommands.SRandMembers(key, count));
        }

        public virtual async Task<byte[][]> SRandMembersBytesAsync(string key, long count)
        {
            return await WriteAsync(RedisCommands.SRandMembersBytes(key, count));
        }

        /// <summary>
        /// Remove one or more members from a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="members">Set members to remove</param>
        /// <returns>Number of elements removed from set</returns>
        public virtual async Task<long> SRemAsync(string key, params object[] members)
        {
            return await WriteAsync(RedisCommands.SRem(key, members));
        }

        /// <summary>
        /// Add multiple sets
        /// </summary>
        /// <param name="keys">Set keys to union</param>
        /// <returns>Array of elements in resulting set</returns>
        public virtual async Task<string[]> SUnionAsync(params string[] keys)
        {
            return await WriteAsync(RedisCommands.SUnion(keys));
        }

        public virtual async Task<byte[][]> SUnionBytesAsync(params string[] keys)
        {
            return await WriteAsync(RedisCommands.SUnionBytes(keys));
        }

        /// <summary>
        /// Add multiple sets and store the resulting set in a key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="keys">Set keys to union</param>
        /// <returns>Number of elements in resulting set</returns>
        public virtual async Task<long> SUnionStoreAsync(string destination, params string[] keys)
        {
            return await WriteAsync(RedisCommands.SUnionStore(destination, keys));
        }

        /// <summary>
        /// Iterate the elements of a set field
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="cursor">The cursor returned by the server in the previous call, or 0 if this is the first call</param>
        /// <param name="pattern">Glob-style pattern to filter returned elements</param>
        /// <param name="count">Maximum number of elements to return</param>
        /// <returns>Updated cursor and result set</returns>
        public virtual async Task<RedisScan<string>> SScanAsync(string key, long cursor, string pattern = null, long? count = null)
        {
            return await WriteAsync(RedisCommands.SScan(key, cursor, pattern, count));
        }

        public virtual async Task<RedisScan<byte[]>> SScanBytesAsync(string key, long cursor, string pattern = null, long? count = null)
        {
            return await WriteAsync(RedisCommands.SScanBytes(key, cursor, pattern, count));
        }

        #endregion //end Async

#endif

    }
}
