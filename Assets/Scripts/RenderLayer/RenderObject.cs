using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZMAssetFrameWork;

/// <summary>
/// 渲染对象
/// </summary>
public class RenderObject : MonoBehaviour
{
    /// <summary>
    /// 渲染朝向
    /// </summary>
    private Vector2 mRenderForwardDir;
    
    /// <summary>
    /// 逻辑对象
    /// </summary>
    public LogicObject logicObject;

    /// <summary>
    /// 位置平滑插值速度
    /// </summary>
    protected float m_SmoothPosSpeed = 10f;
    
    /// <summary>
    /// 设置逻辑对象
    /// </summary>
    /// <param name="logicObject"> 逻辑对象 </param>
    public void SetLogicObject(LogicObject logicObject)
    {
        this.logicObject = logicObject;
        //初始化位置
        transform.position = logicObject.LogicPos.ToVector3();
        UpdateDir();
    }

    /// <summary>
    /// 渲染层脚本创建
    /// </summary>
    public virtual void OnCreate()
    {
        
    }
    
    /// <summary>
    /// 渲染层脚本释放
    /// </summary>
    public virtual void OnRelease()
    {
        
    }

    /// <summary>
    /// Unity引擎的渲染帧，根据程序进行配置，一般为一秒30帧，60帧，120帧...
    /// </summary>
    protected virtual void Update()
    {
        UpdatePosition();
        UpdateDir();
    }

    /// <summary>
    /// 通用的位置更新逻辑
    /// </summary>
    private void UpdatePosition()
    {
        // 对逻辑对象的位置进行平滑插值，流畅渲染对象移动
        transform.position = Vector3.Lerp(transform.position, logicObject.LogicPos.ToVector3(),
            Time.deltaTime * m_SmoothPosSpeed);
    }

    /// <summary>
    /// 通用的方向更新逻辑
    /// </summary>
    private void UpdateDir()
    {
        transform.rotation = Quaternion.Euler(logicObject.LogicDir.ToVector3());
        mRenderForwardDir.x = logicObject.LogicXAxis >= 0f ? 0f : -20f;
        mRenderForwardDir.y = logicObject.LogicXAxis >= 0f ? 0f : 180f;
        transform.localEulerAngles = mRenderForwardDir;
    }
    
    /// <summary>
    /// 通过动画文件播放动画
    /// </summary>
    /// <param name="clip"> 动画片段 </param>
    public virtual void PlayAnim(AnimationClip clip)
    {
        
    }

    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="damageValue"> 伤害值 </param>
    /// <param name="source"> 来源 </param>
    public virtual void Damage(int damageValue, DamageSource source)
    {
        GameObject damageEffect = ZMAssetsFrame.Instantiate(AssetPathConfig.GAME_PREFABS + "DamageItem/DamageText", null);
        DamageTextItem damageTextItem = damageEffect.GetComponent<DamageTextItem>();
        damageTextItem.ShowDamageText(damageValue, this);
    }
}
