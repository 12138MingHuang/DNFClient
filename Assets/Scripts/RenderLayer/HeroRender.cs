using System;
using System.Collections;
using System.Collections.Generic;
using FixMath;
using UnityEngine;

public class HeroRender : RenderObject
{
    /// <summary>
    /// 摇杆移动输入
    /// </summary>
    private Vector3 mInputMoveDir;
    /// <summary>
    /// 英雄逻辑层
    /// </summary>
    private HeroLogic mHeroLogic;

    /// <summary>
    /// 左手根部节点
    /// </summary>
    public Transform LeftHandRootTrans;
    /// <summary>
    /// 右手根部节点
    /// </summary>
    public Transform RightHandRootTrans;

    /// <summary>
    /// 技能引导特效对象
    /// </summary>
    private GameObject mSkillGuideEffectObj;
    
    /// <summary>
    /// 角色动画
    /// </summary>
    private Animation mAnim;
    
    public override void OnCreate()
    {
        base.OnCreate();
        mAnim = GetComponent<Animation>();
        mHeroLogic = logicObject as HeroLogic;
        JoystickUGUI.OnMoveCallBack += OnJoystickMove;
    }
    

    public override void OnRelease()
    {
        base.OnRelease();
        JoystickUGUI.OnMoveCallBack -= OnJoystickMove;
    }
    
    private void Start()
    {
        
    }

    protected override void Update()
    {
        base.Update();

        if (mHeroLogic.releasingSkillList == null || mHeroLogic.releasingSkillList.Count == 0)
        {
            // 判断摇杆是否有值输入，如果没有就待机，如果有就播放移动动画
            if (mInputMoveDir.x == 0 && mInputMoveDir.z == 0)
            {
                PlayAnim("Anim_Idle02");
            }
            else
            {
                PlayAnim("Anim_Run");
            }
        }
    }

    private void OnDestroy()
    {
        
    }
    
    /// <summary>
    /// 摇杆移动回调
    /// </summary>
    /// <param name="inputDir"> 输入方向值 </param>
    private void OnJoystickMove(Vector3 inputDir)
    {
        mInputMoveDir = inputDir;
        // 逻辑方向
        FixIntVector3 logicDir = FixIntVector3.zero;

        if (inputDir != Vector3.zero)
        {
            logicDir.x = inputDir.x;
            logicDir.y = inputDir.y;
            logicDir.z = inputDir.z;
        }
        // TODO 向英雄逻辑层直接输入操作帧事件 没有服务端情况下的测试代码
        mHeroLogic.InPutLogicFrameEvent(logicDir);
    }
    
    /// <summary>
    /// 播放角色动画
    /// </summary>
    /// <param name="animName"></param>
    private void PlayAnim(string animName)
    {
        mAnim.CrossFade(animName, 0.2f);
    }

    /// <summary>
    /// 通过动画文件播放动画
    /// </summary>
    /// <param name="clip"> 动画片段 </param>
    public override void PlayAnim(AnimationClip clip)
    {
        base.PlayAnim(clip);

        if (mAnim.GetClip(clip.name) == null)
        {
            mAnim.AddClip(clip, clip.name);
        }
        mAnim.clip = clip;
        PlayAnim(clip.name);
    }

    public override Transform GetTransParent(TransParentType parentType)
    {
        switch (parentType)
        {
            case TransParentType.LeftHand:
                return LeftHandRootTrans;
            case TransParentType.RightHand:
                return RightHandRootTrans;
        }
        return null;
    }

    /// <summary>
    /// 初始化技能引导特效对象
    /// </summary>
    /// <param name="skillId"> 技能ID </param>
    private void InitSkillGuide(int skillId)
    {
        if (mSkillGuideEffectObj == null)
        {
            Skill skill = mHeroLogic.GetSkill(skillId);
            mSkillGuideEffectObj = GameObject.Instantiate(skill.SkillConfig.skillGuideObj);
            mSkillGuideEffectObj.transform.localScale = Vector3.one;
        }
    }

    /// <summary>
    /// 更新技能引导特效对象位置和朝向
    /// </summary>
    /// <param name="skillGuideType"></param>
    /// <param name="skillId"></param>
    /// <param name="isPress"></param>
    /// <param name="pos"></param>
    /// <param name="skillRange"></param>
    public void UpdateSkillGuide(SkillGuideType skillGuideType, int skillId, bool isPress, Vector3 pos, float skillRange)
    {
        // 初始化引导特效
        InitSkillGuide(skillId);
        // 更新引导特效位置
        if (skillGuideType == SkillGuideType.Position)
        {
            Vector3 skillGuidePos = transform.position + pos;
            // 限制当前位置的z轴不能超过地图
            skillGuidePos = new Vector3(skillGuidePos.x, 0, Mathf.Clamp(skillGuidePos.z, -1f, 8.6f));
            mSkillGuideEffectObj.transform.position = skillGuidePos;
        }
    }

    /// <summary>
    /// 释放技能引导特效对象
    /// </summary>
    public void OnGuideRelease()
    {
        if (mSkillGuideEffectObj != null)
        {
            GameObject.Destroy(mSkillGuideEffectObj);
        }
    }
}
