/*--------------------------------------------------------------------------------------
* Title: 业务逻辑脚本自动生成工具
* Author: 铸梦xy
* Date:2024/12/24 11:29:44
* Description:业务逻辑层,主要负责游戏的业务逻辑处理
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using System.Collections.Generic;

namespace ZMGC.Battle
{
	public class BattleLogicCtrl : ILogicBehaviour
	{
		private	HeroLogicCtrl mHeroLogicCtrl;
		private MonsterLogicCtrl mMonsterLogicCtrl;
		
		public void OnCreate()
		{
			mHeroLogicCtrl = BattleWorld.GetExitsLogicCtrl<HeroLogicCtrl>();
			mMonsterLogicCtrl = BattleWorld.GetExitsLogicCtrl<MonsterLogicCtrl>();
		}

		/// <summary>
		/// 获取敌对列表
		/// </summary>
		/// <param name="objectType"> 所发起攻击的类型 </param>
		/// <returns> 敌人列表 </returns>
		public List<LogicActor> GetEnemyList(LogicObjectType objectType)
		{
			List<LogicActor> enemyList = new List<LogicActor>();
			switch (objectType)
			{
				case LogicObjectType.Hero:
					foreach (var monsterLogic in mMonsterLogicCtrl.monsterLogicList)
					{
						enemyList.Add(monsterLogic);
					}
					break;
				case LogicObjectType.Monster:
					enemyList.Add(mHeroLogicCtrl.HeroLogic);
					break;
			}
			return enemyList;
		}
		
		public void OnDestroy()
		{
		
		}
	
	}
}
