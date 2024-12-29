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
    /// 技能配置
    /// </summary>
    public SkillConfig SkillConfig => mSkillDataConfig.skillConfig;

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
    /// 是否自动匹配蓄力阶段
    /// </summary>
    private bool mAutoMatchStockStage;
    
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
        mAutoMatchStockStage = false;
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
        if(skillState == SkillState.None || skillState == SkillState.End) return;
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
        OnLogicFrameUpdateDamage();
        // 更新行动逻辑帧
        OnLogicFrameUpdateAction();
        // 更新音效逻辑帧
        OnLogicFrameUpdateAudio();
        // 更新子弹逻辑帧
        
        // 蓄力技能需要通过蓄力时间进行触发，所以和技能的结束逻辑分开处理
        if (mSkillDataConfig.skillConfig.skillType == SkillType.StockPile)
        {
            int stockDaraCount = mSkillDataConfig.skillConfig.stockPileStageData.Count;
            if (stockDaraCount > 0)
            {
                // 1.处理手指按下立马抬起的一种情况
                if (mAutoMatchStockStage)
                {
                    // 自动匹配第一阶段蓄力技能进行释放
                    StockPileStageData stageData = mSkillDataConfig.skillConfig.stockPileStageData[0];
                    if (mCurLogicFrameAccTime >= stageData.startTimeMs)
                    {
                        StockPileFinish(stageData);
                    }
                }
                else
                {
                    // 2.处理超时蓄力的逻辑
                    StockPileStageData stageData = mSkillDataConfig.skillConfig.stockPileStageData[stockDaraCount - 1];
                    // 计算蓄力时间是否达到最大值，如果达到最大值就自动触发最大蓄力技能（比如按着蓄力不动）
                    if (mCurLogicFrameAccTime >= stageData.endTimeMs)
                    {
                        StockPileFinish(stageData);
                    }
                }
            }
        }
        else
        {
            // 判断技能是否释放结束
            if (mCurLogicFrame == mSkillDataConfig.character.logicFrame)
            {
                SkillEnd();
            }
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
        OnReleaseAfter?.Invoke(this);
    }

    /// <summary>
    /// 技能结束
    /// </summary>
    private void SkillEnd()
    {
        skillState = SkillState.End;
        OnReleaseSkillEnd?.Invoke(this, false);
        ReleaseAllEffect();
        if (mSkillDataConfig.skillConfig.combinationSkillId != 0)
        {
            mSkillCreator.ReleaseSkill(mSkillDataConfig.skillConfig.combinationSkillId);
        }
    }
    
    /// <summary>
    /// 主动触发蓄力技能
    /// </summary>
    public void TriggerStockPileSkill()
    {
        // 1.蓄力技能符合蓄力阶段配置中的某一个技能
        foreach (var stageData in mSkillDataConfig.skillConfig.stockPileStageData)
        {
            if(mCurLogicFrameAccTime >= stageData.startTimeMs && mCurLogicFrameAccTime <= stageData.endTimeMs)
            {
                StockPileFinish(stageData);
                return;
            }
        }
        
        // 2.蓄力技能不符合蓄力阶段配置中的任何一个
        mAutoMatchStockStage = true;
    }

    /// <summary>
    /// 蓄力技能释放完成
    /// </summary>
    /// <param name="stageData"> 蓄力阶段数据 </param>
    public void StockPileFinish(StockPileStageData stageData)
    {
        SkillEnd();
        if (stageData.skillId == 0)
        {
            Debug.LogError("技能蓄力阶段数据错误, 技能id为0, 无法释放技能!");
        }
        else
        {
            mSkillCreator.ReleaseSkill(stageData.skillId);
        }
    }
}
