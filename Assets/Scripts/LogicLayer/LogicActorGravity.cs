using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 处理逻辑对象重力
/// </summary>
public partial class LogicActor
{
    /// <summary>
    /// 重力g
    /// </summary>
    protected FixInt gravity = 9.8f;
    
    /// <summary>
    /// 初始速度
    /// </summary>
    public FixIntVector3 velocity;

    /// <summary>
    /// 上升时间
    /// </summary>
    protected FixInt mRisingTime;
    
    /// <summary>
    /// 是否添加力
    /// </summary>
    public bool isAddForce = false;
    
    /// <summary>
    /// 逻辑帧更新重力
    /// </summary>
    public void OnLogicFrameUpdateGravity()
    {
        if (isAddForce)
        {
            velocity.y -= gravity * LogicFrameConfig.LogicFrameInterval;
            // 计算要移动的新的位置
            FixIntVector3 newPos = new FixIntVector3(LogicPos.x, FixIntMath.Clamp(LogicPos.y + velocity.y * LogicFrameConfig.LogicFrameInterval, 0, FixInt.MaxValue), LogicPos.z);

            if (newPos.y <= 0)
            {
                isAddForce = false;
                velocity = new FixIntVector3(0, 2, 0);
            }
            LogicPos = newPos;
        }
    }
}
