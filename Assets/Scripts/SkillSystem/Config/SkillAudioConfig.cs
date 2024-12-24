using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] [HideMonoScript]
public class SkillAudioConfig
{
    [AssetList] [BoxGroup("技能音效文件")] [PreviewField(70, ObjectFieldAlignment.Left)] [OnValueChanged("OnAudioChange")]
    public AudioClip skillAudio;

    [BoxGroup("技能音效文件")] [LabelText("技能音效文件名称")] [ReadOnly, GUIColor("green")]
    public string audioName;

    [BoxGroup("参数配置")] [LabelText("技能音效触发帧")] [ReadOnly, GUIColor("green")]
    public int triggerFrame;

    [ToggleGroup("IsLoop", "是否循环")]
    public bool isLoop = false;
    
    [ToggleGroup("IsLoop", "结束帧")]
    public int endFrame;
    
    
    private void OnAudioChange()
    {
        if (skillAudio != null)
        {
            audioName = skillAudio.name;
        }
    }
}
