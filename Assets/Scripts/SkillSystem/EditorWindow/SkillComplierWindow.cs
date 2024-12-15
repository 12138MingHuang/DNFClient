using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class SkillComplierWindow : OdinEditorWindow
{
    [TabGroup("Skill", "Character", SdfIconType.PersonFill, TextColor = "orange")]
    public SkillCharacterConfig character = new SkillCharacterConfig();
    [TabGroup("SkillComplier", "Skill", SdfIconType.Robot, TextColor = "lightmagenta")]
    public SkillConfig skill = new SkillConfig();
    [TabGroup("SkillComplier", "Damage", SdfIconType.At, TextColor = "lightmagenta")]
    public List<SkillDamageConfig> damageList = new List<SkillDamageConfig>();
    [TabGroup("SkillComplier", "Effect", SdfIconType.OpticalAudio, TextColor = "blue")]
    public List<SkillEffectConfig> effectList = new List<SkillEffectConfig>();

#if UNITY_EDITOR
    
    /// <summary>
    /// 是否开始播放技能
    /// </summary>
    private bool isStartPlaySkill = false;
    
    [MenuItem("Skill/技能编辑器")]
    public static SkillComplierWindow ShowWindow()
    {
        return GetWindowWithRect<SkillComplierWindow>(new Rect(0, 0, 1000, 600));
    }

    /// <summary>
    /// 保存技能数据
    /// </summary>
    public void SaveSkillData()
    {
        SkillDataConfig.SaveSkillData(character, skill, effectList, damageList);
    }
    
    /// <summary>
    /// 加载技能数据
    /// </summary>
    /// <param name="skillData"> 技能数据 </param>
    public void LoadSkillData(SkillDataConfig skillData)
    {
        character = skillData.character;
        skill = skillData.skillConfig;
        effectList = skillData.effectCfgList;
        damageList = skillData.damageCfgList;
    }
    
    /// <summary>
    /// 获取技能编辑窗口
    /// </summary>
    /// <returns> 技能编辑窗口 </returns>
    public static SkillComplierWindow GetWindow()
    {
        return GetWindow<SkillComplierWindow>();
    }

    /// <summary>
    /// 获取Editor模式下角色位置
    /// </summary>
    /// <returns> 角色位置 </returns>
    public static Vector3 GetCharacterPos()
    {
        SkillComplierWindow window = GetWindow<SkillComplierWindow>();
        if(window.character.skillCharacter != null)
        {
            return window.character.skillCharacter.transform.position;
        }
        return Vector3.zero;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        foreach (var damage in damageList)
        {
            damage.OnInit();
        }
        EditorApplication.update += OnEditorUpdate;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        foreach (var damage in damageList)
        {
            damage.OnRelease();
        }
        EditorApplication.update -= OnEditorUpdate;
    }
    
    /// <summary>
    /// 开始播放技能
    /// </summary>
    public void StartPlaySkill()
    {
        foreach (var item in effectList)
        {
            item.StartPlaySkill();
        }

        foreach (var item in damageList)
        {
            item.StartPlaySkill();
        }
        isStartPlaySkill = true;
        mAccLogicRunTime = 0;
        mNextLogicFrameTime = 0;
        mLastUpdateTime = 0;
    }

    /// <summary>
    /// 暂停播放技能
    /// </summary>
    public void PausePlaySkill()
    {
        foreach (var item in effectList)
        {
            item.SkillPause();
        }
        
        foreach (var item in damageList)
        {
            item.EndPlaySkill();
        }
    }

    /// <summary>
    /// 结束播放技能
    /// </summary>
    public void EndPlaySkill()
    {
        foreach (var item in effectList)
        {
            item.PlaySkillEnd();
        }

        foreach (var item in damageList)
        {
            item.EndPlaySkill();
        }
        isStartPlaySkill = false;
        mAccLogicRunTime = 0;
        mNextLogicFrameTime = 0;
        mLastUpdateTime = 0;
    }

    private void OnEditorUpdate()
    {
        try
        {
            character.OnUpdate(Focus);

            if (isStartPlaySkill)
            {
                OnLogicUpdate();
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    // 逻辑帧累计运行时间
    private float mAccLogicRunTime;
    // 下一个逻辑帧的时间
    private float mNextLogicFrameTime;
    // 动画缓动时间 当前帧的增量时间
    private float mDeltaTime;
    // 上次更新的时间
    private double mLastUpdateTime;
    
    /// <summary>
    /// 逻辑Update
    /// </summary>
    private void OnLogicUpdate()
    {
        // 模拟帧同步更新，以0.066秒的间隔进行更新
        if (mLastUpdateTime == 0)
        {
            mLastUpdateTime = EditorApplication.timeSinceStartup;
        }
        //计算逻辑帧累计运行时间
        mAccLogicRunTime = (float)(EditorApplication.timeSinceStartup - mLastUpdateTime);
        while (mAccLogicRunTime > mNextLogicFrameTime)
        {
            OnLogicFrameUpdate();
            //下一个逻辑帧的时间
            mNextLogicFrameTime += LogicFrameConfig.LogicFrameInterval;
        }
    }

    private void OnLogicFrameUpdate()
    {
        foreach (var item in effectList)
        {
            item.OnLogicFrameUpdate();
        }

        foreach (var item in damageList)
        {
            item.OnLogicFrameUpdate();
        }
    }

#endif
    
}
