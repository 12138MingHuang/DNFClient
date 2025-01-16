using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SkillEffectPathEditor
{
    public static string[] skillDataCfgPathArr = new string[]
    {
        "Assets/GameData/Game/SkillSystem/SkillData/",
        "Assets/GameData/Game/SkillSystem/BuffData/"
    };

    [MenuItem("Tools/SyncCfgPrefabPath")]
    public static void SyncCfgPrefabPath()
    {
        for (int i = 0; i < skillDataCfgPathArr.Length; i++)
        {
            string path = skillDataCfgPathArr[i];
            string[] filePathArr = Directory.GetFiles(path, "*.asset");
            for (int j = 0; j < filePathArr.Length; j++)
            {
                if(i == 0) // 技能数据路径处理逻辑
                {
                    SkillDataConfig skillData = AssetDatabase.LoadAssetAtPath<SkillDataConfig>(filePathArr[j]);
                    skillData.skillConfig.GetObjectPath(skillData.skillConfig.skillHitEffect);

                    foreach (SkillEffectConfig effectCfg in skillData.effectCfgList)
                    {
                        effectCfg.GetObjectPath(effectCfg.skillEffect);
                    }
                    
                    foreach (SkillBulletConfig bulletCfg in skillData.bulletCfgList)
                    {
                        bulletCfg.GetBulletObjectPath(bulletCfg.bulletPrefab);
                        bulletCfg.GetHitEffectObjectPath(bulletCfg.hitEffect);
                    }
                    
                    skillData.SaveAssets();
                }
                else // Buff数据路径处理逻辑
                {
                    BuffConfig buffCfg = AssetDatabase.LoadAssetAtPath<BuffConfig>(filePathArr[j]);
                    buffCfg.GetObjectPath(buffCfg.buffHitEffectObj);
                    
                    if(buffCfg.effectConfig !=null)
                        buffCfg.effectConfig.GetObjectPath(buffCfg.effectConfig.effect);
                    
                    buffCfg.SaveAssets();
                }
            }
            
            AssetDatabase.SaveAssets();
        }
    }
}
