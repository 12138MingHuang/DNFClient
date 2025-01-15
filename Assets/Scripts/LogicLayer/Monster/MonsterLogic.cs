using FixIntPhysics;
using FixMath;
using UnityEngine;

public class MonsterLogic : LogicActor
{
    /// <summary>
    /// 怪物ID
    /// </summary>
    public int MonsterId { get; private set; }

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