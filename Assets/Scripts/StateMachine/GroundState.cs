using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GroundState : StateMachineNode
{

    public PlayerMovementScript script;
    public InputMappingContext IMC;

    public void ConditionUpdate(object invoker)
    {
        if (invoker == null) return;
        if (invoker is not PlayerMovementScript) return;

    }

    public void EnterState(object invoker, StateMachineNode fromState)
    {
        if (invoker == null) return;
        if (invoker is not PlayerMovementScript) return;
        script = (PlayerMovementScript)invoker;
        IMC = InputBufferManager.instance.GroundIMC;
        InputBufferManager.SetMappingContext(IMC);

    }

    public void ExitState(object invoker, StateMachineNode toState)
    {
        if (invoker == null) return;
        if (invoker is not PlayerMovementScript) return;
        

    }

    public void FrameTick(object invoker)
    {

    }

    public void PhysicsTick(object invoker)
    {
        if(InputBuffer.GetKeyDown("Up"))Debug.Log("Forward");
        if(InputBuffer.GetKeyDown("Down"))Debug.Log("Down");
        if(InputBuffer.GetKeyDown("Right"))Debug.Log("Right");
        if(InputBuffer.GetKeyDown("Left"))Debug.Log("Left");
    }
}
