using FixIntPhysics;
using FixMath;
using System.Collections.Generic;
using UnityEngine;
using ZMGC.Battle;

/// <summary>
/// 伤害来源
/// </summary>
public enum DamageSource
{
    /// <summary>
    /// 无
    /// </summary>
    None,
    /// <summary>
    /// 技能伤害
    /// </summary>
    Skill,
    /// <summary>
    /// buff伤害
    /// </summary>
    Buff,
    /// <summary>
    /// 子弹伤害
    /// </summary>
    Bullet
}

public partial class Skill
{
    /// <summary>
    /// 特效对象字典，key为特效配置的Hashcode value为特效对象
    /// </summary>
    private Dictionary<int, ColliderBehaviour> mColliderDic = new Dictionary<int, ColliderBehaviour>();
    /// <summary>
    /// 当前累计的伤害时间
    /// </summary>
    private int mCurDamageAccTime;

    /// <summary>
    /// 逻辑帧更新特效
    /// </summary>
    private void OnLogicFrameUpdateDamage()
    {
        if (mSkillDataConfig.damageCfgList != null && mSkillDataConfig.damageCfgList.Count > 0)
        {
            foreach (var skillData in mSkillDataConfig.damageCfgList)
            {
                int hashcode = skillData.GetHashCode();
                
                if (mCurLogicFrame == skillData.triggerFrame)
                {
                    DestroyCollider(skillData);
                    ColliderBehaviour collider = CreateCollider(skillData);
                    mColliderDic.Add(hashcode, collider);
                    
                    // 处理碰撞体伤害检测
                    if (skillData.triggerIntervalMS == 0)
                    {
                        // 触发一次伤害
                        if (mColliderDic.ContainsKey(hashcode))
                        {
                            TriggerColliderDamage(mColliderDic[hashcode], skillData);
                        }
                    }
                }
                
                // 处理碰撞体伤害检测
                if (skillData.triggerIntervalMS != 0)
                {
                    mCurDamageAccTime += LogicFrameConfig.LogicFrameIntervalMS;
                    // 如果累计时间大于间隔时间
                    if (mCurDamageAccTime >= skillData.triggerIntervalMS)
                    {
                        // 触发一次伤害
                        mCurDamageAccTime = 0;
                        if (mColliderDic.ContainsKey(hashcode))
                        {
                            TriggerColliderDamage(mColliderDic[hashcode], skillData);
                        }
                    }
                }
                
                // 销毁碰撞体
                if (mCurLogicFrame == skillData.endFrame)
                {
                    DestroyCollider(skillData);
                }
            }
        }
    }
    
    /// <summary>
    /// 创建对应配置伤害的碰撞体
    /// </summary>
    /// <param name="skillDamageConfig"> 伤害配置 </param>
    /// <returns> 碰撞体对象 </returns>
    private ColliderBehaviour CreateCollider(SkillDamageConfig skillDamageConfig)
    {
        ColliderBehaviour collider = null;
        
        // 创建对应的定点数碰撞体
        switch (skillDamageConfig.detectionMode)
        {
            case DamageDetectionMode.Box3D:
                FixIntVector3 boxSize = new FixIntVector3(skillDamageConfig.boxSize);
                FixIntVector3 boxOffset = new FixIntVector3(skillDamageConfig.boxOffset) * mSkillCreator.LogicXAxis;
                boxOffset.y = FixIntMath.Abs(boxOffset.y);
                collider = new FixIntBoxCollider(boxSize, boxOffset);
                collider.SetBoxData(boxOffset, boxSize);
                collider.UpdateColliderInfo(mSkillCreator.LogicPos, boxSize);
                break;
            case DamageDetectionMode.Sphere3D:
                FixIntVector3 sphereOffset = new FixIntVector3(skillDamageConfig.sphereOffset) * mSkillCreator.LogicXAxis;
                sphereOffset.y = FixIntMath.Abs(sphereOffset.y);
                collider = new FixIntSphereCollider(skillDamageConfig.radius, sphereOffset);
                collider.SetBoxData(skillDamageConfig.radius, sphereOffset);
                collider.UpdateColliderInfo(mSkillCreator.LogicPos, FixIntVector3.zero, skillDamageConfig.radius);
                break;
        }
        
        return collider;
    }

    /// <summary>
    /// 碰撞体触发伤害
    /// </summary>
    /// <param name="collider"> 碰撞体对象 </param>
    /// <param name="skillDamageConfig"> 伤害配置 </param>
    private void TriggerColliderDamage(ColliderBehaviour collider, SkillDamageConfig skillDamageConfig)
    {
        // 1.根据攻击者获取敌人目标列表
        List<LogicActor> enemyList = BattleWorld.GetExitsLogicCtrl<BattleLogicCtrl>().GetEnemyList(mSkillCreator.ObjectType);
        
        // 2.通过碰撞检测逻辑，去检测碰撞到的敌人
        List<LogicActor> damageTargetList = new List<LogicActor>();
        foreach (var target in enemyList)
        {
            switch (collider.ColliderType)
            {
                case ColliderType.Box:
                    if (PhysicsManager.IsCollision(collider as FixIntBoxCollider, target.Collider))
                    {
                        damageTargetList.Add(target);
                    }
                    break;
                case ColliderType.Shpere:
                    if (PhysicsManager.IsCollision(target.Collider, collider as FixIntSphereCollider))
                    {
                        damageTargetList.Add(target);
                    }
                    break;
            }
        }
        // 释放列表
        enemyList.Clear();
        // 3.获取到攻击目标后，对这些敌人造成伤害
        foreach (var target in damageTargetList)
        {
            // 造成伤害
            target.SkillDamage(9999, skillDamageConfig);
            
            // 添加 Buff TODO
            // 添加击中特效 TODO
            // 播放击中音效 TODO
        }
    }

    /// <summary>
    /// 销毁对应配置伤害的碰撞体
    /// </summary>
    /// <param name="skillDamageConfig"> 伤害配置 </param>
    private void DestroyCollider(SkillDamageConfig skillDamageConfig)
    {
        ColliderBehaviour collider = null;
        int hashCode = skillDamageConfig.GetHashCode();
        mColliderDic.TryGetValue(hashCode, out collider);
        if (collider != null)
        {
            mColliderDic.Remove(hashCode);
            collider.OnRelease();
        }
    }
}
