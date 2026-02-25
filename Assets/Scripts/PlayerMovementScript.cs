using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public Rigidbody rigidbody { get; private set; }
    public StateMachineNode currentState;
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
