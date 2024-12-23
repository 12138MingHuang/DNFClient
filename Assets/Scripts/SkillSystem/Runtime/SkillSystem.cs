using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem
{
    /// <summary>
    /// 技能系统创建者
    /// </summary>
    private LogicActor mSkillCreator;

    /// <summary>
    /// 技能列表
    /// </summary>
    private List<Skill> mSkillList = new List<Skill>();

    public SkillSystem(LogicActor skillCreator)
    {
        mSkillCreator = skillCreator;
    }

    /// <summary>
    /// 初始化技能
    /// </summary>
    /// <param name="skillIdArr"> 技能id数组</param>
    public void InitSkills(int[] skillIdArr)
    {
        foreach (var skillId in skillIdArr)
        {
            mSkillList.Add(new Skill(skillId, mSkillCreator));
        }
        Debug.Log("技能初始化完成, 技能个数：" + skillIdArr.Length);
    }

    public Skill ReleaseSkill(int skillId, Action<Skill> onReleaseAfter, Action<Skill> onReleaseSkillEnd)
    {
        foreach (var skill in mSkillList)
        {
            if(skill.skillId == skillId)
            {
                // 释放技能
                skill.ReleaseSkill(onReleaseAfter, (sk, isCombinationSkill) =>
                {
                    // 技能释放完成回调
                    onReleaseSkillEnd?.Invoke(sk);
                    // 如果当前技能是组合技能，处理技能组的逻辑
                    if (isCombinationSkill)
                    {
                        // TODO: 技能组释放完成，处理技能组的逻辑
                    }
                });
                
                return skill;
            }
        }
        
        Debug.LogError("skillId: " + skillId + " 技能不存在, 配置中没找到");
        return null;
    }
    
    /// <summary>
    /// 技能逻辑帧更新
    /// </summary>
    public void OnLogicFrameUpdate()
    {
        foreach (var skill in mSkillList)
        {
            skill.OnLogicFrameUpdate();
        }
    }
}