using System.Collections.Generic;

public class LogicActionController : Singleton<LogicActionController>
{
    /// <summary>
    /// 逻辑动作列表
    /// </summary>
    private List<ActionBehaviour> mActionList = new List<ActionBehaviour>();

    /// <summary>
    /// 执行逻辑动作
    /// </summary>
    /// <param name="action">逻辑动作</param>
    public void RunAction(ActionBehaviour action)
    {
        action.actionFinish = false;
        mActionList.Add(action);
    }

    /// <summary>
    /// 逻辑帧更新动作
    /// </summary>
    public void OnLogicFrameUpdate()
    {
        // 移除已经完成的动作
        for (int i = mActionList.Count - 1; i >= 0; i--)
        {
            ActionBehaviour action = mActionList[i];
            if (action.actionFinish)
            {
                action.OnActionFinish();
                RemoveAction(action);
            }
        }

        // 执行逻辑动作更新
        foreach (ActionBehaviour action in mActionList)
        {
            action.OnLogicFrameUpdate();
        }
    }

    /// <summary>
    /// 移除对应的逻辑动作
    /// </summary>
    /// <param name="action"> 逻辑动作</param>
    public void RemoveAction(ActionBehaviour action)
    {
        mActionList.Remove(action);
    }
    
    /// <summary>
    /// 销毁逻辑动作控制器时，清除所有逻辑动作
    /// </summary>
    public void OnDestroy()
    {
        mActionList.Clear();
    }
}