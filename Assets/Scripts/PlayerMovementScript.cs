using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public Rigidbody rigidbody { get; private set; }
    public StateMachineNode currentState;

    
    public JumpSettings jumpSettings;
    public MovementSettings movementSettings;
    public AirMovementSettings airMovementSettings;
    public DashSettings dashSettings;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
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
    }

    public void ChangeState(StateMachineNode toState)
    {
        if (toState == null) return;
        currentState.ExitState(this,toState);
        toState.EnterState(this, currentState);
        currentState = toState;
    }

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

