using FixMath;
using System;

public class LogicTimer : TimerBehaviour
{
    /// <summary>
    /// 延迟时间
    /// </summary>
    private FixInt mDelayTime;
    /// <summary>
    /// 循环次数
    /// </summary>
    private int mLoopCount;

    /// <summary>
    /// 当前逻辑帧累计时间
    /// </summary>
    private FixInt mCurLogicFrameAccTime;

    /// <summary>
    /// 总运行时间
    /// </summary>
    private FixInt mTotalTime;

    public LogicTimer(FixInt delayTime, Action timerFinishCallBack, int loopCount = 1)
    {
        mDelayTime = delayTime;
        mLoopCount = loopCount;
        this.timerFinishCallBack = timerFinishCallBack;
        mTotalTime = delayTime * loopCount;
    }
    
    public override void OnLogicFrameUpdate()
    {
        mCurLogicFrameAccTime += LogicFrameConfig.LogicFrameInterval;
        if (mCurLogicFrameAccTime >= mDelayTime)
        {
            timerFinishCallBack?.Invoke();
            mCurLogicFrameAccTime -= mDelayTime;
            mTotalTime -= mDelayTime;

            if (mLoopCount <= 1 || mTotalTime <= 0)
            {
                timerFinish = true;
                timerFinishCallBack = null;
            }
        }
    }
    
    public override void OnTimerFinish()
    {
        
    }
}