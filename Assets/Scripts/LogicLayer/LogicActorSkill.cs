using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZMGC.Battle;

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
    private int[] mNormalSkillIdArr = new int[] { 1001, 1002, 1003 };
    /// <summary>
    /// 技能id数组
    /// </summary>
    private int[] mSkillIdArr = new int[] { 1004 };

    /// <summary>
    /// 正在释放的技能列表
    /// </summary>
    public List<Skill> releasingSkillList = new List<Skill>();
    
    /// <summary>
    /// 当前普通攻击技能组合索引
    /// </summary>
    private int mCurNormalComboIndex = 0;

    /// <summary>
    /// 初始化技能
    /// </summary>
    public void InitActorSkill()
    {
        HeroDataMgr heroData = BattleWorld.GetExitsDataMgr<HeroDataMgr>();
        mNormalSkillIdArr = heroData.GetHeroNormalSkillIdArray(1000);
        mSkillIdArr = heroData.GetHeroSkillIdArray(1000);
        mSkillSystem = new SkillSystem(this);
        mSkillSystem.InitSkills(mNormalSkillIdArr);
        mSkillSystem.InitSkills(mSkillIdArr);
    }

    /// <summary>
    /// 释放普通攻击
    /// </summary>
    public void ReleaseNormalAttack()
    {
        ReleaseSkill(mNormalSkillIdArr[mCurNormalComboIndex]);
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
            if (!IsNormalAttackSkill(skill.skillId))
            {
                mCurNormalComboIndex = 0;
            }
            ActionState = LogicObjectActionState.SkillReleasing;
        }
    }

    /// <summary>
    /// 触发蓄力技能
    /// </summary>
    /// <param name="skillId"> 技能id </param>
    public void TriggerStockPileSkill(int skillId)
    {
        mSkillSystem.TriggerStockPileSkill(skillId);
    }

    /// <summary>
    /// 是否是普通攻击技能
    /// </summary>
    /// <param name="skillId"> 技能id </param>
    /// <returns> 是否是普通攻击技能 </returns>
    private bool IsNormalAttackSkill(int skillId)
    {
        foreach (int id in mNormalSkillIdArr)
        {
            if (skillId == id)
                return true;
        }
        return false;
    }

    /// <summary>
    /// 技能释放后摇
    /// </summary>
    /// <param name="skill"> 所释放技能 </param>
    private void OnSkillReleaseAfter(Skill skill)
    {
        if (!IsNormalAttackSkill(skill.skillId))
        {
            mCurNormalComboIndex = 0;
        }
        else
        {
            mCurNormalComboIndex++;
            // 如果普通攻击技能组合索引大于等于普通攻击技能id数组长度，则重置为0
            if (mCurNormalComboIndex >= mNormalSkillIdArr.Length)
                mCurNormalComboIndex = 0;
        }
    }

    /// <summary>
    /// 技能释放结束
    /// </summary>
    /// <param name="skill"> 所释放技能 </param>
    private void OnSkillReleaseEnd(Skill skill)
    {
        releasingSkillList.Remove(skill);
        if (releasingSkillList.Count == 0)
        {
            ActionState = LogicObjectActionState.Idle;
            mCurNormalComboIndex = 0;
        }
    }
    
    /// <summary>
    /// 逻辑帧更新技能
    /// </summary>
    public void OnLogicFrameUpdateSkill()
    {
        mSkillSystem.OnLogicFrameUpdate();
    }
    
    /// <summary>
    /// 获取技能
    /// </summary>
    /// <param name="skillId"> 技能id </param>
    /// <returns> 技能 </returns>
    public Skill GetSkill(int skillId)
    {
        return mSkillSystem.GetSkill(skillId);
    }
}
