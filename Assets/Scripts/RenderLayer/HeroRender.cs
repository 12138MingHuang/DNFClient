using System;
using System.Collections;
using System.Collections.Generic;
using FixMath;
using UnityEngine;

public class HeroRender : RenderObject
{
    /// <summary>
    /// 摇杆移动输入
    /// </summary>
    private Vector3 mInputMoveDir;
    /// <summary>
    /// 英雄逻辑层
    /// </summary>
    private HeroLogic mHeroLogic;
    
    /// <summary>
    /// 角色动画
    /// </summary>
    private Animation mAnim;
    
    public override void OnCreate()
    {
        base.OnCreate();
        mAnim = GetComponent<Animation>();
        mHeroLogic = logicObject as HeroLogic;
        JoystickUGUI.OnMoveCallBack += OnJoystickMove;
    }
    

    public override void OnRelease()
    {
        base.OnRelease();
        JoystickUGUI.OnMoveCallBack -= OnJoystickMove;
    }
    
    private void Start()
    {
        
    }

    protected override void Update()
    {
        base.Update();
        // 判断摇杆是否有值输入，如果没有就待机，如果有就播放移动动画
        if (mInputMoveDir.x == 0 && mInputMoveDir.z == 0)
        {
            PlayAnim("Anim_Idle02");
        }
        else
        {
            PlayAnim("Anim_Run");
        }
    }

    private void OnDestroy()
    {
        
    }
    
    /// <summary>
    /// 摇杆移动回调
    /// </summary>
    /// <param name="inputDir"> 输入方向值 </param>
    private void OnJoystickMove(Vector3 inputDir)
    {
        mInputMoveDir = inputDir;
        // 逻辑方向
        FixIntVector3 logicDir = FixIntVector3.zero;

        if (inputDir != Vector3.zero)
        {
            logicDir.x = inputDir.x;
            logicDir.y = inputDir.y;
            logicDir.z = inputDir.z;
        }
        // TODO 向英雄逻辑层直接输入操作帧事件 没有服务端情况下的测试代码
        mHeroLogic.InPutLogicFrameEvent(logicDir);
    }
    
    /// <summary>
    /// 播放角色动画
    /// </summary>
    /// <param name="animName"></param>
    public void PlayAnim(string animName)
    {
        mAnim.CrossFade(animName, 0.2f);
    }
}
