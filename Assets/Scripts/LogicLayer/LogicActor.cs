using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LogicActor : LogicObject
{
    public override void OnCreate()
    {
        base.OnCreate();
        InitActorSkill();
    }

    public override void OnLogicFrameUpdate()
    {
        base.OnLogicFrameUpdate();
        // 更新移动帧
        OnLogicFrameUpdateMove();
        // 更新技能帧
        OnLogicFrameUpdateSkill();
        // 更新重力帧
        OnLogicFrameUpdateGravity();
    }
    
    /// <summary>
    /// 播放动画
    /// </summary>
    /// <param name="characterSkillAnim"> 角色动画 </param>
    public void PlayAnim(AnimationClip characterAnim)
    {
        RenderObject.PlayAnim(characterAnim);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}
