using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

namespace UnityDebugger
{
    public class LogData
    {
        public string log;
        public string trace;
        public LogType type;
    }
    
    public class UnityLogHelper : MonoBehaviour
    {
        /// <summary>
        /// 文件写入流
        /// </summary>
        private StreamWriter _streamWriter;
        
        /// <summary>
        /// 日志数据队列
        /// </summary>
        private readonly ConcurrentQueue<LogData> _conCurrentQueue = new ConcurrentQueue<LogData>();
        
        /// <summary>
        /// 工作信号事件
        /// </summary>
        private readonly ManualResetEvent _manualResetEvent = new ManualResetEvent(false);
        
        private bool _threadRunning = false;
        
        private string NowTime
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        public void InitLogFileModule(string savePath, string logFileName)
        {
            string logFilePath = Path.Combine(savePath, logFileName);
            Debug.Log("日志文件路径: " + logFilePath);
            _streamWriter = new StreamWriter(logFilePath);
            Application.logMessageReceivedThreaded += OnLogMessageReceivedThreaded;
            _threadRunning = true;
            Thread fileThread = new Thread(FileLogThread);
            fileThread.Start();
        }
        private void FileLogThread()
        {
            while (_threadRunning)
            {
                _manualResetEvent.WaitOne(); // 让线程进入等待，并进行阻塞
                if(_streamWriter == null) break;
                LogData data;
                while (_conCurrentQueue.Count > 0 && _conCurrentQueue.TryDequeue(out data))
                {
                    if (data.type == LogType.Log)
                    {
                        _streamWriter.Write("Log >>> ");
                        _streamWriter.WriteLine(data.log);
                        _streamWriter.WriteLine(data.trace);
                    }
                    else if (data.type == LogType.Warning)
                    {
                        _streamWriter.Write("Warning >>> ");
                        _streamWriter.WriteLine(data.log);
                        _streamWriter.WriteLine(data.trace);
                    }
                    else if (data.type == LogType.Error)
                    {
                        _streamWriter.Write("Error >>> ");
                        _streamWriter.WriteLine(data.log);
                        _streamWriter.Write("\n");
                        _streamWriter.WriteLine(data.trace);
                    }
                    _streamWriter.Write("\r\n");
                }
                
                // 保存当前文件内容，使其生效
                _streamWriter.Flush();
                _manualResetEvent.Reset(); // 重置信号，表示没有人指定需要工作
                Thread.Sleep(1);
            }
        }

        private void OnApplicationQuit()
        {
            Application.logMessageReceivedThreaded -= OnLogMessageReceivedThreaded;
            _threadRunning = false;
            _manualResetEvent.Set(); // 设置一个信号，表示线程是需要工作的
            _streamWriter.Close();
            _streamWriter = null;
        }

        private void OnLogMessageReceivedThreaded(string condition, string stacktrace, LogType type)
        {
            _conCurrentQueue.Enqueue(new LogData
            {
                log = NowTime + " " + condition,
                trace = stacktrace,
                type = type
            });
            _manualResetEvent.Set(); // 设置一个信号，表示线程是需要工作的
        }
    }
}


