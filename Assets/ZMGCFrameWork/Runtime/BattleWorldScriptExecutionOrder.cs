using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZMGC.Battle;

public class BattleWorldScriptExecutionOrder : IBehaviourExecution
{
    private static Type[] _logicBehaviourExecution = new Type[]
    {
        typeof(HeroLogicCtrl),
        typeof(MonsterLogicCtrl),
        typeof(BattleLogicCtrl),
    };
    
    private static Type[] _dataBehaviourExecution = new Type[]
    {

    };
    
    private static Type[] _msgBehaviourExecution = new Type[]
    {

    };

    public Type[] GetLogicBehaviourExecution()
    {
        return _logicBehaviourExecution;
    }
    public Type[] GetDataBehaviourExecution()
    {
        return _dataBehaviourExecution;
    }
    public Type[] GetMsgBehaviourExecution()
    {
        return _msgBehaviourExecution;
    }
}
