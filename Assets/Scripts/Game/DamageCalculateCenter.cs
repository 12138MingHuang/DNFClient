using FixMath;
using System;

public class DamageCalculateCenter
{
    /// <summary>
    /// 物理攻击力(武器基础物理攻击X（1+力量/250)
    /// </summary>
    /// <param name="attacker"> 攻击者</param>
    /// <returns> 物理攻击力</returns>
    public static FixInt GetADAttack(LogicActor attacker)
    {
        return attacker.AD * (1 + attacker.STR / 250);
    }

    /// <summary>
    /// 魔法攻击力(武器基础物理攻击X（1+力量/250)
    /// </summary>
    /// <param name="attacker"> 攻击者</param>
    /// <returns> 魔法攻击力</returns>
    public static FixInt GetAPAttack(LogicActor attacker)
    {
        return attacker.AP * (1 + attacker.INT / 250);
    }

    /// <summary>
    /// 获取物理伤害减免 减伤百分比=自身防御/（攻击方等级X200+自身防御）减伤封顶75%
    /// </summary>
    /// <param name="attacker"> 攻击者</param>
    /// <param name="attackTarget"> 被攻击者</param>
    /// <returns> 物理伤害减免</returns>
    public static FixInt GetADReduction(LogicActor attacker, LogicActor attackTarget)
    {
        FixInt damageReductionRate = attackTarget.ADDef / (attacker.Level * 200 + attackTarget.ADDef);
        return damageReductionRate > 0.75f ? 0.75f : damageReductionRate;
    }

    /// <summary>
    /// 获取魔法伤害减免 减伤百分比=自身防御/（攻击方等级X250+自身防御）减伤封顶75%
    /// </summary>
    /// <param name="attacker"> 攻击者</param>
    /// <param name="attackTarget"> 被攻击者</param>
    /// <returns> 魔法伤害减免</returns>
    public static FixInt GetAPReduction(LogicActor attacker, LogicActor attackTarget)
    {
        FixInt damageReductionRate = attackTarget.APdef / (attacker.Level * 200 + attackTarget.APdef);
        return damageReductionRate > 0.75f ? 0.75f : damageReductionRate;
    }

    /// <summary>
    /// 获取物理暴击伤害 暴击伤害=总伤害X（100%+50%）
    /// </summary>
    /// <param name="totalDamage"> 总伤害</param>
    /// <param name="target"> 被攻击者</param>
    /// <returns> 物理暴击伤害</returns>
    public static FixInt GetADPCTDamage(FixInt totalDamage, LogicActor target)
    {
        return totalDamage * (1 + target.PCT);
    }

    /// <summary>
    /// 获取魔法暴击伤害 暴击伤害=总伤害X（100%+50%）
    /// </summary>
    /// <param name="totalDamage"> 总伤害</param>
    /// <param name="target"> 被攻击者</param>
    /// <returns> 魔法暴击伤害</returns>
    public static FixInt GetAPMCTDamage(FixInt totalDamage, LogicActor target)
    {
        return totalDamage * (1 + target.MCT);
    }

    /// <summary>
    /// 计算总伤害
    /// </summary>
    /// <param name="damageConfig"> 技能伤害配置</param>
    /// <param name="attacker"> 攻击者</param>
    /// <param name="attackTarget"> 被攻击者</param>
    /// <returns> 总伤害</returns>
    public static FixInt CalculateDamage(SkillDamageConfig damageConfig, LogicActor attacker, LogicActor attackTarget)
    {
        FixInt finalDamage = FixInt.Zero;
        switch (damageConfig.damageType)
        {

            case DamageType.None:
            case DamageType.ADDamage:
                finalDamage = GetADReduction(attacker, attackTarget) * GetADAttack(attacker);
                break;
            case DamageType.APDamage:
                finalDamage = GetAPReduction(attacker, attackTarget) * GetAPAttack(attacker);
                break;
        }
        return finalDamage * (damageConfig.damageRate / 100);
    }
    
    public static FixInt CalculateDamage(BuffConfig damageConfig, LogicActor attacker, LogicActor attackTarget)
    {
        FixInt finalDamage = FixInt.Zero;
        DamageType damageType = damageConfig.targetConfig.isOpen ? damageConfig.targetConfig.damageConfig.damageType : damageConfig.damageType;
        switch (damageType)
        {

            case DamageType.None:
            case DamageType.ADDamage:
                finalDamage = GetADReduction(attacker, attackTarget) * GetADAttack(attacker);
                break;
            case DamageType.APDamage:
                finalDamage = GetAPReduction(attacker, attackTarget) * GetAPAttack(attacker);
                break;
        }
        return finalDamage * (damageConfig.targetConfig.isOpen ? damageConfig.targetConfig.damageConfig.damageRate / 100 : damageConfig.damageRate / 100);
    }
}