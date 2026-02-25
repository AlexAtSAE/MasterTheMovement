using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GroundState : StateMachineNode
{

    public PlayerMovementScript pms;
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
        pms = (PlayerMovementScript)invoker;
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
        Rigidbody rb = pms.rigidbody;
        float forwardInput = InputBuffer.GetKey("Up")  ? 1 : 0;
        float downInput = InputBuffer.GetKey("Down")   ? 1 : 0;
        float rightInput = InputBuffer.GetKey("Right") ? 1 : 0;
        float leftInput = InputBuffer.GetKey("Left")   ? 1 : 0;
        float fwd = forwardInput - downInput;
        float leftright = rightInput - leftInput;
        Vector3 IntendedVelocity = fwd*pms.meshTransform.forward + leftright*pms.meshTransform.right;
        rb.velocity = new Vector3(IntendedVelocity.x, rb.velocity.y, IntendedVelocity.z).normalized*pms.movementSettings.movementSpeed;
        
        bool jumpInput = InputBuffer.GetKeyDown("Jump");

        if (jumpInput)
        {
            Debug.Log("Jump");
            //Switch state to IN AIR
        }
    }
}
