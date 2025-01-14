using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZMAssetFrameWork;

public class MonsterRender : RenderObject
{
    private Animation mAnim;
    private string mCurAnimName;
    private int mMonsterId;
    private MonsterLogic mMonsterLogic;
        
    public override void OnCreate()
    {
        base.OnCreate();
        mAnim = GetComponentInChildren<Animation>();
        mMonsterLogic = (logicObject as MonsterLogic);
        mMonsterId = mMonsterLogic.MonsterId;
    }

    public override void OnRelease()
    {
        base.OnRelease();
    }

    private void Start()
    {
        
    }

    public override void PlayAnim(string animName)
    {
        base.PlayAnim(animName);
        if(mAnim == null)
            return;
        
        if(logicObject.ObjectState == LogicObjectState.Death && !string.Equals(animName, AnimationName.Anim_Dead)) return;
        mCurAnimName = animName;
        mAnim.Play(animName);
    }

    public override string GetCurAnimName()
    {
        return mCurAnimName;
    }

    public override void OnHit(GameObject hitEffect, int hitEffectSurvivalTimeMs, LogicObject source)
    {
        base.OnHit(hitEffect, hitEffectSurvivalTimeMs, source);
        //通过怪物配置文件，配置怪物的信息，如怪物的id、基础血量、攻击力、移动速度、受击音效、攻击音效等 Excel
        //加载怪物的时候读取配置，播放音效也是读配置的。
        //临时代码
        AudioClip audioClip = null;
        if (mMonsterId == 20001)//哥布林
        {
            audioClip = ZMAssetsFrame.LoadAudio(AssetPathConfig.GAME_AUIDO_PATH + "Gebulin/GoblinAttackC.wav");
        }
        else if (mMonsterId == 20005)//蜘蛛
        {
            audioClip = ZMAssetsFrame.LoadAudio(AssetPathConfig.GAME_AUIDO_PATH + "zhizu/NorthrendGhoulWound1.wav");
        }
        if (audioClip != null)
        {
            AudioController.Instance.PlaySoundByAudioClip(audioClip, false, 2);
        }
        
    }

    protected override void Update()
    {
        base.Update();
    }

    private void OnDestroy()
    {
        
    }
}
