using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable] [HideMonoScript]
public class SkillConfig
{
    [HideInInspector]
    public bool showStockPileData = false; // 是否显示蓄力技能数据
    [HideInInspector]
    public bool showSkillGuide = false; // 是否显示技能引导数据

    [LabelText("技能图标"), LabelWidth(0.1f), PreviewField(70, ObjectFieldAlignment.Left), SuffixLabel("技能图标")]
    public Sprite skillIcon;
    
    [LabelText("技能id")]
    public string skillId;
    [LabelText("技能名称")]
    public string skillName;
    [LabelText("技能所需蓝量")]
    public int needMagicValue = 100;
    [LabelText("技能前摇时间")]
    public int skillShakeBeforeTimeMs;
    [LabelText("技能攻击持续时间")]
    public int skillAttackDurationMs;
    [LabelText("技能后摇时间")]
    public int skillShakeAfterMs;
    [LabelText("技能冷却时间")]
    public int skillCdTimeMs;
    [LabelText("技能类型"), OnValueChanged("OnSkillTypeChange")]
    public SkillType skillType;
    [LabelText("蓄力阶段配置(若第一阶段触发时间不为0,则空挡时间为动画表现时间)"), ShowIf("showStockPileData")]
    public List<StockPileStageData> stockPileStageData = new List<StockPileStageData>();
    [LabelText("技能引导特效"), ShowIf("showSkillGuide")]
    public GameObject skillGuideObj;
    [LabelText("技能引导范围"), ShowIf("showSkillGuide")]
    public float skillGuideRange;
    [LabelText("组合技能id(衔接下一个技能的id)"), Tooltip("比如：技能A由 技能 B C D 组成")]
    public int combinationSkillId;
    
    //技能渲染相关
    [LabelText("技能命中特效"), TitleGroup("技能渲染", "所有英雄渲染数据会在开始释放技能时触发")]
    public GameObject skillHitEffect;
    [LabelText("技能命中特效存活时间"), TitleGroup("技能渲染", "所有英雄渲染数据会在开始释放技能时触发")]
    public int hitEffectSurvivalTimeMs = 100;
    [LabelText("技能命中音效"), TitleGroup("技能渲染", "所有英雄渲染数据会在开始释放技能时触发")]
    public AudioClip skillHitAudio;
    [LabelText("是否显示技能立绘"), TitleGroup("技能渲染", "所有英雄渲染数据会在开始释放技能时触发")]
    public bool showSkillPortrait;
    [LabelText("技能立绘对象"), TitleGroup("技能渲染", "所有英雄渲染数据会在开始释放技能时触发"), ShowIf("showSkillPortrait")]
    public GameObject skillPortraitObj;
    [LabelText("技能描述"), TitleGroup("技能渲染", "所有英雄渲染数据会在开始释放技能时触发")]
    public string skillDes;

    /// <summary>
    /// 技能类型变化时，设置是否显示相关技能数据
    /// </summary>
    /// <param name="skillType"> 技能类型 </param>
    public void OnSkillTypeChange(SkillType skillType)
    {
        showStockPileData = skillType == SkillType.StockPile;
        showSkillGuide = skillType == SkillType.PosGuide;
    }
}

public enum SkillType
{
    [LabelText("无配置(瞬发技能)")] None,
    [LabelText("吟唱型技能")] Chnat,
    [LabelText("弹道型技能")] Ballistic,
    [LabelText("蓄力技能")] StockPile,
    [LabelText("位置引导技能")] PosGuide,
}

/// <summary>
/// 蓄力技能阶段数据
/// </summary>
[Serializable]
public class StockPileStageData
{
    [LabelText("蓄力阶段id")]
    public int stage;
    [LabelText("当前蓄力阶段触发的技能id")]
    public int skillId;
    [LabelText("当前阶段触发开始时间")]
    public int startTimeMs;
    [LabelText("当前阶段触发结束时间")]
    public int endTimeMs;
}
