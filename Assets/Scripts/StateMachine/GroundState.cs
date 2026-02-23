using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundState : StateMachineNode
{
    public void ConditionUpdate(object invoker)
    {
        if (invoker == null) return;
        if (invoker is not PlayerMovementScript) return;
        PlayerMovementScript script = (PlayerMovementScript)invoker;
       
    }

    public void EnterState(object invoker, StateMachineNode fromState)
    {
        if (invoker == null) return;
        if (invoker is not PlayerMovementScript) return;
        PlayerMovementScript script = (PlayerMovementScript)invoker;
    }

    public void ExitState(object invoker, StateMachineNode toState)
    {
        if (invoker == null) return;
        if (invoker is not PlayerMovementScript) return;
        PlayerMovementScript script = (PlayerMovementScript)invoker;
    }

    public void FrameTick(object invoker)
    {
        if (invoker == null) return;
        if (invoker is not PlayerMovementScript) return;
        PlayerMovementScript script = (PlayerMovementScript)invoker;
    }

    public void PhysicsTick(object invoker)
    {
        if (invoker == null) return;
        if (invoker is not PlayerMovementScript) return;
        PlayerMovementScript script = (PlayerMovementScript)invoker;
    }
}
