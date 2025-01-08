using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能行动类型
/// </summary>
public enum MoveActionType
{
    [LabelText("指定目标位置")] TargetPos,
    [LabelText("引导位置")] GuidePos,
    [LabelText("贝塞尔移动")] BezierPos,
}

/// <summary>
/// 行动动作完成的操作
/// </summary>
public enum MoveActionFinishOperation
{
    [LabelText("无操作")] None,
    [LabelText("行动后添加技能")] Skill,
    [LabelText("行动后添加Buff")] Buff,
}

[Serializable] [HideMonoScript]
public class SkillActionConfig
{
    /// <summary>
    /// 是否显示移动位置
    /// </summary>
    private bool mIsShowMovePos;
    /// <summary>
    /// 是否显示行动完成参数
    /// </summary>
    private bool mIsShowFinishParam;
    /// <summary>
    /// 是否显示贝塞尔数据参数
    /// </summary>
    private bool mIsShowBezierPos;

    [LabelText("触发帧")]
    public int triggerFrame;
    [LabelText("移动方式"), OnValueChanged("OnMoveActionTypeChange")]
    public MoveActionType moveActionType;
    [LabelText("最高点位置"), ShowIf("mIsShowBezierPos")]
    public Vector3 heightPos;
    [LabelText("移动位置"), ShowIf("mIsShowMovePos")]
    public Vector3 movePos;
    [LabelText("移动所需时间(MS)")]
    public int durationMS;
    [LabelText("移动完成操作"), OnValueChanged("OnMoveActionFinishOperationChange")]
    public MoveActionFinishOperation actionFinishOperation;
    [LabelText("触发参数"), ShowIf("mIsShowFinishParam")]
    public List<int> actionFinishIdList;

    public void OnMoveActionTypeChange(MoveActionType moveActionType)
    {
        mIsShowMovePos = moveActionType == MoveActionType.TargetPos || moveActionType == MoveActionType.BezierPos;
        mIsShowBezierPos = moveActionType == MoveActionType.BezierPos;
    }

    public void OnMoveActionFinishOperationChange(MoveActionFinishOperation actionFinishOperation)
    {
        mIsShowFinishParam = actionFinishOperation != MoveActionFinishOperation.None;
    }
}
