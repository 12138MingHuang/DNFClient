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
    
    /// <summary>
    /// 当前组合技能列表
    /// </summary>
    private List<int> mCombinationSkillList = new List<int>();
    
    /// <summary>
    /// 当前释放的技能
    /// </summary>
    private Skill mCurReleaseSkill;

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
            Skill skill = new Skill(skillId, mSkillCreator);
            mSkillList.Add(skill);
            if(skill.SkillConfig.combinationSkillId != 0)
            {
                InitSkills(new int[] { skill.SkillConfig.combinationSkillId });
            }
            
            // 初始化蓄力技能
            if(skill.SkillConfig.stockPileStageData.Count > 0)
            {
                foreach (var stage in skill.SkillConfig.stockPileStageData)
                {
                    InitSkills(new int[] {stage.skillId});
                }
            }
        }
        Debug.Log("技能初始化完成, 技能个数：" + skillIdArr.Length);
    }

    public Skill ReleaseSkill(int skillId, Action<Skill> onReleaseAfter, Action<Skill> onReleaseSkillEnd)
    {
        // 如果当前的技能不为空与当前技能为前摇或者释放的状态就不能释放其他技能
        if(mCurReleaseSkill != null && mCurReleaseSkill.skillState != SkillState.End && mCurReleaseSkill.skillState != SkillState.After)
            return null;
        
        // 如果当前技能是组合技能，并且不在组合列表中就不能释放其他技能
        if(mCombinationSkillList.Count > 0 && !mCombinationSkillList.Contains(skillId))
            return null;
        
        foreach (var skill in mSkillList)
        {
            if(skill.skillId == skillId)
            {
                if (skill.skillState != SkillState.None && skill.skillState != SkillState.End)
                {
                    Debug.Log("skillId: " + skillId + " 技能正在释放中, 无法重复释放");
                    return null;
                }
                
                // 计算组合技能列表
                if (skill.SkillConfig.combinationSkillId != 0)
                {
                    CalculateCombinationSkillIdList(skill.SkillConfig.combinationSkillId);
                }
                
                // 释放技能
                skill.ReleaseSkill(onReleaseAfter, (sk, isCombinationSkill) =>
                {
                    // 技能释放完成回调
                    onReleaseSkillEnd?.Invoke(sk);
                    // 如果当前技能不是组合技能 就重置当前技能状态和组合列表
                    if (!isCombinationSkill)
                    {
                        mCurReleaseSkill = null;
                        if (skill.SkillConfig.combinationSkillId == 0 && mCombinationSkillList.Count > 0)
                        {
                            mCombinationSkillList.Clear();
                        }
                    }
                });
                mCurReleaseSkill = skill;
                return skill;
            }
        }
        
        Debug.LogError("skillId: " + skillId + " 技能不存在, 配置中没找到");
        return null;
    }
    /// <summary>
    /// 触发蓄力技能效果
    /// </summary>
    /// <param name="skillId"> 技能id</param>
    public void TriggerStockPileSkill(int skillId)
    {
        // 如果当前的技能不为空与当前技能为前摇或者释放的状态就不能触发蓄力效果
        if(mCurReleaseSkill != null && mCurReleaseSkill.skillId != skillId)
            return;
        
        // 如果当前技能是组合技能，并且不在组合列表中就不能触发蓄力效果
        if(mCombinationSkillList.Count > 0 && !mCombinationSkillList.Contains(skillId))
            return;
        
        Skill skill = GetSkill(skillId);
        if (skill != null)
        {
            skill.TriggerStockPileSkill();
        }
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

    /// <summary>
    /// 获取技能
    /// </summary>
    /// <param name="skillId"> 技能id</param>
    /// <returns> 技能</returns>
    public Skill GetSkill(int skillId)
    {
        foreach (var skill in mSkillList)
        {
            if(skill.skillId == skillId)
            {
                return skill;
            }
        }
        Debug.LogError("技能不存在, 配置中没找到");
        return null;
    }
    
    /// <summary>
    /// 计算组合技能id列表
    /// </summary>
    /// <param name="skillId"> 技能id</param>
    public void CalculateCombinationSkillIdList(int skillId)
    {
        if (skillId != 0)
        {
            int combinationSkillId = skillId;
            while (combinationSkillId != 0)
            {
                mCombinationSkillList.Add(combinationSkillId);
                combinationSkillId = GetSkill(combinationSkillId).SkillConfig.combinationSkillId;
            }
        }
        else
        {
            Debug.LogError("技能不存在, 配置中没找到");
        }
    }
}