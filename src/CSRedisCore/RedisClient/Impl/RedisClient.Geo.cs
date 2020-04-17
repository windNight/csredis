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

        #region Geo redis-server 3.2

        public virtual long GeoAdd(string key, params (decimal longitude, decimal latitude, object member)[] values)
        {
            if (values == null || values.Length == 0) throw new Exception("values 参数不能为空");
            var args = new List<object>();
            args.Add(key);
            foreach (var v in values) args.AddRange(new object[] { v.longitude, v.latitude, v.member });
            return Write(new RedisInt("GEOADD", args.ToArray()));
        }
        public virtual decimal? GeoDist(string key, object member1, object member2, GeoUnit unit = GeoUnit.m)
        {
            if (unit == GeoUnit.m) return Write(new RedisFloat.Nullable("GEODIST", key, member1, member2));
            return Write(new RedisFloat.Nullable("GEODIST", key, member1, member2, unit));
        }
        public virtual string[] GeoHash(string key, object[] members)
        {
            if (members == null || members.Length == 0) throw new Exception("values 参数不能为空");
            var args = new List<object>();
            args.Add(key);
            args.AddRange(members);
            return Write(new RedisArray.Strings("GEOHASH", args.ToArray()));
        }
        public virtual (decimal longitude, decimal latitude)?[] GeoPos(string key, object[] members)
        {
            if (members == null || members.Length == 0) throw new Exception("values 参数不能为空");
            var args = new List<object>();
            args.Add(key);
            args.AddRange(members);
            var ret = Write(new RedisArray.Generic<decimal[]>(new RedisArray.Generic<decimal>(new RedisFloat("GEOPOS", args.ToArray()))));
            return ret.Select(a => a != null && a.Length == 2 ? new (decimal, decimal)?((a[0], a[1])) : null).ToArray();
        }
        public virtual (string member, decimal dist, decimal longitude, decimal latitude, long hash)[] GeoRadius(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null, bool withCoord = false, bool withDist = false, bool withHash = false)
        {
            var args = new List<object>(new object[] { key, longitude, latitude, radius, unit });
            if (withCoord) args.Add("WITHCOORD");
            if (withDist) args.Add("WITHDIST");
            if (withHash) args.Add("WITHHASH");
            if (count.HasValue) args.AddRange(new object[] { "COUNT", count });
            if (sorting.HasValue) args.Add(sorting);

            var cmd = new RedisTuple.Generic<string, decimal, long, decimal[]>.Single(
                new RedisString(null),
                withDist == false ? null : new RedisFloat(null),
                withHash == false ? null : new RedisInt(null),
                withCoord == false ? null : new RedisArray.Generic<decimal>(new RedisFloat(null)), "GEORADIUS", args.ToArray());
            var ret = Write(new RedisArray.Generic<Tuple<string, decimal, long, decimal[]>>(cmd));
            return ret.Select(a => (a.Item1, a.Item2, a.Item4 == null ? default(decimal) : a.Item4[0], a.Item4 == null ? default(decimal) : a.Item4[1], a.Item3)).ToArray();
        }
        public virtual (byte[] member, decimal dist, decimal longitude, decimal latitude, long hash)[] GeoRadiusBytes(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null, bool withCoord = false, bool withDist = false, bool withHash = false)
        {
            var args = new List<object>(new object[] { key, longitude, latitude, radius, unit });
            if (withCoord) args.Add("WITHCOORD");
            if (withDist) args.Add("WITHDIST");
            if (withHash) args.Add("WITHHASH");
            if (count.HasValue) args.AddRange(new object[] { "COUNT", count });
            if (sorting.HasValue) args.Add(sorting);

            var cmd = new RedisTuple.Generic<byte[], decimal, long, decimal[]>.Single(
                new RedisBytes(null),
                withDist == false ? null : new RedisFloat(null),
                withHash == false ? null : new RedisInt(null),
                withCoord == false ? null : new RedisArray.Generic<decimal>(new RedisFloat(null)), "GEORADIUS", args.ToArray());
            var ret = Write(new RedisArray.Generic<Tuple<byte[], decimal, long, decimal[]>>(cmd));
            return ret.Select(a => (a.Item1, a.Item2, a.Item4 == null ? default(decimal) : a.Item4[0], a.Item4 == null ? default(decimal) : a.Item4[1], a.Item3)).ToArray();
        }
        public virtual (string member, decimal dist, decimal longitude, decimal latitude, long hash)[] GeoRadiusByMember(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null, bool withCoord = false, bool withDist = false, bool withHash = false)
        {
            var args = new List<object>(new object[] { key, member, radius, unit });
            if (withCoord) args.Add("WITHCOORD");
            if (withDist) args.Add("WITHDIST");
            if (withHash) args.Add("WITHHASH");
            if (count.HasValue) args.AddRange(new object[] { "COUNT", count });
            if (sorting.HasValue) args.Add(sorting);

            var cmd = new RedisTuple.Generic<string, decimal, long, decimal[]>.Single(
                new RedisString(null),
                withDist == false ? null : new RedisFloat(null),
                withHash == false ? null : new RedisInt(null),
                withCoord == false ? null : new RedisArray.Generic<decimal>(new RedisFloat(null)), "GEORADIUSBYMEMBER", args.ToArray());
            var ret = Write(new RedisArray.Generic<Tuple<string, decimal, long, decimal[]>>(cmd));
            return ret.Select(a => (a.Item1, a.Item2, a.Item4 == null ? default(decimal) : a.Item4[0], a.Item4 == null ? default(decimal) : a.Item4[1], a.Item3)).ToArray();
        }
        public virtual (byte[] member, decimal dist, decimal longitude, decimal latitude, long hash)[] GeoRadiusBytesByMember(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null, bool withCoord = false, bool withDist = false, bool withHash = false)
        {
            var args = new List<object>(new object[] { key, member, radius, unit });
            if (withCoord) args.Add("WITHCOORD");
            if (withDist) args.Add("WITHDIST");
            if (withHash) args.Add("WITHHASH");
            if (count.HasValue) args.AddRange(new object[] { "COUNT", count });
            if (sorting.HasValue) args.Add(sorting);

            var cmd = new RedisTuple.Generic<byte[], decimal, long, decimal[]>.Single(
                new RedisBytes(null),
                withDist == false ? null : new RedisFloat(null),
                withHash == false ? null : new RedisInt(null),
                withCoord == false ? null : new RedisArray.Generic<decimal>(new RedisFloat(null)), "GEORADIUSBYMEMBER", args.ToArray());
            var ret = Write(new RedisArray.Generic<Tuple<byte[], decimal, long, decimal[]>>(cmd));
            return ret.Select(a => (a.Item1, a.Item2, a.Item4 == null ? default(decimal) : a.Item4[0], a.Item4 == null ? default(decimal) : a.Item4[1], a.Item3)).ToArray();
        }

        #endregion

