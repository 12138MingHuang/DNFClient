/*
 *-------------------------
 *Title:UI表现层脚本自动生成工具
 *Author:ZHANGBIN
 *Date:2024/12/3 22:30:32
 *Description:改脚本只负责UI界面的交互，表现上的更新，不建议在此填写业务层的相关逻辑
 *注意：以下文件是自动生成的，再次生成不会覆盖原有的代码，会在原有的代码上新增
 *--------------------------
 */

using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZMAssetFrameWork;
using ZMGC.Hall;
using ZMUIFrameWork;

public class CreateRoleWindow : WindowBase
{
	public CreateRoleWindowDataComponent dataCompt = null;
	
	private UserDataMgr mUserDataMgr;
	private List<RoleSelectItem> mRoleItemList = new List<RoleSelectItem>();
	private int mCurSelectRoleId;
	private DOTweenAnimation mDOTweenAnim;
	private GameObject mPortraitObj;

	#region 生命周期函数
	//调用机制与Mono Awake一致
	public override void OnAwake()
	{
		mDisableAnimation = true;
		base.OnAwake();
		dataCompt = gameObject.GetComponent<CreateRoleWindowDataComponent>();
		dataCompt.InitComponent(this);
		
		mDOTweenAnim = dataCompt.RolePortraitRootTransform.GetComponent<DOTweenAnimation>();
		mUserDataMgr = HallWorld.GetExitsDataMgr<UserDataMgr>();

		for (int i = 0; i < mUserDataMgr.CreateRoleIdList.Count; i++)
		{
			GameObject roleItemObj = ZMAssetsFrame.Instantiate(AssetPathConfig.HALL_PREFABS_ITEM_PATH + "RoleSelectItem",
				dataCompt.ContentTransform, Vector3.zero, Vector3.one, Quaternion.identity);
			RoleSelectItem itemScript = roleItemObj.GetComponent<RoleSelectItem>();
			itemScript.SetItemData(mUserDataMgr.CreateRoleIdList[i]);
			mRoleItemList.Add(itemScript);
		}
		SelectRoleUpdate(mUserDataMgr.CreateRoleIdList[0]);
	}
	//当界面显示时调用。
	public override void OnShow()
	{
		base.OnShow();
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
	
	public void SelectRoleUpdate(int roleId)
	{
		mCurSelectRoleId = roleId;
		mUserDataMgr.RoleId = roleId;
		mUserDataMgr.UserName = dataCompt.NameInputField.text;
		HideAllItemSelect();
		SelectTargetItem();
		
		if(mPortraitObj != null)
		{
			ZMAssetsFrame.Release(mPortraitObj);
			mPortraitObj = null;
		}
		
		// 加载角色头像
		mPortraitObj = ZMAssetsFrame.Instantiate(AssetPathConfig.Hall_EFFECTS_PATH + "RulePortrait/" + roleId, 
			dataCompt.RolePortraitRootTransform, Vector3.zero, Vector3.one, Quaternion.identity);
		dataCompt.RolePortraitRootTransform.localPosition = new Vector3(-1500, 0, 0);
		mDOTweenAnim.DORestart();
	}
	private void HideAllItemSelect()
	{
		for (int i = 0; i < mRoleItemList.Count; i++)
		{
			mRoleItemList[i].SetSelectState(true);
		}
	}
	private void SelectTargetItem()
	{
		for (int i = 0; i < mRoleItemList.Count; i++)
		{
			if (mRoleItemList[i].roleId == mCurSelectRoleId)
			{
				mRoleItemList[i].SetSelectState(false);
				break;
			}
		}
	}
	#endregion

	#region UI组件生成事件
	public void OnCloseButtonClick()
	{

		HideWindow();
	}

	public void OnEnterGameButtonClick()
	{
		Debug.Log("点击开始游戏");
		dataCompt.EnterGameButton.interactable = false;
		HallWorld.EnterBattleWorld();
	}

	public void OnNameInputChange(string text)
	{

	}

	public void OnNameInputEnd(string text)
	{
		HallWorld.GetExitsDataMgr<UserDataMgr>().UserName = text;
	}

	#endregion
}
