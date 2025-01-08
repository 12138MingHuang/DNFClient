using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[Serializable] [HideMonoScript]
public class SkillCharacterConfig
{
    [AssetList] [LabelText("角色模型")] [PreviewField(70, ObjectFieldAlignment.Center)]
    public GameObject skillCharacter;

    [TitleGroup("技能渲染", "所有英雄渲染数据会在技能开始释放时触发")] [LabelText("技能动画")]
    public AnimationClip skillAnim;

    [BoxGroup("动画数据")] [ProgressBar(0, 100, r: 0, g: 255, b: 0, Height = 30)] [HideLabel] [OnValueChanged("OnAnimaProgressValueChange")]
    public short animProgress = 0;

    [BoxGroup("动画数据")] [LabelText("是否循环动画")]
    public bool isLoopAnim = false;
    
    [BoxGroup("动画数据")] [LabelText("动画循环次数")] [ShowIf("isLoopAnim")]
    public int animLoopCount = 0;

    [BoxGroup("动画数据")] [LabelText("逻辑帧数")]
    public int logicFrame = 0;

    [BoxGroup("动画数据")] [LabelText("动画长度")]
    public float animLength = 0f;

    [BoxGroup("动画数据")] [LabelText("技能推荐时长(毫秒ms)")]
    public float skillDurationMS = 0f;

    // 临时角色对象
    private GameObject mTempCharacter;
    // 是否播放动画,用来控制暂停动画
    private bool mIsPlayAnim = false;
    // 上一次运行时间
    private double mLastRunTime = .0f;
    // 动画组件
    private Animation mAnimation = null;
    
    [ButtonGroup("按钮数组")] [Button("播放", ButtonSizes.Large)] [GUIColor(.4f, .8f, 1.0f)]
    public void Play()
    {
        if (skillCharacter != null)
        {
            //先从场景中查找技能对象，如果查找不到，则创建
            string characterName = skillCharacter.name;
            mTempCharacter = GameObject.Find(characterName);
            if (mTempCharacter == null)
            {
                mTempCharacter = GameObject.Instantiate(skillCharacter);
                mTempCharacter.name = mTempCharacter.name.Replace("(Clone)", "");
            }
        }
        
        // 判断模型身上是否有该动画，没有则添加
        mAnimation = mTempCharacter.GetComponent<Animation>();
        if (!mAnimation.GetClip(skillAnim.name))
        {
            mAnimation.AddClip(skillAnim, skillAnim.name);
        }

        mAnimation.clip = skillAnim;
        //计算动画文件长度
        animLength = isLoopAnim ? skillAnim.length * animLoopCount : skillAnim.length;
        //计算逻辑帧长度(个数)
        logicFrame = (int)(isLoopAnim ? skillAnim.length / 0.066f * animLoopCount : skillAnim.length / 0.066f);
        //计算技能推荐时长
        skillDurationMS = (int)(isLoopAnim ? skillAnim.length * animLoopCount * 1000 : skillAnim.length * 1000);
        mLastRunTime = .0f;
        //开始播放角色动画
        mIsPlayAnim = true;
        SkillComplierWindow window = SkillComplierWindow.GetWindow();
        window?.StartPlaySkill();
    }
    
    [ButtonGroup("按钮数组")] [Button("暂停", ButtonSizes.Large)]
    public void Pause()
    {
        mIsPlayAnim = false;
        SkillComplierWindow window = SkillComplierWindow.GetWindow();
        window?.PausePlaySkill();
    }
    
    [ButtonGroup("按钮数组")] [Button("保存配置", ButtonSizes.Large)] [GUIColor(.0f, 1.0f, .0f)]
    public void SaveAssets()
    {
        SkillComplierWindow window = SkillComplierWindow.GetWindow();
        window?.SaveSkillData();
    }

    public void OnUpdate(Action onProgressUpdate)
    {
        if (mIsPlayAnim)
        {
            if(mLastRunTime == .0f)
            {
                mLastRunTime = EditorApplication.timeSinceStartup;
            }
            //获取当前运行时间
            double curRunTime = EditorApplication.timeSinceStartup - mLastRunTime;
            
            //计算动画播放进度
            float curAnimNormalizationValue = (float) curRunTime / animLength;
            animProgress = (short)Mathf.Clamp(curAnimNormalizationValue * 100, 0, 100);
            //计算逻辑帧
            logicFrame = (int)(curRunTime / LogicFrameConfig.LogicFrameInterval);
            //采样动画，进行动画播放
            mAnimation.clip.SampleAnimation(mTempCharacter, (float)curRunTime);

            if (animProgress == 100)
            {
                //动画播放完成
                mIsPlayAnim = false;
                SkillComplierWindow window = SkillComplierWindow.GetWindow();
                window?.EndPlaySkill();
            }
            //触发窗口聚焦回调，刷新窗口，主要解决动画进度条无法及时刷新的问题
            onProgressUpdate?.Invoke();
        }
    }

    /// <summary>
    /// 动画进度值改变监听
    /// </summary>
    /// <param name="value"> 进度值 </param>
    public void OnAnimaProgressValueChange(float value)
    {
        if (skillCharacter != null)
        {
            //先从场景中查找技能对象，如果查找不到，则创建
            string characterName = skillCharacter.name;
            mTempCharacter = GameObject.Find(characterName);
            if (mTempCharacter == null)
            {
                mTempCharacter = GameObject.Instantiate(skillCharacter);
                mTempCharacter.name = mTempCharacter.name.Replace("(Clone)", "");
            }
        }
        // 判断模型身上是否有该动画，没有则添加
        mAnimation = mTempCharacter.GetComponent<Animation>();
        //根据当前动画进度进行采样
        float progressValue = (value / 100f) * skillAnim.length;
        logicFrame = (int)(progressValue / LogicFrameConfig.LogicFrameInterval);
        //采样动画，进行动画播放
        mAnimation.clip.SampleAnimation(mTempCharacter, progressValue);
    }
}
