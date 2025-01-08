using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理所有的buff 释放、移除 、更新逻辑
/// </summary>
public class BuffSystem : Singleton<BuffSystem>
{
    /// <summary>
    /// 所有Buff的列表
    /// </summary>
    private List<Buff> mBuffList = new List<Buff>();
    
    public void OnCreate()
    {
        
    }

    /// <summary>
    /// 附加一个buff
    /// </summary>
    /// <param name="buffId"> Buff唯一标识ID</param>
    /// <param name="releaser"> Buff施放者</param>
    /// <param name="attachTarget"> Buff附着目标</param>
    /// <param name="skill"> 隶属技能</param>
    /// <param name="paramsObjs"> Buff所需要的一些参数</param>
    public Buff AttachBuff(int buffId, LogicActor releaser, LogicActor attachTarget, Skill skill, object[] paramsObjs)
    {
        if (buffId == 0)
        {
            Debug.LogError("Buff id 不能为0，当前附加Buff为无效buff！");
            return null;
        }
        Buff buff = new Buff(buffId, releaser, attachTarget, skill, paramsObjs);
        buff.OnCreate();
        mBuffList.Add(buff);
        return buff;
    }

    /// <summary>
    /// Buff逻辑帧更新
    /// </summary>
    public void OnLogicFrameUpdate()
    {
        for (int i = mBuffList.Count - 1; i >= 0; i--)
        {
            mBuffList[i].OnLogicFrameUpdate();
        }
    }

    /// <summary>
    /// 移除一个指定buff
    /// </summary>
    /// <param name="buff"> 要移除的buff</param>
    public void RemoveBuff(Buff buff)
    {
        if (mBuffList.Contains(buff))
        {
            mBuffList.Remove(buff);
        }
    }
    
    public void OnDestroy()
    {
        for (int i = mBuffList.Count - 1; i >= 0; i--)
        {
            mBuffList[i].OnDestroy();
        }
        mBuffList.Clear();
    }
}