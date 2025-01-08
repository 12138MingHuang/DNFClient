public partial class Skill
{
    /// <summary>
    /// 更新技能逻辑帧音频
    /// </summary>
    private void OnLogicFrameUpdateAudio()
    {
        if (mSkillDataConfig.audioCfgList != null && mSkillDataConfig.audioCfgList.Count > 0)
        {
            foreach (var skillAudioCfg in mSkillDataConfig.audioCfgList)
            {
                if (skillAudioCfg.triggerFrame == mCurLogicFrame)
                {
                    AudioController.Instance.PlaySoundByAudioClip(skillAudioCfg.skillAudio, skillAudioCfg.isLoop, 100);
                }

                if (skillAudioCfg.isLoop && skillAudioCfg.endFrame == mCurLogicFrame)
                {
                    AudioController.Instance.StopSound(skillAudioCfg.skillAudio);
                }
            }
        }
    }

    /// <summary>
    /// 播放技能命中音效
    /// </summary>
    public void PlyHitAudio()
    {
        AudioController.Instance.PlaySoundByAudioClip(mSkillDataConfig.skillConfig.skillHitAudio, false, 100);
    }
}
