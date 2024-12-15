using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable] [HideMonoScript]
public class SkillDamageConfig
{
    [LabelText("触发帧")]
    public int triggerFrame;
    [LabelText("结束帧")]
    public int endFrame;
    [LabelText("伤害触发间隔")]
    public int triggerIntervalMS;
    [LabelText("伤害是否跟随特效")]
    public bool isFollowEffect;
    [LabelText("伤害类型")]
    public DamageType damageType;
    [LabelText("伤害倍率")]
    public int damageRate;
    [LabelText("伤害检测方式")] [OnValueChanged("OnDetectionValueChange")]
    public DamageDetectionMode detectionMode;
    [LabelText("Box碰撞体的大小")] [ShowIf("mShowBox3D")]
    public Vector3 boxSize = Vector3.one;
    [LabelText("Box碰撞体偏移值")] [ShowIf("mShowBox3D")]
    public Vector3 boxOffset = Vector3.zero;
    [LabelText("圆球碰撞体偏移值")] [ShowIf("mShowSphere3D")]
    public Vector3 sphereOffset = new Vector3(0, .9f, 0);
    [LabelText("圆球伤害检测半径")] [ShowIf("mShowSphere3D")]
    public float radius = 1;
    [LabelText("圆球检测半径高度")] [ShowIf("mShowSphere3D")]
    public float radiusHeight = 0;
    [LabelText("碰撞体位置类型")]
    public ColliderPosType colliderPosType = ColliderPosType.FollowDir;
    [LabelText("伤害触发目标")]
    public TargetType targetType = TargetType.None;
    [TitleGroup("附加Buff", "伤害生效的一瞬间，附加指定的多个Buff")]
    public int[] addBuffs;
    [TitleGroup("触发后续技能", "造成伤害后且技能释放完毕后触发的技能")]
    public int triggerSkillId;

    // 是否显示3DBox碰撞体
    private bool mShowBox3D;
    // 是否显示3D圆球碰撞体
    private bool mShowSphere3D;

    private void OnDetectionValueChange(DamageDetectionMode detectionMode)
    {
        mShowBox3D = detectionMode == DamageDetectionMode.Box3D;
        mShowSphere3D = detectionMode == DamageDetectionMode.Sphere3D;
    }
}

public enum ColliderPosType
{
    [LabelText("跟随角色朝向")] FollowDir,
    [LabelText("跟随角色位置")] FollowPos,
    [LabelText("中心坐标")] CenterPos,
    [LabelText("目标位置")] TargetPos,
}

public enum DamageType
{
    [LabelText("无伤害")] None,
    [LabelText("物理伤害")] ADDamage,
    [LabelText("魔法伤害")] APDamage
}

public enum DamageDetectionMode
{
    [LabelText("无配置")] None,
    [LabelText("3DBox碰撞检测")] Box3D,
    [LabelText("3D圆球碰撞检测")] Sphere3D,
    [LabelText("3D圆柱碰撞检测")] Cylinder3D,
    [LabelText("半径的距离")] RadiusDistance,
    [LabelText("所有目标")] AllTarget
}

public enum TargetType
{
    [LabelText("无配置")] None,
    [LabelText("队友")] Teammate,
    [LabelText("敌人")] Enemy,
    [LabelText("自身")] Self,
    [LabelText("所有对象")] AllObject
}