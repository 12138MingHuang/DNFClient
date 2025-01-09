using FixMath;
using System.Collections.Generic;
using UnityEngine;

public partial class Skill
{
    /// <summary>
    /// 当前所有子弹的累加时间列表
    /// </summary>
    private List<int> mCurCreateBulletAccTimeList = new List<int>();
    /// <summary>
    /// 随机种子随机数生成器
    /// </summary>
    private LogicRandom mLogicRandom;

    /// <summary>
    /// 初始化子弹数据
    /// </summary>
    public void OnBulletInit()
    {
        mLogicRandom = new LogicRandom(10);
        if(mSkillDataConfig.bulletCfgList != null && mSkillDataConfig.bulletCfgList.Count > 0)
        {
            for (int i = 0; i < mSkillDataConfig.bulletCfgList.Count; i++)
            {
                mCurCreateBulletAccTimeList.Add(0);
            }
        }
    }

    public void OnLogicFrameUpdateBullet()
    {
        if (mSkillDataConfig.bulletCfgList != null && mSkillDataConfig.bulletCfgList.Count > 0)
        {
            for (int i = 0; i < mSkillDataConfig.bulletCfgList.Count; i++)
            {
                mCurCreateBulletAccTimeList[i] += LogicFrameConfig.LogicFrameIntervalMS;
                SkillBulletConfig bulletCfg = mSkillDataConfig.bulletCfgList[i];
                if (bulletCfg.triggerFrame == mCurLogicFrame)
                {
                    // 创建子弹
                    CreateBullet(bulletCfg);
                }
                
                // 判断子弹是否循环创建
                if (bulletCfg.isLoopCreate)
                {
                    // 增强代码优化，如果子弹的间隔为0，则不进行循环创建子弹操作
                    if (bulletCfg.loopIntervalMS == 0)
                    {
                        Debug.LogError("bulletCfg.loopIntervalMs == 0,不进行子弹循环创建");
                        continue;
                    }
                    
                    // 当前创建的子弹累加时间超过了子弹的间隔，就创建子弹
                    while (mCurCreateBulletAccTimeList[i] >= bulletCfg.loopIntervalMS)
                    {
                        CreateBullet(bulletCfg);
                        mCurCreateBulletAccTimeList[i] -= bulletCfg.loopIntervalMS;
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// 创建子弹
    /// </summary>
    /// <param name="bulletCfg"> 子弹配置 </param>
    private void CreateBullet(SkillBulletConfig bulletCfg)
    {
        GameObject bulletObj = GameObject.Instantiate(bulletCfg.bulletPrefab);
        // 处理渲染层
        SkillBulletRender bulletRender = bulletObj.GetComponent<SkillBulletRender>();
        if (bulletRender == null)
        {
            bulletRender = bulletObj.AddComponent<SkillBulletRender>();
        }
        
        FixIntVector3 rangePos = FixIntVector3.zero;
        if (bulletCfg.isLoopCreate)
        {
            // 随机XYZ轴坐标
            FixInt x = mLogicRandom.Range(bulletCfg.minRandomRangeVect3.x, bulletCfg.maxRandomRangeVect3.x);
            FixInt y = mLogicRandom.Range(bulletCfg.minRandomRangeVect3.y, bulletCfg.maxRandomRangeVect3.y);
            FixInt z = mLogicRandom.Range(bulletCfg.minRandomRangeVect3.z, bulletCfg.maxRandomRangeVect3.z);
            rangePos = new FixIntVector3(x, y, z);
        }
        
        // 处理逻辑层
        SKillBulletLogic bulletLogic = new SKillBulletLogic(this, mSkillCreator, bulletRender, bulletCfg, rangePos);
        bulletRender.SetRenderData(bulletLogic, bulletCfg);
        mSkillCreator.AddBullet(bulletLogic);
    }
    
    public void OnBulletRelease()
    {
        mCurCreateBulletAccTimeList.Clear();
        mLogicRandom = null;
    }
}