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
    public interface IRedisClientUseScript
    {

        #region Scripting
        /// <summary>
        /// Execute a Lua script server side
        /// </summary>
        /// <param name="script">Script to run on server</param>
        /// <param name="keys">Keys used by script</param>
        /// <param name="arguments">Arguments to pass to script</param>
        /// <returns>Redis object</returns>
        object Eval(string script, string[] keys, params object[] arguments);




        /// <summary>
        /// Execute a Lua script server side, sending only the script's cached SHA hash
        /// </summary>
        /// <param name="sha1">SHA1 hash of script</param>
        /// <param name="keys">Keys used by script</param>
        /// <param name="arguments">Arguments to pass to script</param>
        /// <returns>Redis object</returns>
        object EvalSHA(string sha1, string[] keys, params object[] arguments);




        /// <summary>
        /// Check existence of script SHA hashes in the script cache
        /// </summary>
        /// <param name="sha1s">SHA1 script hashes</param>
        /// <returns>Array of boolean values indicating script existence on server</returns>
        bool[] ScriptExists(params string[] sha1s);




        /// <summary>
        /// Remove all scripts from the script cache
        /// </summary>
        /// <returns>Status code</returns>
        string ScriptFlush();




        /// <summary>
        /// Kill the script currently in execution
        /// </summary>
        /// <returns>Status code</returns>
        string ScriptKill();




        /// <summary>
        /// Load the specified Lua script into the script cache
        /// </summary>
        /// <param name="script">Lua script to load</param>
        /// <returns>SHA1 hash of script</returns>
        string ScriptLoad(string script);



        #endregion

#if !net40

        #region Scripting
        /// <summary>
        /// Execute a Lua script server side
        /// </summary>
        /// <param name="script">Script to run on server</param>
        /// <param name="keys">Keys used by script</param>
        /// <param name="arguments">Arguments to pass to script</param>
        /// <returns>Redis object</returns>
        Task<object> EvalAsync(string script, string[] keys, params object[] arguments);




        /// <summary>
        /// Execute a Lua script server side, sending only the script's cached SHA hash
        /// </summary>
        /// <param name="sha1">SHA1 hash of script</param>
        /// <param name="keys">Keys used by script</param>
        /// <param name="arguments">Arguments to pass to script</param>
        /// <returns>Redis object</returns>
        Task<object> EvalSHAAsync(string sha1, string[] keys, params object[] arguments);




        /// <summary>
        /// Check existence of script SHA hashes in the script cache
        /// </summary>
        /// <param name="sha1s">SHA1 script hashes</param>
        /// <returns>Array of boolean values indicating script existence on server</returns>
        Task<bool[]> ScriptExistsAsync(params string[] sha1s);




        /// <summary>
        /// Remove all scripts from the script cache
        /// </summary>
        /// <returns>Status code</returns>
        Task<string> ScriptFlushAsync();




        /// <summary>
        /// Kill the script currently in execution
        /// </summary>
        /// <returns>Status code</returns>
        Task<string> ScriptKillAsync();




        /// <summary>
        /// Load the specified Lua script into the script cache
        /// </summary>
        /// <param name="script">Lua script to load</param>
        /// <returns>SHA1 hash of script</returns>
        Task<string> ScriptLoadAsync(string script);



        #endregion

#endif

    }
}
