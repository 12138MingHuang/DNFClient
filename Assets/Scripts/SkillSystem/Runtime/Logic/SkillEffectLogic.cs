using FixIntPhysics;
using FixMath;

public class SkillEffectLogic : LogicObject
{
    /// <summary>
    /// 技能创建者
    /// </summary>
    private LogicActor mSkillCreator;
    /// <summary>
    /// 技能特效配置
    /// </summary>
    private SkillEffectConfig mEffectcfg;
    /// <summary>
    /// 特效碰撞体
    /// </summary>
    private ColliderBehaviour mCollider;
    /// <summary>
    /// 累计运行时间，用于触发间隔伤害
    /// </summary>
    private int mAccRunTime;

    public SkillEffectLogic(LogicObjectType objType, SkillEffectConfig effectCfg, RenderObject renderObject, LogicActor skillCreator)
    {
        ObjectType = objType;
        mEffectcfg = effectCfg;
        mSkillCreator = skillCreator;
        RenderObject = renderObject;
        LogicXAxis = skillCreator.LogicXAxis;
        //初始化特效逻辑位置
        if (effectCfg.effectPosType == EffectPosType.FollowDir || effectCfg.effectPosType == EffectPosType.FollowPosDir)
        {
            FixIntVector3 offsetPos = new FixIntVector3(effectCfg.effectOffsetPos) * LogicXAxis;
            offsetPos.y = FixIntMath.Abs(offsetPos.y);
            LogicPos = skillCreator.LogicPos + offsetPos;
        }
        else if (effectCfg.effectPosType == EffectPosType.Zero)
        {
            LogicPos = FixIntVector3.zero;
        }
    }

    public void OnLogicFrameEffectUpdate(Skill skill, int curLogicFrame)
    {
        if (mEffectcfg.effectPosType == EffectPosType.FollowPosDir)
        {
            FixIntVector3 offsetPos = new FixIntVector3(mEffectcfg.effectOffsetPos) * LogicXAxis;
            offsetPos.y = FixIntMath.Abs(offsetPos.y);
            LogicPos = mSkillCreator.LogicPos + offsetPos;
        }
        
        // 1.处理特效行动配置， 让特效也能随着配置移动
        if (mEffectcfg.isAttachAction && mEffectcfg.actionConfig.triggerFrame == curLogicFrame)
        {
            skill.AddMoveAction(mEffectcfg.actionConfig, this, () =>
            {
                mCollider.OnRelease();
                skill.DestroyEffect(mEffectcfg);
                mCollider = null;
            });
        }
        
        // 2.处理伤害配置,让伤害碰撞体能够随着动效移动
        if (mEffectcfg.isAttachDamage)
        {
            // 创建伤害碰撞体
            if (mEffectcfg.damageConfig.triggerFrame == curLogicFrame)
            {
                mCollider = skill.CreateOrUpdateCollider(mEffectcfg.damageConfig, null);
                if (mEffectcfg.damageConfig.triggerIntervalMS == 0)
                {
                    skill.TriggerColliderDamage(mCollider, mEffectcfg.damageConfig);
                }
            }
            // 如果有间隔，则每隔一段时间触发一次
            if (mEffectcfg.damageConfig.triggerIntervalMS != 0 && mCollider != null)
            {
                mAccRunTime += LogicFrameConfig.LogicFrameIntervalMS;
                if (mAccRunTime >= mEffectcfg.damageConfig.triggerIntervalMS)
                {
                    skill.TriggerColliderDamage(mCollider, mEffectcfg.damageConfig);
                    mAccRunTime -= mEffectcfg.damageConfig.triggerIntervalMS;
                }
            }
                
            // 更新碰撞体位置
            if (mEffectcfg.damageConfig.isFollowEffect)
            {
                skill.CreateOrUpdateCollider(mEffectcfg.damageConfig, mCollider, this);
            }
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        RenderObject.OnRelease();
    }
}
