using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerRotation : MonoBehaviour
{
    public Vector2 mouseSensitivity;
    public Transform cam;
    public Transform bodyTransform;
    private PlayerController PlayerController;

    private void Start()
    {
        PlayerController= GetComponent<PlayerController>();
        LockCamera();
    }

    void Update()
    {
        RotateCamera();
    }

    public static void LockCamera()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void UnlockCamera()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    float cameraAnglePitch;
    float cameraAngleYaw;
    Vector2 cameraInputValue;
    public void LookEvent(InputAction.CallbackContext context)
    {
        cameraInputValue = context.ReadValue<Vector2>();
    }

    void RotateCamera()
    {
        cameraAnglePitch += cameraInputValue.y * mouseSensitivity.y;
        cameraAngleYaw += cameraInputValue.x * mouseSensitivity.x;
        cameraAnglePitch = Mathf.Clamp(cameraAnglePitch, -75f, 80f);
        cam.rotation = Quaternion.Euler(-cameraAnglePitch, cameraAngleYaw, 0);
        PlayerController.transform.rotation = Quaternion.Euler(0, cameraAngleYaw, 0);

        
    }
}
