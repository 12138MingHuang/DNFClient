using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZMAssetFrameWork;

namespace ZMGC.Battle
{

    public class BattleWorld : World
    {
        public override void OnCreate()
        {
            base.OnCreate();
            Debug.Log("BattleWorld OnCreate");
            ZMAssetsFrame.Instantiate(AssetPathConfig.GAME_PREFABS_HERO+"Cube", null);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override void OnDestroyPostProcess(object args)
        {
            base.OnDestroyPostProcess(args);
        }
    }
}