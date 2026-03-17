using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class GroundState : StateMachineNode
{
    public string Name { get { return "Ground"; } }
    private PlayerController pms;
    private Rigidbody rb;

    public void ConditionUpdate(object invoker)
    {
        if (invoker == null) return;
        if (invoker is not PlayerController) return;

        if (pms.JumpInput || !onGround) pms.ChangeState(new InAirState());
        if (!onGround) { Debug.Log("NOT ON THE GROUND"); }
        if (pms.DashInput) pms.ChangeState(new DashState());
        

    }

    public void EnterState(object invoker, StateMachineNode fromState)
    {
        if (invoker == null) return;
        if (invoker is not PlayerController) return;
        pms = (PlayerController)invoker;
        rb = pms.GetComponent<Rigidbody>();

    }

    public void ExitState(object invoker, StateMachineNode toState)
    {
        if (invoker == null) return;
        if (invoker is not PlayerController) return;

    }

    public void FrameTick(object invoker)
    {

    }

    public void PhysicsTick(object invoker)
    {   
        
        GroundCheck();

        Vector3 IntendedVelocity = pms.movementInput.y * pms.transform.forward + pms.movementInput.x * pms.transform.right;
        
        rb.linearVelocity = new Vector3(IntendedVelocity.x, 0, IntendedVelocity.z).normalized*pms.movementSettings.movementSpeed;


        bool jumpInput = pms.JumpInput;
        if (jumpInput) rb.linearVelocity = rb.linearVelocity + new Vector3(0, pms.jumpSettings.jumpForce,0);

    }
    private bool onGround = true;
    private Vector3 Ground = new Vector3();

    private void GroundCheck()
    {
        bool ledgeRaycastResult = Physics.Raycast(pms.airMovementSettings.GroundRaycastOrigin.position, Vector3.down , 0.15f);
        if (!ledgeRaycastResult) { onGround = false; return; }

        Ray ray = new Ray(pms.airMovementSettings.GroundRaycastOrigin.position, Vector3.down * 0.05f);
        RaycastHit hit;
        bool raycastResult = Physics.Raycast(ray, out hit);
        Ground = hit.point;

        
    }
    public StateMachineNode Clone() => new GroundState();
}
