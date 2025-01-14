using FixMath;
using System;

public class MoveBezierAction : ActionBehaviour
{
    private LogicObject mActionObj;
    private FixIntVector3 mStartPos;
    private FixIntVector3 mHeightPos;
    private FixIntVector3 mEndPos;
    private FixInt mMoveTime;
    private FixInt mAccRunTime;
    private FixInt mTimeScale;

    public MoveBezierAction(LogicObject actionObj, FixIntVector3 startPos, FixIntVector3 heightPos, FixIntVector3 endPos, FixInt time, Action moveFinishCallBack, Action updateCallBack)
    {
        mActionObj = actionObj;
        mStartPos = startPos;
        mHeightPos = heightPos;
        mEndPos = endPos;
        mMoveTime = time == FixInt.Zero ? 0.1f : time;
        OnMoveFinishAction = moveFinishCallBack;
        OnMoveUpdateAction = updateCallBack;
    }

    public override void OnLogicFrameUpdate()
    {
        mAccRunTime += LogicFrameConfig.LogicFrameIntervalMS;
        mTimeScale = mAccRunTime / mMoveTime;

        if (mTimeScale >= 1)
        {
            mTimeScale = 1;
            actionFinish = true;
        }
        OnMoveUpdateAction?.Invoke();
        mActionObj.LogicPos = BezierUtils.BezierCurve(mStartPos, mHeightPos, mEndPos, mTimeScale);
    }
    public override void OnActionFinish()
    {
        if(actionFinish) OnMoveFinishAction?.Invoke();
    }
}