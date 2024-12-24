using FixIntPhysics;
using FixMath;
using System.Collections.Generic;
using UnityEngine;

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
    public void OnLogicFrameUpdateDamage()
    {
        if (mSkillDataConfig.damageCfgList != null && mSkillDataConfig.damageCfgList.Count > 0)
        {
            foreach (var skillData in mSkillDataConfig.damageCfgList)
            {
                if (mCurLogicFrame == skillData.triggerFrame)
                {
                    DestroyCollider(skillData);
                    ColliderBehaviour collider = CreateCollider(skillData);
                    mColliderDic.Add(skillData.GetHashCode(), collider);
                }
                
                // 处理碰撞体伤害检测
                if (skillData.triggerIntervalMS == 0)
                {
                    // 触发一次伤害
                }
                else
                {
                    mCurDamageAccTime += LogicFrameConfig.LogicFrameIntervalMS;
                    // 如果累计时间大于间隔时间
                    if (mCurDamageAccTime >= skillData.triggerIntervalMS)
                    {
                        // 触发一次伤害
                        mCurDamageAccTime = 0;
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
    public ColliderBehaviour CreateCollider(SkillDamageConfig skillDamageConfig)
    {
        ColliderBehaviour collider = null;
        
        // 创建对应的定点数碰撞体
        if (skillDamageConfig.detectionMode == DamageDetectionMode.Box3D)
        {
            FixIntVector3 boxSize = new FixIntVector3(skillDamageConfig.boxSize);
            FixIntVector3 offset = new FixIntVector3(skillDamageConfig.boxOffset) * mSkillCreator.LogicXAxis;
            offset.y = FixIntMath.Abs(offset.y);
            collider = new FixIntBoxCollider(boxSize, offset);
            collider.SetBoxData(offset, boxSize);
            collider.UpdateColliderInfo(mSkillCreator.LogicPos, boxSize);
        }
        else if (skillDamageConfig.detectionMode == DamageDetectionMode.Sphere3D)
        {
            FixIntVector3 offset = new FixIntVector3(skillDamageConfig.sphereOffset) * mSkillCreator.LogicXAxis;
            offset.y = FixIntMath.Abs(offset.y);
            collider = new FixIntSphereCollider(skillDamageConfig.radius, offset);
            collider.SetBoxData(skillDamageConfig.radius, offset);
            collider.UpdateColliderInfo(mSkillCreator.LogicPos, FixIntVector3.zero, skillDamageConfig.radius);
        }
        
        return collider;
    }

    /// <summary>
    /// 销毁对应配置伤害的碰撞体
    /// </summary>
    /// <param name="skillDamageConfig"> 伤害配置 </param>
    public void DestroyCollider(SkillDamageConfig skillDamageConfig)
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
