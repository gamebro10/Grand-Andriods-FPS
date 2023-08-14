using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("references")]
    [SerializeField] WallRun wallRun;


    //camera look sens
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    [SerializeField] Transform cam = null;
    [SerializeField] Transform orientation = null;

    float mouseX;
    float mouseY;

    float multiplier = 0.10f;

    //Rotation Angles
    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //cursor when playing a game
        Cursor.visible = false;
    }

    private void Update()
    {
        MyInput();
    }

    void MyInput()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation += mouseX * sensX * multiplier; //horizontal rotation
        xRotation -= mouseY * sensY * multiplier; //vertical rotation

        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //clamping vertical rotation

        cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, wallRun.tilt); //practical only the cam rotates vertically based on input
        orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0); // and the player rotates horizontally based on input
    }
}
