using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class DashState : StateMachineNode
{
    public string Name { get { return "Dash"; } }
    private PlayerMovementScript pms;
    private InputMappingContext IMC;
    private StateMachineNode previousState;
    bool pressedJump = false;
    Vector3 Direction;

    public void ConditionUpdate(object invoker)
    {
        if (invoker == null) return;
        if (invoker is not PlayerMovementScript) return;
        if (timeSpent > pms.dashSettings.dashTime)
        {
            //pms.ChangeState(previousState);
            pms.ChangeState(new InAirState());
        }
    }

    public void EnterState(object invoker, StateMachineNode fromState)
    {
        if (invoker == null) return;
        if (invoker is not PlayerMovementScript) return;
        pms = (PlayerMovementScript)invoker;
        IMC = InputBufferManager.instance.GroundIMC;
        InputBufferManager.SetMappingContext(IMC);
        previousState = fromState.Clone();
        float forwardInput = InputBuffer.GetKey("Up") ? 1 : 0;
        float downInput = InputBuffer.GetKey("Down") ? 1 : 0;
        float rightInput = InputBuffer.GetKey("Right") ? 1 : 0;
        float leftInput = InputBuffer.GetKey("Left") ? 1 : 0;
        float fwd = forwardInput - downInput;
        float leftright = rightInput - leftInput;
        if (fwd == 0 && leftright == 0) Direction = pms.transform.forward;
        else Direction = fwd * pms.transform.forward + leftright * pms.transform.right;
        Direction=Direction.normalized;
    }

    public void ExitState(object invoker, StateMachineNode toState)
    {
        if (invoker == null) return;
        if (invoker is not PlayerMovementScript) return;
        if (pressedJump && previousState is GroundState)
        {
            pms.rigidbody.linearVelocity = new Vector3(pms.rigidbody.linearVelocity.x, 0, pms.rigidbody.linearVelocity.z).normalized
            * pms.dashSettings.dashJumpExitSpeed + Vector3.up * pms.dashSettings.dashJumpForce;
        }
        else 
        {
            pms.rigidbody.linearVelocity = new Vector3(pms.rigidbody.linearVelocity.x, 0, pms.rigidbody.linearVelocity.z).normalized
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
        if (!pressedJump) pressedJump = InputBuffer.GetKeyDown("Jump");
        pms.rigidbody.linearVelocity = new Vector3(Direction.x,0,Direction.z) * pms.dashSettings.dashSpeed;
    }

    public StateMachineNode Clone() => new DashState();
    
}
