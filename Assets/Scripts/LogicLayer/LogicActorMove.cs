using System.Collections;
using System.Collections.Generic;
using FixMath;
using UnityEngine;

/// <summary>
/// 处理逻辑对象移动脚本
/// </summary>
public partial class LogicActor
{
    /// <summary>
    /// 输入移动方向
    /// </summary>
    private FixIntVector3 mInputMoveDir;
    
    /// <summary>
    /// 逻辑帧更新移动
    /// </summary>
    public void OnLogicFrameUpdateMove()
    {
        if (ActionState != LogicObjectActionState.Idle && ActionState != LogicObjectActionState.Move)
            return;
        // 计算逻辑位置
        LogicPos += mInputMoveDir * LogicMoveSpeed * (FixInt)LogicFrameConfig.LogicFrameInterval;
        
        //计算逻辑对象朝向
        if (LogicDir != mInputMoveDir)
        {
            LogicDir = mInputMoveDir;
        }
        //计算逻辑轴向
        if (LogicDir.x != FixInt.Zero)
        {
            LogicXAxis = LogicDir.x > 0 ? 1 : -1;
        }
    }
    
    public void InPutLogicFrameEvent(FixIntVector3 inputDir)
    {
        mInputMoveDir = inputDir;    
    }
}