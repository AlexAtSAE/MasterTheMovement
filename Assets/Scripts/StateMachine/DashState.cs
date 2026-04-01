using UnityEngine;

public class DashState : StateMachineNode
{
    public string Name { get { return "Dash"; } }
    private PlayerController PlayerController;
    private StateMachineNode previousState;
    bool pressedJump = false;
    Vector3 Direction;

    public void ConditionUpdate(object invoker)
    {
        if (invoker == null) return;

        if (timeSpent > PlayerController.dashSettings.dashTime)
        {
            //PlayerController.ChangeState(previousState);
            PlayerController.ChangeState(new InAirState());
        }
    }

    public void EnterState(object invoker, StateMachineNode fromState)
    {
        if (invoker == null) return;

        PlayerController = (PlayerController)invoker;
        previousState = fromState.Clone();
        Vector2 InputDirection = PlayerController.movementInput;
        float fwd = InputDirection.y;
        float leftright = InputDirection.x;

        if (fwd == 0 && leftright == 0) Direction = PlayerController.transform.forward;
        else Direction = fwd * PlayerController.transform.forward + leftright * PlayerController.transform.right;
        Direction = Direction.normalized;
    }

    public void ExitState(object invoker, StateMachineNode toState)
    {
        if (invoker == null) return;
        Rigidbody rb = PlayerController.GetComponent<Rigidbody>();
        if (pressedJump && previousState is GroundState)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).normalized
            * PlayerController.dashSettings.dashJumpExitSpeed + Vector3.up * PlayerController.dashSettings.dashJumpForce;
        }
        else 
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).normalized
                * PlayerController.dashSettings.dashExitSpeed + Vector3.up * PlayerController.dashSettings.dashExitUpwardsVelocity;
        }
        
        
    }

    float timeSpent = 0f;

    public void FrameTick(object invoker)
    {
        timeSpent += Time.deltaTime;
        
    }

    public void PhysicsTick(object invoker)
    {
        if (!pressedJump) pressedJump = PlayerController.JumpInput;
        PlayerController.GetComponent<Rigidbody>().linearVelocity = new Vector3(Direction.x,0,Direction.z) * PlayerController.dashSettings.dashSpeed;
    }

    public StateMachineNode Clone() => new DashState();
    
}
