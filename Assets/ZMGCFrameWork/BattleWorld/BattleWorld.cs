using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZMAssetFrameWork;

namespace ZMGC.Battle
{

    public class BattleWorld : World
    {
        /// <summary>
        /// 逻辑帧累计运行时间
        /// </summary>
        private float mAccLogicRuntime;

        /// <summary>
        /// 下一个逻辑帧开始事件时间
        /// </summary>
        private float mNextLogicFrameTime;

        /// <summary>
        /// 逻辑帧动画缓动时间
        /// </summary>
        public float logicDeltaTime;
        
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

            UIModule.PopUpWindow<BattleWindow>();
            Debug.Log("BattleWorld OnCreate");
        }

        /// <summary>
        /// Unity渲染帧更新 模拟逻辑帧更新
        /// </summary>
        public override void OnUpdate()
        {
            base.OnUpdate();
            
            // 逻辑帧累计运行时间累加
            mAccLogicRuntime += Time.deltaTime;

            // 当前逻辑帧运行时间如果大于下一个逻辑帧时间，就需要更新逻辑帧
            // 追帧操作，控制帧数，保证所有设备逻辑帧帧数的一致性，并进行追帧操作
            while (mAccLogicRuntime > mNextLogicFrameTime)
            {
                // 更新逻辑帧
                OnLogicFrameUpdate();
                // 计算下一个逻辑帧的运行时间
                mNextLogicFrameTime += LogicFrameConfig.LogicFrameInterval;
                //逻辑帧id自增
                LogicFrameConfig.LogicFrameId++;
            }
            
            // 逻辑帧 1秒15帧 渲染一秒60帧
            // 0-1 ---- L
            //mAccLoginRuntime 0.01     LogicFrameConfig.LogicFrameInterval 0.066    mNextLogicFrameTime 0.066/0.066
            // (0.01+0.066-0.066)/0.066 = 0.01/0.066 = 当前值/最大值 与血条计算比例是一致的
            logicDeltaTime = (mAccLogicRuntime + LogicFrameConfig.LogicFrameInterval - mNextLogicFrameTime) / LogicFrameConfig.LogicFrameInterval;
        }
        
        /// <summary>
        /// 逻辑帧更新 TODO （后期通过服务端进行调用）
        /// </summary>
        private void OnLogicFrameUpdate()
        {
            HeroLogicCtrl.OnLogicFrameUpdate();
            MonsterLogicCtrl.OnLogicFrameUpdate();
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