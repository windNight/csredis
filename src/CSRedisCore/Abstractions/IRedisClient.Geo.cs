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
    public interface IRedisClientUseGeo
    {
        #region Geo redis-server 3.2
        long GeoAdd(string key, params (decimal longitude, decimal latitude, object member)[] values);
        decimal? GeoDist(string key, object member1, object member2, GeoUnit unit = GeoUnit.m);
        string[] GeoHash(string key, object[] members);
        (decimal longitude, decimal latitude)?[] GeoPos(string key, object[] members);
        (string member, decimal dist, decimal longitude, decimal latitude, long hash)[] GeoRadius(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null, bool withCoord = false, bool withDist = false, bool withHash = false);
        (byte[] member, decimal dist, decimal longitude, decimal latitude, long hash)[] GeoRadiusBytes(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null, bool withCoord = false, bool withDist = false, bool withHash = false);
        (string member, decimal dist, decimal longitude, decimal latitude, long hash)[] GeoRadiusByMember(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null, bool withCoord = false, bool withDist = false, bool withHash = false);
        (byte[] member, decimal dist, decimal longitude, decimal latitude, long hash)[] GeoRadiusBytesByMember(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null, bool withCoord = false, bool withDist = false, bool withHash = false);
        #endregion


#if !net40

        #region Geo redis-server 3.2
        Task<long> GeoAddAsync(string key, params (decimal longitude, decimal latitude, object member)[] values);
        Task<decimal?> GeoDistAsync(string key, object member1, object member2, GeoUnit unit = GeoUnit.m);
        Task<string[]> GeoHashAsync(string key, object[] members);
        Task<(decimal longitude, decimal latitude)?[]> GeoPosAsync(string key, object[] members);
        Task<(string member, decimal dist, decimal longitude, decimal latitude, long hash)[]> GeoRadiusAsync(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null, bool withCoord = false, bool withDist = false, bool withHash = false);
        Task<(byte[] member, decimal dist, decimal longitude, decimal latitude, long hash)[]> GeoRadiusBytesAsync(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null, bool withCoord = false, bool withDist = false, bool withHash = false);
        Task<(string member, decimal dist, decimal longitude, decimal latitude, long hash)[]> GeoRadiusByMemberAsync(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null, bool withCoord = false, bool withDist = false, bool withHash = false);
        Task<(byte[] member, decimal dist, decimal longitude, decimal latitude, long hash)[]> GeoRadiusBytesByMemberAsync(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null, bool withCoord = false, bool withDist = false, bool withHash = false);
        #endregion

#endif

    }
    public enum GeoUnit { m, km, mi, ft }
    public enum GeoOrderBy { ASC, DESC }
}
