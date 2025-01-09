using System.Collections.Generic;

public partial class LogicActor
{
    /// <summary>
    /// 子弹逻辑集合
    /// </summary>
    private List<SKillBulletLogic> mBulletLogicList = new List<SKillBulletLogic>();

    public void OnLogicFrameUpdateBullet()
    {
        for (int i = mBulletLogicList.Count - 1; i >= 0; i--)
        {
            if (mBulletLogicList[i].isFailure)
            {
                RemoveBullet(mBulletLogicList[i]);
            }
        }

        foreach (var bulletLogic in mBulletLogicList)
        {
            bulletLogic.OnLogicFrameUpdate();
        }
    }

    /// <summary>
    /// 添加子弹逻辑
    /// </summary>
    /// <param name="bulletLogic"> 子弹逻辑</param>
    public void AddBullet(SKillBulletLogic bulletLogic)
    {
        mBulletLogicList.Add(bulletLogic);
    }

    /// <summary>
    /// 移除子弹逻辑
    /// </summary>
    /// <param name="bulletLogic"> 子弹逻辑</param>
    public void RemoveBullet(SKillBulletLogic bulletLogic)
    {
        mBulletLogicList.Remove(bulletLogic);
    }
}
