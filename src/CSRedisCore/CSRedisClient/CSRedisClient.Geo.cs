﻿using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
#if !net40
using System.Threading.Tasks;
#endif

namespace CSRedis
{
    public partial class CSRedisClient
    {

        #region Geo redis-server 3.2

        /// <summary>
        /// 将指定的地理空间位置（纬度、经度、成员）添加到指定的key中。这些数据将会存储到sorted set这样的目的是为了方便使用GEORADIUS或者GEORADIUSBYMEMBER命令对数据进行半径查询等操作。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="member">成员</param>
        /// <returns>是否成功</returns>
        public bool GeoAdd(string key, decimal longitude, decimal latitude, object member) => GeoAdd(key, (longitude, latitude, member)) == 1;
        /// <summary>
        /// 将指定的地理空间位置（纬度、经度、成员）添加到指定的key中。这些数据将会存储到sorted set这样的目的是为了方便使用GEORADIUS或者GEORADIUSBYMEMBER命令对数据进行半径查询等操作。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="values">批量添加的值</param>
        /// <returns>添加到sorted set元素的数目，但不包括已更新score的元素。</returns>
        public long GeoAdd(string key, params (decimal longitude, decimal latitude, object member)[] values)
        {
            if (values == null || values.Any() == false) return 0;
            var args = values.Select(z => (z.longitude, z.latitude, this.SerializeRedisValueInternal(z.member))).ToArray();
            return ExecuteScalar(key, (c, k) => c.Value.GeoAdd(k, args));
        }
        /// <summary>
        /// 返回两个给定位置之间的距离。如果两个位置之间的其中一个不存在， 那么命令返回空值。GEODIST 命令在计算距离时会假设地球为完美的球形， 在极限情况下， 这一假设最大会造成 0.5% 的误差。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member1">成员1</param>
        /// <param name="member2">成员2</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <returns>计算出的距离会以双精度浮点数的形式被返回。 如果给定的位置元素不存在， 那么命令返回空值。</returns>
        public decimal? GeoDist(string key, object member1, object member2, GeoUnit unit = GeoUnit.m)
        {
            var args1 = this.SerializeRedisValueInternal(member1);
            var args2 = this.SerializeRedisValueInternal(member2);
            return ExecuteScalar(key, (c, k) => c.Value.GeoDist(k, args1, args2, unit));
        }
        /// <summary>
        /// 返回一个或多个位置元素的 Geohash 表示。通常使用表示位置的元素使用不同的技术，使用Geohash位置52点整数编码。由于编码和解码过程中所使用的初始最小和最大坐标不同，编码的编码也不同于标准。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="members">多个查询的成员</param>
        /// <returns>一个数组， 数组的每个项都是一个 geohash 。 命令返回的 geohash 的位置与用户给定的位置元素的位置一一对应。</returns>
        public string[] GeoHash(string key, object[] members)
        {
            if (members == null || members.Any() == false) return new string[0];
            var args = members.Select(z => this.SerializeRedisValueInternal(z)).ToArray();
            return ExecuteScalar(key, (c, k) => c.Value.GeoHash(k, args));
        }
        /// <summary>
        /// 从key里返回所有给定位置元素的位置（经度和纬度）。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="members">多个查询的成员</param>
        /// <returns>GEOPOS 命令返回一个数组， 数组中的每个项都由两个元素组成： 第一个元素为给定位置元素的经度， 而第二个元素则为给定位置元素的纬度。当给定的位置元素不存在时， 对应的数组项为空值。</returns>
        public (decimal longitude, decimal latitude)?[] GeoPos(string key, object[] members)
        {
            if (members == null || members.Any() == false) return new (decimal, decimal)?[0];
            var args = members.Select(z => this.SerializeRedisValueInternal(z)).ToArray();
            return ExecuteScalar(key, (c, k) => c.Value.GeoPos(k, args));
        }

        /// <summary>
        /// 以给定的经纬度为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        public string[] GeoRadius(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            ExecuteScalar(key, (c, k) => c.Value.GeoRadius(k, longitude, latitude, radius, unit, count, sorting, false, false, false)).Select(a => a.member).ToArray();
        /// <summary>
        /// 以给定的经纬度为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        public T[] GeoRadius<T>(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            ExecuteScalar(key, (c, k) => c.Value.GeoRadiusBytes(k, longitude, latitude, radius, unit, count, sorting, false, false, false)).Select(a => this.DeserializeRedisValueInternal<T>(a.member)).ToArray();

