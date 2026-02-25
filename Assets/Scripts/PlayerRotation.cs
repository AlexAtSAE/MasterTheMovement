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
        cameraAnglePitch += cameraInputValue.x * mouseSensitivity.x;
        cameraAngleYaw += cameraInputValue.y * mouseSensitivity.y;
        Debug.Log(cameraAngleYaw);
        cameraAngleYaw = Mathf.Clamp(cameraAngleYaw, -75f, 80f);
        cam.rotation = Quaternion.Euler(-cameraAngleYaw, cameraAnglePitch, 0);
        pms.meshTransform.rotation = Quaternion.Euler(0,cameraAnglePitch, 0);
    }
}
