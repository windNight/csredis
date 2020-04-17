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

        #region Scripting

        /// <summary>
        /// Execute a Lua script server side
        /// </summary>
        /// <param name="script">Script to run on server</param>
        /// <param name="keys">Keys used by script</param>
        /// <param name="arguments">Arguments to pass to script</param>
        /// <returns>Redis object</returns>
        public virtual object Eval(string script, string[] keys, params object[] arguments)
        {
            return Write(RedisCommands.Eval(script, keys, arguments));
        }

        /// <summary>
        /// Execute a Lua script server side, sending only the script's cached SHA hash
        /// </summary>
        /// <param name="sha1">SHA1 hash of script</param>
        /// <param name="keys">Keys used by script</param>
        /// <param name="arguments">Arguments to pass to script</param>
        /// <returns>Redis object</returns>
        public virtual object EvalSHA(string sha1, string[] keys, params object[] arguments)
        {
            return Write(RedisCommands.EvalSHA(sha1, keys, arguments));
        }

        /// <summary>
        /// Check existence of script SHA hashes in the script cache
        /// </summary>
        /// <param name="sha1s">SHA1 script hashes</param>
        /// <returns>Array of boolean values indicating script existence on server</returns>
        public virtual bool[] ScriptExists(params string[] sha1s)
        {
            return Write(RedisCommands.ScriptExists(sha1s));
        }

        /// <summary>
        /// Remove all scripts from the script cache
        /// </summary>
        /// <returns>Status code</returns>
        public virtual string ScriptFlush()
        {
            return Write(RedisCommands.ScriptFlush());
        }

        /// <summary>
        /// Kill the script currently in execution
        /// </summary>
        /// <returns>Status code</returns>
        public virtual string ScriptKill()
        {
            return Write(RedisCommands.ScriptKill());
        }

        /// <summary>
        /// Load the specified Lua script into the script cache
        /// </summary>
        /// <param name="script">Lua script to load</param>
        /// <returns>SHA1 hash of script</returns>
        public virtual string ScriptLoad(string script)
        {
            return Write(RedisCommands.ScriptLoad(script));
        }

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
        public virtual async Task<object> EvalAsync(string script, string[] keys, params object[] arguments)
        {
            return await WriteAsync(RedisCommands.Eval(script, keys, arguments));
        }

        /// <summary>
        /// Execute a Lua script server side, sending only the script's cached SHA hash
        /// </summary>
        /// <param name="sha1">SHA1 hash of script</param>
        /// <param name="keys">Keys used by script</param>
        /// <param name="arguments">Arguments to pass to script</param>
        /// <returns>Redis object</returns>
        public virtual async Task<object> EvalSHAAsync(string sha1, string[] keys, params object[] arguments)
        {
            return await WriteAsync(RedisCommands.EvalSHA(sha1, keys, arguments));
        }

        /// <summary>
        /// Check existence of script SHA hashes in the script cache
        /// </summary>
        /// <param name="sha1s">SHA1 script hashes</param>
        /// <returns>Array of boolean values indicating script existence on server</returns>
        public virtual async Task<bool[]> ScriptExistsAsync(params string[] sha1s)
        {
            return await WriteAsync(RedisCommands.ScriptExists(sha1s));
        }

        /// <summary>
        /// Remove all scripts from the script cache
        /// </summary>
        /// <returns>Status code</returns>
        public virtual async Task<string> ScriptFlushAsync()
        {
            return await WriteAsync(RedisCommands.ScriptFlush());
        }

        /// <summary>
        /// Kill the script currently in execution
        /// </summary>
        /// <returns>Status code</returns>
        public virtual async Task<string> ScriptKillAsync()
        {
            return await WriteAsync(RedisCommands.ScriptKill());
        }

        /// <summary>
        /// Load the specified Lua script into the script cache
        /// </summary>
        /// <param name="script">Lua script to load</param>
        /// <returns>SHA1 hash of script</returns>
        public virtual async Task<string> ScriptLoadAsync(string script)
        {
            return await WriteAsync(RedisCommands.ScriptLoad(script));
        }

        #endregion

#endif

    }
}
