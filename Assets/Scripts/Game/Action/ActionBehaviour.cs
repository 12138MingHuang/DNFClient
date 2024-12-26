using System;

/// <summary>
/// 移动类型
/// </summary>
public enum MoveType
{
    Target, X, Y, Z
}

public abstract class ActionBehaviour
{
    /// <summary>
    /// 是否行动完成
    /// </summary>
    public bool actionFinish = false;
    /// <summary>
    /// 行动完成回调
    /// </summary>
    protected Action OnMoveFinishAction;
    /// <summary>
    /// 行动更新回调
    /// </summary>
    protected Action OnMoveUpdateAction;
    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    public abstract void OnLogicFrameUpdate();
    /// <summary>
    /// 行动完成
    /// </summary>
    public abstract void OnActionFinish();
}