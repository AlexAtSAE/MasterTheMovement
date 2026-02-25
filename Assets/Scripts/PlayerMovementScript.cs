using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public Rigidbody rigidbody { get; private set; }
    public StateMachineNode currentState;
    public Transform meshTransform;
    
    public JumpSettings jumpSettings;
    public MovementSettings movementSettings;
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
