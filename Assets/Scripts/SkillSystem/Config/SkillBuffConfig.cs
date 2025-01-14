using Sirenix.OdinInspector;
using System;

[Serializable] [HideMonoScript]
public class SkillBuffConfig
{
    [LabelText("触发帧")] [GUIColor("green")]
    public int triggerFrame;

    [LabelText("附加buffId")]
    public int buffId;
}