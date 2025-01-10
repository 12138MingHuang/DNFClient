using FixMath;
using System.Collections.Generic;

public class AttributeModify_Buff_Group : BuffComposite
{
    /// <summary>
    /// 配置值，用于修改属性值。例如：hp等
    /// </summary>
    private FixInt configValue;
    /// <summary>
    /// 碰撞器，用于触发Buff效果。例如：碰撞到敌人时触发等
    /// </summary>
    private BuffCollider mBuffCollider;
    
    public AttributeModify_Buff_Group(Buff buff) : base(buff) { }
    
    public override void BuffDelay()
    {

    }
    
    public override void BuffStart()
    {
        if(buff.BuffConfig.buffParamsList.Count > 0)
        {
            configValue = buff.BuffConfig.buffParamsList[0].value;
        }

        if (buff.BuffConfig.targetConfig.isOpen)
        {
            mBuffCollider = new BuffCollider(buff);
            mBuffCollider.CreateOrUpdateCollider();
        }
    }
    
    public override void BuffTrigger()
    {
        if (buff.BuffConfig.targetConfig.isOpen)
        {
            // 获取当前碰撞体所碰撞到的目标
            List<LogicActor> targetList = mBuffCollider.CalculateColliderTargetObjects();
            for (int i = 0; i < targetList.Count; i++)
            {
                // 获取Buff击中目标
                LogicActor targetActor = targetList[i];
                if (targetActor.ObjectState != LogicObjectState.Death)
                {
                    // 造成伤害
                    targetActor.BuffDamage(configValue, buff.BuffConfig.targetConfig.damageConfig);
                    targetActor.OnHit(buff.BuffConfig.buffHitEffectObj, 1, buff.releaser, buff.releaser.LogicXAxis);
                    // 处理造成伤害后的Buff的附加
                    int[] buffidArr = buff.BuffConfig.targetConfig.damageConfig.addBuffs;
                    if (buffidArr != null && buffidArr.Length > 0)
                    {
                        for (int j = 0; j < buffidArr.Length; j++)
                        {
                            BuffSystem.Instance.AttachBuff(buffidArr[j], buff.releaser, targetActor, buff.skill);
                        }
                    }
                }
            }
            targetList.Clear();
            targetList = null;
        }
    }
    
    public override void BuffEnd()
    {
        mBuffCollider.OnRelease();
        mBuffCollider = null;
    }
}