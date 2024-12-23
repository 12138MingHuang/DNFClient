using System;
using ZMAssetFrameWork;

public class Skill
{
    /// <summary>
    /// 技能ID
    /// </summary>
    private int mSkillId;
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
    
    /// <summary>
    /// 创建技能
    /// </summary>
    /// <param name="skillId"> 技能id </param>
    /// <param name="skillCreator"> 技能创建者 </param>
    public Skill(int skillId, LogicActor skillCreator)
    {
        mSkillId = skillId;
        mSkillCreator = skillCreator;
        mSkillDataConfig = ZMAssetsFrame.LoadScriptableObject<SkillDataConfig>(AssetPathConfig.SKILL_DATA_PATH + skillId.ToString());
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
        PlayAnim();
    }

    /// <summary>
    /// 开始释放技能
    /// </summary>
    private void SkillStart()
    {
        // TODO: 开始释放技能时候，初始化将数据
    }

    /// <summary>
    /// 播放技能动画
    /// </summary>
    private void PlayAnim()
    {
        // TODO: 播放角色动画
    }

    /// <summary>
    /// 技能后摇
    /// </summary>
    private void SkillAfter()
    {
        
    }

    /// <summary>
    /// 技能结束
    /// </summary>
    private void SkillEnd()
    {
        
    }
}
