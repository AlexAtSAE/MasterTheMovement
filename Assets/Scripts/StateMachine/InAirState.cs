using UnityEngine;
using UnityEngine.XR;

public class InAirState : StateMachineNode
{
    public string Name { get { return "InAir"; }}
    private PlayerController pms;
  

    public void ConditionUpdate(object invoker)
    {
        if (invoker == null) return;
        if (invoker is not PlayerController) return;
        if (onGround) pms.ChangeState(new GroundState());
        if (pms.DashInput) pms.ChangeState(new DashState());
    }

    public void EnterState(object invoker, StateMachineNode fromState)
    {
        if (invoker == null) return;
        if (invoker is not PlayerController) return;
        pms = (PlayerController)invoker;
    }

    public void ExitState(object invoker, StateMachineNode toState)
    {
        if (invoker == null) return;
        if (invoker is not PlayerController) return;


    }
    float timeOnWall = 0;

    public void FrameTick(object invoker)
    {
        if (onWall) timeOnWall += Time.deltaTime;
    }

    public void PhysicsTick(object invoker)
    {



        Vector3 IntendedDirection = pms.movementInput.y * pms.transform.forward + pms.movementInput.x * pms.transform.right;

        Vector3 IntendedVelocity = IntendedDirection.normalized * pms.airMovementSettings.movementSpeed; //the Input from the player
        pms.GetComponent<Rigidbody>().linearVelocity += new Vector3(IntendedVelocity.x, 0, IntendedVelocity.z);

        if (pms.GetComponent<Rigidbody>().linearVelocity.y < 0) WallCheck(IntendedDirection);
        if (onWall) pms.GetComponent<Rigidbody>().linearVelocity += Vector3.down * pms.airMovementSettings.wallGravity * WallGravityCurve(timeOnWall);
        else pms.GetComponent<Rigidbody>().linearVelocity += Vector3.down * pms.airMovementSettings.gravity;
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
