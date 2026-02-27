using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class DashState : StateMachineNode
{
    private PlayerMovementScript pms;
    private InputMappingContext IMC;
    private StateMachineNode previousState;

    public void ConditionUpdate(object invoker)
    {
        if (invoker == null) return;
        if (invoker is not PlayerMovementScript) return;
        if (timeSpent > pms.dashSettings.dashTime)
        {
            pms.ChangeState(previousState);
            Debug.Log($"trying to change state to {previousState.GetType().ToString()}");
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
    }

    public void ExitState(object invoker, StateMachineNode toState)
    {
        if (invoker == null) return;
        if (invoker is not PlayerMovementScript) return;
        pms.rigidbody.linearVelocity = new Vector3(pms.rigidbody.linearVelocity.x, 0, pms.rigidbody.linearVelocity.z).normalized
            * pms.dashSettings.dashExitSpeed + Vector3.up * 2;

    }

    float timeSpent = 0f;

    public void FrameTick(object invoker)
    {
        timeSpent += Time.deltaTime;
        
    }

    public void PhysicsTick(object invoker)
    {
        pms.rigidbody.linearVelocity = pms.transform.forward * pms.dashSettings.dashSpeed;
    }

    public StateMachineNode Clone() => new DashState();
    
}
