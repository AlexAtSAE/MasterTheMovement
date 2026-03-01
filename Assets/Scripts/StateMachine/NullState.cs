using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullState : StateMachineNode
{
    public string Name { get { return "NullState"; } }
    public void ConditionUpdate(object invoker)
    {
        if (!(invoker is PlayerMovementScript)) return;
        PlayerMovementScript pms = (PlayerMovementScript)invoker;
        ExitState(pms, this);
        pms.currentState = new GroundState();
        pms.currentState.EnterState(pms, this);
    }

    public void EnterState(object invoker, StateMachineNode fromState)
    {
        
    }

    public void ExitState(object invoker, StateMachineNode toState)
    {
        
    }

    public void FrameTick(object invoker)
    {
        
    }

    public void PhysicsTick(object invoker)
    {
        
    }
    public StateMachineNode Clone() => new NullState();
}
