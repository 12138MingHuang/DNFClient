using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class JoystickUGUI : MonoBehaviour
{
    
#if UNITY_EDITOR

    /// <summary>
    /// 在scene视图中绘制线 方便查看可点击区域
    /// </summary>
    private void OnDrawGizmos()
    {
        if(!Application.isPlaying)
        {
            return;
        }
        Gizmos.color = Color.red;
        
        //本地坐标
        Vector3 max = new Vector3(transform.localPosition.x + showRangeMax.x + 119f,
            transform.localPosition.y + showRangeMax.y + 119f, 0f);
        Vector3 min = new Vector3(transform.localPosition.x + showRangeMin.x - 119f,
            transform.localPosition.y + showRangeMin.y - 119f, 0f);
        
        //世界坐标
        max = transform.parent.TransformPoint(max);
        min = transform.parent.TransformPoint(min);

        Gizmos.DrawLine(new Vector3(min.x, min.y, 0f), new Vector3(max.x, min.y, 0f));
        Gizmos.DrawLine(new Vector3(max.x, min.y, 0f), new Vector3(max.x, max.y, 0f));
        Gizmos.DrawLine(new Vector3(max.x, max.y, 0f), new Vector3(min.x, max.y, 0f));
        Gizmos.DrawLine(new Vector3(min.x, max.y, 0f), new Vector3(min.x, min.y, 0f));
    }
    
#endif

    public enum JoystickState
    {
        /// <summary>
        /// 闲置
        /// </summary>
        Idle,
        
        /// <summary>
        /// 抬起
        /// </summary>
        TouchUp,
        
        /// <summary>
        /// 按下
        /// </summary>
        TouchDown,
        
        /// <summary>
        /// 准备
        /// </summary>
        Ready,
        
        /// <summary>
        /// 拖拽
        /// </summary>
        Drag,
    }

    [Header("Canvas")]
    public Canvas canvas;
    [Header("摇杆触发点击区域")]
    public Transform triggerAreaTrans;
    [Header("摇杆总节点")]
    public Transform joystickRootTrans;
    [Header("摇杆背景")]
    public Transform backgroundTrans;
    [Header("中心")]
    public Transform stickTrans;
    [Header("摇杆方向箭头")]
    public Transform directionTrans;

    private Vector3 m_JoystickDirection;
    /// <summary>
    /// 摇杆方向
    /// </summary>
    public Vector3 JoystickDirection => m_JoystickDirection;

    /// <summary>
    /// 摇杆抬起位置
    /// </summary>
    [Header("摇杆抬起位置(初始化位置)")]
    public Vector3 joystickInitPosition = new Vector3(165f, 165f, 0f);
    
    /// <summary>
    /// 点击触发范围
    /// </summary>
    [Header("点击触发范围")]
    public Vector2 triggeredRange = new Vector2(500f, 400f);
    
    /// <summary>
    /// 摇杆显示最小坐标值(相对于左下角)
    /// </summary>
    [Header("摇杆左边显示宽高")]
    public Vector2 showRangeMin = new Vector2(145f, 145f);
    
    /// <summary>
    /// 摇杆显示最大坐标值(相对于左下角)
    /// </summary>
    [Header("摇杆右边显示宽高")]
    public Vector2 showRangeMax = new Vector2(350f, 200f);

    /// <summary>
    /// 默认触发位置
    /// </summary>
    private Vector3 m_DefaultTriggerPos;
    
    /// <summary>
    /// 摇杆状态
    /// </summary>
    public JoystickState joystickState = JoystickState.Idle;

    /// <summary>
    /// 按下状态位置
    /// </summary>
    private Vector3 m_TouchPosition;

    /// <summary>
    /// 切换到拖动状态最小距离差
    /// </summary>
    [Header("摇杆球最小拖动距离")]
    public float stickMoveMin = 20f;
    
    /// <summary>
    /// 杆拖动最大值
    /// </summary>
    [Header("摇杆球最大拖动距离")]
    public float stickMoveMax = 73f;

    /// <summary>
    /// 显示指向 鼠标距离中心最小距离
    /// </summary>
    [Header("鼠标距离中心最小距离(显示指向)")]
    public float showDirection = 20f;

    /// <summary>
    /// 摇杆移动回调
    /// </summary>
    public static Action<Vector3> OnMoveCallBack;
    
    private void Start()
    {
        m_DefaultTriggerPos = triggerAreaTrans.transform.localPosition;
        
        UIEventListener uiEventListener = transform.GetComponent<UIEventListener>();
        
        Debug.Log("绑定委托");
        if (uiEventListener != null)
        {
            uiEventListener.OnDrag += OnDrag;
            uiEventListener.OnPress += OnPress;
            uiEventListener.OnUp += OnUp;
        }


        InitState();
    }

    /// <summary>
    /// 拖拽时触发
    /// </summary>
    /// <param name="eventData"> 事件信息 </param>
    private void OnDrag(PointerEventData eventData)
    {
        SwitchJoystickState(JoystickState.Drag);
    }

    /// <summary>
    /// 按下触发
    /// </summary>
    /// <param name="eventData"> 事件信息 </param>
    private void OnPress(PointerEventData eventData)
    {
        SwitchJoystickState(JoystickState.TouchDown);
    }

    /// <summary>
    /// 抬起触发
    /// </summary>
    /// <param name="eventData"> 事件信息 </param>
    private void OnUp(PointerEventData eventData)
    {
        SwitchJoystickState(JoystickState.TouchUp);
    }

    /// <summary>
    /// 切换摇杆状态
    /// </summary>
    /// <param name="state"> 状态 </param>
    public void SwitchJoystickState(JoystickState state)
    {
        joystickState = state;
        
        Action();
    }

    /// <summary>
    /// 动作
    /// </summary>
    private void Action()
    {
        Debug.Log("Action JoystickState:"+ joystickState);

        if (joystickState == JoystickState.Idle)
        {
            return;
        }
        
        switch (joystickState)
        {
            case JoystickState.TouchUp:

                InitState();

                SwitchJoystickState(JoystickState.Idle);

                break;
            case JoystickState.TouchDown:

                TouchState();

                SwitchJoystickState(JoystickState.Ready);

                break;
            case JoystickState.Ready:

                ReadyState();

                break;
            case JoystickState.Drag:

                DragState();

                break;
        }
    }

    /// <summary>
    /// 抬起动作
    /// </summary>
    private void InitState()
    {
        joystickRootTrans.localPosition = joystickInitPosition;
        stickTrans.localPosition = Vector3.zero;
        directionTrans?.gameObject.SetActive(false);
        // 设置虚拟摇杆 抬起触发区域
        triggerAreaTrans.transform.localPosition = m_DefaultTriggerPos;
        OnMoveCallBack?.Invoke(Vector3.zero);
    }

    /// <summary>
    /// 按下动作
    /// </summary>
    private void TouchState()
    {
        m_TouchPosition = GetMouseLocalPosition(transform);

        Vector3 position = m_TouchPosition;
        // 如果超出显示范围则取临界值
        position.x = Math.Min(showRangeMax.x, Math.Max(position.x, showRangeMin.x));
        position.y = Math.Min(showRangeMax.y, Math.Max(position.y, showRangeMin.y));
        joystickRootTrans.localPosition = position;
        // 设置虚拟摇杆 按下 触发区域
        triggerAreaTrans.transform.localPosition = m_DefaultTriggerPos;
    }

    /// <summary>
    /// 准备动作
    /// </summary>
    private void ReadyState()
    {
        Vector3 position = GetMouseLocalPosition(transform);
        
        float distance = Vector3.Distance(position, m_TouchPosition);
        
        // 点击屏幕拖动大于切换拖动状态最小距离 则切换到拖动状态
        if (distance > stickMoveMin)
        {
            SwitchJoystickState(JoystickState.Drag);
        }
        // 设置虚拟摇杆 准备 触发区域
        triggerAreaTrans.transform.localPosition = m_DefaultTriggerPos;
    }

    /// <summary>
    /// 拖动动作
    /// </summary>
    private void DragState()
    {
        Vector3 mouseLocalPosition = GetMouseLocalPosition(joystickRootTrans);
        
        // 鼠标与摇杆的距离
        float distance = Vector3.Distance(mouseLocalPosition, backgroundTrans.localPosition);
        
        // 设置摇杆的位置
        Vector3 stickLocalPosition = mouseLocalPosition;
        
        // 鼠标位置大于杠拖动的最大位置
        if (distance > stickMoveMax)
        {
            float proportion = stickMoveMax / distance;
            stickLocalPosition = (mouseLocalPosition - backgroundTrans.localPosition) * proportion;
        }
        
        stickTrans.localPosition = stickLocalPosition;
        
        // 摇杆与鼠标的距离大于指向显示的最小距离则显示指向
        if (distance > showDirection)
        {
            directionTrans?.gameObject.SetActive(true);

            // 获取鼠标位置与摇杆角度
            Double angle = Math.Atan2(mouseLocalPosition.y - backgroundTrans.localPosition.y,
                mouseLocalPosition.x - backgroundTrans.localPosition.x) * 180 / Math.PI;
            if (directionTrans != null)
            {
                directionTrans.eulerAngles = new Vector3(0, 0, (float)angle);
            }

            // 设置摇杆指向
            m_JoystickDirection = mouseLocalPosition - backgroundTrans.localPosition;
            m_JoystickDirection.z = 0;
        }
        else
        {
            directionTrans?.gameObject.SetActive(false);
        }
        // 设置虚拟摇杆 拖动 触发区域
        triggerAreaTrans.transform.localPosition = m_DefaultTriggerPos;
        Vector3 dir = m_JoystickDirection.normalized;
        OnMoveCallBack.Invoke(new Vector3(dir.x, 0, dir.y));
    }

    private Vector2 GetMouseLocalPosition(Transform transform)
    {
        // 获取鼠标屏幕位置
        Vector2 mousePosition;
        // 转换为Canvas针对物体的局部坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.GetComponent<RectTransform>(),
            Input.mousePosition, canvas.worldCamera, out mousePosition);
        
        return mousePosition;
    }
}