using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetPathConfig
{
    /// <summary>
    /// 资源加载路径总结点
    /// </summary>
    public const string GAMEDATA = "Assets/GameData/";
    /// <summary>
    /// 游戏内资源路径
    /// </summary>
    public const string GAME = GAMEDATA + "Game/";
    /// <summary>
    /// 游戏内预制体路径
    /// </summary>
    public const string GAME_PREFABS = GAME + "Prefabs/";
    /// <summary>
    /// 游戏内英雄预制体路径
    /// </summary>
    public const string GAME_PREFABS_HERO = GAME_PREFABS + "Hero/";
    /// <summary>
    /// 游戏内怪物预制体路径
    /// </summary>
    public const string GAME_PREFABS_MONSTER = GAME_PREFABS + "Monster/";
    /// <summary>
    /// 游戏内大厅路径
    /// </summary>
    public const string HALL = GAMEDATA + "Hall/";
    /// <summary>
    /// 游戏内技能路径
    /// </summary>
    public const string SKILL_DATA_PATH = GAME + "SkillSystem/";
    
}
