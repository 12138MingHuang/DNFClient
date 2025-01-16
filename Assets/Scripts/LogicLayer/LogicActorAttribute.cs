using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 逻辑对象属性
/// </summary>
public partial class LogicActor
{
    #region 内部属性(最基础的属性)
    
    /// <summary>
    /// 等级
    /// </summary>
    protected FixInt level = 1;
    /// <summary>
    /// 怪物id
    /// </summary>
    protected FixInt id;
    /// <summary>
    /// 怪物名称
    /// </summary>
    protected string name;
    /// <summary>
    /// 怪物类型
    /// </summary>
    protected FixInt type;
    /// <summary>
    /// 怪物描述
    /// </summary>
    protected string MonsterDes;
    /// <summary>
    /// 技能数组
    /// </summary>
    protected FixInt[] skillidArr;
    /// <summary>
    /// 血量
    /// </summary>
    protected FixInt hp;
    /// <summary>
    /// 法力值
    /// </summary>
    protected FixInt mp;
    /// <summary>
    /// 魔法攻击力
    /// </summary>
    protected FixInt ap;
    /// <summary>
    /// 物理攻击力
    /// </summary>
    protected FixInt ad;
    /// <summary>
    /// 物理防御力
    /// </summary>
    protected FixInt adDef;
    /// <summary>
    /// 魔法防御力
    /// </summary>
    protected FixInt apDef;
    /// <summary>
    /// 物理暴击率
    /// </summary>
    protected FixInt pct;
    /// <summary>
    /// 魔法暴击率
    /// </summary>
    protected FixInt mct;
    /// <summary>
    /// 物理暴击倍率
    /// </summary>
    protected float adPctRate;
    /// <summary>
    /// 魔法暴击倍率
    /// </summary>
    protected float apMctRate;
    /// <summary>
    /// 力量
    /// </summary>
    protected FixInt str;
    /// <summary>
    /// 体力
    /// </summary>
    protected FixInt sta;
    /// <summary>
    /// 智力
    /// </summary>
    protected FixInt Int;
    /// <summary>
    /// 精神
    /// </summary>
    protected FixInt spi;
    /// <summary>
    /// 敏捷
    /// </summary>
    protected FixInt agl;
    
    #endregion
    
    #region 战斗时通过buff增加的属性

    /// <summary>
    /// 战斗时通过Buff增加的物理防御力
    /// </summary>
    public FixInt addADDef;
    /// <summary>
    /// 战斗时通过Buff增加的魔法防御力
    /// </summary>
    public FixInt addAPDef;
    /// <summary>
    /// 战斗时通过Buff增加的物理攻击力
    /// </summary>
    public FixInt addAD;
    /// <summary>
    /// 战斗时通过Buff增加的魔法攻击力
    /// </summary>
    public FixInt addAP;
    /// <summary>
    /// 战斗时通过Buff增加的物理暴击率
    /// </summary>
    public FixInt addPCT;
    /// <summary>
    /// 战斗时通过Buff增加的魔法暴击率
    /// </summary>
    public FixInt addMCT;
    /// <summary>
    /// 战斗时通过Buff增加的物理暴击倍率
    /// </summary>
    public FixInt addADMctRate;
    /// <summary>
    /// 战斗时通过Buff增加的魔法暴击倍率
    /// </summary>
    public FixInt addAPMctRate;

    /// <summary>
    /// 战斗时通过Buff增加的力量
    /// </summary>
    public FixInt addStr;
    /// <summary>
    /// 战斗时通过Buff增加的体力
    /// </summary>
    public FixInt addSta;
    /// <summary>
    /// 战斗时通过Buff增加的智力
    /// </summary>
    public FixInt addInt;
    /// <summary>
    /// 战斗时通过Buff增加的精神
    /// </summary>
    public FixInt addSpi;
    /// <summary>
    /// 战斗时通过Buff增加的敏捷
    /// </summary>
    public FixInt addAgl;
    
    #endregion
    
    #region 公开属性
    
    /// <summary>
    /// 等级
    /// </summary>
    public FixInt Level { get { return level; } }
    /// <summary>
    /// 血量
    /// </summary>
    public FixInt HP { get { return hp ; }}
    /// <summary>
    /// 法力值
    /// </summary>
    public FixInt MP { get { return mp; } }
    /// <summary>
    /// 魔法攻击力
    /// </summary>
    public FixInt AP { get { return addAP + ap; } }
    /// <summary>
    /// 物理攻击力
    /// </summary>
    public FixInt AD { get { return addAD + ad; } }
    /// <summary>
    /// 物理防御力
    /// </summary>
    public FixInt ADDef { get { return addADDef + adDef; } }
    /// <summary>
    /// 魔法防御力
    /// </summary>
    public FixInt APdef { get { return addAPDef + apDef; } }
    /// <summary>
    /// 物理暴击率
    /// </summary>
    public FixInt PCT { get { return addPCT + pct; } }
    /// <summary>
    /// 魔法暴击率
    /// </summary>
    public FixInt MCT { get { return addMCT + mct; } }
    /// <summary>
    /// 物理暴击倍率
    /// </summary>
    public FixInt ADPCTRate { get { return addADMctRate + adPctRate; } }
    /// <summary>
    /// 魔法暴击倍率
    /// </summary>
    public FixInt APMCTRate { get { return addADMctRate + apMctRate; } }
    /// <summary>
    /// 力量
    /// </summary>
    public FixInt STR { get { return addStr + str; } }
    /// <summary>
    /// 体力
    /// </summary>
    public FixInt STA { get { return addSta + sta; } }
    /// <summary>
    /// 智力
    /// </summary>
    public FixInt INT { get { return addInt + Int; } }
    /// <summary>
    /// 精神
    /// </summary>
    public FixInt SPI { get { return addSpi + spi; } }
    /// <summary>
    /// 敏捷
    /// </summary>
    public FixInt AGL { get { return addAgl + agl; } }
    
    #endregion

    /// <summary>
    /// 减少血量
    /// </summary>
    /// <param name="reduceHp"> 减少的血量 </param>
    public void ReduceHP(FixInt reduceHp)
    {
        hp -= reduceHp;
        if (hp <= 0) hp = 0;
    }
}
