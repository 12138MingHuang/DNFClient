using Sirenix.OdinInspector;
using System;
using UnityEngine;

[Serializable] [HideMonoScript]
public class SkillBulletConfig
{
    [AssetList, LabelText("子弹特效"), PreviewField(70, ObjectFieldAlignment.Left)]
    public GameObject bulletPrefab;
    [ReadOnly]
    public string bulletPrefabPath;
    [LabelText("智能锁定寻敌(常用于普通子弹，智能调整子弹发射角度射向前方敌人)")]
    public bool interlligentAttack;
    [LabelText("触发帧")]
    public int triggerFrame;
    [LabelText("是否循环创建"), BoxGroup("循环创建参数")]
    public bool isLoopCreate;
    [LabelText("循环间隔(ms 毫秒)"), BoxGroup("循环创建参数")]
    public int loopIntervalMS;
    [LabelText("最小随机位置波动范围"), ShowIf("isLoopCreate"), BoxGroup("循环创建参数")]
    public Vector3 minRandomRangeVect3;
    [LabelText("最大随机位置波动范围"), ShowIf("isLoopCreate"), BoxGroup("循环创建参数")]
    public Vector3 maxRandomRangeVect3;
    [LabelText("子弹移动速度")]
    public float moveSpeed;
    [LabelText("子弹存活时间(ms 毫秒)")]
    public int survivalTimeMS;
    [LabelText("子弹重力速度")]
    public Vector2 gravitySpeed;
    [LabelText("子弹发射偏移")]
    public Vector3 offset;
    [LabelText("子弹发射方向")]
    public Vector3 dir;
    [LabelText("子弹发射角度")]
    public Vector3 angle;
    [LabelText("子弹击中后是否销毁")]
    public bool isHitDestroy;
    [AssetList, LabelText("子弹击中特效"), PreviewField(70, ObjectFieldAlignment.Left)]
    public GameObject hitEffect;
    [ReadOnly]
    public string hitEffectPath;
    [LabelText("子弹击中特效存活时间(ms 毫秒)")]
    public int hitEffectSurvivalTimeMS = 3000;
    [LabelText("击中音效")]
    public AudioClip hitAudio;
    [ToggleGroup("isAttachDamage", "是否附加伤害")]
    public bool isAttachDamage = false;
    [ToggleGroup("isAttachDamage", "附加伤害配置")]
    public SkillDamageConfig damageConfig;
}