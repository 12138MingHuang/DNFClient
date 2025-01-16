using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZMAssetFrameWork;

public class ConfigCenter : Singleton<ConfigCenter>
{
    // 配置字典，存储不同类型的配置数据
    private Dictionary<string, object> mConfigDic = new Dictionary<string, object>();

    /// <summary>
    /// 初始化游戏配置数据，加载所有需要的配置文件。
    /// </summary>
    public void InitGameCfg()
    {
        // 初始化时加载所有配置文件
        LoadConfig<MonsterCfg>("MonsterCfg.json");
        LoadConfig<HeroDataCfg>("HeroDataCfg.json");
        // 可以继续在这里加载其他配置
    }

    #region 配置加载
    /// <summary>
    /// 加载配置文件，并将其反序列化为列表。
    /// </summary>
    /// <param name="fileName"> 配置文件名</param>
    /// <typeparam name="T"> 配置数据类型</typeparam>
    private void LoadConfig<T>(string fileName)
    {
        // 加载配置文件
        TextAsset textAsset = ZMAssetsFrame.LoadTextAsset(AssetPathConfig.GAME_DATA_PATH + fileName);
        if (textAsset == null)
        {
            Debug.LogError($"LoadConfig Failed, textAsset is Null for {fileName}!");
            return;
        }

        // 反序列化Json
        List<T> configList = JsonConvert.DeserializeObject<List<T>>(textAsset.text);
        mConfigDic.Add(fileName.Replace(".json", ""), configList);

        Debug.Log($"LoadConfig Success for {fileName}, Count: {configList.Count}");
    }
    #endregion

    #region 配置获取
    /// <summary>
    /// 通过配置文件名称和ID获取配置数据
    /// </summary>
    public T GetConfigById<T>(int id)
    {
        string fileName = typeof(T).Name;
        
        if (!mConfigDic.ContainsKey(fileName))
        {
            Debug.Log($"Config for {fileName} not loaded! Load it first!");
            LoadConfig<T>(fileName);
            GetConfigById<T>(id);
        }

        var configList = mConfigDic[fileName] as List<T>;
        if (configList != null)
        {
            foreach (var config in configList)
            {
                var field = config.GetType().GetField("id");
                if (field != null && field.GetValue(config).Equals(id))
                {
                    return config;
                }
            }
        }

        return default(T); // 如果没有找到对应的配置数据
    }
    #endregion
}