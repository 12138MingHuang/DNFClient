using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AnimationAgent
{
    
#if UNITY_EDITOR

    private Animation mAnim;
    private double mLastRunTime;

    public void InitPlayAnim(Transform trans)
    {
        mAnim = trans.GetComponentInChildren<Animation>();
        EditorApplication.update += OnUpdate;
    }

    public void OnDestroy()
    {
        EditorApplication.update -= OnUpdate;
    }

    private void OnUpdate()
    {
        if (mAnim == null || mAnim.clip == null) return;
        
        if (mLastRunTime == 0)
        {
            mLastRunTime = EditorApplication.timeSinceStartup;
        }
        //获取当前运行时间
        double curRunTime = EditorApplication.timeSinceStartup - mLastRunTime;
        //计算播放动画进度
        float curAnimNormalizationValue = (float)(curRunTime / mAnim.clip.length);
        //采样动画，进行播放
        mAnim.clip.SampleAnimation(mAnim.gameObject, (float)curRunTime);

    }

#endif

}
