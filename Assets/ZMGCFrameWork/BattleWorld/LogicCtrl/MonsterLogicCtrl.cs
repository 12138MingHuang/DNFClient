/*--------------------------------------------------------------------------------------
* Title: 业务逻辑脚本自动生成工具
* Author: 铸梦xy
* Date:2024/12/15 20:22:56
* Description:业务逻辑层,主要负责游戏的业务逻辑处理
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using System.Collections.Generic;
using FixIntPhysics;
using FixMath;
using UnityEngine;
using ZMAssetFrameWork;

namespace ZMGC.Battle
{
	public class MonsterLogicCtrl : ILogicBehaviour
	{

		/// <summary>
		/// 怪物逻辑层脚本列表
		/// </summary>
		public List<MonsterLogic> monsterLogicList = new List<MonsterLogic>();
		
		private Vector3[] m_MonsterPosArr = new[]
		{
			new Vector3(-1, 0, 0),
			new Vector3(-3, 0 ,0)
		};
		private int[] m_MonsterIdArr = new[]
		{
			20001,
			20005
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
			foreach (var id in m_MonsterIdArr)
			{
				FixIntVector3 initPos = new FixIntVector3(m_MonsterPosArr[index]);
				// 生成怪物到场景中
				GameObject monsterObj = ZMAssetsFrame.Instantiate(AssetPathConfig.GAME_PREFABS_MONSTER + id, null);
				
				//处理怪物的碰撞数据
				BoxColliderGizmo boxInfo = monsterObj.GetComponent<BoxColliderGizmo>();
				boxInfo.enabled = false;
				// 创建定点数碰撞体
				FixIntBoxCollider monsterBox = new FixIntBoxCollider(boxInfo.mSize, boxInfo.mConter);
				monsterBox.SetBoxData(boxInfo.mConter, boxInfo.mSize);
				monsterBox.UpdateColliderInfo(initPos, new FixIntVector3(boxInfo.mSize));
				
				//创建怪物的逻辑层和渲染层
				MonsterRender monsterRender = monsterObj.GetComponent<MonsterRender>();
				MonsterLogic monsterLogic = new MonsterLogic(id, monsterRender, monsterBox, initPos);
				monsterRender.SetLogicObject(monsterLogic);
				
				monsterLogic.OnCreate();
				monsterRender.OnCreate();
				
				monsterLogicList.Add(monsterLogic);
				
				index++;
			}
		}

		public void OnLogicFrameUpdate()
		{
			for (int i = monsterLogicList.Count - 1; i >= 0; i--)
			{
				monsterLogicList[i].OnLogicFrameUpdate();
				
			}
		}
		
		public void OnDestroy()
		{
		
		}
	
	}
}
