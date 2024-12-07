using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEventControl
{
    /// <summary>
    /// 委托事件
    /// </summary>
    public delegate void EventHandler(Object data);
    
    /// <summary>
    /// 事件派发注册字典
    /// </summary>
    private static Dictionary<UIEventEnum, List<EventHandler>> _eventDic = new Dictionary<UIEventEnum, List<EventHandler>>();

    public static void AddEvent(UIEventEnum eventType, EventHandler eventHandler)
    {
        if(!_eventDic.ContainsKey(eventType))
        {
            _eventDic.Add(eventType, new List<EventHandler>());
        }
        if(!_eventDic[eventType].Contains(eventHandler))
        {
            _eventDic[eventType].Add(eventHandler);
        }
    }

    /// <summary>
    /// 移除事件
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="eventHandler"></param>
    public static void RemoveEvent(UIEventEnum eventType, EventHandler eventHandler)
    {
        if(_eventDic.ContainsKey(eventType))
        {
            if(_eventDic[eventType].Contains(eventHandler))
            {
                _eventDic[eventType].Remove(eventHandler);
            }
        }
    }
    
    /// <summary>
    /// 分发事件
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    public static void DispensEvent(UIEventEnum eventType, Object data = null)
    {
        List<EventHandler> eventList = null;
        if(_eventDic.ContainsKey(eventType))
        {
            eventList = _eventDic[eventType];
        }
        for (int i = 0; i < eventList.Count; i++)
        {
            eventList[i]?.Invoke(data);
        }
    }
}