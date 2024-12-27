using Sirenix.Utilities.Editor;
using System.Collections.Generic;
using UnityEngine;

public partial class Skill
{
    /// <summary>
    /// 特效对象字典，key为特效配置的Hashcode value为特效对象
    /// </summary>
    private Dictionary<int, SkillEffectLogic> mEffectDic = new Dictionary<int, SkillEffectLogic>();

    /// <summary>
    /// 逻辑帧更新特效
    /// </summary>
    private void OnLogicFrameUpdateEffect()
    {
        if (mSkillDataConfig.effectCfgList != null && mSkillDataConfig.effectCfgList.Count > 0)
        {
            foreach (var skillData in mSkillDataConfig.effectCfgList)
            {
                if (skillData.skillEffect != null && mCurLogicFrame == skillData.triggerFrame)
                {
                    DestroyEffect(skillData);

                    Transform effectParent = null;
                    if (skillData.isSetTransParent)
                    {
                        // 获取左手或右手节点
                        effectParent = mSkillCreator.RenderObject.GetTransParent(skillData.transParent);
                    }
                    
                    // 技能特效生成
                    GameObject effectObj = GameObject.Instantiate(skillData.skillEffect, effectParent);
                    effectObj.transform.localPosition = Vector3.zero;
                    effectObj.transform.localRotation = Quaternion.identity;
                    effectObj.transform.localScale = Vector3.one;

                    SkillEffectRender effectRender = effectObj.GetComponent<SkillEffectRender>();
                    if (effectRender == null)
                    {
                        effectRender = effectObj.AddComponent<SkillEffectRender>();
                    }

                    SkillEffectLogic effectLogic = new SkillEffectLogic(LogicObjectType.Effect, skillData, effectRender, mSkillCreator);
                    effectRender.SetLogicObject(effectLogic, skillData.effectPosType != EffectPosType.Zero);

                    mEffectDic.Add(skillData.GetHashCode(), effectLogic);
                }

                if (mCurLogicFrame == skillData.endFrame && !skillData.isAttachAction)
                {
                    // 技能特效结束，技能特效销毁
                    DestroyEffect(skillData);
                    continue;
                }
                SkillEffectLogic effectLogicObj = null;
                if(mEffectDic.TryGetValue(skillData.GetHashCode(), out effectLogicObj) && effectLogicObj != null)
                {
                    effectLogicObj.OnLogicFrameEffectUpdate(this, mCurLogicFrame);
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
        SkillEffectLogic effect = null;
        int hashCode = skillEffectConfig.GetHashCode();
        mEffectDic.TryGetValue(hashCode, out effect);
        if (effect != null)
        {
            mEffectDic.Remove(hashCode);
            effect.OnDestroy();
        }
    }

}
