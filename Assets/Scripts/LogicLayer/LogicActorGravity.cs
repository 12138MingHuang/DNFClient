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
            velocity.y -= gravity * LogicFrameConfig.LogicFrameInterval * mRisingTime;
            // 计算要移动的新的位置
            FixIntVector3 newPos = new FixIntVector3(LogicPos.x, FixIntMath.Clamp(LogicPos.y + velocity.y, 0, FixInt.MaxValue), LogicPos.z);

            if (newPos.y <= 0)
            {
                isAddForce = false;
                TriggerGround();
            }
            else
            {
                // 判断对象是否处于上升阶段
                if (velocity.y > 0)
                {
                    Floating(true);
                }
                else
                {
                    Floating(false);
                }
            }
            LogicPos = newPos;
        }
    }
    
    /// <summary>
    /// 添加上升力
    /// </summary>
    /// <param name="risingForceValue"> 上升力大小</param>
    /// <param name="risingTime"> 上升时间</param>
    public void AddRisingForce(FixInt risingForceValue, int risingTime)
    {
        velocity.y = risingForceValue;
        mRisingTime = risingTime * 1.0f / 1000;
        isAddForce = true;
    }
}
