/*--------------------------------------------------------------------------------------
* Title: 业务逻辑脚本自动生成工具
* Author: 铸梦xy
* Date:2024/12/15 20:22:35
* Description:业务逻辑层,主要负责游戏的业务逻辑处理
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using UnityEngine;
using ZMAssetFrameWork;

namespace ZMGC.Battle
{
	public class HeroLogicCtrl : ILogicBehaviour
	{
		public HeroLogic HeroLogic { get; private set; }
		
		public void OnCreate()
		{
		
		}

		/// <summary>
		/// 初始化英雄
		/// </summary>
		public void InitHero()
		{
			GameObject heroObj =  ZMAssetsFrame.Instantiate(AssetPathConfig.GAME_PREFABS_HERO + "1000", null);
			// 获取英雄渲染层
			HeroRender heroRender = heroObj.GetComponent<HeroRender>();
			global::HeroLogic heroLogic = new global::HeroLogic(1000, heroRender);
			HeroLogic = heroLogic;
			heroRender.SetLogicObject(heroLogic);
			// 初始化英雄逻辑层和渲染层
			heroLogic.OnCreate();
			heroRender.OnCreate(); 
		}
		
		public void OnLogicFrameUpdate()
		{
			HeroLogic.OnLogicFrameUpdate();
		}
		
		public void OnDestroy()
		{
		
		}
	
	}
}
