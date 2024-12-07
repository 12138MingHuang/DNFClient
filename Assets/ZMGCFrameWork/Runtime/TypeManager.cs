using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class TypeManager
{
    private static IBehaviourExecution _behaviourExecution;
    
    public static void InitlizateWorldAssemblies(World world, IBehaviourExecution behaviourExecution)
    {
        _behaviourExecution = behaviourExecution;
        //湖片区Unity和创建的脚本所在的程序集
        Assembly[] assemblyArr = AppDomain.CurrentDomain.GetAssemblies();
        Assembly worldAssembly = null;
        //获取当前脚本运行的程序集
        foreach (var assembly in assemblyArr)
        {
            if (assembly.GetName().Name == "Assembly-CSharp")
            {
                worldAssembly = assembly;
                break;
            }
        }
        
        if(worldAssembly == null)
        {
            Debug.LogError("worldAssembly is Null Please Check Create Assembly");
            return;
        }
        
        //先获取当前游戏世界的命名空间
        //然后获取该命名空间下的所有脚本
        //判断当前脚本是否继承了Behaviour 如果基层的是框架脚本，就需要维护创建和销毁任务
        string nameSpace = world.GetType().Namespace;
        Type logicType = typeof(ILogicBehaviour);
        Type dataType = typeof(IDataBehaviour);
        Type msgType = typeof(IMsgBehaviour);
        //获取当前程序集下的所有类
        Type[] typeArr = worldAssembly.GetTypes();

        List<TypeOrder> logicBehaviourList = new List<TypeOrder>();
        List<TypeOrder> dataBehaviourList = new List<TypeOrder>();
        List<TypeOrder> msgBehaviourList = new List<TypeOrder>();
        
        foreach (var type in typeArr)
        {
            string space = type.Namespace;
            if (space == nameSpace)
            {
                if(type.IsAbstract)
                    continue;

                if (logicType.IsAssignableFrom(type))
                {
                    //获取当前类的初始顺序
                    int order = GetLogicBehaviourOrderIndex(type);
                    TypeOrder typeOrder = new TypeOrder(order, type);
                    logicBehaviourList.Add(typeOrder);
                }
                else if (dataType.IsAssignableFrom(type))
                {
                    int order = GetDataBehaviourOrderIndex(type);
                    TypeOrder typeOrder = new TypeOrder(order, type);
                    dataBehaviourList.Add(typeOrder);
                }
                else if (msgType.IsAssignableFrom(type))
                {
                    int order = GetMsgBehaviourOrderIndex(type);
                    TypeOrder typeOrder = new TypeOrder(order, type);
                    msgBehaviourList.Add(typeOrder);
                }
            }
        }
        //小的排在前面
        logicBehaviourList.Sort((a, b) => a.order.CompareTo(b.order));
        dataBehaviourList.Sort((a, b) => a.order.CompareTo(b.order));
        msgBehaviourList.Sort((a, b) => a.order.CompareTo(b.order));
        
        //初始化数据层脚本，消息层脚本，逻辑层脚本
        for (int i = 0; i < dataBehaviourList.Count; i++)
        {
            IDataBehaviour data = (IDataBehaviour)Activator.CreateInstance(dataBehaviourList[i].type);
            world.AddDataMgr(data);
        }
        for (int i = 0; i < msgBehaviourList.Count; i++)
        {
            IMsgBehaviour msg = (IMsgBehaviour)Activator.CreateInstance(msgBehaviourList[i].type);
            world.AddMsgMgr(msg);
        }
        for (int i = 0; i < logicBehaviourList.Count; i++)
        {
            ILogicBehaviour logic = (ILogicBehaviour)Activator.CreateInstance(logicBehaviourList[i].type);
            world.AddLogicCtrl(logic);
        }
        
        logicBehaviourList.Clear();
        dataBehaviourList.Clear();
        msgBehaviourList.Clear();
        _behaviourExecution = null;
    }
    
    private static int GetLogicBehaviourOrderIndex(Type type)
    {
        if (_behaviourExecution == null) return 999;
        Type[] logicTypes = _behaviourExecution.GetLogicBehaviourExecution();
        for (int i = 0; i < logicTypes.Length; i++)
        {
            if (logicTypes[i] == type)
            {
                return i;
            }
        }
        return 999;
    }

    private static int GetDataBehaviourOrderIndex(Type type)
    {
        if (_behaviourExecution == null) return 999;
        Type[] dataTypes = _behaviourExecution.GetDataBehaviourExecution();
        for (int i = 0; i < dataTypes.Length; i++)
        {
            if (dataTypes[i] == type)
            {
                return i;
            }
        }
        return 999;
    }

    private static int GetMsgBehaviourOrderIndex(Type type)
    {
        if (_behaviourExecution == null) return 999;
        Type[] msgTypes = _behaviourExecution.GetMsgBehaviourExecution();
        for (int i = 0; i < msgTypes.Length; i++)
        {
            if (msgTypes[i] == type)
            {
                return i;
            }
        }
        return 999;
    }
}
