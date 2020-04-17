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

        public virtual Tuple<string, decimal>[] ZPopMax(string key, long count)
        {
            return Write(RedisCommands.ZPopMax(key, count));
        }
        public virtual Tuple<byte[], decimal>[] ZPopMaxBytes(string key, long count)
        {
            return Write(RedisCommands.ZPopMaxBytes(key, count));
        }
        public virtual Tuple<string, decimal>[] ZPopMin(string key, long count)
        {
            return Write(RedisCommands.ZPopMin(key, count));
        }
        public virtual Tuple<byte[], decimal>[] ZPopMinBytes(string key, long count)
        {
            return Write(RedisCommands.ZPopMinBytes(key, count));
        }

        /// <summary>
        /// Add one or more members to a sorted set, or update its score if it already exists
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="scoreMembers">Array of member scores to add to sorted set</param>
        /// <returns>Number of elements added to the sorted set (not including member updates)</returns>
        public virtual long ZAdd<TScore, TMember>(string key, params Tuple<TScore, TMember>[] scoreMembers)
        {
            return Write(RedisCommands.ZAdd(key, scoreMembers));
        }

        /// <summary>
        /// Add one or more members to a sorted set, or update its score if it already exists
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="scoreMembers">Array of member scores [s1, m1, s2, m2, ..]</param>
        /// <returns>Number of elements added to the sorted set (not including member updates)</returns>
        public virtual long ZAdd(string key, params object[] scoreMembers)
        {
            return Write(RedisCommands.ZAdd(key, scoreMembers));
        }

        /// <summary>
        /// Get the number of members in a sorted set
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <returns>Number of elements in the sorted set</returns>
        public virtual long ZCard(string key)
        {
            return Write(RedisCommands.ZCard(key));
        }

        /// <summary>
        /// Count the members in a sorted set with scores within the given values
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score</param>
        /// <param name="max">Maximum score</param>
        /// <param name="exclusiveMin">Minimum score is exclusive</param>
        /// <param name="exclusiveMax">Maximum score is exclusive</param>
        /// <returns>Number of elements in the specified score range</returns>
        public virtual long ZCount(string key, decimal min, decimal max, bool exclusiveMin = false, bool exclusiveMax = false)
        {
            return Write(RedisCommands.ZCount(key, min, max, exclusiveMin, exclusiveMax));
        }

        /// <summary>
        /// Count the members in a sorted set with scores within the given values
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score</param>
        /// <param name="max">Maximum score</param>
        /// <returns>Number of elements in the specified score range</returns>
        public virtual long ZCount(string key, string min, string max)
        {
            return Write(RedisCommands.ZCount(key, min, max));
        }

        /// <summary>
        /// Increment the score of a member in a sorted set
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="increment">Increment by value</param>
        /// <param name="member">Sorted set member to increment</param>
        /// <returns>New score of member</returns>
        public virtual decimal ZIncrBy(string key, decimal increment, object member)
        {
            return Write(RedisCommands.ZIncrBy(key, increment, member));
        }

        /// <summary>
        /// Intersect multiple sorted sets and store the resulting set in a new key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="weights">Multiplication factor for each input set</param>
        /// <param name="aggregate">Aggregation function of resulting set</param>
        /// <param name="keys">Sorted set keys to intersect</param>
        /// <returns>Number of elements in the resulting sorted set</returns>
        public virtual long ZInterStore(string destination, decimal[] weights = null, RedisAggregate? aggregate = null, params string[] keys)
        {
            return Write(RedisCommands.ZInterStore(destination, weights, aggregate, keys));
        }

        /// <summary>
        /// Intersect multiple sorted sets and store the resulting set in a new key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="keys">Sorted set keys to intersect</param>
        /// <returns>Number of elements in the resulting sorted set</returns>
        public virtual long ZInterStore(string destination, params string[] keys)
        {
            return ZInterStore(destination, null, null, keys);
        }

        /// <summary>
        /// Return a range of members in a sorted set, by index
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <param name="withScores">Include scores in result</param>
        /// <returns>Array of elements in the specified range (with optional scores)</returns>
        public virtual string[] ZRange(string key, long start, long stop, bool withScores = false)
        {
            return Write(RedisCommands.ZRange(key, start, stop, withScores));
        }
        public virtual byte[][] ZRangeBytes(string key, long start, long stop, bool withScores = false)
        {
            return Write(RedisCommands.ZRangeBytes(key, start, stop, withScores));
        }

        /// <summary>
        /// Return a range of members in a sorted set, by index, with scores
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <returns>Array of elements in the specified range with scores</returns>
        public virtual Tuple<string, decimal>[] ZRangeWithScores(string key, long start, long stop)
        {
            return Write(RedisCommands.ZRangeWithScores(key, start, stop));
        }
        public virtual Tuple<byte[], decimal>[] ZRangeBytesWithScores(string key, long start, long stop)
        {
            return Write(RedisCommands.ZRangeBytesWithScores(key, start, stop));
        }

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
        public virtual string[] ZRangeByScore(string key, decimal min, decimal max, bool withScores = false, bool exclusiveMin = false, bool exclusiveMax = false, long? offset = null, long? count = null)
        {
            return Write(RedisCommands.ZRangeByScore(key, min, max, withScores, exclusiveMin, exclusiveMax, offset, count));
        }
        public virtual byte[][] ZRangeBytesByScore(string key, decimal min, decimal max, bool withScores = false, bool exclusiveMin = false, bool exclusiveMax = false, long? offset = null, long? count = null)
        {
            return Write(RedisCommands.ZRangeBytesByScore(key, min, max, withScores, exclusiveMin, exclusiveMax, offset, count));
        }

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
        public virtual string[] ZRangeByScore(string key, string min, string max, bool withScores = false, long? offset = null, long? count = null)
        {
            return Write(RedisCommands.ZRangeByScore(key, min, max, withScores, offset, count));
        }
        public virtual byte[][] ZRangeBytesByScore(string key, string min, string max, bool withScores = false, long? offset = null, long? count = null)
        {
            return Write(RedisCommands.ZRangeBytesByScore(key, min, max, withScores, offset, count));
        }

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
        public virtual Tuple<string, decimal>[] ZRangeByScoreWithScores(string key, decimal min, decimal max, bool exclusiveMin = false, bool exclusiveMax = false, long? offset = null, long? count = null)
        {
            return Write(RedisCommands.ZRangeByScoreWithScores(key, min, max, exclusiveMin, exclusiveMax, offset, count));
        }
        public virtual Tuple<byte[], decimal>[] ZRangeBytesByScoreWithScores(string key, decimal min, decimal max, bool exclusiveMin = false, bool exclusiveMax = false, long? offset = null, long? count = null)
        {
            return Write(RedisCommands.ZRangeBytesByScoreWithScores(key, min, max, exclusiveMin, exclusiveMax, offset, count));
        }

        /// <summary>
        /// Return a range of members in a sorted set, by score, with scores
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score</param>
        /// <param name="max">Maximum score</param>
        /// <param name="offset">Start offset</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>List of elements in the specified range (with optional scores)</returns>
        public virtual Tuple<string, decimal>[] ZRangeByScoreWithScores(string key, string min, string max, long? offset = null, long? count = null)
        {
            return Write(RedisCommands.ZRangeByScoreWithScores(key, min, max, offset, count));
        }
        public virtual Tuple<byte[], decimal>[] ZRangeBytesByScoreWithScores(string key, string min, string max, long? offset = null, long? count = null)
        {
            return Write(RedisCommands.ZRangeBytesByScoreWithScores(key, min, max, offset, count));
        }

        /// <summary>
        /// Determine the index of a member in a sorted set
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="member">Member to lookup</param>
        /// <returns>Rank of member or null if key does not exist</returns>
        public virtual long? ZRank(string key, object member)
        {
            return Write(RedisCommands.ZRank(key, member));
        }

        /// <summary>
        /// Remove one or more members from a sorted set
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="members">Members to remove</param>
        /// <returns>Number of elements removed</returns>
        public virtual long ZRem(string key, params object[] members)
        {
            return Write(RedisCommands.ZRem(key, members));
        }

        /// <summary>
        /// Remove all members in a sorted set within the given indexes
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <returns>Number of elements removed</returns>
        public virtual long ZRemRangeByRank(string key, long start, long stop)
        {
            return Write(RedisCommands.ZRemRangeByRank(key, start, stop));
        }

        /// <summary>
        /// Remove all members in a sorted set within the given scores
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score</param>
        /// <param name="max">Maximum score</param>
        /// <param name="exclusiveMin">Minimum score is exclusive</param>
        /// <param name="exclusiveMax">Maximum score is exclusive</param>
        /// <returns>Number of elements removed</returns>
        public virtual long ZRemRangeByScore(string key, decimal min, decimal max, bool exclusiveMin = false, bool exclusiveMax = false)
        {
            return Write(RedisCommands.ZRemRangeByScore(key, min, max, exclusiveMin, exclusiveMax));
        }
        public virtual long ZRemRangeByScore(string key, string min, string max)
        {
            return Write(RedisCommands.ZRemRangeByScore(key, min, max));
        }

        /// <summary>
        /// Return a range of members in a sorted set, by index, with scores ordered from high to low
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <param name="withScores">Include scores in result</param>
        /// <returns>List of elements in the specified range (with optional scores)</returns>
        public virtual string[] ZRevRange(string key, long start, long stop, bool withScores = false)
        {
            return Write(RedisCommands.ZRevRange(key, start, stop, withScores));
        }
        public virtual byte[][] ZRevRangeBytes(string key, long start, long stop, bool withScores = false)
        {
            return Write(RedisCommands.ZRevRangeBytes(key, start, stop, withScores));
        }

        /// <summary>
        /// Return a range of members in a sorted set, by index, with scores ordered from high to low
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <returns>List of elements in the specified range (with optional scores)</returns>
        public virtual Tuple<string, decimal>[] ZRevRangeWithScores(string key, long start, long stop)
        {
            return Write(RedisCommands.ZRevRangeWithScores(key, start, stop));
        }
        public virtual Tuple<byte[], decimal>[] ZRevRangeBytesWithScores(string key, long start, long stop)
        {
            return Write(RedisCommands.ZRevRangeBytesWithScores(key, start, stop));
        }

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
        public virtual string[] ZRevRangeByScore(string key, decimal max, decimal min, bool withScores = false, bool exclusiveMax = false, bool exclusiveMin = false, long? offset = null, long? count = null)
        {
            return Write(RedisCommands.ZRevRangeByScore(key, max, min, withScores, exclusiveMax, exclusiveMin, offset, count));
        }
        public virtual byte[][] ZRevRangeBytesByScore(string key, decimal max, decimal min, bool withScores = false, bool exclusiveMax = false, bool exclusiveMin = false, long? offset = null, long? count = null)
        {
            return Write(RedisCommands.ZRevRangeBytesByScore(key, max, min, withScores, exclusiveMax, exclusiveMin, offset, count));
        }

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
        public virtual string[] ZRevRangeByScore(string key, string max, string min, bool withScores = false, long? offset = null, long? count = null)
        {
            return Write(RedisCommands.ZRevRangeByScore(key, max, min, withScores, offset, count));
        }
        public virtual byte[][] ZRevRangeBytesByScore(string key, string max, string min, bool withScores = false, long? offset = null, long? count = null)
        {
            return Write(RedisCommands.ZRevRangeBytesByScore(key, max, min, withScores, offset, count));
        }

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
        public virtual Tuple<string, decimal>[] ZRevRangeByScoreWithScores(string key, decimal max, decimal min, bool exclusiveMax = false, bool exclusiveMin = false, long? offset = null, long? count = null)
        {
            return Write(RedisCommands.ZRevRangeByScoreWithScores(key, max, min, exclusiveMax, exclusiveMin, offset, count));
        }
        public virtual Tuple<byte[], decimal>[] ZRevRangeBytesByScoreWithScores(string key, decimal max, decimal min, bool exclusiveMax = false, bool exclusiveMin = false, long? offset = null, long? count = null)
        {
            return Write(RedisCommands.ZRevRangeBytesByScoreWithScores(key, max, min, exclusiveMax, exclusiveMin, offset, count));
        }

        /// <summary>
        /// Return a range of members in a sorted set, by score, with scores ordered from high to low
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="max">Maximum score</param>
        /// <param name="min">Minimum score</param>
        /// <param name="offset">Start offset</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>List of elements in the specified score range (with optional scores)</returns>
        public virtual Tuple<string, decimal>[] ZRevRangeByScoreWithScores(string key, string max, string min, long? offset = null, long? count = null)
        {
            return Write(RedisCommands.ZRevRangeByScoreWithScores(key, max, min, offset, count));
        }
        public virtual Tuple<byte[], decimal>[] ZRevRangeBytesByScoreWithScores(string key, string max, string min, long? offset = null, long? count = null)
        {
            return Write(RedisCommands.ZRevRangeBytesByScoreWithScores(key, max, min, offset, count));
        }

        /// <summary>
        /// Determine the index of a member in a sorted set, with scores ordered from high to low
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="member">Member to lookup</param>
        /// <returns>Rank of member, or null if member does not exist</returns>
        public virtual long? ZRevRank(string key, object member)
        {
            return Write(RedisCommands.ZRevRank(key, member));
        }

        /// <summary>
        /// Get the score associated with the given member in a sorted set
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="member">Member to lookup</param>
        /// <returns>Score of member, or null if member does not exist</returns>
        public virtual decimal? ZScore(string key, object member)
        {
            return Write(RedisCommands.ZScore(key, member));
        }

        /// <summary>
        /// Add multiple sorted sets and store the resulting sorted set in a new key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="weights">Multiplication factor for each input set</param>
        /// <param name="aggregate">Aggregation function of resulting set</param>
        /// <param name="keys">Sorted set keys to union</param>
        /// <returns>Number of elements in the resulting sorted set</returns>
        public virtual long ZUnionStore(string destination, decimal[] weights = null, RedisAggregate? aggregate = null, params string[] keys)
        {
            return Write(RedisCommands.ZUnionStore(destination, weights, aggregate, keys));
        }

        /// <summary>
        /// Add multiple sorted sets and store the resulting sorted set in a new key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="keys">Sorted set keys to union</param>
        /// <returns>Number of elements in the resulting sorted set</returns>
        public virtual long ZUnionStore(string destination, params string[] keys)
        {
            return Write(RedisCommands.ZUnionStore(destination, null, null, keys));
        }

        /// <summary>
        /// Iterate the scores and elements of a sorted set field
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="cursor">The cursor returned by the server in the previous call, or 0 if this is the first call</param>
        /// <param name="pattern">Glob-style pattern to filter returned elements</param>
        /// <param name="count">Maximum number of elements to return</param>
        /// <returns>Updated cursor and result set</returns>
        public virtual RedisScan<Tuple<string, decimal>> ZScan(string key, long cursor, string pattern = null, long? count = null)
        {
            return Write(RedisCommands.ZScan(key, cursor, pattern, count));
        }
        public virtual RedisScan<Tuple<byte[], decimal>> ZScanBytes(string key, long cursor, string pattern = null, long? count = null)
        {
            return Write(RedisCommands.ZScanBytes(key, cursor, pattern, count));
        }

        /// <summary>
        /// Retrieve all the elements in a sorted set with a value between min and max
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Lexagraphic start value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <param name="max">Lexagraphic stop value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <param name="offset">Limit result set by offset</param>
        /// <param name="count">Limimt result set by size</param>
        /// <returns>List of elements in the specified range</returns>
        public virtual string[] ZRangeByLex(string key, string min, string max, long? offset = null, long? count = null)
        {
            return Write(RedisCommands.ZRangeByLex(key, min, max, offset, count));
        }
        public virtual byte[][] ZRangeBytesByLex(string key, string min, string max, long? offset = null, long? count = null)
        {
            return Write(RedisCommands.ZRangeBytesByLex(key, min, max, offset, count));
        }

        /// <summary>
        /// Remove all elements in the sorted set with a value between min and max
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Lexagraphic start value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <param name="max">Lexagraphic stop value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <returns>Number of elements removed</returns>
        public virtual long ZRemRangeByLex(string key, string min, string max)
        {
            return Write(RedisCommands.ZRemRangeByLex(key, min, max));
        }

        /// <summary>
        /// Returns the number of elements in the sorted set with a value between min and max.
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Lexagraphic start value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <param name="max">Lexagraphic stop value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <returns>Number of elements in the specified score range</returns>
        public virtual long ZLexCount(string key, string min, string max)
        {
            return Write(RedisCommands.ZLexCount(key, min, max));
        }

        #endregion //end Sync