#if !net40

        #region Geo redis-server 3.2
        public virtual async Task<long> GeoAddAsync(string key, params (decimal longitude, decimal latitude, object member)[] values)
        {
            if (values == null || values.Length == 0) throw new Exception("values 参数不能为空");
            var args = new List<object>();
            args.Add(key);
            foreach (var v in values) args.AddRange(new object[] { v.longitude, v.latitude, v.member });
            return await WriteAsync(new RedisInt("GEOADD", args.ToArray()));
        }
        
        public virtual async Task<decimal?> GeoDistAsync(string key, object member1, object member2, GeoUnit unit = GeoUnit.m)
        {
            if (unit == GeoUnit.m) return await WriteAsync(new RedisFloat.Nullable("GEODIST", key, member1, member2));
            return await WriteAsync(new RedisFloat.Nullable("GEODIST", key, member1, member2, unit));
        }
       
        public virtual async Task<string[]> GeoHashAsync(string key, object[] members)
        {
            if (members == null || members.Length == 0) throw new Exception("values 参数不能为空");
            var args = new List<object>();
            args.Add(key);
            args.AddRange(members);
            return await WriteAsync(new RedisArray.Strings("GEOHASH", args.ToArray()));
        }
      
        public virtual async Task<(decimal longitude, decimal latitude)?[]> GeoPosAsync(string key, object[] members)
        {
            if (members == null || members.Length == 0) throw new Exception("values 参数不能为空");
            var args = new List<object>();
            args.Add(key);
            args.AddRange(members);
            var ret = await WriteAsync(new RedisArray.Generic<decimal[]>(new RedisArray.Generic<decimal>(new RedisFloat("GEOPOS", args.ToArray()))));
            return ret.Select(a => a != null && a.Length == 2 ? new (decimal, decimal)?((a[0], a[1])) : null).ToArray();
        }
      
        public virtual async Task<(string member, decimal dist, decimal longitude, decimal latitude, long hash)[]> GeoRadiusAsync(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null, bool withCoord = false, bool withDist = false, bool withHash = false)
        {
            var args = new List<object>(new object[] { key, longitude, latitude, radius, unit });
            if (withCoord) args.Add("WITHCOORD");
            if (withDist) args.Add("WITHDIST");
            if (withHash) args.Add("WITHHASH");
            if (count.HasValue) args.Add(count);
            if (sorting.HasValue) args.Add(sorting);

            var cmd = new RedisTuple.Generic<string, decimal, long, decimal[]>.Single(
                new RedisString(null),
                withDist == false ? null : new RedisFloat(null),
                withHash == false ? null : new RedisInt(null),
                withCoord == false ? null : new RedisArray.Generic<decimal>(new RedisFloat(null)), "GEORADIUS", args.ToArray());
            var ret = await WriteAsync(new RedisArray.Generic<Tuple<string, decimal, long, decimal[]>>(cmd));
            return ret.Select(a => (a.Item1, a.Item2, a.Item4 == null ? default(decimal) : a.Item4[0], a.Item4 == null ? default(decimal) : a.Item4[1], a.Item3)).ToArray();
        }
      
        public virtual async Task<(byte[] member, decimal dist, decimal longitude, decimal latitude, long hash)[]> GeoRadiusBytesAsync(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null, bool withCoord = false, bool withDist = false, bool withHash = false)
        {
            var args = new List<object>(new object[] { key, longitude, latitude, radius, unit });
            if (withCoord) args.Add("WITHCOORD");
            if (withDist) args.Add("WITHDIST");
            if (withHash) args.Add("WITHHASH");
            if (count.HasValue) args.Add(count);
            if (sorting.HasValue) args.Add(sorting);

            var cmd = new RedisTuple.Generic<byte[], decimal, long, decimal[]>.Single(
                new RedisBytes(null),
                withDist == false ? null : new RedisFloat(null),
                withHash == false ? null : new RedisInt(null),
                withCoord == false ? null : new RedisArray.Generic<decimal>(new RedisFloat(null)), "GEORADIUS", args.ToArray());
            var ret = await WriteAsync(new RedisArray.Generic<Tuple<byte[], decimal, long, decimal[]>>(cmd));
            return ret.Select(a => (a.Item1, a.Item2, a.Item4 == null ? default(decimal) : a.Item4[0], a.Item4 == null ? default(decimal) : a.Item4[1], a.Item3)).ToArray();
        }
       
        public virtual async Task<(string member, decimal dist, decimal longitude, decimal latitude, long hash)[]> GeoRadiusByMemberAsync(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null, bool withCoord = false, bool withDist = false, bool withHash = false)
        {
            var args = new List<object>(new object[] { key, member, radius, unit });
            if (withCoord) args.Add("WITHCOORD");
            if (withDist) args.Add("WITHDIST");
            if (withHash) args.Add("WITHHASH");
            if (count.HasValue) args.Add(count);
            if (sorting.HasValue) args.Add(sorting);

            var cmd = new RedisTuple.Generic<string, decimal, long, decimal[]>.Single(
                new RedisString(null),
                withDist == false ? null : new RedisFloat(null),
                withHash == false ? null : new RedisInt(null),
                withCoord == false ? null : new RedisArray.Generic<decimal>(new RedisFloat(null)), "GEORADIUSBYMEMBER", args.ToArray());
            var ret = await WriteAsync(new RedisArray.Generic<Tuple<string, decimal, long, decimal[]>>(cmd));
            return ret.Select(a => (a.Item1, a.Item2, a.Item4 == null ? default(decimal) : a.Item4[0], a.Item4 == null ? default(decimal) : a.Item4[1], a.Item3)).ToArray();
        }
        
        public virtual async Task<(byte[] member, decimal dist, decimal longitude, decimal latitude, long hash)[]> GeoRadiusBytesByMemberAsync(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null, bool withCoord = false, bool withDist = false, bool withHash = false)
        {
            var args = new List<object>(new object[] { key, member, radius, unit });
            if (withCoord) args.Add("WITHCOORD");
            if (withDist) args.Add("WITHDIST");
            if (withHash) args.Add("WITHHASH");
            if (count.HasValue) args.Add(count);
            if (sorting.HasValue) args.Add(sorting);

            var cmd = new RedisTuple.Generic<byte[], decimal, long, decimal[]>.Single(
                new RedisBytes(null),
                withDist == false ? null : new RedisFloat(null),
                withHash == false ? null : new RedisInt(null),
                withCoord == false ? null : new RedisArray.Generic<decimal>(new RedisFloat(null)), "GEORADIUSBYMEMBER", args.ToArray());
            var ret = await WriteAsync(new RedisArray.Generic<Tuple<byte[], decimal, long, decimal[]>>(cmd));
            return ret.Select(a => (a.Item1, a.Item2, a.Item4 == null ? default(decimal) : a.Item4[0], a.Item4 == null ? default(decimal) : a.Item4[1], a.Item3)).ToArray();
        }
       
        #endregion

#endif


    }
}
