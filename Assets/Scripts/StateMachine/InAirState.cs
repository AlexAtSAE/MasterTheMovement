using UnityEngine;
using UnityEngine.XR;

public class InAirState : StateMachineNode
{
    public string Name { get { return "InAir"; }}
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
    }

    public void ExitState(object invoker, StateMachineNode toState)
    {
        if (invoker == null) return;
        if (invoker is not PlayerMovementScript) return;


    }
    float timeOnWall = 0;

    public void FrameTick(object invoker)
    {
        if (onWall) timeOnWall += Time.deltaTime;
    }

    public void PhysicsTick(object invoker)
    {
        


        float forwardInput = InputBuffer.GetKey("Up") ? 1 : 0;
        float downInput = InputBuffer.GetKey("Down") ? 1 : 0;
        float rightInput = InputBuffer.GetKey("Right") ? 1 : 0;
        float leftInput = InputBuffer.GetKey("Left") ? 1 : 0;
        float fwd = forwardInput - downInput;
        float leftright = rightInput - leftInput;
        Vector3 IntendedDirection = fwd * pms.transform.forward + leftright * pms.transform.right;

        Vector3 IntendedVelocity = IntendedDirection.normalized * pms.airMovementSettings.movementSpeed; //the Input from the player
        pms.rigidbody.linearVelocity += new Vector3(IntendedVelocity.x, 0, IntendedVelocity.z);

        if(pms.rigidbody.linearVelocity.y<0)WallCheck(IntendedDirection);
        if(onWall) pms.rigidbody.linearVelocity += Vector3.down*pms.airMovementSettings.wallGravity * WallGravityCurve(timeOnWall);
        else pms.rigidbody.linearVelocity += Vector3.down * pms.airMovementSettings.gravity;
        GroundCheck();


    }


    private bool onGround = false;
    private void GroundCheck()
    {
        bool raycastResult = Physics.Raycast(pms.airMovementSettings.GroundRaycastOrigin.position,Vector3.down,0.15f);
        if (raycastResult) onGround = true;
    }
    private bool onWall = false;
    private void WallCheck(Vector3 direction)
    {
        bool raycastResult = Physics.Raycast(pms.airMovementSettings.WallRaycastOrigin.position, direction, 0.75f);
        if (raycastResult) onWall = true;
        else onWall = false;
        if (onWall) { Debug.Log($"On wall: {timeOnWall}"); }
    }

    private float WallGravityCurve(float t)
    {
        return Mathf.Clamp01(t);
    }
    public StateMachineNode Clone() => new InAirState();
}
