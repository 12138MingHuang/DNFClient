using System;
using ZMAssetFrameWork;

public enum BuffState
{
    None,
    /// <summary>
    /// 延迟阶段
    /// </summary>
    Delay,
    /// <summary>
    /// 开始阶段
    /// </summary>
    Start,
    /// <summary>
    /// 更新阶段
    /// </summary>
    Update,
    /// <summary>
    /// 结束阶段
    /// </summary>
    End
}

public class Buff
{
    /// <summary>
    /// Buff配置
    /// </summary>
    public BuffConfig BuffConfig { get; private set; }
    
    /// <summary>
    /// Buff状态
    /// </summary>
    public BuffState buffState;

    /// <summary>
    /// Buff唯一标识ID
    /// </summary>
    public readonly int buffId;
    
    /// <summary>
    /// Buff施放者
    /// </summary>
    public LogicActor releaser;

    /// <summary>
    /// Buff附着目标
    /// </summary>
    public LogicActor attachTarget;
    
    /// <summary>
    /// 隶属技能
    /// </summary>
    public Skill skill;

    /// <summary>
    /// Buff所需要的一些参数
    /// </summary>
    public object[] paramsObjs;

    /// <summary>
    /// Buff当前延迟时间
    /// </summary>
    private int mCurDelayTime;

    /// <summary>
    /// Buff逻辑组合对象
    /// </summary>
    private BuffComposite mBuffLogic;
    
    /// <summary>
    /// Buff当前真实运行时间
    /// </summary>
    private int mCurRealRuntime;
    
    /// <summary>
    /// Buff当前累计运行时间
    /// </summary>
    private int mAccRuntime;

    /// <summary>
    /// Buff逻辑初始化
    /// </summary>
    /// <param name="buffId"> Buff唯一标识ID</param>
    /// <param name="releaser"> Buff施放者</param>
    /// <param name="attachTarget"> Buff附着目标</param>
    /// <param name="skill"> 隶属技能</param>
    /// <param name="paramsObjs"> Buff所需要的一些参数</param>
    public Buff(int buffId, LogicActor releaser, LogicActor attachTarget, Skill skill, object[] paramsObjs)
    {
        this.buffId = buffId;
        this.releaser = releaser;
        this.attachTarget = attachTarget;
        this.skill = skill;
        this.paramsObjs = paramsObjs;
    }

    public void OnCreate()
    {
        BuffConfig = ZMAssetsFrame.LoadScriptableObject<BuffConfig>(AssetPathConfig.BUFF_DATA_PATH + buffId.ToString() + ".asset");

        switch (BuffConfig.buffType)
        {
            case BuffType.Repel:
                mBuffLogic = new RepelBuff(this);
                break;
            case BuffType.Floating:
                mBuffLogic = new FloatingBuff(this);
                break;
            case BuffType.Stiff:
                mBuffLogic = new StiffBuff(this);
                break;
            case BuffType.HP_Modify_Group:
                mBuffLogic = new AttributeModify_Buff_Group(this);
                break;
        }
        
        buffState = BuffConfig.buffDelay == 0 ? BuffState.Start : BuffState.Delay;
        mCurDelayTime = BuffConfig.buffDelay;
    }

    public void OnLogicFrameUpdate()
    {
        switch (buffState)
        {
            case BuffState.Delay:
                if (mCurDelayTime == BuffConfig.buffDelay)
                {
                    mBuffLogic.BuffDelay();
                }
                mCurDelayTime -= LogicFrameConfig.LogicFrameIntervalMS;
                if (mCurDelayTime <= 0)
                {
                    buffState = BuffState.Start;
                }
                break;
            case BuffState.Start:
                mBuffLogic.BuffStart();
                BuffStart();
                mBuffLogic.BuffTrigger();
                BuffTrigger();
                
                // 判断Buff是否需要切换为更新状态，如果当前buff持续时间为有限或无限，才进入更新状态
                buffState = (BuffConfig.buffDurationMS == -1 ||BuffConfig.buffDurationMS > 0) ? BuffState.Update : BuffState.End;
                break;
            case BuffState.Update:
                UpdateBuffLogic();
                break;
            case BuffState.End:
                OnDestroy();
                break;
        }
    }

    private void UpdateBuffLogic()
    {
        int logicFrameIntervalMS = LogicFrameConfig.LogicFrameIntervalMS;
        if (BuffConfig.buffIntervalMS > 0)
        {
            mCurRealRuntime += logicFrameIntervalMS;
            if (mCurRealRuntime >= BuffConfig.buffIntervalMS)
            {
                mBuffLogic.BuffTrigger();
                mCurRealRuntime -= BuffConfig.buffIntervalMS;
            }
        }
        UpdateBuffDurationTime();
    }

    private void UpdateBuffDurationTime()
    {
        mAccRuntime += LogicFrameConfig.LogicFrameIntervalMS;
        if (mAccRuntime >= BuffConfig.buffDurationMS)
        {
            buffState = BuffState.End;
        }
    }
    
    private void BuffStart()
    {
        attachTarget.AddBuff(this);
    }

    private void BuffTrigger()
    {
        switch (BuffConfig.buffTriggerAnim)
        {
            case ObjectAnimationState.BeHit:
                attachTarget.PlayAnim(AnimationName.Anim_Beiji_01);
                break;
            case ObjectAnimationState.Stiff:
                attachTarget.PlayAnim(AnimationName.Anim_Beiji_02);
                break;
        }
        
        // 处理音效
        if (BuffConfig.buffAudio != null)
        {
            AudioController.Instance.PlaySoundByAudioClip(BuffConfig.buffAudio, false, 2);
        }
    }

    public void OnDestroy()
    {
        mBuffLogic.BuffEnd();
        BuffSystem.Instance.RemoveBuff(this);
        attachTarget.RemoveBuff(this);
    }
}