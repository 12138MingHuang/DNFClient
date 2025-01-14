using FixMath;
using System;
using UnityEngine;

public partial class Skill
{
    /// <summary>
    /// 行动逻辑帧更新
    /// </summary>
    private void OnLogicFrameUpdateAction()
    {
        if (mSkillDataConfig.actionCfgList != null && mSkillDataConfig.actionCfgList.Count > 0)
        {
            foreach (var actionConfig in mSkillDataConfig.actionCfgList)
            {
                // 触发行动
                if(actionConfig.triggerFrame == mCurLogicFrame)
                    AddMoveAction(actionConfig, mSkillCreator);
            }
        }
    }
    
    /// <summary>
    /// 添加移动动作
    /// </summary>
    /// <param name="actionConfig"> 行动配置 </param>
    /// <param name="logicMoveObj"> 逻辑移动对象 </param>
    /// <param name="offset"> 偏移量 </param>
    /// <param name="onMoveFinish"> 移动完成回调 </param>
    /// <param name="moveUpdateCallBack"> 移动更新回调 </param>
    public void AddMoveAction(SkillActionConfig actionConfig, LogicObject logicMoveObj, Vector3 offset = default(Vector3), Action onMoveFinish = null, Action moveUpdateCallBack=null)
    {
        void OnActionFinish()
        {
            onMoveFinish?.Invoke();
            switch (actionConfig.actionFinishOperation)
            {
                case MoveActionFinishOperation.None:
                    break;
                case MoveActionFinishOperation.Skill:
                    foreach (var skillActionId in actionConfig.actionFinishIdList)
                    {
                        mSkillCreator.ReleaseSkill(skillActionId);
                    }
                    break;
                case MoveActionFinishOperation.Buff:
                    skillGuidePos = logicMoveObj.LogicPos;
                    foreach (var actionId in actionConfig.actionFinishIdList)
                    {
                        BuffSystem.Instance.AttachBuff(actionId, mSkillCreator, mSkillCreator, this);
                    }
                    break;
            }
        }
        
        FixIntVector3 movePos = new FixIntVector3(actionConfig.movePos);
        FixIntVector3 targetPos = logicMoveObj.LogicPos + movePos * logicMoveObj.LogicXAxis;
        FixIntVector3 startPos = logicMoveObj.LogicPos;
        // 计算移动类型
        MoveType moveType = MoveType.Target;
        switch (actionConfig.moveActionType)
        {
            case MoveActionType.TargetPos:
                if (movePos.x != FixInt.Zero && movePos.y == FixInt.Zero && movePos.z == FixInt.Zero)
                {
                    moveType = MoveType.X;
                }
                else if (movePos.x == FixInt.Zero && movePos.y != FixInt.Zero && movePos.z == FixInt.Zero)
                {
                    moveType = MoveType.Y;
                }
                else if (movePos.x == FixInt.Zero && movePos.y == FixInt.Zero && movePos.z != FixInt.Zero)
                {
                    moveType = MoveType.Z;
                }
                break;
            case MoveActionType.GuidePos:
                targetPos = skillGuidePos;
                startPos = targetPos + mSkillCreator.LogicXAxis * new FixIntVector3(offset);
                startPos.y = FixIntMath.Abs(startPos.y);
                break;
            case MoveActionType.BezierPos:
                // 计算起始位置
                startPos = mSkillCreator.LogicPos + mSkillCreator.LogicXAxis * new FixIntVector3(offset);
                startPos.y = FixIntMath.Abs(startPos.y);
                // 计算高度位置
                FixIntVector3 heightPosOffset = new FixIntVector3(actionConfig.heightPos) * mSkillCreator.LogicXAxis;
                heightPosOffset.y = FixIntMath.Abs(heightPosOffset.y);
                FixIntVector3 heightPos = mSkillCreator.LogicPos + heightPosOffset;
                // 计算结束位置
                FixIntVector3 endPosOffset = new FixIntVector3(actionConfig.movePos) * mSkillCreator.LogicXAxis;
                endPosOffset.y = FixIntMath.Abs(endPosOffset.y);
                targetPos = mSkillCreator.LogicPos + endPosOffset;
                // 构建贝塞尔运动
                MoveBezierAction moveBezierAction = new MoveBezierAction(logicMoveObj, startPos, heightPos, targetPos, actionConfig.durationMS, OnActionFinish, moveUpdateCallBack);
                LogicActionController.Instance.RunAction(moveBezierAction);
                return;
        }

        // 构建普通直线行动
        MoveToAction action = new MoveToAction(logicMoveObj, startPos, targetPos, actionConfig.durationMS, OnActionFinish, moveUpdateCallBack, moveType);
        
        LogicActionController.Instance.RunAction(action);
    }
}