#if !net40

        #region Async

        public virtual async Task<Tuple<string, decimal>[]> ZPopMaxAsync(string key, long count)
        {
            return await WriteAsync(RedisCommands.ZPopMax(key, count));
        }

        public virtual async Task<Tuple<byte[], decimal>[]> ZPopMaxBytesAsync(string key, long count)
        {
            return await WriteAsync(RedisCommands.ZPopMaxBytes(key, count));
        }

        public virtual async Task<Tuple<string, decimal>[]> ZPopMinAsync(string key, long count)
        {
            return await WriteAsync(RedisCommands.ZPopMin(key, count));
        }

        public virtual async Task<Tuple<byte[], decimal>[]> ZPopMinBytesAsync(string key, long count)
        {
            return await WriteAsync(RedisCommands.ZPopMinBytes(key, count));
        }


        /// <summary>
        /// Add one or more members to a sorted set, or update its score if it already exists
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="scoreMembers">Array of member scores to add to sorted set</param>
        /// <returns>Number of elements added to the sorted set (not including member updates)</returns>
        public virtual async Task<long> ZAddAsync<TScore, TMember>(string key, params Tuple<TScore, TMember>[] scoreMembers)
        {
            return await WriteAsync(RedisCommands.ZAdd(key, scoreMembers));
        }

        /// <summary>
        /// Add one or more members to a sorted set, or update its score if it already exists
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="scoreMembers">Array of member scores [s1, m1, s2, m2, ..]</param>
        /// <returns>Number of elements added to the sorted set (not including member updates)</returns>
        public virtual async Task<long> ZAddAsync(string key, params object[] scoreMembers)
        {
            return await WriteAsync(RedisCommands.ZAdd(key, scoreMembers));
        }

        /// <summary>
        /// Get the number of members in a sorted set
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <returns>Number of elements in the sorted set</returns>
        public virtual async Task<long> ZCardAsync(string key)
        {
            return await WriteAsync(RedisCommands.ZCard(key));
        }

        /// <summary>
        /// Count the members in a sorted set with scores within the given values
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score</param>
        /// <param name="max">Maximum score</param>
        /// <param name="exclusiveMin">Minimum score is exclusive</param>
        /// <param name="exclusiveMax">Maximum score is exclusive</param>
        /// <returns>Number of elements in the specified score range</returns>
        public virtual async Task<long> ZCountAsync(string key, decimal min, decimal max, bool exclusiveMin = false, bool exclusiveMax = false)
        {
            return await WriteAsync(RedisCommands.ZCount(key, min, max, exclusiveMin, exclusiveMax));
        }

        /// <summary>
        /// Count the members in a sorted set with scores within the given values
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score</param>
        /// <param name="max">Maximum score</param>
        /// <returns>Number of elements in the specified score range</returns>
        public virtual async Task<long> ZCountAsync(string key, string min, string max)
        {
            return await WriteAsync(RedisCommands.ZCount(key, min, max));
        }

        /// <summary>
        /// Increment the score of a member in a sorted set
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="increment">Increment by value</param>
        /// <param name="member">Sorted set member to increment</param>
        /// <returns>New score of member</returns>
        public virtual async Task<decimal> ZIncrByAsync(string key, decimal increment, object member)
        {
            return await WriteAsync(RedisCommands.ZIncrBy(key, increment, member));
        }

        /// <summary>
        /// Intersect multiple sorted sets and store the resulting set in a new key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="weights">Multiplication factor for each input set</param>
        /// <param name="aggregate">Aggregation function of resulting set</param>
        /// <param name="keys">Sorted set keys to intersect</param>
        /// <returns>Number of elements in the resulting sorted set</returns>
        public virtual async Task<long> ZInterStoreAsync(string destination, decimal[] weights = null, RedisAggregate? aggregate = null, params string[] keys)
        {
            return await WriteAsync(RedisCommands.ZInterStore(destination, weights, aggregate, keys));
        }

        /// <summary>
        /// Intersect multiple sorted sets and store the resulting set in a new key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="keys">Sorted set keys to intersect</param>
        /// <returns>Number of elements in the resulting sorted set</returns>
        public virtual async Task<long> ZInterStoreAsync(string destination, params string[] keys)
        {
            return await ZInterStoreAsync(destination, null, null, keys);
        }

        /// <summary>
        /// Return a range of members in a sorted set, by index
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <param name="withScores">Include scores in result</param>
        /// <returns>Array of elements in the specified range (with optional scores)</returns>
        public virtual async Task<string[]> ZRangeAsync(string key, long start, long stop, bool withScores = false)
        {
            return await WriteAsync(RedisCommands.ZRange(key, start, stop, withScores));
        }

        public virtual async Task<byte[][]> ZRangeBytesAsync(string key, long start, long stop, bool withScores = false)
        {
            return await WriteAsync(RedisCommands.ZRangeBytes(key, start, stop, withScores));
        }

        /// <summary>
        /// Return a range of members in a sorted set, by index, with scores
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <returns>Array of elements in the specified range with scores</returns>
        public virtual async Task<Tuple<string, decimal>[]> ZRangeWithScoresAsync(string key, long start, long stop)
        {
            return await WriteAsync(RedisCommands.ZRangeWithScores(key, start, stop));
        }

        public virtual async Task<Tuple<byte[], decimal>[]> ZRangeBytesWithScoresAsync(string key, long start, long stop)
        {
            return await WriteAsync(RedisCommands.ZRangeBytesWithScores(key, start, stop));
        }

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
        public virtual async Task<string[]> ZRangeByScoreAsync(string key, decimal min, decimal max, bool withScores = false, bool exclusiveMin = false, bool exclusiveMax = false, long? offset = null, long? count = null)
        {
            return await WriteAsync(RedisCommands.ZRangeByScore(key, min, max, withScores, exclusiveMin, exclusiveMax, offset, count));
        }

        public virtual async Task<byte[][]> ZRangeBytesByScoreAsync(string key, decimal min, decimal max, bool withScores = false, bool exclusiveMin = false, bool exclusiveMax = false, long? offset = null, long? count = null)
        {
            return await WriteAsync(RedisCommands.ZRangeBytesByScore(key, min, max, withScores, exclusiveMin, exclusiveMax, offset, count));
        }

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
        public virtual async Task<string[]> ZRangeByScoreAsync(string key, string min, string max, bool withScores = false, long? offset = null, long? count = null)
        {
            return await WriteAsync(RedisCommands.ZRangeByScore(key, min, max, withScores, offset, count));
        }

        public virtual async Task<byte[][]> ZRangeBytesByScoreAsync(string key, string min, string max, bool withScores = false, long? offset = null, long? count = null)
        {
            return await WriteAsync(RedisCommands.ZRangeBytesByScore(key, min, max, withScores, offset, count));
        }

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
        public virtual async Task<Tuple<string, decimal>[]> ZRangeByScoreWithScoresAsync(string key, decimal min, decimal max, bool exclusiveMin = false, bool exclusiveMax = false, long? offset = null, long? count = null)
        {
            return await WriteAsync(RedisCommands.ZRangeByScoreWithScores(key, min, max, exclusiveMin, exclusiveMax, offset, count));
        }

        public virtual async Task<Tuple<byte[], decimal>[]> ZRangeBytesByScoreWithScoresAsync(string key, decimal min, decimal max, bool exclusiveMin = false, bool exclusiveMax = false, long? offset = null, long? count = null)
        {
            return await WriteAsync(RedisCommands.ZRangeBytesByScoreWithScores(key, min, max, exclusiveMin, exclusiveMax, offset, count));
        }

        /// <summary>
        /// Return a range of members in a sorted set, by score, with scores
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score</param>
        /// <param name="max">Maximum score</param>
        /// <param name="offset">Start offset</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>List of elements in the specified range (with optional scores)</returns>
        public virtual async Task<Tuple<string, decimal>[]> ZRangeByScoreWithScoresAsync(string key, string min, string max, long? offset = null, long? count = null)
        {
            return await WriteAsync(RedisCommands.ZRangeByScoreWithScores(key, min, max, offset, count));
        }

        public virtual async Task<Tuple<byte[], decimal>[]> ZRangeBytesByScoreWithScoresAsync(string key, string min, string max, long? offset = null, long? count = null)
        {
            return await WriteAsync(RedisCommands.ZRangeBytesByScoreWithScores(key, min, max, offset, count));
        }

        /// <summary>
        /// Determine the index of a member in a sorted set
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="member">Member to lookup</param>
        /// <returns>Rank of member or null if key does not exist</returns>
        public virtual async Task<long?> ZRankAsync(string key, object member)
        {
            return await WriteAsync(RedisCommands.ZRank(key, member));
        }

        /// <summary>
        /// Remove one or more members from a sorted set
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="members">Members to remove</param>
        /// <returns>Number of elements removed</returns>
        public virtual async Task<long> ZRemAsync(string key, params object[] members)
        {
            return await WriteAsync(RedisCommands.ZRem(key, members));
        }

        /// <summary>
        /// Remove all members in a sorted set within the given indexes
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <returns>Number of elements removed</returns>
        public virtual async Task<long> ZRemRangeByRankAsync(string key, long start, long stop)
        {
            return await WriteAsync(RedisCommands.ZRemRangeByRank(key, start, stop));
        }

        /// <summary>
        /// Remove all members in a sorted set within the given scores
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Minimum score</param>
        /// <param name="max">Maximum score</param>
        /// <param name="exclusiveMin">Minimum score is exclusive</param>
        /// <param name="exclusiveMax">Maximum score is exclusive</param>
        /// <returns>Number of elements removed</returns>
        public virtual async Task<long> ZRemRangeByScoreAsync(string key, decimal min, decimal max, bool exclusiveMin = false, bool exclusiveMax = false)
        {
            return await WriteAsync(RedisCommands.ZRemRangeByScore(key, min, max, exclusiveMin, exclusiveMax));
        }

        public virtual async Task<long> ZRemRangeByScoreAsync(string key, string min, string max)
        {
            return await WriteAsync(RedisCommands.ZRemRangeByScore(key, min, max));
        }

        /// <summary>
        /// Return a range of members in a sorted set, by index, with scores ordered from high to low
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <param name="withScores">Include scores in result</param>
        /// <returns>List of elements in the specified range (with optional scores)</returns>
        public virtual async Task<string[]> ZRevRangeAsync(string key, long start, long stop, bool withScores = false)
        {
            return await WriteAsync(RedisCommands.ZRevRange(key, start, stop, withScores));
        }

        public virtual async Task<byte[][]> ZRevRangeBytesAsync(string key, long start, long stop, bool withScores = false)
        {
            return await WriteAsync(RedisCommands.ZRevRangeBytes(key, start, stop, withScores));
        }

        /// <summary>
        /// Return a range of members in a sorted set, by index, with scores ordered from high to low
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="start">Start offset</param>
        /// <param name="stop">Stop offset</param>
        /// <returns>List of elements in the specified range (with optional scores)</returns>
        public virtual async Task<Tuple<string, decimal>[]> ZRevRangeWithScoresAsync(string key, long start, long stop)
        {
            return await WriteAsync(RedisCommands.ZRevRangeWithScores(key, start, stop));
        }

        public virtual async Task<Tuple<byte[], decimal>[]> ZRevRangeBytesWithScoresAsync(string key, long start, long stop)
        {
            return await WriteAsync(RedisCommands.ZRevRangeBytesWithScores(key, start, stop));
        }

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
        public virtual async Task<string[]> ZRevRangeByScoreAsync(string key, decimal max, decimal min, bool withScores = false, bool exclusiveMax = false, bool exclusiveMin = false, long? offset = null, long? count = null)
        {
            return await WriteAsync(RedisCommands.ZRevRangeByScore(key, max, min, withScores, exclusiveMax, exclusiveMin, offset, count));
        }

        public virtual async Task<byte[][]> ZRevRangeBytesByScoreAsync(string key, decimal max, decimal min, bool withScores = false, bool exclusiveMax = false, bool exclusiveMin = false, long? offset = null, long? count = null)
        {
            return await WriteAsync(RedisCommands.ZRevRangeBytesByScore(key, max, min, withScores, exclusiveMax, exclusiveMin, offset, count));
        }

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
        public virtual async Task<string[]> ZRevRangeByScoreAsync(string key, string max, string min, bool withScores = false, long? offset = null, long? count = null)
        {
            return await WriteAsync(RedisCommands.ZRevRangeByScore(key, max, min, withScores, offset, count));
        }

        public virtual async Task<byte[][]> ZRevRangeBytesByScoreAsync(string key, string max, string min, bool withScores = false, long? offset = null, long? count = null)
        {
            return await WriteAsync(RedisCommands.ZRevRangeBytesByScore(key, max, min, withScores, offset, count));
        }

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
        public virtual async Task<Tuple<string, decimal>[]> ZRevRangeByScoreWithScoresAsync(string key, decimal max, decimal min, bool exclusiveMax = false, bool exclusiveMin = false, long? offset = null, long? count = null)
        {
            return await WriteAsync(RedisCommands.ZRevRangeByScoreWithScores(key, max, min, exclusiveMax, exclusiveMin, offset, count));
        }

        public virtual async Task<Tuple<byte[], decimal>[]> ZRevRangeBytesByScoreWithScoresAsync(string key, decimal max, decimal min, bool exclusiveMax = false, bool exclusiveMin = false, long? offset = null, long? count = null)
        {
            return await WriteAsync(RedisCommands.ZRevRangeBytesByScoreWithScores(key, max, min, exclusiveMax, exclusiveMin, offset, count));
        }

        /// <summary>
        /// Return a range of members in a sorted set, by score, with scores ordered from high to low
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="max">Maximum score</param>
        /// <param name="min">Minimum score</param>
        /// <param name="offset">Start offset</param>
        /// <param name="count">Number of elements to return</param>
        /// <returns>List of elements in the specified score range (with optional scores)</returns>
        public virtual async Task<Tuple<string, decimal>[]> ZRevRangeByScoreWithScoresAsync(string key, string max, string min, long? offset = null, long? count = null)
        {
            return await WriteAsync(RedisCommands.ZRevRangeByScoreWithScores(key, max, min, offset, count));
        }

        public virtual async Task<Tuple<byte[], decimal>[]> ZRevRangeBytesByScoreWithScoresAsync(string key, string max, string min, long? offset = null, long? count = null)
        {
            return await WriteAsync(RedisCommands.ZRevRangeBytesByScoreWithScores(key, max, min, offset, count));
        }

        /// <summary>
        /// Determine the index of a member in a sorted set, with scores ordered from high to low
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="member">Member to lookup</param>
        /// <returns>Rank of member, or null if member does not exist</returns>
        public virtual async Task<long?> ZRevRankAsync(string key, object member)
        {
            return await WriteAsync(RedisCommands.ZRevRank(key, member));
        }

        /// <summary>
        /// Get the score associated with the given member in a sorted set
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="member">Member to lookup</param>
        /// <returns>Score of member, or null if member does not exist</returns>
        public virtual async Task<decimal?> ZScoreAsync(string key, object member)
        {
            return await WriteAsync(RedisCommands.ZScore(key, member));
        }

        /// <summary>
        /// Add multiple sorted sets and store the resulting sorted set in a new key
        /// </summary>
        /// <param name="destination">Destination key</param>
        /// <param name="weights">Multiplication factor for each input set</param>
        /// <param name="aggregate">Aggregation function of resulting set</param>
        /// <param name="keys">Sorted set keys to union</param>
        /// <returns>Number of elements in the resulting sorted set</returns>
        public virtual async Task<long> ZUnionStoreAsync(string destination, decimal[] weights = null, RedisAggregate? aggregate = null, params string[] keys)
        {
            return await WriteAsync(RedisCommands.ZUnionStore(destination, weights, aggregate, keys));
        }

        /// <summary>
        /// Iterate the scores and elements of a sorted set field
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="cursor">The cursor returned by the server in the previous call, or 0 if this is the first call</param>
        /// <param name="pattern">Glob-style pattern to filter returned elements</param>
        /// <param name="count">Maximum number of elements to return</param>
        /// <returns>Updated cursor and result set</returns>
        public virtual async Task<RedisScan<Tuple<string, decimal>>> ZScanAsync(string key, long cursor, string pattern = null, long? count = null)
        {
            return await WriteAsync(RedisCommands.ZScan(key, cursor, pattern, count));
        }

        public virtual async Task<RedisScan<Tuple<byte[], decimal>>> ZScanBytesAsync(string key, long cursor, string pattern = null, long? count = null)
        {
            return await WriteAsync(RedisCommands.ZScanBytes(key, cursor, pattern, count));
        }

        /// <summary>
        /// Retrieve all the elements in a sorted set with a value between min and max
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Lexagraphic start value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <param name="max">Lexagraphic stop value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <param name="offset">Limit result set by offset</param>
        /// <param name="count">Limimt result set by size</param>
        /// <returns>List of elements in the specified range</returns>
        public virtual async Task<string[]> ZRangeByLexAsync(string key, string min, string max, long? offset = null, long? count = null)
        {
            return await WriteAsync(RedisCommands.ZRangeByLex(key, min, max, offset, count));
        }

        public virtual async Task<byte[][]> ZRangeBytesByLexAsync(string key, string min, string max, long? offset = null, long? count = null)
        {
            return await WriteAsync(RedisCommands.ZRangeBytesByLex(key, min, max, offset, count));
        }

        /// <summary>
        /// Remove all elements in the sorted set with a value between min and max
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Lexagraphic start value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <param name="max">Lexagraphic stop value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <returns>Number of elements removed</returns>
        public virtual async Task<long> ZRemRangeByLexAsync(string key, string min, string max)
        {
            return await WriteAsync(RedisCommands.ZRemRangeByLex(key, min, max));
        }

        /// <summary>
        /// Returns the number of elements in the sorted set with a value between min and max.
        /// </summary>
        /// <param name="key">Sorted set key</param>
        /// <param name="min">Lexagraphic start value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <param name="max">Lexagraphic stop value. Prefix value with '(' to indicate exclusive; '[' to indicate inclusive. Use '-' or '+' to specify infinity.</param>
        /// <returns>Number of elements in the specified score range</returns>
        public virtual async Task<long> ZLexCountAsync(string key, string min, string max)
        {
            return await WriteAsync(RedisCommands.ZLexCount(key, min, max));
        }

        #endregion//end Async

#endif

    }
}
