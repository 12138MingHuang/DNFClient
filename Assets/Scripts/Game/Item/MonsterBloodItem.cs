using UnityEngine;
using UnityEngine.UI;
using ZMAssetFrameWork;

public class MonsterBloodItem : MonoBehaviour
{
    /// <summary>
    /// 怪物头像
    /// </summary>
    public Image headImage;
    /// <summary>
    /// 怪物类型图标
    /// </summary>
    public Image monsterTypeImage;
    /// <summary>
    /// 血量条
    /// </summary>
    public MultipleBloodBars bloodBars;
    /// <summary>
    /// 怪物名称
    /// </summary>
    public Text nameText;
    
    private MonsterCfg mMonsterCfg;
    private int monsterId;
    
    /// <summary>
    /// 当前显示的怪物实例ID
    /// </summary>
    [HideInInspector]
    public int curShowMonsterInsId;

    /// <summary>
    /// 初始化血条数据，包括血量、怪物头像和类型图标等。
    /// </summary>
    /// <param name="monsterCfg"> 怪物配置 </param>
    /// <param name="curHp"> 当前血量 </param>
    /// <param name="insId"> 怪物实例ID </param>
    public void InitBloodData(MonsterCfg monsterCfg, int curHp, int insId)
    {
        curShowMonsterInsId = insId;
        // 1.使用血量去初始化血条
        bloodBars.InitBlood(curHp);
        // 2.通过配置去加载怪物的头像和怪物的类型图标
        headImage.sprite = ZMAssetsFrame.LoadSprite(AssetPathConfig.GAME_TEXTURES_PATH + "HeadIcon/" + monsterCfg.id);
        monsterTypeImage.sprite = ZMAssetsFrame.LoadPNGAtlasSprite(AssetPathConfig.GAME_TEXTURES_PATH + "BttlePEV/p_UI_Battle_Pve", GetMonsterTypeName(monsterCfg));
        // 3.设置怪物名称
        nameText.text = monsterCfg.name;
    }
    private string GetMonsterTypeName(MonsterCfg monsterCfg)
    {
        switch (monsterCfg.type)
        {
            case (int)MonsterType.Normal:
                return "UI_Battle_Pve_Tubiao_Putong";
            case (int)MonsterType.Elite:
                return "UI_Battle_Pve_Tubiao_Jingying";
            case (int)MonsterType.Boss:
                return "UI_Battle_Pve_Tubiao_Lingzhu";
        }
        
        return "";
    }
    
    /// <summary>
    /// 怪物受到伤害时调用此方法更新血条
    /// </summary>
    /// <param name="damage"> 伤害值 </param>
    public void Damage(int damage)
    {
        bloodBars.ChangeBlood(damage);
    }
}