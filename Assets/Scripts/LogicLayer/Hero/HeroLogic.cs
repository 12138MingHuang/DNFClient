using FixMath;
using UnityEngine;

public class HeroLogic : LogicActor
{
    /// <summary>
    /// 英雄ID
    /// </summary>
    public int HeroId { get; private set; }

    public HeroLogic(int heroId, RenderObject renderObject)
    {
        this.HeroId = heroId;
        this.RenderObject = renderObject;
        ObjectType = LogicObjectType.Hero;
    }

    public override void OnCreate()
    {
        base.OnCreate();
        InitActorSkill(HeroId);
        InitHeroAttribute();
    }
    /// <summary>
    /// 初始化英雄属性
    /// </summary>
    private void InitHeroAttribute()
    {
        HeroDataCfg dataCfg = ConfigCenter.Instance.GetConfigById<HeroDataCfg>(HeroId);
        if (dataCfg == null)
        {
            Debug.LogError($"英雄数据配置不存在，ID:{HeroId}");
            return;
        }

        hp = dataCfg.hp;
        mp = dataCfg.mp;
        ap = dataCfg.ap;
        ad = dataCfg.ad;
        adDef = dataCfg.adDef;
        apDef = dataCfg.apDef;
        pct = dataCfg.pct;
        mct = dataCfg.mct;
        adPctRate = dataCfg.adPctRate;
        apMctRate = dataCfg.apMctRate;
        str = dataCfg.str;
        sta = dataCfg.sta;
        Int = dataCfg.Int;
        spi = dataCfg.spi;
        agl = dataCfg.agl;
        
        Debug.Log($"初始化英雄属性成功，ID:{HeroId}");
    }
}