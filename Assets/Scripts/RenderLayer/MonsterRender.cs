using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRender : RenderObject
{
    private Animation mAnim;
        
    public override void OnCreate()
    {
        base.OnCreate();
        mAnim = GetComponentInChildren<Animation>();
    }

    public override void OnRelease()
    {
        base.OnRelease();
    }

    private void Start()
    {
        
    }

    public override void PlayAnim(string animName)
    {
        base.PlayAnim(animName);
        if(mAnim == null)
            return;
        
        if(logicObject.ObjectState == LogicObjectState.Death && !string.Equals(animName, AnimationName.Anim_Dead)) return;
        
        mAnim.Play(animName);
    }

    protected override void Update()
    {
        base.Update();
    }

    private void OnDestroy()
    {
        
    }
}
