using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityDebugger
{
    public class Debuger
    {
        public static LogConfig cfg;
        
        [Conditional("OPEN_LOG")]
        public static void InitLog(LogConfig config = null)
        {
            if (config == null)
                cfg = new LogConfig();
            else
                cfg = config;

            if (cfg.logSave)
            {
                GameObject logObj = new GameObject("LogHelper");
                Object.DontDestroyOnLoad(logObj);
                UnityLogHelper unityLogHelper = logObj.AddComponent<UnityLogHelper>();
                unityLogHelper.InitLogFileModule(cfg.LogFileSavePath, cfg.LogFileName);
            }
            if (cfg.showFPS)
            {
                GameObject fpsObj = new GameObject("FPS");
                Object.DontDestroyOnLoad(fpsObj);
                fpsObj.AddComponent<FPS>();
            }
        }

        #region 普通日志
        [Conditional("OPEN_LOG")]
        public static void Log(object obj)
        {
            if(!cfg.openLog) return;
            
            string log = GenerateLog(obj.ToString());
            UnityEngine.Debug.Log(log);
        }
        [Conditional("OPEN_LOG")]
        public static void Log(object obj, params object[] args)
        {
            if (!cfg.openLog) return;

            string content = string.Empty;
            if(args != null && args.Length > 0)
            {
                foreach (var item in args)
                {
                    content += item;
                }
            }
            string log = GenerateLog(obj + content);
            UnityEngine.Debug.Log(log);
        }
        [Conditional("OPEN_LOG")]
        public static void LogWarning(object obj)
        {
            if(!cfg.openLog) return;
            
            string log = GenerateLog(obj.ToString());
            UnityEngine.Debug.LogWarning(log);
        }
        [Conditional("OPEN_LOG")]
        public static void LogWarning(object obj, params object[] args)
        {
            if (!cfg.openLog) return;

            string content = string.Empty;
            if(args != null && args.Length > 0)
            {
                foreach (var item in args)
                {
                    content += item;
                }
            }
            string log = GenerateLog(obj + content);
            UnityEngine.Debug.LogWarning(log);
        }
        [Conditional("OPEN_LOG")]
        public static void LogError(object obj)
        {
            if(!cfg.openLog) return;
            
            string log = GenerateLog(obj.ToString());
            UnityEngine.Debug.LogError(log);
        }
        [Conditional("OPEN_LOG")]
        public static void LogError(object obj, params object[] args)
        {
            if (!cfg.openLog) return;

            string content = string.Empty;
            if(args != null && args.Length > 0)
            {
                foreach (var item in args)
                {
                    content += item;
                }
            }
            string log = GenerateLog(obj + content);
            UnityEngine.Debug.LogError(log);
        }
        
        #endregion

        #region 颜色日志打印
        [Conditional("OPEN_LOG")]
        public static void ColorLog(LogColor color, object obj)
        {
            if (!cfg.openLog) return;
            
            string log = GenerateLog(obj.ToString(), color);
            log = GetUnityColor(log, color);
            UnityEngine.Debug.Log(log);
        }
        [Conditional("OPEN_LOG")]
        public static void LogGreen(object msg)
        {
            ColorLog(LogColor.Green,msg);
        }
        [Conditional("OPEN_LOG")]
        public static void LogYellow(object msg)
        {
            ColorLog(LogColor.Yellow, msg);
        }
        [Conditional("OPEN_LOG")]
        public static void LogOrange(object msg)
        {
            ColorLog(LogColor.Orange, msg);
        }
        [Conditional("OPEN_LOG")]
        public static void LogRed(object msg)
        {
            ColorLog(LogColor.Red, msg);
        }
        [Conditional("OPEN_LOG")]
        public static void LogBlue(object msg)
        {
            ColorLog(LogColor.Blue, msg);
        }
        [Conditional("OPEN_LOG")]
        public static void LogMagenta(object msg)
        {
            ColorLog(LogColor.Magenta, msg);
        }
        [Conditional("OPEN_LOG")]
        public static void LogCyan(object msg)
        {
            ColorLog(LogColor.Cyan, msg);
        }

        #endregion
        
        private static string GenerateLog(string log, LogColor color = LogColor.None)
        {
            StringBuilder stringBuilder = new StringBuilder(cfg.logHeadFix, 100);
            if (cfg.openTime)
            {
                stringBuilder.AppendFormat(" {0:HH:mm:ss-fff}", DateTime.Now);
            }
            if (cfg.showThreadID)
            {
                stringBuilder.AppendFormat(" [ThreadID:{0}]", Thread.CurrentThread.ManagedThreadId);
            }
            if (cfg.showColorName)
            {
                stringBuilder.AppendFormat(" {0}", color.ToString());
            }
            stringBuilder.AppendFormat(" {0}", log);
            return stringBuilder.ToString();
        }

        private static string GetUnityColor(string msg, LogColor color)
        {
            if(color == LogColor.None) return msg;

            switch (color)
            {
                case LogColor.Blue:
                    msg = $"<color=#0000FF>{msg}</color>";
                    break;
                case LogColor.Cyan:
                    msg = $"<color=#00FFFF>{msg}</color>";
                    break;
                case LogColor.Darkblue:
                    msg = $"<color=#8FBC8F>{msg}</color>";
                    break;
                case LogColor.Green:
                    msg = $"<color=#00FF00>{msg}</color>";
                    break;
                case LogColor.Orange:
                    msg = $"<color=#FFA500>{msg}</color>";
                    break;
                case LogColor.Red:
                    msg = $"<color=#FF0000>{msg}</color>";
                    break;
                case LogColor.Yellow:
                    msg = $"<color=#FFFF00>{msg}</color>";
                    break;
                case LogColor.Magenta:
                    msg = $"<color=#FF00FF>{msg}</color>";
                    break;
            }
            return msg;
        }
    }
}
