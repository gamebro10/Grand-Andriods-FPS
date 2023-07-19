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
    public float durationD;
    public bool isDashing;
    private Vector3 PlayerY;

    private PlayerMovement Movement;

    public int dashNum;

    //public float dashCd;
    public float dashTimer;

    private KeyCode dashKey = KeyCode.LeftShift;

    Vector3 move;

    //private GameObject dashEffect;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //public float horizontalSpeed = 2.0F;
    //public float verticalSpeed = 2.0F;

    // Update is called once per frame
    private void Update()
    {
        move = (orientation.right * Input.GetAxis("Horizontal")) +
          (orientation.forward * Input.GetAxis("Vertical"));

        if (Input.GetKey(dashKey))
            Dash();

        //if(dashCdTimer > 0)
        //    dashCdTimer -= Time.deltaTime;
    }


    public void Dash()
    {
        //PlayerY = new Vector3(1.1f, transform.position.y);
        //rb.angularVelocity = Vector3.zero;

        //if (dashTimer > 0) return;
        //else dashTimer -= Time.deltaTime;

       

        rb.AddForce(move * dashSpeed,ForceMode.Impulse);

        //float h = horizontalSpeed * Input.GetAxis("Mouse X");
        //float v = verticalSpeed * Input.GetAxis("Mouse Y");
        //transform.rotation = Quaternion.Euler(h, v, 0);

        //rb.velocity = Vector3.zero;
        //rb.angularVelocity = Vector3.zero;

        //Vector3 forceApply = orientation.forward * dashSpeed + orientation.up * dashUpForce;

        //additionalForce = forceApply;

        //Invoke(nameof(delayedDash), 0.025f);
        //Invoke(nameof(delayedDash), dashDistance);

        //rb.velocity = Vector3.zero;
        //rb.transform.rotation = Quaternion.identity;
        //rb.AddForce(orientation.forward * dashSpeed, ForceMode.Impulse);

        //if (dashTimer <= 0)
        //    isDashing = false;

    }

    //private Vector3 additionalForce;

    //private void delayedDash()
    //{
    //    rb.AddForce(additionalForce, ForceMode.Impulse);
    //}
}
