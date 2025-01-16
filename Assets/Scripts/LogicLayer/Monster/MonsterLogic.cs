using FixIntPhysics;
using FixMath;
using UnityEngine;
using ZMGC.Battle;

public class MonsterLogic : LogicActor
{
    /// <summary>
    /// 怪物ID
    /// </summary>
    public int MonsterId { get; private set; }

    /// <summary>
    /// 怪物攻击范围
    /// </summary>
    private FixInt attackRange = 1;
    /// <summary>
    /// 追踪距离
    /// </summary>
    private FixInt chaseDistance = 4;
    /// <summary>
    /// 追踪的目标对象
    /// </summary>
    private LogicActor mChaseTarget;

    public MonsterLogic(int monsterId, RenderObject renderObject, FixIntBoxCollider boxCollider, FixIntVector3 logicPos)
    {
        MonsterId = monsterId;
        RenderObject = renderObject;
        Collider = boxCollider;
        LogicPos = logicPos;
        ObjectType = LogicObjectType.Monster;
    }

    public override void OnCreate()
    {
        base.OnCreate();
        InitMonsterAttribute();
        mChaseTarget = BattleWorld.GetExitsLogicCtrl<HeroLogicCtrl>().HeroLogic;
        LogicMoveSpeed = 1;
    }

    public override void OnLogicFrameUpdate()
    {
        base.OnLogicFrameUpdate();
        UpdateAIMove();
    }

    /// <summary>
    /// 怪物AI移动逻辑处理，此处仅为简单处理，实际项目中需要根据怪物类型和场景设计不同的AI逻辑处理策略.
    /// </summary>
    private void UpdateAIMove()
    {
        if(ObjectState == LogicObjectState.Death) return;

        FixIntVector3 targetPos = mChaseTarget.LogicPos;
        FixIntVector3 direToTarget = (targetPos - LogicPos).normalized;
        FixInt distance = FixIntVector3.Distance(LogicPos, targetPos);
        if (distance <= attackRange)
        {
            if (ActionState == LogicObjectActionState.Idle)
            {
                PlayAnim(AnimationName.Anim_Gongji_01);
            }
        }
        else if (distance <= chaseDistance)
        {
            if(ActionState == LogicObjectActionState.Idle || ActionState == LogicObjectActionState.Move)
            {
                LogicPos += direToTarget * LogicMoveSpeed * LogicFrameConfig.LogicFrameInterval;
                LogicXAxis = direToTarget.x;
                PlayAnim(AnimationName.Anim_Walk);
            }
        }
        else
        {
            if (ActionState == LogicObjectActionState.Idle)
                PlayAnim(AnimationName.Anim_Idle);
        }
    }

    public override void OnHit(GameObject hitEffect, int hitEffectSurvivalTimeMs, LogicObject source, FixInt logicXAxis)
    {
        base.OnHit(hitEffect, hitEffectSurvivalTimeMs, source, logicXAxis);
        this.LogicXAxis = -logicXAxis;
    }

    public override void Floating(bool isUping)
    {
        base.Floating(isUping);
        string animName = isUping ? AnimationName.Anim_Float_up : AnimationName.Anim_Float_down;
        PlayAnim(animName);

        ActionState = LogicObjectActionState.Floating;
    }

    public override void TriggerGround()
    {
        base.TriggerGround();
        
        // 处理怪物落地的逻辑
        if (ObjectState != LogicObjectState.Death)
        {
            PlayAnim(AnimationName.Anim_Getup);
            
            //当怪物从地面完全站起的时候，需要播放待机动画
            //通过逻辑帧延迟器延迟若干秒触发逻辑  
            LogicTimerManager.Instance.DelayCall(0.5f, () =>
            {
                PlayAnim(AnimationName.Anim_Idle);
                ActionState = LogicObjectActionState.Idle;
            });
        }
        else
        {
            PlayAnim(AnimationName.Anim_Dead);
        }
    }

    private void InitMonsterAttribute()
    {
        MonsterCfg dataCfg = ConfigCenter.Instance.GetConfigById<MonsterCfg>(MonsterId);
        if (dataCfg == null)
        {
            Debug.LogError($"怪物配置不存在，怪物ID：{MonsterId}");
            return;
        }
        
        hp = dataCfg.hp;
        mp = dataCfg.mp;
        ap = dataCfg.ap;
        ad = dataCfg.ad;
        adDef = dataCfg.adDef;
        apDef = dataCfg.apDef;
        pct = dataCfg.pct;
        mct = dataCfg.mct;
        adPctRate = dataCfg.adPctRate;
        apMctRate = dataCfg.apMctRate;
        str = dataCfg.str;
        sta = dataCfg.sta;
        Int = dataCfg.Int;
        spi = dataCfg.spi;
        agl = dataCfg.agl;
        
        Debug.Log($"初始化怪物属性成功，ID:{MonsterId}");
    }
}