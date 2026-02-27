using UnityEngine;
using UnityEngine.XR;

public class InAirState : StateMachineNode
{
    private PlayerMovementScript pms;
    private InputMappingContext IMC;
  

    public void ConditionUpdate(object invoker)
    {
        if (invoker == null) return;
        if (invoker is not PlayerMovementScript) return;
        if (onGround) pms.ChangeState(new GroundState());
        if (InputBuffer.GetKeyDown("Dash")) pms.ChangeState(new DashState());
    }

    public void EnterState(object invoker, StateMachineNode fromState)
    {
        if (invoker == null) return;
        if (invoker is not PlayerMovementScript) return;
        pms = (PlayerMovementScript)invoker;
        IMC = InputBufferManager.instance.GroundIMC;
        InputBufferManager.SetMappingContext(IMC);
        Debug.Log("Entered air state");
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

        
        float forwardInput = InputBuffer.GetKey("Up") ? 1 : 0;
        float downInput = InputBuffer.GetKey("Down") ? 1 : 0;
        float rightInput = InputBuffer.GetKey("Right") ? 1 : 0;
        float leftInput = InputBuffer.GetKey("Left") ? 1 : 0;
        float fwd = forwardInput - downInput;
        float leftright = rightInput - leftInput;
        Vector3 IntendedVelocity = fwd * pms.transform.forward + leftright * pms.transform.right;
        IntendedVelocity = IntendedVelocity.normalized * pms.airMovementSettings.movementSpeed; //the Input from the player
        pms.rigidbody.linearVelocity += new Vector3(IntendedVelocity.x, 0, IntendedVelocity.z);
        pms.rigidbody.linearVelocity += Vector3.down*pms.airMovementSettings.gravity;
        Debug.Log(pms.rigidbody.linearVelocity);
        GroundCheck();


    }


    private bool onGround = false;
    private void GroundCheck()
    {
        bool raycastResult = Physics.Raycast(pms.airMovementSettings.RaycastOrigin.position,Vector3.down,0.01f);
        if (raycastResult) onGround = true;
    }
    public StateMachineNode Clone() => new InAirState();
}
