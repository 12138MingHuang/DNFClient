using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillItem : MonoBehaviour
{
    public Text cdText;
    public Image iconImage;
    public Image cdMaskImage;
    public SKillItemJoyStick skillJoystick;

    private Skill mSkillData;
    private LogicActor mSkillCreator;
    
    /// <summary>
    /// 设置技能数据，并初始化摇杆数据
    /// </summary>
    /// <param name="skillData"> 技能数据</param>
    /// <param name="skillCreator"> 技能创建者</param>
    public void SetItemSkillData(Skill skillData, LogicActor skillCreator)
    {
        mSkillData = skillData;
        mSkillCreator = skillCreator;
        // 初始化技能摇杆数据
        skillJoystick.InitSkillData(GetSkillGuideType(skillData.SkillConfig.skillType), skillData.skillId, skillData.SkillConfig.skillGuideRange);
        skillJoystick.OnReleaseSkill += OnTriggerSkill;
        skillJoystick.OnSkillGuide += OnUpdateSkillGuide;
        
        iconImage.sprite = mSkillData.SkillConfig.skillIcon;
        cdText.gameObject.SetActive(false);
        cdMaskImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// 触发对应的技能技能（释放摇杆时触发）
    /// </summary>
    /// <param name="skillGuideType"> 摇杆类型</param>
    /// <param name="skillPos"> 技能释放位置</param>
    /// <param name="skillId"> 技能ID</param>
    private void OnTriggerSkill(SkillGuideType skillGuideType, Vector3 skillPos, int skillId)
    {
        switch(skillGuideType)
        {
            case SkillGuideType.Click:
                mSkillCreator.ReleaseSkill(skillId);
                break;
            case SkillGuideType.LongPress:
                // 蓄力技能释放逻辑
                mSkillCreator.TriggerStockPileSkill(skillId);
                break;
            case SkillGuideType.Position:
                // TODO: 位置引导技能释放逻辑
                break;
        }
    }
    
    /// <summary>
    /// 更新技能引导（摇杆移动时触发）
    /// </summary>
    /// <param name="skillGuide"> 摇杆类型</param>
    /// <param name="isCancel"> 是否取消引导</param>
    /// <param name="skillPos"> 技能释放位置</param>
    /// <param name="skillId"> 技能ID</param>
    /// <param name="skillDirDis"> 技能方向距离</param>
    private void OnUpdateSkillGuide(SkillGuideType skillGuide, bool isCancel, Vector3 skillPos, int skillId, float skillDirDis)
    {
        switch (skillGuide)
        {
            case SkillGuideType.LongPress:
                // 蓄力技能逻辑
                mSkillCreator.ReleaseSkill(skillId);
                break;
            case SkillGuideType.Position:
                // TODO: 位置引导技能更新逻辑
                break;
        }
    }
    
    /// <summary>
    /// 获取技能摇杆类型
    /// </summary>
    /// <param name="skillType"> 技能类型</param>
    /// <returns> 摇杆类型</returns>
    private SkillGuideType GetSkillGuideType(SkillType skillType)
    {
        SkillGuideType skillGuideType = SkillGuideType.Click;
        switch (skillType)
        {
            case SkillType.StockPile:
                skillGuideType = SkillGuideType.LongPress;
                break;
            case SkillType.Ballistic:
            case SkillType.Chnat:
            case SkillType.None:
                skillGuideType = SkillGuideType.Click;
                break;
            case SkillType.PosGuide:
                skillGuideType = SkillGuideType.Position;
                break;
        }
        return skillGuideType;
    }

    private void OnDestroy()
    {
        skillJoystick.OnReleaseSkill -= OnTriggerSkill;
        skillJoystick.OnSkillGuide -= OnUpdateSkillGuide;
    }
}
