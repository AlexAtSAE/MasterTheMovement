using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public Vector2 mouseSensitivity;
    public Transform cam;
    public Transform bodyTransform;
    private PlayerMovementScript pms;

    private void Start()
    {
        pms= GetComponent<PlayerMovementScript>();
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
    void RotateCamera()
    {
        
        Vector2 cameraInputValue = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        cameraAnglePitch += cameraInputValue.y * mouseSensitivity.y;
        cameraAngleYaw += cameraInputValue.x * mouseSensitivity.x;
        cameraAnglePitch = Mathf.Clamp(cameraAnglePitch, -75f, 80f);
        cam.rotation = Quaternion.Euler(-cameraAnglePitch, cameraAngleYaw, 0);
        pms.transform.rotation = Quaternion.Euler(0, cameraAngleYaw, 0);
    }
}
