using FixMath;

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
    private void AddMoveAction(SkillActionConfig actionConfig, LogicActor logicMoveObj)
    {
        FixIntVector3 movePos = new FixIntVector3(actionConfig.movePos);
        FixIntVector3 targetPos = logicMoveObj.LogicPos + movePos * logicMoveObj.LogicXAxis;
        // 计算移动类型
        MoveType moveType = MoveType.Target;
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
        
        // 构建行动
        MoveToAction action = new MoveToAction(logicMoveObj, logicMoveObj.LogicPos, targetPos, actionConfig.durationMS, () =>
        {
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
        }, null, moveType);
        
        LogicActionController.Instance.RunAction(action);
    }
}
