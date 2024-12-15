using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZMAssetFrameWork;

namespace ZMGC.Battle
{

    public class BattleWorld : World
    {
        public HeroLogicCtrl HeroLogicCtrl
        {
            get;
            private set;
        }
        public MonsterLogicCtrl MonsterLogicCtrl
        {
            get;
            private set;
        }
        
        public override void OnCreate()
        {
            base.OnCreate();

            HeroLogicCtrl = GetExitsLogicCtrl<HeroLogicCtrl>();
            MonsterLogicCtrl = GetExitsLogicCtrl<MonsterLogicCtrl>();
            
            HeroLogicCtrl.InitHero();
            MonsterLogicCtrl.InitMonster();
            
            Debug.Log("BattleWorld OnCreate");
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