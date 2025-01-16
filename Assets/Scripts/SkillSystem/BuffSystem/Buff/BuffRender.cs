using FixMath;
using System;
using UnityEngine;
using ZMAssetFrameWork;

public class BuffRender : RenderObject
{
    /// <summary>
    /// 附着目标
    /// </summary>
    private LogicActor mAttachTarget;
    /// <summary>
    /// buff释放者
    /// </summary>
    private HeroRender mHeroRender;
    /// <summary>
    /// Buff配置信息
    /// </summary>
    private BuffConfig mBuffConfig;
    /// <summary>
    /// 输入位置
    /// </summary>
    private FixIntVector3 mInputPos;

    public void InitBuffRender(LogicActor logicObj, LogicActor attachTarget, BuffConfig buffConfig, FixIntVector3 targetPos)
    {
        base.SetLogicObject(logicObj);
        
        mHeroRender = logicObject.RenderObject as HeroRender;
        mBuffConfig = buffConfig;
        mInputPos = targetPos;
        mAttachTarget = attachTarget;
        
        // 1.处理音效的播放
        if (buffConfig.buffAudio != null)
        {
            AudioController.Instance.PlaySoundByAudioClip(buffConfig.buffAudio, false, 2);
        }
        
        // 3.处理特效位置以及附加节点
        if (buffConfig.effectConfig.attachType == EffectAttachType.Hand)
        {
            transform.SetParent(mHeroRender.GetTransParent(TransParentType.LeftHand)); // 默认左手
            transform.localPosition = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
        else
        {
            switch (mBuffConfig.buffPosType)
            {
                case BuffPosType.HitTargetPos:
                    transform.position = attachTarget.LogicPos.ToVector3();
                    break;
                case BuffPosType.ReleaserPos:
                    transform.position = logicObject.LogicPos.ToVector3();
                    break;
                case BuffPosType.UIInputPos:
                    transform.position = mInputPos.ToVector3();
                    break;
            }
        }
        
        // 3.重置粒子特效播放状态
        PlayParticle();
        
    }

    protected override void Update()
    {
        if (mBuffConfig != null)
        {
            if (mBuffConfig.buffPosType == BuffPosType.FollowTarget)
            {
                transform.position = mAttachTarget.RenderObject.transform.position;
            }
        }
    }

    private void PlayParticle()
    {
        ParticleSystem[] particleArr = transform.GetComponents<ParticleSystem>();
        foreach (var particle in particleArr)
        {
            particle.Play();
        }
    }
    public override void OnRelease()
    {
        base.OnRelease();
        mBuffConfig = null;
        mAttachTarget = null;
        ZMAssetsFrame.Release(gameObject);
    }
}