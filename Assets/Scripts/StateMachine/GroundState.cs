using UnityEngine;

public class GroundState : StateMachineNode
{
    public string Name { get { return "Ground"; } }
    private PlayerController PlayerController;
    private Rigidbody rb;

    public void ConditionUpdate(object invoker)
    {
        if (invoker == null) return;


        if (PlayerController.JumpInput || !onGround) PlayerController.ChangeState(new InAirState());
        //if (!onGround) { Debug.Log("NOT ON THE GROUND"); }
        if (PlayerController.DashInput) PlayerController.ChangeState(new DashState());
        

    }

    public void EnterState(object invoker, StateMachineNode fromState)
    {
        if (invoker == null) return;
        
        PlayerController = (PlayerController)invoker;
        rb = PlayerController.GetComponent<Rigidbody>();

    }

    public void ExitState(object invoker, StateMachineNode toState)
    {
        if (invoker == null) return;


    }

    public void FrameTick(object invoker)
    {

    }


    float skinWidth = 0.015f;

    public void PhysicsTick(object invoker)
    {
        
        GroundCheck();

        Vector3 IntendedVelocity = (PlayerController.movementInput.y * PlayerController.transform.forward + PlayerController.movementInput.x * PlayerController.transform.right).normalized;
        Vector3 iVel = new Vector3(IntendedVelocity.x, 0, IntendedVelocity.z).normalized * PlayerController.groundSettings.movementSpeed;
       

        float capsulePointOffset = PlayerController.playerDetails.capsuleHeight / 2 - PlayerController.playerDetails.capsuleRadius;
        Vector3 capsulePointA = PlayerController.transform.position + Vector3.up * capsulePointOffset;
        Vector3 capsulePointB = PlayerController.transform.position + Vector3.down * capsulePointOffset;

        RaycastHit hit;
        bool capsuleCast = Physics.CapsuleCast(capsulePointA, capsulePointB, PlayerController.playerDetails.capsuleRadius - skinWidth, iVel.normalized, out hit, PlayerController.playerDetails.capsuleRadius + skinWidth);
        
        
        if (capsuleCast && Vector3.Angle(Vector3.up,hit.normal) >= PlayerController.groundSettings.maxSlopeAngle)
        {
            //Wall logic
            Debug.DrawRay(hit.point, Vector3.up,Color.blue,0.1f);
            Vector3 snapToSurface = iVel.normalized * (hit.distance - skinWidth);
            Vector3 leftOver = iVel - snapToSurface;
            float dist = iVel.magnitude + skinWidth;
            if (snapToSurface.magnitude <= skinWidth)
            {
                snapToSurface = Vector3.zero;
            }
            else
            {
                float scale = 1 - Vector3.Dot(
                    new Vector3(hit.normal.x, 0, hit.normal.z).normalized,
                    -new Vector3(iVel.x, 0, iVel.z).normalized
                    );
                float mag = leftOver.magnitude;
                leftOver = Vector3.ProjectOnPlane(leftOver, hit.normal).normalized;
                //leftOver *= mag;
                //leftOver *= scale;
            }
            ForcePlayer(PlayerController.rigidbody.linearVelocity + leftOver * PlayerController.groundSettings.slidingAcceleration);
            //rb.linearVelocity += leftOver*PlayerController.groundSettings.slidingAcceleration;
        }
        else
        {
            Physics.Raycast(PlayerController.airMovementSettings.GroundRaycastOrigin.transform.position, Vector3.down, out hit, 0.1f + PlayerController.playerDetails.capsuleRadius*(1/Mathf.Cos(Mathf.Deg2Rad*(Vector3.Angle(Vector3.up, hit.normal))))); //make the distance be radius * sec(angle) + clarification
            Vector3 newVel = Vector3.ProjectOnPlane(iVel, hit.normal);
            Debug.DrawRay(PlayerController.transform.position, newVel,Color.green,0.1f);
            if(!IntendedVelocity.Equals(Vector3.zero))
                ForcePlayer(newVel);
            else
            {
                //DeceleratePlayer
                PlayerController.rigidbody.linearVelocity *= PlayerController.groundSettings.movementDeceleration;
            }
        }




            bool jumpInput = PlayerController.JumpInput;
        if (jumpInput) rb.linearVelocity = rb.linearVelocity + new Vector3(0, PlayerController.jumpSettings.jumpForce,0);

    }
    private bool onGround = true;
    private Vector3 GroundNormal = new Vector3();

    private void GroundCheck()
    {
        RaycastHit hit;
        bool ledgeRaycastResult = Physics.Raycast(PlayerController.airMovementSettings.GroundRaycastOrigin.position, Vector3.down ,out hit, 0.15f);
        GroundNormal = hit.normal;
        if (!ledgeRaycastResult) { onGround = false; return; }

        /*Ray ray = new Ray(PlayerController.airMovementSettings.GroundRaycastOrigin.position, Vector3.down * 0.05f);
        RaycastHit hit;
        bool raycastResult = Physics.Raycast(ray, out hit);
        Ground = hit.point;*/
    }
    private void ForcePlayer(Vector3 TargetVelocity)
    {
        rb.linearVelocity += TargetVelocity.normalized*PlayerController.groundSettings.movementAcceleration;
        if(rb.linearVelocity.magnitude > PlayerController.groundSettings.movementSpeed)
        {
            rb.linearVelocity = TargetVelocity.normalized*PlayerController.groundSettings.movementSpeed;
        }
    }




    public StateMachineNode Clone() => new GroundState();
}
