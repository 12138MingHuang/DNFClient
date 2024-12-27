using FixMath;
using System;

public class MoveToAction : ActionBehaviour
{
    private LogicObject mActionObj;
    private FixIntVector3 mStartPos;
    private FixInt mMoveTime;
    private MoveType mMoveType;
    /// <summary>
    /// 移动向量距离
    /// </summary>
    private FixIntVector3 mMoveDistance;

    /// <summary>
    /// 当前累计运行时间
    /// </summary>
    private FixInt mAccRunTime;
    /// <summary>
    /// 当前移动的时间缩放
    /// </summary>
    private FixInt mTimeScale;

    public MoveToAction(LogicObject actionObj, FixIntVector3 startPos, FixIntVector3 targetPos, 
        FixInt time, Action moveFinishCallBack, Action updateCallBack, MoveType moveType)
    {
        mActionObj = actionObj;
        mStartPos = startPos;
        mMoveTime = time;
        mMoveType = moveType;
        OnMoveFinishAction = moveFinishCallBack;
        OnMoveUpdateAction = updateCallBack;
        mMoveDistance = targetPos - startPos;
    }

    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    public override void OnLogicFrameUpdate()
    {
        // 计算当前累计运行时间
        mAccRunTime += LogicFrameConfig.LogicFrameIntervalMS;
        // 获取时间缩放比例
        mTimeScale = mAccRunTime / mMoveTime;

        if (mTimeScale >= 1)
        {
            mTimeScale = 1;
            actionFinish = true;
        }
        OnMoveUpdateAction?.Invoke();
        
        // 计算对象需要移动距离
        FixIntVector3 addDistance = FixIntVector3.zero;
        switch (mMoveType)
        {
            case MoveType.Target:
                addDistance = mMoveDistance * mTimeScale;
                break;
            case MoveType.X:
                addDistance.x = mActionObj.LogicPos.x * mTimeScale;
                break;
            case MoveType.Y:
                addDistance.y = mActionObj.LogicPos.y * mTimeScale;
                break;
            case MoveType.Z:
                addDistance.z = mActionObj.LogicPos.z * mTimeScale;
                break;
        }
        mActionObj.LogicPos = mStartPos + addDistance;
    }
    
    public override void OnActionFinish()
    {
        if(actionFinish) OnMoveFinishAction?.Invoke();
    }
}