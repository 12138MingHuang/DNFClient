using System;
using UnityEngine;
using ZMAssetFrameWork;

/// <summary>
/// 技能状态
/// </summary>
public enum SkillState
{
    /// <summary>
    /// 空
    /// </summary>
    None,
    /// <summary>
    /// 技能前摇
    /// </summary>
    Before,
    
    /// <summary>
    /// 技能后摇
    /// </summary>
    After,
    
    /// <summary>
    /// 技能释放结束
    /// </summary>
    End
}

public partial class Skill
{
    /// <summary>
    /// 技能ID
    /// </summary>
    public int skillId;
    /// <summary>
    /// 技能创建者
    /// </summary>
    private LogicActor mSkillCreator;
    /// <summary>
    /// 技能数据配置
    /// </summary>
    private SkillDataConfig mSkillDataConfig;

    /// <summary>
    /// 技能释放后摇
    /// </summary>
    public Action<Skill> OnReleaseAfter;
    /// <summary>
    /// 技能释放结束
    /// </summary>
    public Action<Skill, bool> OnReleaseSkillEnd;

    public SkillState skillState = SkillState.None;
    
    /// <summary>
    /// 当前逻辑帧
    /// </summary>
    private int mCurLogicFrame = 0;
    /// <summary>
    /// 当前逻辑帧累计时间
    /// </summary>
    private int mCurLogicFrameAccTime = 0;
    
    /// <summary>
    /// 创建技能
    /// </summary>
    /// <param name="skillId"> 技能id </param>
    /// <param name="skillCreator"> 技能创建者 </param>
    public Skill(int skillId, LogicActor skillCreator)
    {
        this.skillId = skillId;
        mSkillCreator = skillCreator;
        mSkillDataConfig = ZMAssetsFrame.LoadScriptableObject<SkillDataConfig>(AssetPathConfig.SKILL_DATA_PATH + skillId.ToString() + ".asset");
    }

    /// <summary>
    /// 释放技能
    /// </summary>
    /// <param name="onReleaseAfter"> 技能后摇 </param>
    /// <param name="onReleaseSkillEnd"> 技能释放结束 </param>
    public void ReleaseSkill(Action<Skill> onReleaseAfter, Action<Skill, bool> onReleaseSkillEnd)
    {
        OnReleaseAfter = onReleaseAfter;
        OnReleaseSkillEnd = onReleaseSkillEnd;
        SkillStart();
        skillState = SkillState.Before;
        PlayAnim();
    }

    /// <summary>
    /// 开始释放技能
    /// </summary>
    private void SkillStart()
    {
        // 开始释放技能时候，初始化将数据
        mCurLogicFrame = 0;
        mCurLogicFrameAccTime = 0;
    }

    /// <summary>
    /// 播放技能动画
    /// </summary>
    private void PlayAnim()
    {
        mSkillCreator.PlayAnim(mSkillDataConfig.character.skillAnim);
    }

    public void OnLogicFrameUpdate()
    {
        if(skillState == SkillState.None) return;
        
        // 计算累计运行时间
        mCurLogicFrameAccTime = mCurLogicFrame * LogicFrameConfig.LogicFrameIntervalMS;
        
        // 处理技能后摇
        if (skillState == SkillState.Before && mCurLogicFrameAccTime >= mSkillDataConfig.skillConfig.skillShakeAfterMs)
        {
            SkillAfter();
        }
        
        // 更新不同配置的逻辑帧，处理不同配置的逻辑
        // 更新特效逻辑帧
        OnLogicFrameUpdateEffect();
        // 更新伤害逻辑帧
        // 更新行动逻辑帧
        // 更新音效逻辑帧
        // 更新子弹逻辑帧
        
        // 判断技能是否释放结束
        if (mCurLogicFrame == mSkillDataConfig.character.logicFrame)
        {
            SkillEnd();
        }
        
        // 逻辑帧自增
        mCurLogicFrame++;
    }

    /// <summary>
    /// 技能后摇
    /// </summary>
    private void SkillAfter()
    {
        skillState = SkillState.After;
    }

    /// <summary>
    /// 技能结束
    /// </summary>
    private void SkillEnd()
    {
        skillState = SkillState.End;
        OnReleaseSkillEnd?.Invoke(this, false);
    }
}