        /// <summary>
        /// 以给定的经纬度为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素（包含距离）。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        public (string member, decimal dist)[] GeoRadiusWithDist(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            ExecuteScalar(key, (c, k) => c.Value.GeoRadius(k, longitude, latitude, radius, unit, count, sorting, false, true, false)).Select(a => (a.member, a.dist)).ToArray();
        /// <summary>
        /// 以给定的经纬度为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素（包含距离）。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        public (T member, decimal dist)[] GeoRadiusWithDist<T>(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            ExecuteScalar(key, (c, k) => c.Value.GeoRadiusBytes(k, longitude, latitude, radius, unit, count, sorting, false, true, false)).Select(a => (this.DeserializeRedisValueInternal<T>(a.member), a.dist)).ToArray();

        /// <summary>
        /// 以给定的经纬度为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素（包含经度、纬度）。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        private (string member, decimal longitude, decimal latitude)[] GeoRadiusWithCoord(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            ExecuteScalar(key, (c, k) => c.Value.GeoRadius(k, longitude, latitude, radius, unit, count, sorting, true, false, false)).Select(a => (a.member, a.longitude, a.latitude)).ToArray();
        /// <summary>
        /// 以给定的经纬度为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素（包含经度、纬度）。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        private (T member, decimal longitude, decimal latitude)[] GeoRadiusWithCoord<T>(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            ExecuteScalar(key, (c, k) => c.Value.GeoRadiusBytes(k, longitude, latitude, radius, unit, count, sorting, true, false, false)).Select(a => (this.DeserializeRedisValueInternal<T>(a.member), a.longitude, a.latitude)).ToArray();

        /// <summary>
        /// 以给定的经纬度为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素（包含距离、经度、纬度）。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        public (string member, decimal dist, decimal longitude, decimal latitude)[] GeoRadiusWithDistAndCoord(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            ExecuteScalar(key, (c, k) => c.Value.GeoRadius(k, longitude, latitude, radius, unit, count, sorting, true, true, false)).Select(a => (a.member, a.dist, a.longitude, a.latitude)).ToArray();
        /// <summary>
        /// 以给定的经纬度为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素（包含距离、经度、纬度）。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        public (T member, decimal dist, decimal longitude, decimal latitude)[] GeoRadiusWithDistAndCoord<T>(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            ExecuteScalar(key, (c, k) => c.Value.GeoRadiusBytes(k, longitude, latitude, radius, unit, count, sorting, true, true, false)).Select(a => (this.DeserializeRedisValueInternal<T>(a.member), a.dist, a.longitude, a.latitude)).ToArray();

        /// <summary>
        /// 以给定的成员为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member">成员</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        public string[] GeoRadiusByMember(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            ExecuteScalar(key, (c, k) => c.Value.GeoRadiusByMember(k, member, radius, unit, count, sorting, false, false, false)).Select(a => a.member).ToArray();
        /// <summary>
        /// 以给定的成员为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member">成员</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        public T[] GeoRadiusByMember<T>(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            this.DeserializeRedisValueArrayInternal<T>(ExecuteScalar(key, (c, k) => c.Value.GeoRadiusBytesByMember(k, member, radius, unit, count, sorting, false, false, false)).Select(a => a.member).ToArray());

        /// <summary>
        /// 以给定的成员为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素（包含距离）。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member">成员</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        public (string member, decimal dist)[] GeoRadiusByMemberWithDist(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            ExecuteScalar(key, (c, k) => c.Value.GeoRadiusByMember(k, member, radius, unit, count, sorting, false, true, false)).Select(a => (a.member, a.dist)).ToArray();
        /// <summary>
        /// 以给定的成员为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素（包含距离）。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member">成员</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        public (T member, decimal dist)[] GeoRadiusByMemberWithDist<T>(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            ExecuteScalar(key, (c, k) => c.Value.GeoRadiusBytesByMember(k, member, radius, unit, count, sorting, false, true, false)).Select(a => (this.DeserializeRedisValueInternal<T>(a.member), a.dist)).ToArray();

