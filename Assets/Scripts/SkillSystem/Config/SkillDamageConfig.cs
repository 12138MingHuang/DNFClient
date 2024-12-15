using System;
using System.Collections;
using System.Collections.Generic;
using FixIntPhysics;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
[HideMonoScript]
public class SkillDamageConfig
{
    [LabelText("触发帧")] public int triggerFrame;
    [LabelText("结束帧")] public int endFrame;
    [LabelText("伤害触发间隔")] public int triggerIntervalMS;
    [LabelText("伤害是否跟随特效")] public bool isFollowEffect;
    [LabelText("伤害类型")] public DamageType damageType;
    [LabelText("伤害倍率")] public int damageRate;

    [LabelText("伤害检测方式")] [OnValueChanged("OnDetectionValueChange")]
    public DamageDetectionMode detectionMode;

    [LabelText("Box碰撞体的大小")] [ShowIf("mShowBox3D")] [OnValueChanged("OnBoxValueChange")]
    public Vector3 boxSize = Vector3.one;

    [LabelText("Box碰撞体偏移值")] [ShowIf("mShowBox3D")] [OnValueChanged("OnColliderOffsetChange")]
    public Vector3 boxOffset = Vector3.zero;

    [LabelText("圆球伤害检测半径")] [ShowIf("mShowSphere3D")] [OnValueChanged("OnRadiusValueChange")]
    public float radius = 1;

    [LabelText("圆球碰撞体偏移值")] [ShowIf("mShowSphere3D")] [OnValueChanged("OnColliderOffsetChange")]
    public Vector3 sphereOffset = new Vector3(0, .9f, 0);

    [LabelText("圆球检测半径高度")] [ShowIf("mShowSphere3D")]
    public float radiusHeight = 0;

    [LabelText("碰撞体位置类型")] public ColliderPosType colliderPosType = ColliderPosType.FollowDir;
    [LabelText("伤害触发目标")] public TargetType targetType = TargetType.None;

    [TitleGroup("附加Buff", "伤害生效的一瞬间，附加指定的多个Buff")]
    public int[] addBuffs;

    [TitleGroup("触发后续技能", "造成伤害后且技能释放完毕后触发的技能")]
    public int triggerSkillId;

#if UNITY_EDITOR

    // 是否显示3DBox碰撞体
    private bool mShowBox3D;

    // 是否显示3D圆球碰撞体
    private bool mShowSphere3D;

    // box碰撞体
    private FixIntBoxCollider boxCollider;

    // 圆球碰撞体
    private FixIntSphereCollider sphereCollider;

    // 当前执行到的逻辑帧
    private int mCurLogicFrame = 0;

    /// <summary>
    /// 碰撞检测类型发生变化
    /// </summary>
    /// <param name="detectionMode"> 碰撞检测类型 </param>
    private void OnDetectionValueChange(DamageDetectionMode detectionMode)
    {
        mShowBox3D = detectionMode == DamageDetectionMode.Box3D;
        mShowSphere3D = detectionMode == DamageDetectionMode.Sphere3D;
        CreateCollider();
    }

    /// <summary>
    /// 碰撞体中心点发生变化
    /// </summary>
    private void OnColliderOffsetChange()
    {
        switch (detectionMode)
        {
            case DamageDetectionMode.Box3D:
                boxCollider?.SetBoxData(GetColliderOffsetPos(), boxSize, colliderPosType == ColliderPosType.FollowPos);
                break;
            case DamageDetectionMode.Sphere3D:
                sphereCollider?.SetBoxData(radius, GetColliderOffsetPos(),
                    colliderPosType == ColliderPosType.FollowPos);
                break;
        }
    }

    /// <summary>
    /// Box碰撞体大小发生变化
    /// </summary>
    /// <param name="size"> Box碰撞体大小 </param>
    private void OnBoxValueChange(Vector3 size)
    {
        if (boxCollider != null)
            boxCollider.SetBoxData(GetColliderOffsetPos(), size, colliderPosType == ColliderPosType.FollowPos);
        else Debug.LogError("碰撞体不存在");
    }

    /// <summary>
    /// 圆球碰撞体半径发生变化
    /// </summary>
    /// <param name="radius"> 圆球碰撞体半径 </param>
    private void OnRadiusValueChange(float radius)
    {
        if (sphereCollider != null)
            sphereCollider.SetBoxData(radius, GetColliderOffsetPos(), colliderPosType == ColliderPosType.FollowPos);
        else Debug.LogError("碰撞体不存在");
    }

    /// <summary>
    /// 获取碰撞体的相对于角色的偏移位置
    /// </summary>
    private Vector3 GetColliderOffsetPos()
    {
        Vector3 offsetPos = Vector3.zero;
        Vector3 characterPos = SkillComplierWindow.GetCharacterPos();
        switch (detectionMode)
        {
            case DamageDetectionMode.Box3D:
                offsetPos = characterPos + boxOffset;
                break;
            case DamageDetectionMode.Sphere3D:
                offsetPos = characterPos + sphereOffset;
                break;
            case DamageDetectionMode.Cylinder3D:
                offsetPos = Vector3.zero;
                break;
        }

        return offsetPos;
    }

    /// <summary>
    /// 创建碰撞体
    /// </summary>
    public void CreateCollider()
    {
        DestroyCollider();
        switch (detectionMode)
        {
            case DamageDetectionMode.Box3D:
                boxCollider = new FixIntBoxCollider(boxSize, GetColliderOffsetPos());
                boxCollider.SetBoxData(GetColliderOffsetPos(), boxSize, colliderPosType == ColliderPosType.FollowPos);
                break;
            case DamageDetectionMode.Sphere3D:
                sphereCollider = new FixIntSphereCollider(radius, GetColliderOffsetPos());
                sphereCollider.SetBoxData(radius, GetColliderOffsetPos(), colliderPosType == ColliderPosType.FollowPos);
                break;
        }
    }

    /// <summary>
    /// 销毁碰撞体
    /// </summary>
    public void DestroyCollider()
    {
        if (boxCollider != null)
        {
            boxCollider.OnRelease();
        }

        if (sphereCollider != null)
        {
            sphereCollider.OnRelease();
        }
    }

    /// <summary>
    /// 当前窗口初始化
    /// </summary>
    public void OnInit()
    {
        CreateCollider();
    }

    /// <summary>
    /// 当前窗口关闭
    /// </summary>
    public void OnRelease()
    {
        DestroyCollider();
    }

    /// <summary>
    /// 当前播放技能
    /// </summary>
    public void StartPlaySkill()
    {
        mCurLogicFrame = 0;
        DestroyCollider();
    }

    /// <summary>
    /// 当前播放技能结束
    /// </summary>
    public void EndPlaySkill()
    {
        DestroyCollider();
    }

    /// <summary>
    /// 当前逻辑帧更新
    /// </summary>
    public void OnLogicFrameUpdate()
    {
        if (mCurLogicFrame == triggerFrame)
        {
            CreateCollider();
        }
        else if (mCurLogicFrame == endFrame)
        {
            DestroyCollider();
        }

        mCurLogicFrame++;
    }

#endif
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