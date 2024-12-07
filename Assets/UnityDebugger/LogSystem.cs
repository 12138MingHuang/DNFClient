using System;
using UnityEngine;

namespace UnityDebugger
{
    public class LogSystem : MonoBehaviour
    {
        private void Awake()
        {
#if OPEN_LOG
            Debuger.InitLog(new LogConfig
            {
                openLog = true,
                openTime = true,
                showThreadID = true,
                showColorName = true,
                logSave = true,
                showFPS = true
            });
            Debuger.Log("Log");
            Debuger.LogWarning("LogWarning");
            Debuger.LogError("LogError");
            Debuger.ColorLog(LogColor.Cyan, "ColorLog");
            Debuger.LogGreen("LogGreen");
            Debuger.LogYellow("LogYellow");
#else
            Debug.unityLogger.logEnabled = false;
#endif
        }
    }
}
