using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    public StateMachineNode currentState;

    
    public JumpSettings jumpSettings;
    public MovementSettings movementSettings;
    public AirMovementSettings airMovementSettings;
    public DashSettings dashSettings;
    void Start()
    {
        currentState = new NullState();
        currentState.EnterState(this,currentState);
    }

    void Update()
    {
        currentState.FrameTick(this);
    }
    private void FixedUpdate()
    {
        currentState.PhysicsTick(this);
        currentState.ConditionUpdate(this);
        ResetVariables();


        //Debug.Log($"Jump: {JumpInput}, Dash: {DashInput}");
    }

    public void ChangeState(StateMachineNode toState)
    {
        if (toState == null) return;
        currentState.ExitState(this,toState);
        toState.EnterState(this, currentState);
        currentState = toState;
    }

    private void ResetVariables()
    {
        DashInput = false;
        JumpInput = false;
    }
    public Vector2 movementInput { get; private set; }
    public void MovementEvent(InputAction.CallbackContext context) => movementInput = context.ReadValue<Vector2>();
    
    public bool JumpInput { get; private set; }
    public void JumpEvent(InputAction.CallbackContext context) => JumpInput = context.performed; 

    public bool DashInput { get; private set; }
    // public void DashEvent(InputAction.CallbackContext context) => DashInput = context.performed;
    public void DashEvent(InputAction.CallbackContext context) => DashInput = context.performed; 




}

[System.Serializable]
public struct JumpSettings
{
    public float jumpForce;
}

[System.Serializable]
public struct MovementSettings
{
    public float movementSpeed;
}

[System.Serializable]
public struct AirMovementSettings
{
    public float movementSpeed;
    public float gravity;
    public float wallGravity;
    public Transform GroundRaycastOrigin;
    public Transform WallRaycastOrigin;
}

[System.Serializable]
public struct DashSettings
{
    public float dashSpeed;
    public float dashExitSpeed;
    public float dashTime;
    public float dashJumpForce;
    public float dashJumpExitSpeed;
    public float dashExitUpwardsVelocity;
}

