using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZMGC.Battle;

namespace ZMGC.Hall
{
    public class HallWorld : World
    {
        public override void OnCreate()
        {
            base.OnCreate();
            UIModule.Instance.PopUpWindow<CreateRoleWindow>();
            Debug.Log("HallWorld OnCreate>>>");
        }

        /// <summary>
        /// 进入战斗世界
        /// </summary>
        public static void EnterBattleWorld()
        {
            LoadSceneManager.Instance.LoadSceneAsync(Enum.GetName(typeof(GameSceneEnum), GameSceneEnum.Battle), () =>
            {
                // 清理所有UI窗口
                UIModule.DestroyAllWindow();
                WorldManager.CreateWorld<BattleWorld>();
                Debug.Log($"用户名：{HallWorld.GetExitsDataMgr<UserDataMgr>().UserName}");
            });
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Debug.Log("HallWorld OnDestroy>>>");
        }

        public override void OnDestroyPostProcess(object args)
        {
            base.OnDestroyPostProcess(args);
            Debug.Log("HallWorld OnDestroyPostProcess>>>");
        }
    }
}