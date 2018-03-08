using System;
using System.Collections;
using System.Collections.Generic;


namespace Util.Debugger
{
    public static class DebugTime
    {
        static Dictionary<string, DateTime> logtime;

        public static void StartLog(string LogKey)
        {
            if (logtime == null)
            {
                logtime = new Dictionary<string, DateTime>();
            }

            DateTime currentTime = DateTime.Now;
            if (logtime.ContainsKey(LogKey))
            {
                logtime[LogKey] = currentTime;
            }
            else
            {
                logtime.Add(LogKey, currentTime);
            }

        }
        public static void StopLog(string LogKey)
        {
            if (logtime == null)
            {
                logtime = new Dictionary<string, DateTime>();
            }

            DateTime currentTime = DateTime.Now;
            if (logtime.ContainsKey(LogKey))
            {
                Debugger.Log(LogKey, "Time Elapsed - " + (logtime[LogKey] - currentTime).Milliseconds + "ms");
            }
            else
            {
                UnityEngine.Debug.LogError("NO KEY FOUND! KEY WAS: " + LogKey);
            }
        }
    }
}