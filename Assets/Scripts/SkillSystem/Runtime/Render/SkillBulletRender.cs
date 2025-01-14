using UnityEngine;

public class SkillBulletRender : RenderObject
{
    /// <summary>
    /// 子弹配置信息
    /// </summary>
    private SkillBulletConfig mBulletConfig;

    public void SetRenderData(LogicObject logicObj, SkillBulletConfig bulletConfig)
    {
        SetLogicObject(logicObj);
        this.mBulletConfig = bulletConfig;
    }

    public override void UpdatePosition()
    {
        base.UpdatePosition();
    }

    public override void UpdateDir()
    {
        transform.rotation = Quaternion.Euler(logicObject.LogicAngle.ToVector3());
    }

    public override void OnRelease()
    {
        base.OnRelease();
        GameObject.Destroy(this.gameObject);
    }
}