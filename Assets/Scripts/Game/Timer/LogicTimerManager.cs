using FixMath;
using System;
using System.Collections.Generic;

public class LogicTimerManager : Singleton<LogicTimerManager>
{
    /// <summary>
    /// 逻辑计时器列表。
    /// </summary>
    private List<LogicTimer> mTimerList = new List<LogicTimer>();

    /// <summary>
    /// 延迟调用。
    /// </summary>
    /// <param name="delayTime"> 延迟时间。</param>
    /// <param name="timerCallBack"> 延迟回调。</param>
    /// <param name="loopCount"> 循环次数。</param>
    public void DelayCall(FixInt delayTime, Action timerCallBack, int loopCount = 1)
    {
        LogicTimer timer = new LogicTimer(delayTime, timerCallBack, loopCount);
        mTimerList.Add(timer);
    }

    public void OnLogicFrameUpdate()
    {
        // 移除已经完成的计时器
        for (int i = mTimerList.Count - 1; i >= 0; --i)
        {
            LogicTimer timer = mTimerList[i];
            if (timer.timerFinish)
            {
                mTimerList.RemoveAt(i);
            }
        }
        // 更新计时器
        foreach (var timer in mTimerList)
        {
            timer.OnLogicFrameUpdate();
        }
    }

    /// <summary>
    /// 移除计时器。
    /// </summary>
    /// <param name="timer"> 计时器。</param>
    public void RemoveTimer(LogicTimer timer)
    {
        mTimerList.Remove(timer);
    }
    
    public void OnDestroy()
    {
        mTimerList.Clear();
    }
}