using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class World
{
    /// <summary>
    /// 逻辑层所有类的一个字典
    /// </summary>
    private static Dictionary<string, ILogicBehaviour> _logicBehaviourDic = new Dictionary<string, ILogicBehaviour>();

    /// <summary>
    /// 数据层所有类的一个字典
    /// </summary>
    private static Dictionary<string, IDataBehaviour> _dataBehaviourDic = new Dictionary<string, IDataBehaviour>();
    
    /// <summary>
    /// 消息层所有类的一个字典
    /// </summary>
    private static Dictionary<string, IMsgBehaviour> _msgBehaviourDic = new Dictionary<string, IMsgBehaviour>();
    
    /// <summary>
    /// 世界构建时触发
    /// </summary>
    public virtual void OnCreate(){}
    
    public virtual void OnUpdate(){}

    /// <summary>
    /// 世界销毁时触发
    /// </summary>
    public virtual void OnDestroy(){}

    /// <summary>
    /// 销毁游戏世界
    /// </summary>
    /// <param name="nameSpace">命名空间</param>
    /// <param name="pars">可选参数</param>
    public void DestroyWorld(string nameSpace, object pars = null)
    {
        //需要移除的一个列表
        List<string> needRemoveList = new List<string>();

        //释放逻辑层脚本
        foreach (var item in _logicBehaviourDic)
        {
            if (string.Equals(item.Value.GetType().Namespace, nameSpace))
            {
                needRemoveList.Add(item.Key);
            }
        }
        foreach (var item in needRemoveList)
        {
            _logicBehaviourDic[item].OnDestroy();
            _logicBehaviourDic.Remove(item);
        }

        //释放数据层脚本
        needRemoveList.Clear();
        foreach (var item in _dataBehaviourDic)
        {
            if (string.Equals(item.Value.GetType().Namespace, nameSpace))
            {
                needRemoveList.Add(item.Key);
            }
        }
        foreach (var item in needRemoveList)
        {
            _dataBehaviourDic[item].OnDestroy();
            _dataBehaviourDic.Remove(item);
        }
        
        //释放消息层脚本
        needRemoveList.Clear();
        foreach (var item in _msgBehaviourDic)
        {
            if (string.Equals(item.Value.GetType().Namespace, nameSpace))
            {
                needRemoveList.Add(item.Key);
            }
        }
        foreach (var item in needRemoveList)
        {
            _msgBehaviourDic[item].OnDestroy();
            _msgBehaviourDic.Remove(item);
        }
        
        OnDestroy();
        
        OnDestroyPostProcess(pars);
    }

    /// <summary>
    /// 世界销毁完成后触发
    /// </summary>
    /// <param name="args"></param>
    public virtual void OnDestroyPostProcess(object args){}

    /// <summary>
    /// 获取逻辑层控制器
    /// </summary>
    /// <typeparam name="T">逻辑层控制器类型</typeparam>
    /// <returns>逻辑层控制器</returns>
    public static T GetExitsLogicCtrl<T>() where T : ILogicBehaviour
    {
        ILogicBehaviour logic = null;
        if(_logicBehaviourDic.TryGetValue(typeof(T).Name, out logic))
        {
            return (T)logic;
        }
        Debug.LogError(typeof(T).Name + "Not Get Class Filed! Please Check Params!");
        return default(T);
    }
    
    /// <summary>
    /// 获取数据管理器
    /// </summary>
    /// <typeparam name="T">数据管理器类型</typeparam>
    /// <returns>数据管理器</returns>
    public static T GetExitsDataMgr<T>() where T : IDataBehaviour
    {
        IDataBehaviour data = null;
        if(_dataBehaviourDic.TryGetValue(typeof(T).Name, out data))
        {
            return (T)data;
        }
        Debug.LogError(typeof(T).Name + "Not Get Class Filed! Please Check Params!");
        return default(T);
    }
    
    /// <summary>
    /// 获取消息层管理器
    /// </summary>
    /// <typeparam name="T">消息层管理器类型</typeparam>
    /// <returns>消息层管理器</returns>
    public static T GetExitsMsgMgr<T>() where T : IMsgBehaviour
    {
        IMsgBehaviour msg = null;
        if(_msgBehaviourDic.TryGetValue(typeof(T).Name, out msg))
        {
            return (T)msg;
        }
        Debug.LogError(typeof(T).Name + "Not Get Class Filed! Please Check Params!");
        return default(T);
    }
}
