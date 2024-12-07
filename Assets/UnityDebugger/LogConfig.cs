using System;
using UnityEngine;

namespace UnityDebugger
{
    public class LogConfig
    {
        /// <summary>
        /// 是否打开日志系统
        /// </summary>
        public bool openLog = true;

        /// <summary>
        /// 日志前缀
        /// </summary>
        public string logHeadFix = "###";

        /// <summary>
        /// 是否显示时间
        /// </summary>
        public bool openTime = true;
        
        /// <summary>
        /// 是否显示线程ID
        /// </summary>
        public bool showThreadID = true;

        /// <summary>
        /// 日志文件存储开关
        /// </summary>
        public bool logSave = true;
        
        /// <summary>
        /// 是否显示FPS
        /// </summary>
        public bool showFPS = true;
        
        /// <summary>
        /// 是否显示颜色名称
        /// </summary>
        public bool showColorName = true;

        /// <summary>
        /// 日志文件存储路径
        /// </summary>
        public string LogFileSavePath
        {
            get
            {
                return Application.persistentDataPath + "/";
            }
        }

        /// <summary>
        /// 日志文件名称
        /// </summary>
        public string LogFileName
        {
            get
            {
                return Application.productName + " " + DateTime.Now.ToString("yyyy-MM-dd HH-mm") + ".log";
            }
        }
    }
}