        /// <summary>
        /// 以给定的成员为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素（包含经度、纬度）。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member">成员</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        private (string member, decimal longitude, decimal latitude)[] GeoRadiusByMemberWithCoord(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            ExecuteScalar(key, (c, k) => c.Value.GeoRadiusByMember(k, member, radius, unit, count, sorting, true, false, false)).Select(a => (a.member, a.longitude, a.latitude)).ToArray();
        /// <summary>
        /// 以给定的成员为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素（包含经度、纬度）。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member">成员</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        private (T member, decimal longitude, decimal latitude)[] GeoRadiusByMemberWithCoord<T>(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            ExecuteScalar(key, (c, k) => c.Value.GeoRadiusBytesByMember(k, member, radius, unit, count, sorting, true, false, false)).Select(a => (this.DeserializeRedisValueInternal<T>(a.member), a.longitude, a.latitude)).ToArray();

        /// <summary>
        /// 以给定的成员为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素（包含距离、经度、纬度）。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member">成员</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        public (string member, decimal dist, decimal longitude, decimal latitude)[] GeoRadiusByMemberWithDistAndCoord(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            ExecuteScalar(key, (c, k) => c.Value.GeoRadiusByMember(k, member, radius, unit, count, sorting, true, true, false)).Select(a => (a.member, a.dist, a.longitude, a.latitude)).ToArray();
        /// <summary>
        /// 以给定的成员为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素（包含距离、经度、纬度）。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member">成员</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        public (T member, decimal dist, decimal longitude, decimal latitude)[] GeoRadiusByMemberWithDistAndCoord<T>(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            ExecuteScalar(key, (c, k) => c.Value.GeoRadiusBytesByMember(k, member, radius, unit, count, sorting, true, true, false)).Select(a => (this.DeserializeRedisValueInternal<T>(a.member), a.dist, a.longitude, a.latitude)).ToArray();
        
        #endregion

#if !net40

        #region Geo redis-server 3.2
        /// <summary>
        /// 将指定的地理空间位置（纬度、经度、成员）添加到指定的key中。这些数据将会存储到sorted set这样的目的是为了方便使用GEORADIUS或者GEORADIUSBYMEMBER命令对数据进行半径查询等操作。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="member">成员</param>
        /// <returns>是否成功</returns>
        async public Task<bool> GeoAddAsync(string key, decimal longitude, decimal latitude, object member) => await GeoAddAsync(key, (longitude, latitude, member)) == 1;
        /// <summary>
        /// 将指定的地理空间位置（纬度、经度、成员）添加到指定的key中。这些数据将会存储到sorted set这样的目的是为了方便使用GEORADIUS或者GEORADIUSBYMEMBER命令对数据进行半径查询等操作。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="values">批量添加的值</param>
        /// <returns>添加到sorted set元素的数目，但不包括已更新score的元素。</returns>
        async public Task<long> GeoAddAsync(string key, params (decimal longitude, decimal latitude, object member)[] values)
        {
            if (values == null || values.Any() == false) return 0;
            var args = values.Select(z => (z.longitude, z.latitude, this.SerializeRedisValueInternal(z.member))).ToArray();
            return await ExecuteScalarAsync(key, (c, k) => c.Value.GeoAddAsync(k, args));
        }
        /// <summary>
        /// 返回两个给定位置之间的距离。如果两个位置之间的其中一个不存在， 那么命令返回空值。GEODIST 命令在计算距离时会假设地球为完美的球形， 在极限情况下， 这一假设最大会造成 0.5% 的误差。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member1">成员1</param>
        /// <param name="member2">成员2</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <returns>计算出的距离会以双精度浮点数的形式被返回。 如果给定的位置元素不存在， 那么命令返回空值。</returns>
        public Task<decimal?> GeoDistAsync(string key, object member1, object member2, GeoUnit unit = GeoUnit.m)
        {
            var args1 = this.SerializeRedisValueInternal(member1);
            var args2 = this.SerializeRedisValueInternal(member2);
            return ExecuteScalarAsync(key, (c, k) => c.Value.GeoDistAsync(k, args1, args2, unit));
        }
        /// <summary>
        /// 返回一个或多个位置元素的 Geohash 表示。通常使用表示位置的元素使用不同的技术，使用Geohash位置52点整数编码。由于编码和解码过程中所使用的初始最小和最大坐标不同，编码的编码也不同于标准。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="members">多个查询的成员</param>
        /// <returns>一个数组， 数组的每个项都是一个 geohash 。 命令返回的 geohash 的位置与用户给定的位置元素的位置一一对应。</returns>
        async public Task<string[]> GeoHashAsync(string key, object[] members)
        {
            if (members == null || members.Any() == false) return new string[0];
            var args = members.Select(z => this.SerializeRedisValueInternal(z)).ToArray();
            return await ExecuteScalarAsync(key, (c, k) => c.Value.GeoHashAsync(k, args));
        }
        /// <summary>
        /// 从key里返回所有给定位置元素的位置（经度和纬度）。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="members">多个查询的成员</param>
        /// <returns>GEOPOS 命令返回一个数组， 数组中的每个项都由两个元素组成： 第一个元素为给定位置元素的经度， 而第二个元素则为给定位置元素的纬度。当给定的位置元素不存在时， 对应的数组项为空值。</returns>
        async public Task<(decimal longitude, decimal latitude)?[]> GeoPosAsync(string key, object[] members)
        {
            if (members == null || members.Any() == false) return new (decimal, decimal)?[0];
            var args = members.Select(z => this.SerializeRedisValueInternal(z)).ToArray();
            return await ExecuteScalarAsync(key, (c, k) => c.Value.GeoPosAsync(k, args));
        }

