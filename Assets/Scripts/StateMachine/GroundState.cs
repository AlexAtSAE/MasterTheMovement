using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GroundState : StateMachineNode
{
    public string Name { get { return "Ground"; } }
    private PlayerMovementScript pms;
    private InputMappingContext IMC;

    public void ConditionUpdate(object invoker)
    {
        if (invoker == null) return;
        if (invoker is not PlayerMovementScript) return;

        if (InputBuffer.GetKeyDown("Jump") || !onGround) pms.ChangeState(new InAirState());
        if (InputBuffer.GetKeyDown("Dash")) pms.ChangeState(new DashState());
        

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
        GroundCheck();
        float forwardInput = InputBuffer.GetKey("Up")  ? 1 : 0;
        float downInput = InputBuffer.GetKey("Down")   ? 1 : 0;
        float rightInput = InputBuffer.GetKey("Right") ? 1 : 0;
        float leftInput = InputBuffer.GetKey("Left")   ? 1 : 0;
        float fwd = forwardInput - downInput;
        float leftright = rightInput - leftInput;
        Vector3 IntendedVelocity = fwd*pms.transform.forward + leftright*pms.transform.right;
        rb.linearVelocity = new Vector3(IntendedVelocity.x, 0, IntendedVelocity.z).normalized*pms.movementSettings.movementSpeed;


        bool jumpInput = InputBuffer.GetKeyDown("Jump");
        if (jumpInput) rb.linearVelocity = rb.linearVelocity + new Vector3(0, pms.jumpSettings.jumpForce,0);

    }
    private bool onGround = true;
    private Vector3 Ground = new Vector3();

    private void GroundCheck()
    {
        bool ledgeRaycastResult = Physics.Raycast(pms.airMovementSettings.GroundRaycastOrigin.position, Vector3.down * 0.25f);
        if (!ledgeRaycastResult) { onGround = false; return; }

        Ray ray = new Ray(pms.airMovementSettings.GroundRaycastOrigin.position, Vector3.down * 0.05f);
        RaycastHit hit;
        bool raycastResult = Physics.Raycast(ray, out hit);
        Ground = hit.point;

        
    }
    public StateMachineNode Clone() => new GroundState();
}
