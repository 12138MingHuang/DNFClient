using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager
{
    
    /// <summary>
    /// 游戏世界列表
    /// </summary>
    private static List<World> _worldList = new List<World>();

    /// <summary>
    /// 默认游戏世界
    /// </summary>
    public static World DefaultGameWorld
    {
        get;
        private set;
    }
    
    /// <summary>
    /// 构建一个游戏世界
    /// </summary>
    /// <typeparam name="T">游戏世界类型</typeparam>
    public static void CreateWorld<T>() where T: World, new()
    {
        T world = new T();
        DefaultGameWorld = world;
        
        //初始化当前游戏世界的程序集脚本
        TypeManager.InitlizateWorldAssemblies(world, GetBehaviourExecution(world));
        world.OnCreate();
        _worldList.Add(world);
    }
    
    public static IBehaviourExecution GetBehaviourExecution(World world)
    {
        if (world.GetType().Name == "HallWorld")
        {
            return new HallWorldScriptExecutionOrder();
        }
        return null;
    }

    public static void DestroyWorld<T>() where T : World
    {
        for (int i = 0; i < _worldList.Count; i++)
        {
            if(_worldList[i].GetType().Name == typeof(T).Name)
            {
                _worldList[i].DestroyWorld(typeof(T).Namespace);
                _worldList.Remove(_worldList[i]);
                break;
            }
        }
    }
}
