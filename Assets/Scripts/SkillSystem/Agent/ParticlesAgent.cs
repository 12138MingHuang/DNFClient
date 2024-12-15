using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ParticlesAgent
{
    
#if UNITY_EDITOR

    public ParticleSystem[] mParticleArr;
    private double mLastRunTime;

    public void InitPlayParticles(Transform trans)
    {
        mParticleArr = trans.GetComponentsInChildren<ParticleSystem>();
        EditorApplication.update += OnUpdate;
    }

    public void OnDestroy()
    {
        EditorApplication.update -= OnUpdate;
    }

    private void OnUpdate()
    {
        if(mLastRunTime == 0)
        {
            mLastRunTime = EditorApplication.timeSinceStartup;
        }
        // 获取当前运行时间
        double curRunTime = EditorApplication.timeSinceStartup;

        if (mParticleArr != null)
        {
            foreach (var particle in mParticleArr)
            {
                if (particle != null)
                {
                    // 停止所有粒子特效的播放
                    particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    //关闭由随机种子播放的粒子特效
                    particle.useAutoRandomSeed = false;
                    // 模拟粒子特效的播放
                    particle.Simulate((float)curRunTime);
                }
            }
        }
    }

#endif

}
