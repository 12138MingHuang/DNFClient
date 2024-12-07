using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class World
{
    public void AddLogicCtrl(ILogicBehaviour behaviour)
    {
        _logicBehaviourDic.Add(behaviour.GetType().Name, behaviour);
        behaviour.OnCreate();
    }

    public void AddDataMgr(IDataBehaviour behaviour)
    {
        _dataBehaviourDic.Add(behaviour.GetType().Name, behaviour);
        behaviour.OnCreate();
    }

    public void AddMsgMgr(IMsgBehaviour behaviour)
    {
        _msgBehaviourDic.Add(behaviour.GetType().Name, behaviour);
        behaviour.OnCreate();
    }
}
