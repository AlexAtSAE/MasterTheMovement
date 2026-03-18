using UnityEngine;

public class DashState : StateMachineNode
{
    public string Name { get { return "Dash"; } }
    private PlayerController pms;
    private StateMachineNode previousState;
    bool pressedJump = false;
    Vector3 Direction;

    public void ConditionUpdate(object invoker)
    {
        if (invoker == null) return;
        if (invoker is not PlayerController) return;
        if (timeSpent > pms.dashSettings.dashTime)
        {
            //pms.ChangeState(previousState);
            pms.ChangeState(new InAirState());
        }
    }

    public void EnterState(object invoker, StateMachineNode fromState)
    {
        if (invoker == null) return;
        if (invoker is not PlayerController) return;
        pms = (PlayerController)invoker;
        previousState = fromState.Clone();
        Vector2 InputDirection = pms.movementInput;
        float fwd = InputDirection.y;
        float leftright = InputDirection.x;

        if (fwd == 0 && leftright == 0) Direction = pms.transform.forward;
        else Direction = fwd * pms.transform.forward + leftright * pms.transform.right;
        Direction = Direction.normalized;
    }

    public void ExitState(object invoker, StateMachineNode toState)
    {
        if (invoker == null) return;
        if (invoker is not PlayerController) return;
        Rigidbody rb = pms.GetComponent<Rigidbody>();
        if (pressedJump && previousState is GroundState)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).normalized
            * pms.dashSettings.dashJumpExitSpeed + Vector3.up * pms.dashSettings.dashJumpForce;
        }
        else 
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).normalized
                * pms.dashSettings.dashExitSpeed + Vector3.up * pms.dashSettings.dashExitUpwardsVelocity;
        }
        
        
    }

    float timeSpent = 0f;

    public void FrameTick(object invoker)
    {
        timeSpent += Time.deltaTime;
        
    }

    public void PhysicsTick(object invoker)
    {
        if (!pressedJump) pressedJump = pms.JumpInput;
        pms.GetComponent<Rigidbody>().linearVelocity = new Vector3(Direction.x,0,Direction.z) * pms.dashSettings.dashSpeed;
    }

    public StateMachineNode Clone() => new DashState();
    
}
