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
                break;
        }

        // 构建行动
        MoveToAction action = new MoveToAction(logicMoveObj, startPos, targetPos, actionConfig.durationMS, () =>
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
                    // TODO: 添加Buff
                    break;
            }
        }, moveUpdateCallBack, moveType);
        
        LogicActionController.Instance.RunAction(action);
    }
}
