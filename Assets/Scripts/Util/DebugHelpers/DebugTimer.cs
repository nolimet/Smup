using System;
using System.Collections.Generic;
using UnityEngine;

namespace Util.DebugHelpers
{
    public static class DebugTime
    {
        private static Dictionary<string, DateTime> _logtime;

        public static void StartLog(string logKey)
        {
            if (_logtime == null) _logtime = new Dictionary<string, DateTime>();

            var currentTime = DateTime.Now;
            if (_logtime.ContainsKey(logKey))
                _logtime[logKey] = currentTime;
            else
                _logtime.Add(logKey, currentTime);
        }

        public static void StopLog(string logKey)
        {
            if (_logtime == null) _logtime = new Dictionary<string, DateTime>();

            var currentTime = DateTime.Now;
            if (_logtime.ContainsKey(logKey))
                Debugger.Log(logKey, "Time Elapsed - " + (_logtime[logKey] - currentTime).Milliseconds + "ms");
            else
                Debug.LogError("NO KEY FOUND! KEY WAS: " + logKey);
        }
    }
}
