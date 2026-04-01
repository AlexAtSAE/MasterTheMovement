using UnityEngine;
using UnityEngine.XR;

public class InAirState : StateMachineNode
{
    public string Name { get { return "InAir"; }}
    private PlayerController PlayerController;
  

    public void ConditionUpdate(object invoker)
    {
        if (invoker == null) return;

        if (onGround) PlayerController.ChangeState(new GroundState());
        if (PlayerController.DashInput) PlayerController.ChangeState(new DashState());
    }

    public void EnterState(object invoker, StateMachineNode fromState)
    {
        if (invoker == null) return;

        PlayerController = (PlayerController)invoker;
    }

    public void ExitState(object invoker, StateMachineNode toState)
    {
        if (invoker == null) return;


    }
    float timeOnWall = 0;

    public void FrameTick(object invoker)
    {
        if (onWall) timeOnWall += Time.deltaTime;
    }

    public void PhysicsTick(object invoker)
    {



        Vector3 IntendedDirection = PlayerController.movementInput.y * PlayerController.transform.forward + PlayerController.movementInput.x * PlayerController.transform.right;

        Vector3 IntendedVelocity = IntendedDirection.normalized * PlayerController.airMovementSettings.movementSpeed; //the Input from the player
        PlayerController.GetComponent<Rigidbody>().linearVelocity += new Vector3(IntendedVelocity.x, 0, IntendedVelocity.z);

        if (PlayerController.GetComponent<Rigidbody>().linearVelocity.y < 0) WallCheck(IntendedDirection);
        if (onWall) PlayerController.GetComponent<Rigidbody>().linearVelocity += Vector3.down * PlayerController.airMovementSettings.wallGravity * WallGravityCurve(timeOnWall);
        else PlayerController.GetComponent<Rigidbody>().linearVelocity += Vector3.down * PlayerController.airMovementSettings.gravity;
        GroundCheck();


    }


    private bool onGround = false;
    private void GroundCheck()
    {
        bool raycastResult = Physics.Raycast(PlayerController.airMovementSettings.GroundRaycastOrigin.position,Vector3.down,0.25f);
        if (raycastResult) { onGround = true; }
    }
    private bool onWall = false;
    private void WallCheck(Vector3 direction)
    {
        bool raycastResult = Physics.Raycast(PlayerController.airMovementSettings.WallRaycastOrigin.position, direction, 0.75f);
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
