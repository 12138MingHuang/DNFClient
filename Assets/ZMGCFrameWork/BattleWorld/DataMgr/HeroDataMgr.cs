/*--------------------------------------------------------------------------------------
* Title: 数据脚本自动生成工具
* Author: 铸梦xy
* Date:2024/12/26 19:20:13
* Description:数据层,主要负责游戏数据的存储、更新和获取
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using System.Collections.Generic;

namespace ZMGC.Battle
{
	public class HeroDataMgr : IDataBehaviour
	{
		private Dictionary<int, int[]> mHeroNormalSkillCfgDic = new Dictionary<int, int[]>
		{
			{1000, new int[] { 1001, 1002, 1003 }}, // 英雄1的普通技能配置
		};

		private Dictionary<int, int[]> mHeroSkillCfgDic = new Dictionary<int, int[]>
		{
			{1000, new int[] { 1004 }}, // 英雄1的技能配置
		};
		
		public void OnCreate()
		{
		
		}

		/// <summary>
		/// 获取英雄的普通技能ID数组
		/// </summary>
		/// <param name="heroId"> 英雄ID</param>
		/// <returns> 英雄的普通技能ID数组</returns>
		public int[] GetHeroNormalSkillIdArray(int heroId)
		{
			return mHeroNormalSkillCfgDic[heroId];
		}

		/// <summary>
		/// 获取英雄的技能ID数组
		/// </summary>
		/// <param name="heroId"> 英雄ID</param>
		/// <returns> 英雄的技能ID数组</returns>
		public int[] GetHeroSkillIdArray(int heroId)
		{
			return mHeroSkillCfgDic[heroId];
		}
		
		public void OnDestroy()
		{
		
		}
	
	}
}
