using Newtonsoft.Json;

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
        #region Script
       
        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="script">Lua 脚本</param>
        /// <param name="key">用于定位分区节点，不含prefix前辍</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public object Eval(string script, string key, params object[] args)
        {
            var args2 = args?.Select(z => this.SerializeRedisValueInternal(z)).ToArray();
            return ExecuteScalar(key, (c, k) => c.Value.Eval(script, new[] { k }, args2));
        }
        
        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="sha1">脚本缓存的sha1</param>
        /// <param name="key">用于定位分区节点，不含prefix前辍</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public object EvalSHA(string sha1, string key, params object[] args)
        {
            var args2 = args?.Select(z => this.SerializeRedisValueInternal(z)).ToArray();
            return ExecuteScalar(key, (c, k) => c.Value.EvalSHA(sha1, new[] { k }, args2));
        }
        
        /// <summary>
        /// 校验所有分区节点中，脚本是否已经缓存。任何分区节点未缓存sha1，都返回false。
        /// </summary>
        /// <param name="sha1">脚本缓存的sha1</param>
        /// <returns></returns>
        public bool[] ScriptExists(params string[] sha1) => Nodes.Select(a => GetAndExecute(a.Value, c => c.Value.ScriptExists(sha1))?.Where(z => z == false).Any() == false).ToArray();
        
        /// <summary>
        /// 清除所有分区节点中，所有 Lua 脚本缓存
        /// </summary>
        public void ScriptFlush() => Nodes.Select(a => GetAndExecute(a.Value, c => c.Value.ScriptFlush()));
       
        /// <summary>
        /// 杀死所有分区节点中，当前正在运行的 Lua 脚本
        /// </summary>
        public void ScriptKill() => Nodes.Select(a => GetAndExecute(a.Value, c => c.Value.ScriptKill()));
       
        /// <summary>
        /// 在所有分区节点中，缓存脚本后返回 sha1（同样的脚本在任何服务器，缓存后的 sha1 都是相同的）
        /// </summary>
        /// <param name="script">Lua 脚本</param>
        /// <returns></returns>
        public string ScriptLoad(string script) => Nodes.Select(a => GetAndExecute(a.Value, c => (c.Pool.Policy.Name.ToString(), c.Value.ScriptLoad(script)))).First().Item2;
        #endregion


#if !net40

        #region Script

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="script">Lua 脚本</param>
        /// <param name="key">用于定位分区节点，不含prefix前辍</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public Task<object> EvalAsync(string script, string key, params object[] args)
        {
            var args2 = args?.Select(z => this.SerializeRedisValueInternal(z)).ToArray();
            return ExecuteScalarAsync(key, (c, k) => c.Value.EvalAsync(script, new[] { k }, args2));
        }
        
        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="sha1">脚本缓存的sha1</param>
        /// <param name="key">用于定位分区节点，不含prefix前辍</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public Task<object> EvalSHAAsync(string sha1, string key, params object[] args)
        {
            var args2 = args?.Select(z => this.SerializeRedisValueInternal(z)).ToArray();
            return ExecuteScalarAsync(key, (c, k) => c.Value.EvalSHAAsync(sha1, new[] { k }, args2));
        }
      
        /// <summary>
        /// 校验所有分区节点中，脚本是否已经缓存。任何分区节点未缓存sha1，都返回false。
        /// </summary>
        /// <param name="sha1">脚本缓存的sha1</param>
        /// <returns></returns>
        public async Task<bool[]> ScriptExistsAsync(params string[] sha1)
        {
            var ret = new List<bool>();
            foreach (var pool in Nodes.Values)
                ret.Add((await GetAndExecuteAsync(pool, c => c.Value.ScriptExistsAsync(sha1)))?.Where(z => z == false).Any() == false);
            return ret.ToArray();
        }
       
        /// <summary>
        /// 清除所有分区节点中，所有 Lua 脚本缓存
        /// </summary>
        public async Task ScriptFlushAsync()
        {
            foreach (var pool in Nodes.Values)
                await GetAndExecuteAsync(pool, c => c.Value.ScriptFlushAsync());
        }
       
        /// <summary>
        /// 杀死所有分区节点中，当前正在运行的 Lua 脚本
        /// </summary>
        public async Task ScriptKillAsync()
        {
            foreach (var pool in Nodes.Values)
                await GetAndExecuteAsync(pool, c => c.Value.ScriptKillAsync());
        }
       
        /// <summary>
        /// 在所有分区节点中，缓存脚本后返回 sha1（同样的脚本在任何服务器，缓存后的 sha1 都是相同的）
        /// </summary>
        /// <param name="script">Lua 脚本</param>
        /// <returns></returns>
        public async Task<string> ScriptLoadAsync(string script)
        {
            string sha1 = null;
            foreach (var pool in Nodes.Values)
                sha1 = await GetAndExecuteAsync(pool, c => c.Value.ScriptLoadAsync(script));
            return sha1;
        }
        #endregion

#endif

    }
}