        /// <summary>
        /// 以给定的经纬度为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        async public Task<string[]> GeoRadiusAsync(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            (await ExecuteScalarAsync(key, (c, k) => c.Value.GeoRadiusAsync(k, longitude, latitude, radius, unit, count, sorting, false, false, false))).Select(a => a.member).ToArray();
        /// <summary>
        /// 以给定的经纬度为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        async public Task<T[]> GeoRadiusAsync<T>(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            (await ExecuteScalarAsync(key, (c, k) => c.Value.GeoRadiusBytesAsync(k, longitude, latitude, radius, unit, count, sorting, false, false, false))).Select(a => this.DeserializeRedisValueInternal<T>(a.member)).ToArray();

        /// <summary>
        /// 以给定的经纬度为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素（包含距离）。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        async public Task<(string member, decimal dist)[]> GeoRadiusWithDistAsync(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            (await ExecuteScalarAsync(key, (c, k) => c.Value.GeoRadiusAsync(k, longitude, latitude, radius, unit, count, sorting, false, true, false))).Select(a => (a.member, a.dist)).ToArray();
        /// <summary>
        /// 以给定的经纬度为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素（包含距离）。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        async public Task<(T member, decimal dist)[]> GeoRadiusWithDistAsync<T>(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            (await ExecuteScalarAsync(key, (c, k) => c.Value.GeoRadiusBytesAsync(k, longitude, latitude, radius, unit, count, sorting, false, true, false))).Select(a => (this.DeserializeRedisValueInternal<T>(a.member), a.dist)).ToArray();

        /// <summary>
        /// 以给定的经纬度为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素（包含经度、纬度）。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        async private Task<(string member, decimal longitude, decimal latitude)[]> GeoRadiusWithCoordAsync(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            (await ExecuteScalarAsync(key, (c, k) => c.Value.GeoRadiusAsync(k, longitude, latitude, radius, unit, count, sorting, true, false, false))).Select(a => (a.member, a.longitude, a.latitude)).ToArray();
        /// <summary>
        /// 以给定的经纬度为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素（包含经度、纬度）。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        async private Task<(T member, decimal longitude, decimal latitude)[]> GeoRadiusWithCoordAsync<T>(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            (await ExecuteScalarAsync(key, (c, k) => c.Value.GeoRadiusBytesAsync(k, longitude, latitude, radius, unit, count, sorting, true, false, false))).Select(a => (this.DeserializeRedisValueInternal<T>(a.member), a.longitude, a.latitude)).ToArray();

        /// <summary>
        /// 以给定的经纬度为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素（包含距离、经度、纬度）。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        async public Task<(string member, decimal dist, decimal longitude, decimal latitude)[]> GeoRadiusWithDistAndCoordAsync(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            (await ExecuteScalarAsync(key, (c, k) => c.Value.GeoRadiusAsync(k, longitude, latitude, radius, unit, count, sorting, true, true, false))).Select(a => (a.member, a.dist, a.longitude, a.latitude)).ToArray();
        /// <summary>
        /// 以给定的经纬度为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素（包含距离、经度、纬度）。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        async public Task<(T member, decimal dist, decimal longitude, decimal latitude)[]> GeoRadiusWithDistAndCoordAsync<T>(string key, decimal longitude, decimal latitude, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            (await ExecuteScalarAsync(key, (c, k) => c.Value.GeoRadiusBytesAsync(k, longitude, latitude, radius, unit, count, sorting, true, true, false))).Select(a => (this.DeserializeRedisValueInternal<T>(a.member), a.dist, a.longitude, a.latitude)).ToArray();

