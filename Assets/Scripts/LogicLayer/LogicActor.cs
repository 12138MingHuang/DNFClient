using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LogicActor : LogicObject
{
    public override void OnCreate()
    {
        base.OnCreate();
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
        // 更新子弹帧
        OnLogicFrameUpdateBullet();
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
    /// 播放动画
    /// </summary>
    /// <param name="animName"> 动画名称 </param>
    public void PlayAnim(string animName)
    {
        RenderObject.PlayAnim(animName);
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
    /// buff伤害效果，比如冰冻，减速等效果造成的伤害
    /// </summary>
    /// <param name="damageValue"> 伤害值 </param>
    /// <param name="skillDamageConfig"> 技能伤害配置 </param>
    public virtual void BuffDamage(FixInt damageValue, SkillDamageConfig skillDamageConfig)
    {
        Debug.Log("BuffDamage:" + damageValue);
        CalculateDamage(damageValue, DamageSource.Skill);
    }
    
    /// <summary>
    /// 子弹伤害效果，比如子弹造成的伤害
    /// </summary>
    public virtual void BulletDamage(FixInt damageValue, SkillDamageConfig skillDamageConfig)
    {
        Debug.Log("BulletDamage: " + damageValue);
        CalculateDamage(damageValue, DamageSource.Bullet);
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
            // 1.对象逻辑层血量减少
            ReduceHP(damage);
            // 2.判断对象是否死亡，如果死亡就处理死亡逻辑
            if (HP <= 0)
            {
                Collider.Active = false;
                ObjectState = LogicObjectState.Death;
                RenderObject.OnDeath();
            }
            // 3.进行伤害飘字渲染
            RenderObject.Damage(damage.RawInt, damageSource);
        }
    }

    /// <summary>
    /// 受击
    /// </summary>
    /// <param name="hitEffect"> 受击特效 </param>
    /// <param name="hitEffectSurvivalTimeMs"> 受击特效存活时间 </param>
    /// <param name="source"> 施法者 </param>
    /// <param name="logicXAxis"> 逻辑x轴 </param>
    public virtual void OnHit(GameObject hitEffect, int hitEffectSurvivalTimeMs, LogicObject source, FixInt logicXAxis)
    {
        RenderObject.OnHit(hitEffect, hitEffectSurvivalTimeMs, source);
    }
    
    /// <summary>
    /// 浮动效果
    /// </summary>
    /// <param name="isUping"> 是否向上中 </param>
    public virtual void Floating(bool isUping)
    {
        
    }
    
    /// <summary>
    /// 触发地面
    /// </summary>
    public virtual void TriggerGround()
    {
        
    }
    
    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}
