/*
 *-------------------------
 *Title:UI表现层脚本自动生成工具
 *Author:ZHANGBIN
 *Date:2024/12/23 17:42:47
 *Description:改脚本只负责UI界面的交互，表现上的更新，不建议在此填写业务层的相关逻辑
 *注意：以下文件是自动生成的，再次生成不会覆盖原有的代码，会在原有的代码上新增
 *--------------------------
 */

using FixMath;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZMAssetFrameWork;
using ZMGC.Battle;
using ZMUIFrameWork;

public class BattleWindow : WindowBase
{
	public BattleWindowDataComponent dataCompt = null;
	
	private HeroLogic mHeroLogic = null;
	
	// 上次显示血条的时间，用于控制血条的显示频率
	private float mLastShowBloodTime = 0f;
	// 当前显示的血条
	private MonsterBloodItem mCurBloodItem;

	// 技能按钮列表的根节点
	private List<Transform> mSkillItemRootList;
	// 技能列表
	private List<SkillItem> mSkillItemList = new List<SkillItem>(); 

	#region 生命周期函数
	//调用机制与Mono Awake一致
	public override void OnAwake()
	{
		base.OnAwake();
		dataCompt = gameObject.GetComponent<BattleWindowDataComponent>();
		dataCompt.InitComponent(this);
		mSkillItemRootList = new List<Transform>();
		for (int i = 0; i < dataCompt.SkillRootTransform.childCount; ++i)
		{
			mSkillItemRootList.Add(dataCompt.SkillRootTransform.GetChild(i));
		}
	}
	//当界面显示时调用。
	public override void OnShow()
	{
		base.OnShow();
		mHeroLogic = BattleWorld.GetExitsLogicCtrl<HeroLogicCtrl>().HeroLogic;
		
		// 获取角色技能id数组
		int[] heroSkillIds = BattleWorld.GetExitsDataMgr<HeroDataMgr>().GetHeroSkillIdArray(mHeroLogic.HeroId);
		// 遍历角色技能数组，生成对应的技能按钮
		for (int i = 0; i < heroSkillIds.Length; ++i)
		{
			GameObject skillItemObj = ZMAssetFrameWork.ZMAssetsFrame.Instantiate(AssetPathConfig.GAME_PREFABS + "Item/SkillItem", mSkillItemRootList[i]);
			SkillItem skillItem = skillItemObj.GetComponent<SkillItem>();
			mSkillItemList.Add(skillItem);
			skillItem.SetItemSkillData(mHeroLogic.GetSkill(heroSkillIds[i]), mHeroLogic);
			skillItemObj.transform.localPosition = Vector3.zero;
			skillItemObj.transform.localRotation = Quaternion.identity;
			skillItemObj.transform.localScale = Vector3.one;
		}
	}
	//当界面隐藏时调用。
	public override void OnHide()
	{
		base.OnHide();
	}
	//当界面销毁时调用。
	public override void OnDestroy()
	{
		base.OnDestroy();
	}
	#endregion

	#region API Function
	/// <summary>
	/// 显示怪物血条
	/// </summary>
	/// <param name="monsterCfg">怪物配置</param>
	/// <param name="insId">对象Instanceid</param>
	/// <param name="curHp">怪物当前的血量</param>
	/// <param name="damageHp">伤害量</param>
	public void ShowMonsterDamage(MonsterCfg monsterCfg, int insId, FixInt curHp, FixInt damageHp)
	{
		// 同一个怪物的血条显示
		if (mCurBloodItem != null && mCurBloodItem.curShowMonsterInsId == insId)
		{
			mLastShowBloodTime = Time.realtimeSinceStartup;
			mCurBloodItem.Damage(-damageHp.RawInt);
			return;
		}
		// 血条显示冷却
		if(monsterCfg.type != (int)MonsterType.Boss && Time.realtimeSinceStartup - mLastShowBloodTime < 0.5f)
		{
			return;
		}
		// 回收血条对象到对象池
		if (mCurBloodItem != null)
		{
			ZMAssetsFrame.Release(mCurBloodItem.gameObject);
			mCurBloodItem = null;
		}
		// 生成血条对象
		string bloodName = monsterCfg.type == (int)MonsterType.Boss ? "BossBlood" : "MonsterBlood";
		GameObject bloodObj = ZMAssetsFrame.Instantiate(AssetPathConfig.GAME_PREFABS + "DamageItem/" + bloodName, dataCompt.BloodRootTransform, 
			Vector3.zero, Vector3.one, Quaternion.identity);
		mCurBloodItem = bloodObj.GetComponent<MonsterBloodItem>();
		mCurBloodItem.InitBloodData(monsterCfg, curHp.RawInt, insId);
		mCurBloodItem.Damage(-damageHp.RawInt);
		// 记录最后一次显示血条的时间
		mLastShowBloodTime = Time.realtimeSinceStartup;
	}
	#endregion

	#region UI组件生成事件
	public void OnNormalAttackButtonClick()
	{
		Debug.Log("普通攻击");
		mHeroLogic.ReleaseNormalAttack();
	}

	#endregion
}
