public enum MonsterType
{ 
	Normal = 1,
	Elite = 2,
	Boss = 5,
}

[System.Serializable]
public class MonsterCfg
{
	/// <summary>
	/// 怪物id
	/// </summary>
	public int id;
	/// <summary>
	/// 怪物名称
	/// </summary>
	public string name;
	/// <summary>
	/// 怪物类型
	/// </summary>
	public int type;
	/// <summary>
	/// 怪物描述
	/// </summary>
	public string MonsterDes;
	/// <summary>
	/// 技能数组
	/// </summary>
	public int[] skillidArr;
	/// <summary>
	/// 血量
	/// </summary>
	public int hp;
	/// <summary>
	/// 法力值
	/// </summary>
	public int mp;
	/// <summary>
	/// 魔法攻击力
	/// </summary>
	public int ap;
	/// <summary>
	/// 物理攻击力
	/// </summary>
	public int ad;
	/// <summary>
	/// 物理防御力
	/// </summary>
	public int adDef;
	/// <summary>
	/// 魔法防御力
	/// </summary>
	public int apDef;
	/// <summary>
	/// 物理暴击率
	/// </summary>
	public int pct;
	/// <summary>
	/// 魔法暴击率
	/// </summary>
	public int mct;
	/// <summary>
	/// 物理暴击倍率
	/// </summary>
	public float adPctRate;
	/// <summary>
	/// 魔法暴击倍率
	/// </summary>
	public float apMctRate;
	/// <summary>
	/// 力量
	/// </summary>
	public int str;
	/// <summary>
	/// 体力
	/// </summary>
	public int sta;
	/// <summary>
	/// 智力
	/// </summary>
	public int Int;
	/// <summary>
	/// 精神
	/// </summary>
	public int spi;
	/// <summary>
	/// 敏捷
	/// </summary>
	public int agl;
}
