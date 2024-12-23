using System.Collections.Generic;
using UnityEngine;

public partial class Skill
{
    /// <summary>
    /// 特效对象字典，key为特效配置的Hashcode value为特效对象
    /// </summary>
    private Dictionary<int, GameObject> mEffectDic = new Dictionary<int, GameObject>();

    /// <summary>
    /// 逻辑帧更新特效
    /// </summary>
    public void OnLogicFrameUpdateEffect()
    {
        if (mSkillDataConfig.effectCfgList != null && mSkillDataConfig.effectCfgList.Count > 0)
        {
            foreach (var skillData in mSkillDataConfig.effectCfgList)
            {
                if (skillData.skillEffect != null && mCurLogicFrame == skillData.triggerFrame)
                {
                    DestroyEffect(skillData);
                    // 技能特效生成
                    GameObject effectObj = GameObject.Instantiate(skillData.skillEffect);
                    effectObj.transform.localPosition = Vector3.zero;
                    effectObj.transform.localRotation = Quaternion.identity;
                    effectObj.transform.localScale = Vector3.one;

                    mEffectDic.Add(skillData.GetHashCode(), effectObj);
                }

                if (mCurLogicFrame == skillData.endFrame)
                {
                    // 技能特效结束，技能特效销毁
                    DestroyEffect(skillData);
                }
            }
        }
    }

    /// <summary>
    /// 销毁对应配置生成的特效
    /// </summary>
    /// <param name="skillEffectConfig"> 特效配置 </param>
    public void DestroyEffect(SkillEffectConfig skillEffectConfig)
    {
        GameObject effect = null;
        int hashCode = skillEffectConfig.GetHashCode();
        mEffectDic.TryGetValue(hashCode, out effect);
        if (effect != null)
        {
            mEffectDic.Remove(hashCode);
            GameObject.Destroy(effect);
        }
    }

}
