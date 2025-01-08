using System;

public abstract class TimerBehaviour
{
    /// <summary>
    /// 计时器是否完成
    /// </summary>
    public bool timerFinish = false;
    /// <summary>
    /// 计时器完成回调
    /// </summary>
    protected Action timerFinishCallBack = null;
    /// <summary>
    /// 计时器更新回调
    /// </summary>
    protected Action timerUpdateCallBack = null;

    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    public abstract void OnLogicFrameUpdate();

    /// <summary>
    /// 计时器完成
    /// </summary>
    public abstract void OnTimerFinish();
}