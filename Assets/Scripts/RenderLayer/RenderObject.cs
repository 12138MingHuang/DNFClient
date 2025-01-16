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
    /// 是否更新位置和旋转
    /// </summary>
    protected bool mIsUpdatePosAndRot = true;
    
    /// <summary>
    /// 设置逻辑对象
    /// </summary>
    /// <param name="logicObject"> 逻辑对象 </param>
    public void SetLogicObject(LogicObject logicObject, bool isUpdatePosAndRot = true)
    {
        this.logicObject = logicObject;
        mIsUpdatePosAndRot = isUpdatePosAndRot;
        //初始化位置
        transform.position = logicObject.LogicPos.ToVector3();
        if(!isUpdatePosAndRot) transform.localPosition = Vector3.zero;
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
    public virtual void UpdatePosition()
    {
        if(!mIsUpdatePosAndRot) return;
        
        // 对逻辑对象的位置进行平滑插值，流畅渲染对象移动
        transform.position = Vector3.Lerp(transform.position, logicObject.LogicPos.ToVector3(),
            Time.deltaTime * m_SmoothPosSpeed);
    }

    /// <summary>
    /// 通用的方向更新逻辑
    /// </summary>
    public virtual void UpdateDir()
    {
        if(!mIsUpdatePosAndRot) return;
        
        transform.rotation = Quaternion.Euler(logicObject.LogicDir.ToVector3());
        // mRenderForwardDir.x = logicObject.LogicXAxis >= 0f ? 0f : -20f;
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
    /// 播放动画
    /// </summary>
    /// <param name="animName"> 动画名称 </param>
    public virtual void PlayAnim(string animName)
    {
        
    }

    /// <summary>
    /// 获取当前播放的动画名称
    /// </summary>
    /// <returns> 动画名称 </returns>
    public virtual string GetCurAnimName()
    {
        return "";
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
    
    /// <summary>
    /// 技能命中受击
    /// </summary>
    /// <param name="hitEffectPath"> 技能命中特效路径 </param>
    /// <param name="hitEffectSurvivalTimeMs"> 特效存活时间 </param>
    /// <param name="source"> 技能创建者 </param>
    public virtual void OnHit(string hitEffectPath, int hitEffectSurvivalTimeMs, LogicObject source)
    {
        if (!string.IsNullOrEmpty(hitEffectPath))
        {
            GameObject hitEffectObj = ZMAssetsFrame.Instantiate(hitEffectPath, null);
            hitEffectObj.transform.position = transform.position;
            hitEffectObj.transform.localScale = source.LogicXAxis > 0 ? Vector3.one : new Vector3(-1f, 1f, 1f);
            Destroy(hitEffectObj, hitEffectSurvivalTimeMs * 1.0f / 1000f);
        }
    }
    
    /// <summary>
    /// 死亡逻辑
    /// </summary>
    public virtual void OnDeath()
    {
        
    }

    /// <summary>
    /// 获取父节点
    /// </summary>
    /// <param name="parentType"> 父节点类型 </param>
    /// <returns> 父节点 </returns>
    public virtual Transform GetTransParent(TransParentType parentType)
    {
        return null;
    }

    /// <summary>
    /// 显示技能立绘
    /// </summary>
    /// <param name="portraitObj"> 立绘对象 </param>
    public virtual void ShowSkillPortrait(GameObject portraitObj)
    {
        GameObject portraitGo = GameObject.Instantiate(portraitObj);
        GameObject.Destroy(portraitGo, 3f);
    }
}
