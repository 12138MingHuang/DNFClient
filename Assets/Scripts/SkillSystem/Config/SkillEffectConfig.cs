using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable] [HideMonoScript]
public class SkillEffectConfig
{
    [AssetList] [LabelText("技能特效对象")] [PreviewField(70, ObjectFieldAlignment.Left)]
    public GameObject skillEffect;
    
    [Title("当技能为蓄力技能时候，特效存活时间跟随技能持续时间，触发帧功能保留，其他配置全部无效")]
    [LabelText("触发帧")]
    public int triggerFrame = 0;
    [LabelText("结束帧")]
    public int endFrame = 0;
    [LabelText("特效偏移位置")] 
    public Vector3 effectOffsetPos = Vector3.zero;
    [LabelText("特效位置类型")]
    public EffectPosType effectPosType;
    [ToggleGroup("isSetTransParent", "是否设置特效父节点")]
    public bool isSetTransParent = false;
    [ToggleGroup("isSetTransParent", "节点类型")]
    public TransParentType transParent;

#if UNITY_EDITOR

    // Editor模式下克隆的特效对象
    private GameObject mCloneEffect;
    // 动画代理
    private AnimationAgent mAnimationAgent;
    // 粒子代理
    private ParticlesAgent mParticlesAgent;
    // 当前逻辑帧
    private int mCurLogicFrame = 0;

    /// <summary>
    /// 开始播放技能
    /// </summary>
    public void StartPlaySkill()
    {
        mCurLogicFrame = 0;
        DestroyEffect();
    }
    
    /// <summary>
    /// 技能暂停
    /// </summary>
    public void SkillPause()
    {
        DestroyEffect();
    }

    /// <summary>
    /// 播放技能结束
    /// </summary>
    public void PlaySkillEnd()
    {
        DestroyEffect();
    }

    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    public void OnLogicFrameUpdate()
    {
        if (mCurLogicFrame == triggerFrame)
        {
            CreateEffect();
        }
        else if (mCurLogicFrame == endFrame)
        {
            DestroyEffect();
        }
        mCurLogicFrame++;
    }

    /// <summary>
    /// 创建特效
    /// </summary>
    private void CreateEffect()
    {
        if(skillEffect != null)
        {
            mCloneEffect = GameObject.Instantiate(skillEffect);
            mCloneEffect.transform.position = SkillComplierWindow.GetCharacterPos();

            mAnimationAgent = new AnimationAgent();
            mAnimationAgent.InitPlayAnim(mCloneEffect.transform);
            
            mParticlesAgent = new ParticlesAgent();
            mParticlesAgent.InitPlayParticles(mCloneEffect.transform);
        }
    }

    /// <summary>
    /// 销毁特效
    /// </summary>
    private void DestroyEffect()
    {
        if (mCloneEffect != null)
        {
            GameObject.DestroyImmediate(mCloneEffect);
        }

        if (mAnimationAgent != null)
        {
            mAnimationAgent.OnDestroy();
            mAnimationAgent = null;
        }
        
        if (mParticlesAgent != null)
        {
            mParticlesAgent.OnDestroy();
            mParticlesAgent = null;
        }    
    }

#endif
}

public enum EffectPosType
{
    [LabelText("跟随角色位置和方向")] FollowPosDir,
    [LabelText("跟随角色方向")] FollowDir,
    [LabelText("屏幕中心位置")] CenterPos,
    [LabelText("引导位置")] GuidePos,
    [LabelText("跟随特效移动位置")] FollowEffectMovePos
}

public enum TransParentType
{
    [LabelText("无配置")] None,
    [LabelText("左手")] LeftHand,
    [LabelText("右手")] RightHand
}