using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace UnityDebugger
{
    public class LogEditor
    {
        [MenuItem("ZMLog/打开日志系统", priority = 1)]
        public static void LoadReport()
        {
            ScriptingDefineSymbols.AddScriptingDefineSymbol("OPEN_LOG");
            GameObject reportObj = GameObject.Find("Reporter");
            if (reportObj == null)
            {
                reportObj = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/UnityDebugger/Unity-Logs-Viewer/Reporter.prefab"));
                reportObj.name = "Reporter";
                AssetDatabase.SaveAssets();
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                AssetDatabase.Refresh();
                Debug.Log("日志系统已打开");
            }
        }

        [MenuItem("ZMLog/关闭日志系统")]
        public static void CloseReport()
        {
            ScriptingDefineSymbols.RemoveScriptingDefineSymbol("OPEN_LOG");
            GameObject reportObj = GameObject.Find("Reporter");
            if (reportObj != null)
            {
                GameObject.DestroyImmediate(reportObj);
                AssetDatabase.SaveAssets();
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                AssetDatabase.Refresh();
                Debug.Log("日志系统已关闭");
            }
        }
    }
}