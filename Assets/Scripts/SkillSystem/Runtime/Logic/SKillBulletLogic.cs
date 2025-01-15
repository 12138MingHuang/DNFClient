using FixIntPhysics;
using FixMath;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using ZMGC.Battle;

public class SKillBulletLogic : LogicObject
{
    /// <summary>
    /// 子弹隶属技能
    /// </summary>
    private Skill mSkill;

    /// <summary>
    /// 子弹发射者
    /// </summary>
    private LogicActor mFireLogicActor;

    /// <summary>
    /// 子弹配置信息
    /// </summary>
    private SkillBulletConfig mBulletConfig;

     /// <summary>
    /// 子弹碰撞器
    /// </summary>
    private ColliderBehaviour mBulletCollider;

     /// <summary>
     /// 子弹逻辑帧
     /// </summary>
    private int mCurLogicFrame = 0;
    /// <summary>
    /// 子弹逻辑帧累计时间
    /// </summary>
    private int mCurLogicFrameAccTime;
    /// <summary>
    /// 子弹是否命中
    /// </summary>
    private bool mBulletIsHit = false;
    /// <summary>
    /// 子弹是否失效
    /// </summary>
    public bool isFailure = false;
    /// <summary>
    /// 子弹当前帧命中目标列表
    /// </summary>
    private List<LogicActor> mHitTargetList = new List<LogicActor>();

    public SKillBulletLogic(Skill skill, LogicActor fireLogicActor, RenderObject renderObject, SkillBulletConfig bulletConfig, FixIntVector3 rangePos)
    {
        mSkill = skill;
        mFireLogicActor = fireLogicActor;
        mBulletConfig = bulletConfig;
        RenderObject = renderObject;
        
        // 更新轴向
        LogicXAxis = fireLogicActor.LogicXAxis;
        // 初始化逻辑对象方向
        LogicDir = new FixIntVector3(LogicXAxis, 0, 0) + new FixIntVector3(bulletConfig.dir);
        // 初始化当前对象偏移位置
        FixIntVector3 pos = LogicXAxis * (new FixIntVector3(mBulletConfig.offset) + rangePos);
        pos.y = FixIntMath.Abs(pos.y);
        LogicPos = fireLogicActor.LogicPos + pos;
        
        //更新旋转角度
        LogicAngle = new FixIntVector3(bulletConfig.angle) * LogicXAxis;
        
        // 当前这个子弹是否附加伤害
        if (bulletConfig.isAttachDamage)
        {
            SkillDamageConfig damageCfg = bulletConfig.damageConfig;
            switch (damageCfg.detectionMode)
            {
                case DamageDetectionMode.Box3D:
                    mBulletCollider = new FixIntBoxCollider(FixIntVector3.zero, FixIntVector3.zero);
                    break;
                case DamageDetectionMode.Sphere3D:
                    mBulletCollider = new FixIntSphereCollider(damageCfg.radius, FixIntVector3.zero);
                    break;
            }
        }
    }

    public override void OnLogicFrameUpdate()
    {
        base.OnLogicFrameUpdate();
        // 计算逻辑帧累加时间
        mCurLogicFrameAccTime = mCurLogicFrame * LogicFrameConfig.LogicFrameIntervalMS;
        // 逻辑帧自增
        mCurLogicFrame++;
        
        // 子弹击中逻辑
        foreach (LogicActor target in mHitTargetList)
        {
            // 造成子弹伤害
            target.BulletDamage(DamageCalculateCenter.CalculateDamage(mBulletConfig.damageConfig, mFireLogicActor, target), mBulletConfig.damageConfig);
            // 播放击中效果
            target.OnHit(mBulletConfig.hitEffect, mBulletConfig.hitEffectSurvivalTimeMS, this, LogicXAxis);
            // 处理击中音效
            if (mBulletConfig.hitAudio != null)
            {
                AudioController.Instance.PlaySoundByAudioClip(mBulletConfig.hitAudio, false, 1);
            }
            // 处理子弹附加的buff
            AttachBuff(target);

            if (mBulletConfig.isHitDestroy)
            {
                Release();
                break;
            }
        }
        
        // 击中目标处理完成，清理缓存数据
        if (mHitTargetList.Count > 0)
        {
            mHitTargetList.Clear();
        }
        
        // 子弹碰撞体位置更新
        if (mBulletCollider != null)
        {
            if (mBulletConfig.damageConfig.colliderPosType == ColliderPosType.FollowPos)
            {
                // 更新子弹碰撞体位置
                switch (mBulletConfig.damageConfig.detectionMode)
                {
                    case DamageDetectionMode.Box3D:
                        FixIntVector3 boxOffset = LogicXAxis * new FixIntVector3(mBulletConfig.damageConfig.boxOffset);
                        mBulletCollider.SetBoxData(boxOffset, new FixIntVector3(mBulletConfig.damageConfig.boxSize));
                        mBulletCollider.UpdateColliderInfo(LogicPos, new FixIntVector3(mBulletConfig.damageConfig.boxSize));
                        break;
                    case DamageDetectionMode.Sphere3D:
                        FixIntVector3 sphereOffset = LogicXAxis * new FixIntVector3(mBulletConfig.damageConfig.sphereOffset);
                        mBulletCollider.SetBoxData(mBulletConfig.damageConfig.radius, sphereOffset);
                        mBulletCollider.UpdateColliderInfo(LogicPos, FixIntVector3.zero, mBulletConfig.damageConfig.radius);
                        break;
                }
            }
            
            // 获取场景中所有敌人
            List<LogicActor> enemyList = BattleWorld.GetExitsLogicCtrl<BattleLogicCtrl>().GetEnemyList(mFireLogicActor.ObjectType);
            // 计算子弹碰撞体是否击中敌人
            foreach (var target in enemyList)
            {
                switch (mBulletConfig.damageConfig.detectionMode)
                {
                    case DamageDetectionMode.Box3D:
                        mBulletIsHit = PhysicsManager.IsCollision((mBulletCollider as FixIntBoxCollider), target.Collider);
                        break;
                    case DamageDetectionMode.Sphere3D:
                        mBulletIsHit = PhysicsManager.IsCollision(target.Collider, (mBulletCollider as FixIntSphereCollider));
                        break;
                }
                
                // 击中目标，添加到命中列表
                if (mBulletIsHit)
                    mHitTargetList.Add(target);
            }
        }
        
        // 子弹位置更新
        LogicPos += LogicDir * (FixInt)mBulletConfig.moveSpeed * (FixInt)LogicFrameConfig.LogicFrameInterval;
        // 当前运行时间达到了子弹存活时间，就销毁子弹
        if (mCurLogicFrameAccTime >= mBulletConfig.survivalTimeMS)
        {
            Release();
        }
    }

    /// <summary>
    /// 附加buff到目标
    /// </summary>
    /// <param name="target"> 子弹击中目标 </param>
    private void AttachBuff(LogicActor target)
    {
        if (mBulletConfig.damageConfig.addBuffs != null && mBulletConfig.damageConfig.addBuffs.Length > 0)
        {
            foreach (int buffId in mBulletConfig.damageConfig.addBuffs)
            {
                BuffSystem.Instance.AttachBuff(buffId, mFireLogicActor, target, mSkill);
            }
        }
    }
    
    private void Release()
    {
        RenderObject.OnRelease();
        mBulletCollider?.OnRelease();
        mBulletCollider = null;
        isFailure = true;
    }
}