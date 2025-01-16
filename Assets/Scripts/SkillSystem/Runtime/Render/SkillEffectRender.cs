using UnityEngine;
using ZMAssetFrameWork;

public class SkillEffectRender : RenderObject
{
    protected override void Update()
    {
        base.Update();
    }

    public override void OnRelease()
    {
        base.OnRelease();
        ZMAssetsFrame.Release(gameObject);
    }
}