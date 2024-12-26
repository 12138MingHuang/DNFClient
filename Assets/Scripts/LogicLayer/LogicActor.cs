using FixMath;
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

    /// <summary>
    /// 角色技能伤害
    /// </summary>
    /// <param name="skillDamageConfig"> 技能伤害配置 </param>
    public virtual void SkillDamage(FixInt damageValue, SkillDamageConfig skillDamageConfig)
    {
        Debug.Log("SkillDamage: " + damageValue);
        CalculateDamage(damageValue, DamageSource.Skill);
    }
    
    /// <summary>
    /// 计算伤害
    /// </summary>
    /// <param name="damage"> 伤害值 </param>
    /// <param name="damageSource"> 伤害来源 </param>
    private void CalculateDamage(FixInt damage, DamageSource damageSource)
    {
        if (ObjectState == LogicObjectState.Survival)
        {
            // 1.对象逻辑层血量减少 TODO
            
            // 2.判断对象是否死亡，如果死亡就处理死亡逻辑 TODO
            
            // 3.进行伤害飘字渲染
            RenderObject.Damage(damage.RawInt, damageSource);
        }
    }

    /// <summary>
    /// 受击
    /// </summary>
    /// <param name="hitEffect"> 受击特效 </param>
    /// <param name="hitEffectSurvivalTimeMs"> 受击特效存活时间 </param>
    /// <param name="skillCreator"> 施法者 </param>
    public void OnHit(GameObject hitEffect, int hitEffectSurvivalTimeMs, LogicActor skillCreator)
    {
        RenderObject.OnHit(hitEffect, hitEffectSurvivalTimeMs, skillCreator);
    }
    
    public override void OnDestroy()
    {
        base.OnDestroy();
        RenderObject.OnRelease();
    }
}
