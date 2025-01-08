using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SkillGuideType
{
    None,
    /// <summary>
    /// 点击
    /// </summary>
    Click,
    /// <summary>
    /// 长按蓄力
    /// </summary>
    LongPress,
    /// <summary>
    /// 位置选择
    /// </summary>
    Position,
    /// <summary>
    /// 方向选择
    /// </summary>
    Direction,
}

/// <summary>
/// 技能释放回调
/// </summary>
/// <param name="skillGuide">技能引导</param>
/// <param name="skillPos">技能释放位置</param>
/// <param name="skillId">技能id</param>
public delegate void OnReleaseSkillCallBack(SkillGuideType skillGuide, Vector3 skillPos, int skillId);

/// <summary>
/// 技能引导位置回调
/// </summary>
/// <param name="skillGuide">技能引导类型</param>
/// <param name="isCancel">释放取消</param>
/// <param name="skillPos">技能位置</param>
/// <param name="skillId">技能id</param>
/// <param name="skillDirDis">技能方向距离</param>
public delegate void OnSkillGuideCallBack(SkillGuideType skillGuide, bool isCancel, Vector3 skillPos, int skillId, float skillDirDis);

/// <summary>
/// 技能点击或抬起的回调
/// </summary>
/// <param name="isPressDown"></param>
public delegate void OnClickOrPointUpSkill(bool isPressDown);

/// <summary>
/// 技能摇杆按钮
/// </summary>
public class SKillItemJoyStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{

    #region 拖拽赋值

    [Header("摇杆摇点")]
    public Transform JoyPointTrans;
    [Header("摇杆背景")]
    public Transform JoyBgTrans;
    [Header("技能取消图标")]
    public Transform CancelSkillTrans;
    [Header("摇杆最大移动距离(编译器指定)")]
    public int JoyPointMaxDir;
    
    #endregion

    #region 代码初始化

    private float mSkillCancelRadius = 1000f; // 技能取消最大向量(配置)
    private SkillGuideType mSkillGuideType; // 技能引导方式
    private float mSkillDirRange; // 技能范围
    private int mSkillId; // 技能id
    private Vector2 mCenterPos; // 摇杆中心位置
    private RectTransform mRectTrans; // 摇杆Rect
    
    #endregion

    #region 回调
    
    /// <summary>
    /// 释放技能回调
    /// </summary>
    public OnReleaseSkillCallBack OnReleaseSkill;
    /// <summary>
    /// 技能引导回调 拖拽中持续调用
    /// </summary>
    public OnSkillGuideCallBack OnSkillGuide;
    /// <summary>
    /// 技能点击和抬起回调
    /// </summary>
    public OnClickOrPointUpSkill OnClickAndPointerUpSkill;
    
    #endregion

    public void InitSkillData(SkillGuideType skillGuideType, int skillId, float skillRadius)
    {
        mSkillId = skillId;
        mSkillGuideType = skillGuideType;
        mSkillDirRange = skillRadius;
        SetJoyActiveState(false);
        mSkillCancelRadius = Screen.height / 1.0f * 1080 * mSkillCancelRadius;
        mCenterPos = transform.localPosition;
        mRectTrans = transform as RectTransform;
    }
    
