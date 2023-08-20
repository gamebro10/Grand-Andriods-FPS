using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDash : MonoBehaviour
{
    public float dashForce = 10f;
    public float dashDuration = 0.2f;
    public Transform cameraTransform; 

    private Rigidbody rb;
    private bool isDashing = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            Dash();
        }
    }

    private void Dash()
    {
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        Vector3 cameraForward = cameraTransform.forward;

       
        Vector3 dashDirection = (horizontalVelocity + cameraForward).normalized;

        rb.AddForce(dashDirection * dashForce, ForceMode.VelocityChange);

      
        float rotationAngle = Mathf.Atan2(dashDirection.x, dashDirection.z) * Mathf.Rad2Deg;

      
        Quaternion targetRotation = Quaternion.Euler(0f, rotationAngle, 0f);

     
        rb.MoveRotation(targetRotation);

        isDashing = true;
        Invoke(nameof(EndDash), dashDuration);
    }

    private void EndDash()
    {
        isDashing = false;
    }
}

