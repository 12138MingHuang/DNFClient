using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 处理逻辑对象技能
/// </summary>
public partial class LogicActor
{

    /// <summary>
    /// 技能系统
    /// </summary>
    private SkillSystem mSkillSystem;

    /// <summary>
    /// 普通攻击技能id数组
    /// </summary>
    private int[] mNormalSkillIdArr = new int[] { 1001 };

    /// <summary>
    /// 正在释放的技能列表
    /// </summary>
    public List<Skill> releasingSkillList = new List<Skill>();

    /// <summary>
    /// 初始化技能
    /// </summary>
    public void InitActorSkill()
    {
        mSkillSystem = new SkillSystem(this);
        mSkillSystem.InitSkills(mNormalSkillIdArr);
    }

    /// <summary>
    /// 释放技能
    /// </summary>
    /// <param name="skillId"> 技能id </param>
    public void ReleaseSkill(int skillId)
    {
        Skill skill = mSkillSystem.ReleaseSkill(skillId, OnSkillReleaseAfter, OnSkillReleaseEnd);
        if (skill != null)
        {
            releasingSkillList.Add(skill);
        }
    }

    /// <summary>
    /// 技能释放后摇
    /// </summary>
    /// <param name="skill"> 所释放技能 </param>
    private void OnSkillReleaseAfter(Skill skill)
    {
        
    }

    /// <summary>
    /// 技能释放结束
    /// </summary>
    /// <param name="skill"> 所释放技能 </param>
    private void OnSkillReleaseEnd(Skill skill)
    {
        releasingSkillList.Remove(skill);
    }
    
    /// <summary>
    /// 逻辑帧更新技能
    /// </summary>
    public void OnLogicFrameUpdateSkill()
    {
        mSkillSystem.OnLogicFrameUpdate();
    }
}
