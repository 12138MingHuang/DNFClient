using System.Collections;
using System.Collections.Generic;
using FixIntPhysics;
using FixMath;
using UnityEngine;

/// <summary>
/// LogicObject 同时代表怪物和英雄和其他同时具有的属性
/// </summary>
public abstract class LogicObject
{
    private FixIntVector3 m_LogicPos; //逻辑对象逻辑位置
    private FixIntVector3 m_LogicDir; //逻辑对象朝向
    private FixIntVector3 m_LogicAngle; //逻辑对象旋转角度
    private FixInt m_LogicMoveSpeed = 2; //逻辑对象移动速度
    private FixInt _mLogicXXAxis; //逻辑轴向
    private bool m_IsActive; //逻辑对象是否激活
    
    #region 公开属性
    
    /// <summary>
    /// 逻辑对象逻辑位置
    /// </summary>
    public FixIntVector3 LogicPos
    {
        get => m_LogicPos;
        set => m_LogicPos = value;
    }
    
    /// <summary>
    /// 逻辑对象朝向
    /// </summary>
    public FixIntVector3 LogicDir
    {
        get => m_LogicDir;
        set => m_LogicDir = value;
    }
    
    /// <summary>
    /// 逻辑对象旋转角度
    /// </summary>
    public FixIntVector3 LogicAngle
    {
        get => m_LogicAngle;
        set => m_LogicAngle = value;
    }
    
    /// <summary>
    /// 逻辑对象移动速度
    /// </summary>
    public FixInt LogicMoveSpeed
    {
        get => m_LogicMoveSpeed;
        set => m_LogicMoveSpeed = value;
    }
    
    /// <summary>
    /// 逻辑轴向
    /// </summary>
    public FixInt LogicXAxis
    {
        get => _mLogicXXAxis;
        set => _mLogicXXAxis = value;
    }
    
    /// <summary>
    /// 逻辑对象是否激活
    /// </summary>
    public bool IsActive
    {
        get => m_IsActive;
        set => m_IsActive = value;
    }
    
    #endregion
    
    /// <summary>
    /// 渲染对象
    /// </summary>
    public RenderObject RenderObject { get; protected set; }
    
    /// <summary>
    /// 定点数碰撞体
    /// </summary>
    public FixIntBoxCollider Collider { get; protected set; }
    
    /// <summary>
    /// 逻辑对象状态
    /// </summary>
    public LogicObjectState ObjectState { get; set; }
    
    /// <summary>
    /// 逻辑对象类型
    /// </summary>
    public LogicObjectType ObjectType { get; set; }
    
    /// <summary>
    /// 逻辑对象动作状态
    /// </summary>
    public LogicObjectActionState ActionState { get; set; }

    /// <summary>
    /// 初始化接口
    /// </summary>
    public virtual void OnCreate()
    {
        
    }
    
    /// <summary>
    /// 逻辑帧更新接口
    /// </summary>
    public virtual void OnLogicFrameUpdate()
    {
        
    }
   
    /// <summary>
    /// 逻辑对象释放接口
    /// </summary>
    public virtual void OnDestroy()
    {
        
    }
}

/// <summary>
/// 逻辑对象动作状态
/// </summary>
public enum LogicObjectActionState
{
    /// <summary>
    /// 待机
    /// </summary>
    Idle,
    
    /// <summary>
    /// 移动中
    /// </summary>
    Move,
    
    /// <summary>
    /// 释放技能中
    /// </summary>
    SkillReleasing,
    
    /// <summary>
    /// 浮空中
    /// </summary>
    Floating,
    
    /// <summary>
    /// 受击中
    /// </summary>
    Hitting,
    
    /// <summary>
    /// 蓄力中
    /// </summary>
    StockPiling
}

/// <summary>
/// 逻辑对象类型
/// </summary>
public enum LogicObjectType
{
    /// <summary>
    /// 英雄
    /// </summary>
    Hero,
    
    /// <summary>
    /// 怪物
    /// </summary>
    Monster,
    
    /// <summary>
    /// 特效
    /// </summary>
    Effect
}

/// <summary>
/// 逻辑对象状态
/// </summary>
public enum LogicObjectState
{
    /// <summary>
    /// 存活中
    /// </summary>
    Survival,
    
    /// <summary>
    /// 死亡
    /// </summary>
    Death
}