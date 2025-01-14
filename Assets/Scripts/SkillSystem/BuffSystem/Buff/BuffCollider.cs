using FixIntPhysics;
using FixMath;
using System;
using System.Collections.Generic;
using ZMGC.Battle;

public class BuffCollider
{
    /// <summary>
    /// Buff碰撞器
    /// </summary>
    private ColliderBehaviour mBuffCollider;
    /// <summary>
    /// Buff配置信息
    /// </summary>
    private BuffConfig mBuffConfig;
    /// <summary>
    /// Buff技能伤害配置信息
    /// </summary>
    private SkillDamageConfig mDamageConfig;
    /// <summary>
    /// Buff释放者
    /// </summary>
    private LogicActor mReleaser;
    /// <summary>
    /// Buff附着目标
    /// </summary>
    private LogicActor mAttachTarget;
    /// <summary>
    /// 隶属技能
    /// </summary>
    private Skill mSkill;

    public BuffCollider(Buff buff)
    {
        mBuffConfig = buff.BuffConfig;
        mDamageConfig = buff.BuffConfig.targetConfig.damageConfig;
        mReleaser = buff.releaser;
        mAttachTarget = buff.attachTarget;
        mSkill = buff.skill;
    }

    /// <summary>
    /// 创建或更新碰撞器
    /// </summary>
    /// <param name="followObj"> 跟随对象 </param>
    /// <returns> 碰撞器 </returns>
    public ColliderBehaviour CreateOrUpdateCollider(LogicObject followObj = null)
    {
        switch (mDamageConfig.detectionMode)
        {
            case DamageDetectionMode.Box3D:
                FixIntVector3 boxSize = new FixIntVector3(mDamageConfig.boxSize);
                FixIntVector3 boxOffset = new FixIntVector3(mDamageConfig.boxOffset);
                
                // 限制y轴的偏移只能往上偏移
                boxOffset.y = FixIntMath.Abs(boxOffset.y);
                if(mBuffCollider == null)
                    mBuffCollider = new FixIntBoxCollider(boxSize, boxOffset);
                
                mBuffCollider.SetBoxData(boxOffset, boxSize);
                mBuffCollider.UpdateColliderInfo(GetBuffPos(), boxSize);
                break;
            case DamageDetectionMode.Sphere3D:
                FixIntVector3 sphereOffset = new FixIntVector3(mDamageConfig.sphereOffset);
                sphereOffset.y = FixIntMath.Abs(sphereOffset.y);
                
                if(mBuffCollider == null)
                    mBuffCollider = new FixIntSphereCollider(mDamageConfig.radius, sphereOffset);
                
                mBuffCollider.SetBoxData(mDamageConfig.radius, sphereOffset);
                mBuffCollider.UpdateColliderInfo(GetBuffPos(), FixIntVector3.zero, mDamageConfig.radius);
                break;
        }
        return mBuffCollider;
    }

    /// <summary>
    /// 计算碰撞器命中目标对象
    /// </summary>
    /// <returns></returns>
    public List<LogicActor> CalculateColliderTargetObjects()
    {
        // 获取敌人目标列表
        List<LogicActor> enemyList = BattleWorld.GetExitsLogicCtrl<BattleLogicCtrl>().GetEnemyList(mReleaser.ObjectType);
        
        // 通过碰撞检测，检测碰撞器命中目标对象
        List<LogicActor> damageTargetList = new List<LogicActor>();
        foreach (LogicActor enemy in enemyList)
        {
            switch (mDamageConfig.detectionMode)
            {
                case DamageDetectionMode.Box3D:
                    if (PhysicsManager.IsCollision(mBuffCollider as FixIntBoxCollider, enemy.Collider))
                    {
                        damageTargetList.Add(enemy);
                    }
                    break;
                case DamageDetectionMode.Sphere3D:
                    if (PhysicsManager.IsCollision(enemy.Collider, mBuffCollider as FixIntSphereCollider))
                    {
                        damageTargetList.Add(enemy);
                    }
                    break;
            }
        }
        return damageTargetList;
    }

    /// <summary>
    /// 获取Buff的碰撞器位置
    /// </summary>
    /// <returns> Buff碰撞器位置 </returns>
    private FixIntVector3 GetBuffPos()
    {
        switch (mBuffConfig.attachType)
        {
            case BuffAttachType.Creator:
                return mReleaser.LogicPos;
            case BuffAttachType.Target:
                return mAttachTarget.LogicPos;
            case BuffAttachType.Guide_Pos:
                return mSkill.skillGuidePos;
            default:
                return mReleaser.LogicPos;
        }
    }

    /// <summary>
    /// 释放碰撞器资源
    /// </summary>
    public void OnRelease()
    {
        mBuffCollider?.OnRelease();
        mBuffCollider = null;
    }
}