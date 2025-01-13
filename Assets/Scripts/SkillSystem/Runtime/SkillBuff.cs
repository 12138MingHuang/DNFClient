public partial class Skill
{
    public void OnLogicFrameUpdateBuff()
    {
        if (mSkillDataConfig.buffCfgList != null && mSkillDataConfig.buffCfgList.Count > 0)
        {
            for (int i = 0; i < mSkillDataConfig.buffCfgList.Count; i++)
            {
                SkillBuffConfig buffCfg = mSkillDataConfig.buffCfgList[i];
                if (mCurLogicFrame == buffCfg.triggerFrame)
                {
                    BuffSystem.Instance.AttachBuff(buffCfg.buffId, mSkillCreator, mSkillCreator, this);
                }
            }
        }
    }
}