using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZMGC.Battle
{

    public class BattleWorld : World
    {
        public override void OnCreate()
        {
            base.OnCreate();
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