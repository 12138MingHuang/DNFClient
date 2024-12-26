using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "SkillConfig", menuName = "ScriptableObjects/SkillConfig", order = 0)]
public class SkillDataConfig : ScriptableObject
{
    /// <summary>
    /// 角色数据配置
    /// </summary>
    public SkillCharacterConfig character;

    /// <summary>
    /// 技能基础数据配置
    /// </summary>
    public SkillConfig skillConfig;
    
    /// <summary>
    /// 技能特效数据配置列表
    /// </summary>
    public List<SkillEffectConfig> effectCfgList;
    
    /// <summary>
    /// 技能伤害数据配置列表
    /// </summary>
    public List<SkillDamageConfig> damageCfgList;

    /// <summary>
    /// 技能音效数据配置列表
    /// </summary>
    public List<SkillAudioConfig> audioCfgList;
    
    /// <summary>
    /// 技能动作数据配置列表
    /// </summary>
    public List<SkillActionConfig> actionCfgList;

#if UNITY_EDITOR
    
    /// <summary>
    /// 保存技能数据配置
    /// </summary>
    /// <param name="characterConfig"> 角色配置 </param>
    /// <param name="skillConfig"> 技能基础配置 </param>
    /// <param name="effectCfgList"> 技能特效配置列表 </param>
    /// <param name="damageCfgList"> 技能伤害配置列表 </param>
    /// <param name="audioCfgList"> 技能音效配置列表 </param>
    public static void SaveSkillData(SkillCharacterConfig characterConfig, SkillConfig skillConfig,
        List<SkillEffectConfig> effectCfgList, List<SkillDamageConfig> damageCfgList, List<SkillAudioConfig> audioCfgList, List<SkillActionConfig> actionCfgList)
    {
        // 通过代码创建SkillDataConfig的实例，并对字段进行赋值存储
        SkillDataConfig skillDataCfg = CreateInstance<SkillDataConfig>();
        skillDataCfg.character = characterConfig;
        skillDataCfg.skillConfig = skillConfig;
        skillDataCfg.effectCfgList = effectCfgList;
        skillDataCfg.damageCfgList = damageCfgList;
        skillDataCfg.audioCfgList = audioCfgList;
        skillDataCfg.actionCfgList = actionCfgList;
        // 把当前实例存储为.asset文件 ,当作技能配置
        string assetPath = "Assets/GameData/Game/SkillSystem/" + skillConfig.skillId + ".asset";
        // 如果对象已经存在，则删除之前的，然后重新创建
        if (AssetDatabase.LoadAssetAtPath(assetPath, typeof(SkillDataConfig)) != null)
        {
            AssetDatabase.DeleteAsset(assetPath);
        }
        AssetDatabase.CreateAsset(skillDataCfg, assetPath);
    }

    [Button("配置技能" , ButtonSizes.Large), GUIColor("green")]
    public void ShowSkillWindowButtonClick()
    {
        SkillComplierWindow window = SkillComplierWindow.ShowWindow();
        window.LoadSkillData(this);
    }
    
#endif
    
}
