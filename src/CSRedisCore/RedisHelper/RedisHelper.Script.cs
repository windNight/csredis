using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
#if !net40
using System.Threading.Tasks;
#endif


partial class RedisHelper<TMark>
{
    #region Script
    /// <summary>
    /// 执行脚本
    /// </summary>
    /// <param name="script">Lua 脚本</param>
    /// <param name="key">用于定位分区节点，不含prefix前辍</param>
    /// <param name="args">参数</param>
    /// <returns></returns>
    public static object Eval(string script, string key, params object[] args) => Instance.Eval(script, key, args);
    /// <summary>
    /// 执行脚本
    /// </summary>
    /// <param name="sha1">脚本缓存的sha1</param>
    /// <param name="key">用于定位分区节点，不含prefix前辍</param>
    /// <param name="args">参数</param>
    /// <returns></returns>
    public static object EvalSHA(string sha1, string key, params object[] args) => Instance.EvalSHA(sha1, key, args);
    /// <summary>
    /// 校验所有分区节点中，脚本是否已经缓存。任何分区节点未缓存sha1，都返回false。
    /// </summary>
    /// <param name="sha1">脚本缓存的sha1</param>
    /// <returns></returns>
    public static bool[] ScriptExists(params string[] sha1) => Instance.ScriptExists(sha1);
    /// <summary>
    /// 清除所有分区节点中，所有 Lua 脚本缓存
    /// </summary>
    public static void ScriptFlush() => Instance.ScriptFlush();
    /// <summary>
    /// 杀死所有分区节点中，当前正在运行的 Lua 脚本
    /// </summary>
    public static void ScriptKill() => Instance.ScriptKill();
    /// <summary>
    /// 在所有分区节点中，缓存脚本后返回 sha1（同样的脚本在任何服务器，缓存后的 sha1 都是相同的）
    /// </summary>
    /// <param name="script">Lua 脚本</param>
    /// <returns></returns>
    public static string ScriptLoad(string script) => Instance.ScriptLoad(script);
    
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
    public static Task<object> EvalAsync(string script, string key, params object[] args) => Instance.EvalAsync(script, key, args);
    /// <summary>
    /// 执行脚本
    /// </summary>
    /// <param name="sha1">脚本缓存的sha1</param>
    /// <param name="key">用于定位分区节点，不含prefix前辍</param>
    /// <param name="args">参数</param>
    /// <returns></returns>
    public static Task<object> EvalSHAAsync(string sha1, string key, params object[] args) => Instance.EvalSHAAsync(sha1, key, args);
    /// <summary>
    /// 校验所有分区节点中，脚本是否已经缓存。任何分区节点未缓存sha1，都返回false。
    /// </summary>
    /// <param name="sha1">脚本缓存的sha1</param>
    /// <returns></returns>
    public static Task<bool[]> ScriptExistsAsync(params string[] sha1) => Instance.ScriptExistsAsync(sha1);
    /// <summary>
    /// 清除所有分区节点中，所有 Lua 脚本缓存
    /// </summary>
    public static Task ScriptFlushAsync() => Instance.ScriptFlushAsync();
    /// <summary>
    /// 杀死所有分区节点中，当前正在运行的 Lua 脚本
    /// </summary>
    public static Task ScriptKillAsync() => Instance.ScriptKillAsync();
    /// <summary>
    /// 在所有分区节点中，缓存脚本后返回 sha1（同样的脚本在任何服务器，缓存后的 sha1 都是相同的）
    /// </summary>
    /// <param name="script">Lua 脚本</param>
    /// <returns></returns>
    public static Task<string> ScriptLoadAsync(string script) => Instance.ScriptLoadAsync(script);

    #endregion

#endif

}