    /// <summary>
    /// 手指按下
    /// </summary>
    /// <param name="eventData"> 事件数据 </param>
    public void OnPointerDown(PointerEventData eventData)
    {
        SetJoyActiveState(mSkillGuideType != SkillGuideType.Click &&mSkillGuideType !=SkillGuideType.LongPress);
        OnSkillGuide?.Invoke(mSkillGuideType, true, Vector3.zero, mSkillId, mSkillDirRange);
        OnClickAndPointerUpSkill?.Invoke(true);
    }
    /// <summary>
    /// 手指拖拽
    /// </summary>
    /// <param name="eventData">  事件数据 </param>
    public void OnDrag(PointerEventData eventData)
    {
        eventData.position = MousePosToUGUIPosition(eventData.position);
        Vector2 dir = eventData.position - mCenterPos; // 鼠标相对于摇杆中心的向量
        float dirLen = dir.magnitude; // 向量长度
        // 判断是否超出摇杆最大移动距离
        if (dirLen > JoyPointMaxDir)
        {
            Vector2 clampDir = Vector2.ClampMagnitude(dir, JoyPointMaxDir); // 限制摇杆最大移动距离
            JoyPointTrans.localPosition = mCenterPos + clampDir;
        }
        else
        {
            JoyPointTrans.localPosition = eventData.position;
        }

        if (mSkillGuideType == SkillGuideType.Position) // 位置型技能
        {
            if(dir == Vector2.zero) return;
            OnSkillGuide?.Invoke(mSkillGuideType, true, GetSkillPosition(dir), mSkillId, mSkillDirRange);
        }
        else if (mSkillGuideType == SkillGuideType.Direction) // 方向型技能
        {
            OnSkillGuide?.Invoke(mSkillGuideType, true, GetSkillDirection(dir), mSkillId, mSkillDirRange);
        }
        //显示取消 或 关闭
        SetCancelIconActiveState(dirLen > mSkillCancelRadius);
    }
    /// <summary>
    /// 手指抬起事件
    /// </summary>
    /// <param name="eventData"> 事件数据 </param>
    public void OnPointerUp(PointerEventData eventData)
    {
        eventData.position = MousePosToUGUIPosition(eventData.position);
        SetJoyActiveState(false);
        OnSkillGuide?.Invoke(mSkillGuideType, false, Vector3.zero, mSkillId, mSkillDirRange);

        if (mSkillGuideType == SkillGuideType.Click || mSkillGuideType == SkillGuideType.LongPress)
        {
            OnClickAndPointerUpSkill?.Invoke(false); // 按钮抬起
            OnReleaseSkill?.Invoke(mSkillGuideType, Vector3.zero, mSkillId);
            return;
        }
        
        SetCancelIconActiveState(false);
        JoyPointTrans.position = transform.position; // 恢复摇杆中心位置
        Vector2 dir = eventData.position - mCenterPos; // 鼠标相对于摇杆中心的向量
        // 抬起时 移动向量的长度大于施法向量则技能取消
        if(dir.magnitude > mSkillCancelRadius)
        {
            Debug.Log("取消技能释放");
            return;
        }
        
        // 释放技能
        switch (mSkillGuideType)
        {
            case SkillGuideType.Position:
                if (dir == Vector2.zero) return;
                OnReleaseSkill?.Invoke(mSkillGuideType, GetSkillPosition(dir), mSkillId);
                break;
            case SkillGuideType.Direction:
                if (dir == Vector2.zero) return;
                OnReleaseSkill?.Invoke(mSkillGuideType, GetSkillDirection(dir), mSkillId);
                break;
            default:
                Debug.LogError("Skill Type is None !");
                break;
        }
        // 抬起时 释放技能
        OnClickAndPointerUpSkill?.Invoke(false); // 按钮抬起
    }

    /// <summary>
    /// 获取技能位置
    /// </summary>
    /// <param name="dir"> 鼠标相对于摇杆中心的向量 </param>
    /// <returns> 返回技能位置 </returns>
    private Vector3 GetSkillPosition(Vector2 dir)
    {
        dir = dir * 0.025f; // 0.025f代表本地坐标与世界坐标的缩放插值
        Vector2 clampDir = Vector2.ClampMagnitude(dir, mSkillDirRange); // 限制施法范围
        Vector3 skillPos = new Vector3(clampDir.x, 0, clampDir.y); // 把平面向量转为3D坐标
        skillPos = Quaternion.Euler(20, 0, 0) * skillPos; // 处理相机的旋转偏移值
        return skillPos;
    }

    /// <summary>
    /// 获取技能方向向量
    /// </summary>
    /// <param name="dir"> 鼠标相对于摇杆中心的向量 </param>
    /// <returns> 返回技能方向向量 </returns>
    private Vector3 GetSkillDirection(Vector2 dir)
    {
        Vector3 direction = new Vector3(dir.x, 0, dir.y);
        direction = Quaternion.Euler(20, 0, 0) * direction; // 处理相机的旋转偏移值
        return direction;
    }

    /// <summary>
    /// 设置摇杆可见性
    /// </summary>
    /// <param name="isActive"> 是否可见 </param>
    private void SetJoyActiveState(bool isActive)
    {
        JoyPointTrans.gameObject.SetActive(isActive);
        JoyBgTrans.gameObject.SetActive(isActive);
    }
    
    /// <summary>
    /// 设置取消技能图标可见性
    /// </summary>
    /// <param name="isActive"> 是否可见 </param>
    private void SetCancelIconActiveState(bool isActive)
    {
        CancelSkillTrans.gameObject.SetActive(isActive);
    }

    /// <summary>
    /// 获取鼠标相对于当前对象的本地坐标
    /// </summary>
    /// <returns> 返回本地坐标 </returns>
    private Vector2 MousePosToUGUIPosition(Vector3 mousePosition)
    {
        // 获取鼠标屏幕坐标
        Vector2 convertToMousePosition;
        // 转换为Canvas针对物体的局部坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(mRectTrans, mousePosition, UIModule.Instance.UICamera, out convertToMousePosition);
        
        return convertToMousePosition;
    }
}