        /// <summary>
        /// 以给定的成员为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member">成员</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        async public Task<string[]> GeoRadiusByMemberAsync(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            (await ExecuteScalarAsync(key, (c, k) => c.Value.GeoRadiusByMemberAsync(k, member, radius, unit, count, sorting, false, false, false))).Select(a => a.member).ToArray();
        /// <summary>
        /// 以给定的成员为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member">成员</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        async public Task<T[]> GeoRadiusByMemberAsync<T>(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            (await ExecuteScalarAsync(key, (c, k) => c.Value.GeoRadiusBytesByMemberAsync(k, member, radius, unit, count, sorting, false, false, false))).Select(a => this.DeserializeRedisValueInternal<T>(a.member)).ToArray();

        /// <summary>
        /// 以给定的成员为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素（包含距离）。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member">成员</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        async public Task<(string member, decimal dist)[]> GeoRadiusByMemberWithDistAsync(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            (await ExecuteScalarAsync(key, (c, k) => c.Value.GeoRadiusByMemberAsync(k, member, radius, unit, count, sorting, false, true, false))).Select(a => (a.member, a.dist)).ToArray();
        /// <summary>
        /// 以给定的成员为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素（包含距离）。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member">成员</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        async public Task<(T member, decimal dist)[]> GeoRadiusByMemberWithDistAsync<T>(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            (await ExecuteScalarAsync(key, (c, k) => c.Value.GeoRadiusBytesByMemberAsync(k, member, radius, unit, count, sorting, false, true, false))).Select(a => (this.DeserializeRedisValueInternal<T>(a.member), a.dist)).ToArray();

        /// <summary>
        /// 以给定的成员为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素（包含经度、纬度）。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member">成员</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        async private Task<(string member, decimal longitude, decimal latitude)[]> GeoRadiusByMemberWithCoordAsync(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            (await ExecuteScalarAsync(key, (c, k) => c.Value.GeoRadiusByMemberAsync(k, member, radius, unit, count, sorting, true, false, false))).Select(a => (a.member, a.longitude, a.latitude)).ToArray();
        /// <summary>
        /// 以给定的成员为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素（包含经度、纬度）。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member">成员</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        async private Task<(T member, decimal longitude, decimal latitude)[]> GeoRadiusByMemberWithCoordAsync<T>(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            (await ExecuteScalarAsync(key, (c, k) => c.Value.GeoRadiusBytesByMemberAsync(k, member, radius, unit, count, sorting, true, false, false))).Select(a => (this.DeserializeRedisValueInternal<T>(a.member), a.longitude, a.latitude)).ToArray();

        /// <summary>
        /// 以给定的成员为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素（包含距离、经度、纬度）。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member">成员</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        async public Task<(string member, decimal dist, decimal longitude, decimal latitude)[]> GeoRadiusByMemberWithDistAndCoordAsync(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            (await ExecuteScalarAsync(key, (c, k) => c.Value.GeoRadiusByMemberAsync(k, member, radius, unit, count, sorting, true, true, false))).Select(a => (a.member, a.dist, a.longitude, a.latitude)).ToArray();
        /// <summary>
        /// 以给定的成员为中心， 返回键包含的位置元素当中， 与中心的距离不超过给定最大距离的所有位置元素（包含距离、经度、纬度）。
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member">成员</param>
        /// <param name="radius">距离</param>
        /// <param name="unit">m 表示单位为米；km 表示单位为千米；mi 表示单位为英里；ft 表示单位为英尺；</param>
        /// <param name="count">虽然用户可以使用 COUNT 选项去获取前 N 个匹配元素， 但是因为命令在内部可能会需要对所有被匹配的元素进行处理， 所以在对一个非常大的区域进行搜索时， 即使只使用 COUNT 选项去获取少量元素， 命令的执行速度也可能会非常慢。 但是从另一方面来说， 使用 COUNT 选项去减少需要返回的元素数量， 对于减少带宽来说仍然是非常有用的。</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        async public Task<(T member, decimal dist, decimal longitude, decimal latitude)[]> GeoRadiusByMemberWithDistAndCoordAsync<T>(string key, object member, decimal radius, GeoUnit unit = GeoUnit.m, long? count = null, GeoOrderBy? sorting = null) =>
            (await ExecuteScalarAsync(key, (c, k) => c.Value.GeoRadiusBytesByMemberAsync(k, member, radius, unit, count, sorting, true, true, false))).Select(a => (this.DeserializeRedisValueInternal<T>(a.member), a.dist, a.longitude, a.latitude)).ToArray();
        #endregion



#endif

    }
}