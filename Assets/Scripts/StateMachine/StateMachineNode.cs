using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface StateMachineNode
{ 
    //Maybe cast to take info
    public void EnterState(object invoker, StateMachineNode fromState);

    //Can add delay
    public void ExitState(object invoker, StateMachineNode toState);

    public void FrameTick(object invoker);

    public void PhysicsTick(object invoker);

    //Call exit state to change state
    public void ConditionUpdate(object invoker);

    public StateMachineNode Clone();
}
