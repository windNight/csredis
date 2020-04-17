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
    public interface IRedisClientUseSetCommands
    {
        #region Sets
        /// <summary>
        /// Add one or more members to a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="members">Members to add to set</param>
        /// <returns>Number of elements added to set</returns>
        long SAdd(string key, params object[] members);

        /// <summary>
        /// Get the number of members in a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <returns>Number of elements in set</returns>
        long SCard(string key);

        /// <summary>
        /// Subtract multiple sets
        /// </summary>
        /// <param name="keys">Set keys to subtract</param>
        /// <returns>Array of elements in resulting set</returns>
        string[] SDiff(params string[] keys);
        byte[][] SDiffBytes(params string[] keys);

        /// <summary>
        /// Subtract multiple sets and store the resulting set in a key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="keys">Set keys to subtract</param>
        /// <returns>Number of elements in the resulting set</returns>
        long SDiffStore(string destination, params string[] keys);


        /// <summary>
        /// Intersect multiple sets
        /// </summary>
        /// <param name="keys">Set keys to intersect</param>
        /// <returns>Array of elements in resulting set</returns>
        string[] SInter(params string[] keys);
        byte[][] SInterBytes(params string[] keys);



        /// <summary>
        /// Intersect multiple sets and store the resulting set in a key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="keys">Set keys to intersect</param>
        /// <returns>Number of elements in resulting set</returns>
        long SInterStore(string destination, params string[] keys);




        /// <summary>
        /// Determine if a given value is a member of a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="member">Member to lookup</param>
        /// <returns>True if member exists in set</returns>
        bool SIsMember(string key, object member);




        /// <summary>
        /// Get all the members in a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <returns>All elements in the set</returns>
        string[] SMembers(string key);
        byte[][] SMembersBytes(string key);



        /// <summary>
        /// Move a member from one set to another
        /// </summary>
        /// <param name="source">Source key</param>
        /// <param name="destination">Destination key</param>
        /// <param name="member">Member to move</param>
        /// <returns>True if element was moved</returns>
        bool SMove(string source, string destination, object member);




        /// <summary>
        /// Remove and
        /// </summary>
        /// <param name="key">Set key</param>
        /// <returns>The removed element</returns>
        string SPop(string key);
        byte[] SPopBytes(string key);

        /// <summary>
        /// Remove and return one or more random member from a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="count">Number of elements to remove and return</param>
        /// <returns>The removed elements</returns>
        string[] SPop(string key, long count);
        byte[][] SPopBytes(string key, long count);

        /// <summary>
        /// Get a random member from a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <returns>One random element from set</returns>
        string SRandMember(string key);
        byte[] SRandMemberBytes(string key);



        /// <summary>
        /// Get one or more random members from a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>One or more random elements from set</returns>
        string[] SRandMembers(string key, long count);
        byte[][] SRandMembersBytes(string key, long count);



        /// <summary>
        /// Remove one or more members from a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="members">Set members to remove</param>
        /// <returns>Number of elements removed from set</returns>
        long SRem(string key, params object[] members);




        /// <summary>
        /// Add multiple sets
        /// </summary>
        /// <param name="keys">Set keys to union</param>
        /// <returns>Array of elements in resulting set</returns>
        string[] SUnion(params string[] keys);
        byte[][] SUnionBytes(params string[] keys);



        /// <summary>
        /// Add multiple sets and store the resulting set in a key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="keys">Set keys to union</param>
        /// <returns>Number of elements in resulting set</returns>
        long SUnionStore(string destination, params string[] keys);




        /// <summary>
        /// Iterate the elements of a set field
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="cursor">The cursor returned by the server in the previous call, or 0 if this is the first call</param>
        /// <param name="pattern">Glob-style pattern to filter returned elements</param>
        /// <param name="count">Maximum number of elements to return</param>
        /// <returns>Updated cursor and result set</returns>
        RedisScan<string> SScan(string key, long cursor, string pattern = null, long? count = null);
        RedisScan<byte[]> SScanBytes(string key, long cursor, string pattern = null, long? count = null);


        #endregion


#if !net40

        #region Sets
        /// <summary>
        /// Add one or more members to a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="members">Members to add to set</param>
        /// <returns>Number of elements added to set</returns>
        Task<long> SAddAsync(string key, params object[] members);




        /// <summary>
        /// Get the number of members in a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <returns>Number of elements in set</returns>
        Task<long> SCardAsync(string key);




        /// <summary>
        /// Subtract multiple sets
        /// </summary>
        /// <param name="keys">Set keys to subtract</param>
        /// <returns>Array of elements in resulting set</returns>
        Task<string[]> SDiffAsync(params string[] keys);
        Task<byte[][]> SDiffBytesAsync(params string[] keys);



        /// <summary>
        /// Subtract multiple sets and store the resulting set in a key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="keys">Set keys to subtract</param>
        /// <returns>Number of elements in the resulting set</returns>
        Task<long> SDiffStoreAsync(string destination, params string[] keys);




        /// <summary>
        /// Intersect multiple sets
        /// </summary>
        /// <param name="keys">Set keys to intersect</param>
        /// <returns>Array of elements in resulting set</returns>
        Task<string[]> SInterAsync(params string[] keys);
        Task<byte[][]> SInterBytesAsync(params string[] keys);



        /// <summary>
        /// Intersect multiple sets and store the resulting set in a key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="keys">Set keys to intersect</param>
        /// <returns>Number of elements in resulting set</returns>
        Task<long> SInterStoreAsync(string destination, params string[] keys);




        /// <summary>
        /// Determine if a given value is a member of a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="member">Member to lookup</param>
        /// <returns>True if member exists in set</returns>
        Task<bool> SIsMemberAsync(string key, object member);




        /// <summary>
        /// Get all the members in a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <returns>All elements in the set</returns>
        Task<string[]> SMembersAsync(string key);
        Task<byte[][]> SMembersBytesAsync(string key);



        /// <summary>
        /// Move a member from one set to another
        /// </summary>
        /// <param name="source">Source key</param>
        /// <param name="destination">Destination key</param>
        /// <param name="member">Member to move</param>
        /// <returns>True if element was moved</returns>
        Task<bool> SMoveAsync(string source, string destination, object member);




        /// <summary>
        /// Remove and return a random member from a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <returns>The removed element</returns>
        Task<string> SPopAsync(string key);
        Task<byte[]> SPopBytesAsync(string key);


        /// <summary>
        /// Remove and return one or more random members from a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="count">Number of elements to remove and return</param>
        /// <returns></returns>
        Task<string[]> SPopAsync(string key, long count);
        Task<byte[][]> SPopBytesAsync(string key, long count);


        /// <summary>
        /// Get a random member from a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <returns>One random element from set</returns>
        Task<string> SRandMemberAsync(string key);
        Task<byte[]> SRandMemberBytesAsync(string key);



        /// <summary>
        /// Get one or more random members from a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>One or more random elements from set</returns>
        Task<string[]> SRandMembersAsync(string key, long count);
        Task<byte[][]> SRandMembersBytesAsync(string key, long count);



        /// <summary>
        /// Remove one or more members from a set
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="members">Set members to remove</param>
        /// <returns>Number of elements removed from set</returns>
        Task<long> SRemAsync(string key, params object[] members);




        /// <summary>
        /// Add multiple sets
        /// </summary>
        /// <param name="keys">Set keys to union</param>
        /// <returns>Array of elements in resulting set</returns>
        Task<string[]> SUnionAsync(params string[] keys);
        Task<byte[][]> SUnionBytesAsync(params string[] keys);



        /// <summary>
        /// Add multiple sets and store the resulting set in a key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="keys">Set keys to union</param>
        /// <returns>Number of elements in resulting set</returns>
        Task<long> SUnionStoreAsync(string destination, params string[] keys);




        /// <summary>
        /// Iterate the elements of a set field
        /// </summary>
        /// <param name="key">Set key</param>
        /// <param name="cursor">The cursor returned by the server in the previous call, or 0 if this is the first call</param>
        /// <param name="pattern">Glob-style pattern to filter returned elements</param>
        /// <param name="count">Maximum number of elements to return</param>
        /// <returns>Updated cursor and result set</returns>
        Task<RedisScan<string>> SScanAsync(string key, long cursor, string pattern = null, long? count = null);
        Task<RedisScan<byte[]>> SScanBytesAsync(string key, long cursor, string pattern = null, long? count = null);


        #endregion

#endif

    }
}
