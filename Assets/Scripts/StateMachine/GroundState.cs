using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using static UnityEditor.PlayerSettings;

public class GroundState : StateMachineNode
{
    public string Name { get { return "Ground"; } }
    private PlayerController pms;
    private Rigidbody rb;

    public void ConditionUpdate(object invoker)
    {
        if (invoker == null) return;
        if (invoker is not PlayerController) return;

        if (pms.JumpInput || !onGround) pms.ChangeState(new InAirState());
        if (!onGround) { Debug.Log("NOT ON THE GROUND"); }
        if (pms.DashInput) pms.ChangeState(new DashState());
        

    }

    public void EnterState(object invoker, StateMachineNode fromState)
    {
        if (invoker == null) return;
        if (invoker is not PlayerController) return;
        pms = (PlayerController)invoker;
        rb = pms.GetComponent<Rigidbody>();

    }

    public void ExitState(object invoker, StateMachineNode toState)
    {
        if (invoker == null) return;
        if (invoker is not PlayerController) return;

    }

    public void FrameTick(object invoker)
    {

    }


    float skinWidth = 0.015f;
    float maxSlopeAngle = 60f;

    public void PhysicsTick(object invoker)
    {
        
        GroundCheck();

        Vector3 IntendedVelocity = (pms.movementInput.y * pms.transform.forward + pms.movementInput.x * pms.transform.right).normalized;
        Vector3 iVel = new Vector3(IntendedVelocity.x, 0, IntendedVelocity.z).normalized * pms.movementSettings.movementSpeed;
       

        float capsulePointOffset = pms.playerDetails.capsuleHeight / 2 - pms.playerDetails.capsuleRadius;
        Vector3 capsulePointA = pms.transform.position + Vector3.up * capsulePointOffset;
        Vector3 capsulePointB = pms.transform.position + Vector3.down * capsulePointOffset;

        RaycastHit hit;
        if (Physics.CapsuleCast(capsulePointA, capsulePointB,pms.playerDetails.capsuleRadius-skinWidth, iVel.normalized, out hit, pms.playerDetails.capsuleRadius + skinWidth) 
            && Vector3.Angle(Vector3.up,hit.normal) >= maxSlopeAngle)
        {
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
                leftOver *= mag;
                leftOver *= scale;
            }
            rb.linearVelocity = leftOver;
        }
        else
        {
            Physics.Raycast(pms.airMovementSettings.GroundRaycastOrigin.transform.position, Vector3.down, out hit, 0.1f);
            Vector3 newVel = Vector3.ProjectOnPlane(iVel, hit.normal);
            Debug.DrawRay(pms.transform.position, newVel,Color.green,0.1f);
            rb.linearVelocity = newVel;
        }




            bool jumpInput = pms.JumpInput;
        if (jumpInput) rb.linearVelocity = rb.linearVelocity + new Vector3(0, pms.jumpSettings.jumpForce,0);

    }
    private bool onGround = true;
    private Vector3 Ground = new Vector3();

    private void GroundCheck()
    {
        bool ledgeRaycastResult = Physics.Raycast(pms.airMovementSettings.GroundRaycastOrigin.position, Vector3.down , 0.15f);
        if (!ledgeRaycastResult) { onGround = false; return; }

        Ray ray = new Ray(pms.airMovementSettings.GroundRaycastOrigin.position, Vector3.down * 0.05f);
        RaycastHit hit;
        bool raycastResult = Physics.Raycast(ray, out hit);
        Ground = hit.point;
    }




    public StateMachineNode Clone() => new GroundState();
}
