using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityDebugger;
using UnityEngine;

/// <summary>
/// Protobuff 转为Josn 字符串，并进行打印
/// </summary>
public class ProtoBuffConvert
{
    /// <summary>
    /// Protobuff 转为Josn 字符串，并进行打印
    /// </summary>
    /// <param name="proto"> Protobuff 对象</param>
    /// <typeparam name="T"> Protobuff 类型</typeparam>
    public static void ToJson<T>(T proto)
    {
        Debuger.Log(JsonConvert.SerializeObject(proto));
    }
}