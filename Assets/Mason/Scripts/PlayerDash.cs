using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerDash : MonoBehaviour
{
    private Rigidbody rb;
    public Transform orientation;
    public Transform playerCam;


    public float dashSpeed;
    public float dashDuration;
    public float dashUpForce;
    public float durationD;
    public bool isDashing;
    //private Vector3 PlayerY;

    private PlayerMovement Movement;

    //public int dashNum;

    public float dashCd;
    private float dashTimer;

    private KeyCode dashKey = KeyCode.LeftShift;

    Vector3 move;

    //private GameObject dashEffect;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private float horizontalSpeed = 2.0F;
    private float verticalSpeed = 2.0F;

    // Update is called once per frame
    private void Update()
    {
        //move = (orientation.right * Input.GetAxis("Horizontal")) +
         // (orientation.forward * Input.GetAxis("Vertical"));

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            float h = horizontalSpeed * Input.GetAxis("Mouse X");
            float v = verticalSpeed * Input.GetAxis("Mouse Y");
            transform.rotation = Quaternion.Euler(h, v, 0);
        }

        if (Input.GetKey(dashKey))
            Dash();

        if(dashTimer > 0)
            dashTimer -= Time.deltaTime;
    }


    public void Dash()
    {
        //PlayerY = new Vector3(1.1f, transform.position.y);
        //rb.angularVelocity = Vector3.zero;

        if (dashTimer > 0) return;
        else dashTimer = dashCd;

        isDashing = true;

        //rb.AddForce(move * dashSpeed,ForceMode.Impulse);

        //float h = horizontalSpeed * Input.GetAxis("Mouse X");
        //float v = verticalSpeed * Input.GetAxis("Mouse Y");
        //transform.rotation = Quaternion.Euler(h, v, 0);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            float h = horizontalSpeed * Input.GetAxis("Mouse X");
            float v = verticalSpeed * Input.GetAxis("Mouse Y");
            transform.rotation = Quaternion.Euler(h, v, 0);
        }

        rb.velocity = Vector3.zero;
        //rb.angularVelocity = Vector3.zero;

        Vector3 forceApply = orientation.forward * dashSpeed + orientation.up * dashUpForce;

        //additionalForce = forceApply;

       // Invoke(nameof(delayedDash), 0.025f);
       // Invoke(nameof(delayedDash), dashDuration);

        rb.velocity = Vector3.zero;
        rb.transform.rotation = Quaternion.identity;
        rb.AddForce(orientation.forward * dashSpeed, ForceMode.Impulse);

        
           isDashing = false;

    }

    //private Vector3 additionalForce;

  //  private void delayedDash()
   // {
  //      rb.AddForce(additionalForce, ForceMode.Impulse);
  //  }
}
