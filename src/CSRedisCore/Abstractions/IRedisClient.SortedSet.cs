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
    public interface IRedisClientUseSortedSetCommands
    {

        #region Sorted Sets
        /// <summary>
        /// Add one or more members to a sorted set, or update its score if it already exists
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="scoreMembers">Array of member scores to add to sorted set</param>
        /// <returns>Number of elements added to the sorted set (not including member updates);</returns>
        long ZAdd<TScore, TMember>(string key, params Tuple<TScore, TMember>[] scoreMembers);




        /// <summary>
        /// Add one or more members to a sorted set, or update its score if it already exists
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="scoreMembers">Array of member scores [s1, m1, s2, m2, ..]</param>
        /// <returns>Number of elements added to the sorted set (not including member updates);</returns>
        long ZAdd(string key, params object[] scoreMembers);




        /// <summary>
        /// Get the number of members in a sorted set
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <returns>Number of elements in the sorted set</returns>
        long ZCard(string key);




        /// <summary>
        /// Count the members in a sorted set with scores within the given values
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score</param>
        /// <param name="max">Maximum score</param>
        /// <param name="exclusiveMin">Minimum score is exclusive</param>
        /// <param name="exclusiveMax">Maximum score is exclusive</param>
        /// <returns>Number of elements in the specified score range</returns>
        long ZCount(string key, decimal min, decimal max, bool exclusiveMin = false, bool exclusiveMax = false);




        /// <summary>
        /// Count the members in a sorted set with scores within the given values
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score</param>
        /// <param name="max">Maximum score</param>
        /// <returns>Number of elements in the specified score range</returns>
        long ZCount(string key, string min, string max);




        /// <summary>
        /// Increment the score of a member in a sorted set
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="increment">Increment by value</param>
        /// <param name="member">Sorted set member to increment</param>
        /// <returns>New score of member</returns>
        decimal ZIncrBy(string key, decimal increment, object member);




        /// <summary>
        /// Intersect multiple sorted sets and store the resulting set in a new key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="weights">Multiplication factor for each input set</param>
        /// <param name="aggregate">Aggregation function of resulting set</param>
        /// <param name="keys">Sorted set keys to intersect</param>
        /// <returns>Number of elements in the resulting sorted set</returns>
        long ZInterStore(string destination, decimal[] weights = null, RedisAggregate? aggregate = null, params string[] keys);




        /// <summary>
        /// Intersect multiple sorted sets and store the resulting set in a new key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="keys">Sorted set keys to intersect</param>
        /// <returns>Number of elements in the resulting sorted set</returns>
        long ZInterStore(string destination, params string[] keys);




        /// <summary>
        ///
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <param name="withScores">Include scores in result</param>
        /// <returns>Array of elements in the specified range (with optional scores);</returns>
        string[] ZRange(string key, long start, long stop, bool withScores = false);
        byte[][] ZRangeBytes(string key, long start, long stop, bool withScores = false);




        /// <summary>
        ///
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <returns>Array of elements in the specified range with scores</returns>
        Tuple<string, decimal>[] ZRangeWithScores(string key, long start, long stop);
        Tuple<byte[], decimal>[] ZRangeBytesWithScores(string key, long start, long stop);



        /// <summary>
        ///
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score</param>
        /// <param name="max">Maximum score</param>
        /// <param name="withScores">Include scores in result</param>
        /// <param name="exclusiveMin">Minimum score is exclusive</param>
        /// <param name="exclusiveMax">Maximum score is exclusive</param>
        /// <param name="offset">Start offset</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>List of elements in the specified range (with optional scores);</returns>
        string[] ZRangeByScore(string key, decimal min, decimal max, bool withScores = false, bool exclusiveMin = false, bool exclusiveMax = false, long? offset = null, long? count = null);
        byte[][] ZRangeBytesByScore(string key, decimal min, decimal max, bool withScores = false, bool exclusiveMin = false, bool exclusiveMax = false, long? offset = null, long? count = null);



        /// <summary>
        ///
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score</param>
        /// <param name="max">Maximum score</param>
        /// <param name="withScores">Include scores in result</param>
        /// <param name="offset">Start offset</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>List of elements in the specified range (with optional scores);</returns>
        string[] ZRangeByScore(string key, string min, string max, bool withScores = false, long? offset = null, long? count = null);
        byte[][] ZRangeBytesByScore(string key, string min, string max, bool withScores = false, long? offset = null, long? count = null);



        /// <summary>
        ///
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score</param>
        /// <param name="max">Maximum score</param>
        /// <param name="exclusiveMin">Minimum score is exclusive</param>
        /// <param name="exclusiveMax">Maximum score is exclusive</param>
        /// <param name="offset">Start offset</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>List of elements in the specified range (with optional scores);</returns>
        Tuple<string, decimal>[] ZRangeByScoreWithScores(string key, decimal min, decimal max, bool exclusiveMin = false, bool exclusiveMax = false, long? offset = null, long? count = null);
        Tuple<byte[], decimal>[] ZRangeBytesByScoreWithScores(string key, decimal min, decimal max, bool exclusiveMin = false, bool exclusiveMax = false, long? offset = null, long? count = null);



        /// <summary>
        ///
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score</param>
        /// <param name="max">Maximum score</param>
        /// <param name="offset">Start offset</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>List of elements in the specified range (with optional scores);</returns>
        Tuple<string, decimal>[] ZRangeByScoreWithScores(string key, string min, string max, long? offset = null, long? count = null);
        Tuple<byte[], decimal>[] ZRangeBytesByScoreWithScores(string key, string min, string max, long? offset = null, long? count = null);



        /// <summary>
        /// Determine the index of a member in a sorted set
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="member">Member to lookup</param>
        /// <returns>Rank of member or null if key does not exist</returns>
        long? ZRank(string key, object member);




        /// <summary>
        /// Remove one or more members from a sorted set
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="members">Members to remove</param>
        /// <returns>Number of elements removed</returns>
        long ZRem(string key, params object[] members);




        /// <summary>
        /// Remove all members in a sorted set within the given indexes
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <returns>Number of elements removed</returns>
        long ZRemRangeByRank(string key, long start, long stop);




        /// <summary>
        /// Remove all members in a sorted set within the given scores
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score</param>
        /// <param name="max">Maximum score</param>
        /// <param name="exclusiveMin">Minimum score is exclusive</param>
        /// <param name="exclusiveMax">Maximum score is exclusive</param>
        /// <returns>Number of elements removed</returns>
        long ZRemRangeByScore(string key, decimal min, decimal max, bool exclusiveMin = false, bool exclusiveMax = false);
        long ZRemRangeByScore(string key, string min, string max);




        /// <summary>
        ///
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <param name="withScores">Include scores in result</param>
        /// <returns>List of elements in the specified range (with optional scores);</returns>
        string[] ZRevRange(string key, long start, long stop, bool withScores = false);
        byte[][] ZRevRangeBytes(string key, long start, long stop, bool withScores = false);



        /// <summary>
        ///
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <returns>List of elements in the specified range (with optional scores);</returns>
        Tuple<string, decimal>[] ZRevRangeWithScores(string key, long start, long stop);
        Tuple<byte[], decimal>[] ZRevRangeBytesWithScores(string key, long start, long stop);




        /// <summary>
        ///
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="max">Maximum score</param>
        /// <param name="min">Minimum score</param>
        /// <param name="withScores">Include scores in result</param>
        /// <param name="exclusiveMax">Maximum score is exclusive</param>
        /// <param name="exclusiveMin">Minimum score is exclusive</param>
        /// <param name="offset">Start offset</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>List of elements in the specified score range (with optional scores);</returns>
        string[] ZRevRangeByScore(string key, decimal max, decimal min, bool withScores = false, bool exclusiveMax = false, bool exclusiveMin = false, long? offset = null, long? count = null);
        byte[][] ZRevRangeBytesByScore(string key, decimal max, decimal min, bool withScores = false, bool exclusiveMax = false, bool exclusiveMin = false, long? offset = null, long? count = null);



        /// <summary>
        ///
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="max">Maximum score</param>
        /// <param name="min">Minimum score</param>
        /// <param name="withScores">Include scores in result</param>
        /// <param name="offset">Start offset</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>List of elements in the specified score range (with optional scores);</returns>
        string[] ZRevRangeByScore(string key, string max, string min, bool withScores = false, long? offset = null, long? count = null);
        byte[][] ZRevRangeBytesByScore(string key, string max, string min, bool withScores = false, long? offset = null, long? count = null);



        /// <summary>
        ///
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="max">Maximum score</param>
        /// <param name="min">Minimum score</param>
        /// <param name="exclusiveMax">Maximum score is exclusive</param>
        /// <param name="exclusiveMin">Minimum score is exclusive</param>
        /// <param name="offset">Start offset</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>List of elements in the specified score range (with optional scores);</returns>
        Tuple<string, decimal>[] ZRevRangeByScoreWithScores(string key, decimal max, decimal min, bool exclusiveMax = false, bool exclusiveMin = false, long? offset = null, long? count = null);
        Tuple<byte[], decimal>[] ZRevRangeBytesByScoreWithScores(string key, decimal max, decimal min, bool exclusiveMax = false, bool exclusiveMin = false, long? offset = null, long? count = null);



        /// <summary>
        ///
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="max">Maximum score</param>
        /// <param name="min">Minimum score</param>
        /// <param name="offset">Start offset</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>List of elements in the specified score range (with optional scores);</returns>
        Tuple<string, decimal>[] ZRevRangeByScoreWithScores(string key, string max, string min, long? offset = null, long? count = null);
        Tuple<byte[], decimal>[] ZRevRangeBytesByScoreWithScores(string key, string max, string min, long? offset = null, long? count = null);



        /// <summary>
        /// Determine the index of a member in a sorted set, with scores ordered from high to low
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="member">Member to lookup</param>
        /// <returns>Rank of member, or null if member does not exist</returns>
        long? ZRevRank(string key, object member);




        /// <summary>
        /// Get the score associated with the given member in a sorted set
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="member">Member to lookup</param>
        /// <returns>Score of member, or null if member does not exist</returns>
        decimal? ZScore(string key, object member);




        /// <summary>
        /// Add multiple sorted sets and store the resulting sorted set in a new key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="weights">Multiplication factor for each input set</param>
        /// <param name="aggregate">Aggregation function of resulting set</param>
        /// <param name="keys">Sorted set keys to union</param>
        /// <returns>Number of elements in the resulting sorted set</returns>
        long ZUnionStore(string destination, decimal[] weights = null, RedisAggregate? aggregate = null, params string[] keys);




        /// <summary>
        /// Add multiple sorted sets and store the resulting sorted set in a new key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="keys">Sorted set keys to union</param>
        /// <returns>Number of elements in the resulting sorted set</returns>
        long ZUnionStore(string destination, params string[] keys);




        /// <summary>
        /// Iterate the scores and elements of a sorted set field
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="cursor">The cursor returned by the server in the previous call, or 0 if this is the first call</param>
        /// <param name="pattern">Glob-style pattern to filter returned elements</param>
        /// <param name="count">Maximum number of elements to return</param>
        /// <returns>Updated cursor and result set</returns>
        RedisScan<Tuple<string, decimal>> ZScan(string key, long cursor, string pattern = null, long? count = null);
        RedisScan<Tuple<byte[], decimal>> ZScanBytes(string key, long cursor, string pattern = null, long? count = null);



        /// <summary>
        /// Retrieve all the elements in a sorted set with a value between min and max
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Lexagraphic start value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <param name="max">Lexagraphic stop value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <param name="offset">Limit result set by offset</param>
        /// <param name="count">Limimt result set by size</param>
        /// <returns>List of elements in the specified range</returns>
        string[] ZRangeByLex(string key, string min, string max, long? offset = null, long? count = null);
        byte[][] ZRangeBytesByLex(string key, string min, string max, long? offset = null, long? count = null);



        /// <summary>
        /// Remove all elements in the sorted set with a value between min and max
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Lexagraphic start value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <param name="max">Lexagraphic stop value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <returns>Number of elements removed</returns>
        long ZRemRangeByLex(string key, string min, string max);




        /// <summary>
        /// Returns the number of elements in the sorted set with a value between min and max.
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Lexagraphic start value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <param name="max">Lexagraphic stop value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <returns>Number of elements in the specified score range</returns>
        long ZLexCount(string key, string min, string max);



        #endregion

#if !net40

        #region Sorted Sets
        /// <summary>
        /// Add one or more members to a sorted set, or update its score if it already exists
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="scoreMembers">Array of member scores to add to sorted set</param>
        /// <returns>Number of elements added to the sorted set (not including member updates)</returns>
        Task<long> ZAddAsync<TScore, TMember>(string key, params Tuple<TScore, TMember>[] scoreMembers);




        /// <summary>
        /// Add one or more members to a sorted set, or update its score if it already exists
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="scoreMembers">Array of member scores [s1, m1, s2, m2, ..]</param>
        /// <returns>Number of elements added to the sorted set (not including member updates)</returns>
        Task<long> ZAddAsync(string key, params object[] scoreMembers);




        /// <summary>
        /// Get the number of members in a sorted set
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <returns>Number of elements in the sorted set</returns>
        Task<long> ZCardAsync(string key);




        /// <summary>
        /// Count the members in a sorted set with scores within the given values
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score</param>
        /// <param name="max">Maximum score</param>
        /// <param name="exclusiveMin">Minimum score is exclusive</param>
        /// <param name="exclusiveMax">Maximum score is exclusive</param>
        /// <returns>Number of elements in the specified score range</returns>
        Task<long> ZCountAsync(string key, decimal min, decimal max, bool exclusiveMin = false, bool exclusiveMax = false);




        /// <summary>
        /// Count the members in a sorted set with scores within the given values
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score</param>
        /// <param name="max">Maximum score</param>
        /// <returns>Number of elements in the specified score range</returns>
        Task<long> ZCountAsync(string key, string min, string max);




        /// <summary>
        /// Increment the score of a member in a sorted set
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="increment">Increment by value</param>
        /// <param name="member">Sorted set member to increment</param>
        /// <returns>New score of member</returns>
        Task<decimal> ZIncrByAsync(string key, decimal increment, object member);




        /// <summary>
        /// Intersect multiple sorted sets and store the resulting set in a new key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="weights">Multiplication factor for each input set</param>
        /// <param name="aggregate">Aggregation function of resulting set</param>
        /// <param name="keys">Sorted set keys to intersect</param>
        /// <returns>Number of elements in the resulting sorted set</returns>
        Task<long> ZInterStoreAsync(string destination, decimal[] weights = null, RedisAggregate? aggregate = null, params string[] keys);




        /// <summary>
        /// Intersect multiple sorted sets and store the resulting set in a new key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="keys">Sorted set keys to intersect</param>
        /// <returns>Number of elements in the resulting sorted set</returns>
        Task<long> ZInterStoreAsync(string destination, params string[] keys);




        /// <summary>
        /// Return a range of members in a sorted set, by index
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <param name="withScores">Include scores in result</param>
        /// <returns>Array of elements in the specified range (with optional scores)</returns>
        Task<string[]> ZRangeAsync(string key, long start, long stop, bool withScores = false);
        Task<byte[][]> ZRangeBytesAsync(string key, long start, long stop, bool withScores = false);




        /// <summary>
        /// Return a range of members in a sorted set, by index, with scores
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <returns>Array of elements in the specified range with scores</returns>
        Task<Tuple<string, decimal>[]> ZRangeWithScoresAsync(string key, long start, long stop);
        Task<Tuple<byte[], decimal>[]> ZRangeBytesWithScoresAsync(string key, long start, long stop);



        /// <summary>
        /// Return a range of members in a sorted set, by score
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score</param>
        /// <param name="max">Maximum score</param>
        /// <param name="withScores">Include scores in result</param>
        /// <param name="exclusiveMin">Minimum score is exclusive</param>
        /// <param name="exclusiveMax">Maximum score is exclusive</param>
        /// <param name="offset">Start offset</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>List of elements in the specified range (with optional scores)</returns>
        Task<string[]> ZRangeByScoreAsync(string key, decimal min, decimal max, bool withScores = false, bool exclusiveMin = false, bool exclusiveMax = false, long? offset = null, long? count = null);
        Task<byte[][]> ZRangeBytesByScoreAsync(string key, decimal min, decimal max, bool withScores = false, bool exclusiveMin = false, bool exclusiveMax = false, long? offset = null, long? count = null);



        /// <summary>
        /// Return a range of members in a sorted set, by score
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score</param>
        /// <param name="max">Maximum score</param>
        /// <param name="withScores">Include scores in result</param>
        /// <param name="offset">Start offset</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>List of elements in the specified range (with optional scores)</returns>
        Task<string[]> ZRangeByScoreAsync(string key, string min, string max, bool withScores = false, long? offset = null, long? count = null);
        Task<byte[][]> ZRangeBytesByScoreAsync(string key, string min, string max, bool withScores = false, long? offset = null, long? count = null);



        /// <summary>
        /// Return a range of members in a sorted set, by score, with scores
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score</param>
        /// <param name="max">Maximum score</param>
        /// <param name="exclusiveMin">Minimum score is exclusive</param>
        /// <param name="exclusiveMax">Maximum score is exclusive</param>
        /// <param name="offset">Start offset</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>List of elements in the specified range (with optional scores)</returns>
        Task<Tuple<string, decimal>[]> ZRangeByScoreWithScoresAsync(string key, decimal min, decimal max, bool exclusiveMin = false, bool exclusiveMax = false, long? offset = null, long? count = null);
        Task<Tuple<byte[], decimal>[]> ZRangeBytesByScoreWithScoresAsync(string key, decimal min, decimal max, bool exclusiveMin = false, bool exclusiveMax = false, long? offset = null, long? count = null);



        /// <summary>
        /// Return a range of members in a sorted set, by score, with scores
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score</param>
        /// <param name="max">Maximum score</param>
        /// <param name="offset">Start offset</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>List of elements in the specified range (with optional scores)</returns>
        Task<Tuple<string, decimal>[]> ZRangeByScoreWithScoresAsync(string key, string min, string max, long? offset = null, long? count = null);
        Task<Tuple<byte[], decimal>[]> ZRangeBytesByScoreWithScoresAsync(string key, string min, string max, long? offset = null, long? count = null);



        /// <summary>
        /// Determine the index of a member in a sorted set
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="member">Member to lookup</param>
        /// <returns>Rank of member or null if key does not exist</returns>
        Task<long?> ZRankAsync(string key, object member);




        /// <summary>
        /// Remove one or more members from a sorted set
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="members">Members to remove</param>
        /// <returns>Number of elements removed</returns>
        Task<long> ZRemAsync(string key, params object[] members);




        /// <summary>
        /// Remove all members in a sorted set within the given indexes
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <returns>Number of elements removed</returns>
        Task<long> ZRemRangeByRankAsync(string key, long start, long stop);




        /// <summary>
        /// Remove all members in a sorted set within the given scores
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score</param>
        /// <param name="max">Maximum score</param>
        /// <param name="exclusiveMin">Minimum score is exclusive</param>
        /// <param name="exclusiveMax">Maximum score is exclusive</param>
        /// <returns>Number of elements removed</returns>
        Task<long> ZRemRangeByScoreAsync(string key, decimal min, decimal max, bool exclusiveMin = false, bool exclusiveMax = false);
        Task<long> ZRemRangeByScoreAsync(string key, string min, string max);




        /// <summary>
        /// Return a range of members in a sorted set, by index, with scores ordered from high to low
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <param name="withScores">Include scores in result</param>
        /// <returns>List of elements in the specified range (with optional scores)</returns>
        Task<string[]> ZRevRangeAsync(string key, long start, long stop, bool withScores = false);
        Task<byte[][]> ZRevRangeBytesAsync(string key, long start, long stop, bool withScores = false);



        /// <summary>
        /// Return a range of members in a sorted set, by index, with scores ordered from high to low
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <returns>List of elements in the specified range (with optional scores)</returns>
        Task<Tuple<string, decimal>[]> ZRevRangeWithScoresAsync(string key, long start, long stop);
        Task<Tuple<byte[], decimal>[]> ZRevRangeBytesWithScoresAsync(string key, long start, long stop);



        /// <summary>
        /// Return a range of members in a sorted set, by score, with scores ordered from high to low
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="max">Maximum score</param>
        /// <param name="min">Minimum score</param>
        /// <param name="withScores">Include scores in result</param>
        /// <param name="exclusiveMax">Maximum score is exclusive</param>
        /// <param name="exclusiveMin">Minimum score is exclusive</param>
        /// <param name="offset">Start offset</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>List of elements in the specified score range (with optional scores)</returns>
        Task<string[]> ZRevRangeByScoreAsync(string key, decimal max, decimal min, bool withScores = false, bool exclusiveMax = false, bool exclusiveMin = false, long? offset = null, long? count = null);
        Task<byte[][]> ZRevRangeBytesByScoreAsync(string key, decimal max, decimal min, bool withScores = false, bool exclusiveMax = false, bool exclusiveMin = false, long? offset = null, long? count = null);



        /// <summary>
        /// Return a range of members in a sorted set, by score, with scores ordered from high to low
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="max">Maximum score</param>
        /// <param name="min">Minimum score</param>
        /// <param name="withScores">Include scores in result</param>
        /// <param name="offset">Start offset</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>List of elements in the specified score range (with optional scores)</returns>
        Task<string[]> ZRevRangeByScoreAsync(string key, string max, string min, bool withScores = false, long? offset = null, long? count = null);
        Task<byte[][]> ZRevRangeBytesByScoreAsync(string key, string max, string min, bool withScores = false, long? offset = null, long? count = null);



        /// <summary>
        /// Return a range of members in a sorted set, by score, with scores ordered from high to low
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="max">Maximum score</param>
        /// <param name="min">Minimum score</param>
        /// <param name="exclusiveMax">Maximum score is exclusive</param>
        /// <param name="exclusiveMin">Minimum score is exclusive</param>
        /// <param name="offset">Start offset</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>List of elements in the specified score range (with optional scores)</returns>
        Task<Tuple<string, decimal>[]> ZRevRangeByScoreWithScoresAsync(string key, decimal max, decimal min, bool exclusiveMax = false, bool exclusiveMin = false, long? offset = null, long? count = null);
        Task<Tuple<byte[], decimal>[]> ZRevRangeBytesByScoreWithScoresAsync(string key, decimal max, decimal min, bool exclusiveMax = false, bool exclusiveMin = false, long? offset = null, long? count = null);



        /// <summary>
        /// Return a range of members in a sorted set, by score, with scores ordered from high to low
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="max">Maximum score</param>
        /// <param name="min">Minimum score</param>
        /// <param name="offset">Start offset</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>List of elements in the specified score range (with optional scores)</returns>
        Task<Tuple<string, decimal>[]> ZRevRangeByScoreWithScoresAsync(string key, string max, string min, long? offset = null, long? count = null);
        Task<Tuple<byte[], decimal>[]> ZRevRangeBytesByScoreWithScoresAsync(string key, string max, string min, long? offset = null, long? count = null);



        /// <summary>
        /// Determine the index of a member in a sorted set, with scores ordered from high to low
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="member">Member to lookup</param>
        /// <returns>Rank of member, or null if member does not exist</returns>
        Task<long?> ZRevRankAsync(string key, object member);




        /// <summary>
        /// Get the score associated with the given member in a sorted set
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="member">Member to lookup</param>
        /// <returns>Score of member, or null if member does not exist</returns>
        Task<decimal?> ZScoreAsync(string key, object member);




        /// <summary>
        /// Add multiple sorted sets and store the resulting sorted set in a new key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="weights">Multiplication factor for each input set</param>
        /// <param name="aggregate">Aggregation function of resulting set</param>
        /// <param name="keys">Sorted set keys to union</param>
        /// <returns>Number of elements in the resulting sorted set</returns>
        Task<long> ZUnionStoreAsync(string destination, decimal[] weights = null, RedisAggregate? aggregate = null, params string[] keys);




        /// <summary>
        /// Iterate the scores and elements of a sorted set field
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="cursor">The cursor returned by the server in the previous call, or 0 if this is the first call</param>
        /// <param name="pattern">Glob-style pattern to filter returned elements</param>
        /// <param name="count">Maximum number of elements to return</param>
        /// <returns>Updated cursor and result set</returns>
        Task<RedisScan<Tuple<string, decimal>>> ZScanAsync(string key, long cursor, string pattern = null, long? count = null);
        Task<RedisScan<Tuple<byte[], decimal>>> ZScanBytesAsync(string key, long cursor, string pattern = null, long? count = null);



        /// <summary>
        /// Retrieve all the elements in a sorted set with a value between min and max
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Lexagraphic start value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <param name="max">Lexagraphic stop value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <param name="offset">Limit result set by offset</param>
        /// <param name="count">Limimt result set by size</param>
        /// <returns>List of elements in the specified range</returns>
        Task<string[]> ZRangeByLexAsync(string key, string min, string max, long? offset = null, long? count = null);
        Task<byte[][]> ZRangeBytesByLexAsync(string key, string min, string max, long? offset = null, long? count = null);



        /// <summary>
        /// Remove all elements in the sorted set with a value between min and max
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Lexagraphic start value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <param name="max">Lexagraphic stop value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <returns>Number of elements removed</returns>
        Task<long> ZRemRangeByLexAsync(string key, string min, string max);




        /// <summary>
        /// Returns the number of elements in the sorted set with a value between min and max.
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Lexagraphic start value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <param name="max">Lexagraphic stop value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <returns>Number of elements in the specified score range</returns>
        Task<long> ZLexCountAsync(string key, string min, string max);



        #endregion

#endif

    }
}
