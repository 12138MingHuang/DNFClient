/*--------------------------------------------------------------------------------------
* Title: 业务逻辑脚本自动生成工具
* Author: 铸梦xy
* Date:2024/12/15 20:22:56
* Description:业务逻辑层,主要负责游戏的业务逻辑处理
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using FixIntPhysics;
using UnityEngine;
using ZMAssetFrameWork;

namespace ZMGC.Battle
{
	public class MonsterLogicCtrl : ILogicBehaviour
	{

		private Vector3[] m_MonsterPosArr = new[]
		{
			new Vector3(0, 0, 0),
		};
		private int[] m_MonsterIdArr = new[]
		{
			20001,
		};
		
		public void OnCreate()
		{
		
		}

		/// <summary>
		/// 初始化怪物
		/// </summary>
		public void InitMonster()
		{
			int index = 0;
			foreach (var item in m_MonsterIdArr)
			{
				// 生成怪物到场景中
				GameObject monsterObj = ZMAssetsFrame.Instantiate(AssetPathConfig.GAME_PREFABS_MONSTER + "20001", null);
				monsterObj.transform.position = m_MonsterPosArr[index];
				
				//处理怪物的碰撞数据
				BoxColliderGizmo boxInfo = monsterObj.GetComponent<BoxColliderGizmo>();
				boxInfo.enabled = false;
				// 创建定点数碰撞体
				FixIntBoxCollider monsterBox = new FixIntBoxCollider(boxInfo.mSize, boxInfo.mConter);
				monsterBox.SetBoxData(boxInfo.mConter, boxInfo.mSize);
				monsterBox.UpdateColliderInfo(monsterObj.transform.position, boxInfo.mSize);
				
				index++;
			}
		}
		
		public void OnDestroy()
		{
		
		}
	
	}
}
