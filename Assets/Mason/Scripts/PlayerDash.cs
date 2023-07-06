using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private Rigidbody rb;
    public Transform orientation;
    public Transform playerCam;



    public float dashSpeed;
    public float dashUpForce;
    public float dashDistance;
    public float durationD;
    bool isDashing;

    private PlayerMovement player;

    public int dashNum;

    public float dashCd;
    private float dashCdTimer;

    public KeyCode dashKey = KeyCode.LeftShift;

    public GameObject dashEffect;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public float horizontalSpeed = 2.0F;
    public float verticalSpeed = 2.0F;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            float h = horizontalSpeed * Input.GetAxis("Mouse X");
            float v = verticalSpeed * Input.GetAxis("Mouse Y");
            transform.rotation = Quaternion.Euler(h, v, 0);
        }
        if (Input.GetKeyDown(dashKey))
            Dash();

        if(dashCdTimer > 0)
            dashCdTimer -= Time.deltaTime;
    }


    public void Dash()
    {
        if (dashCdTimer > 0) return;
        else dashCdTimer = dashCd;

        //isDashing = true;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            float h = horizontalSpeed * Input.GetAxis("Mouse X");
            float v = verticalSpeed * Input.GetAxis("Mouse Y");
            transform.rotation = Quaternion.Euler(h, v, 0);
        }

        rb.velocity = Vector3.zero;
        //rb.angularVelocity = Vector3.zero;

        Vector3 forceApply = orientation.forward * dashSpeed + orientation.up * dashUpForce;

        //additionalForce = forceApply;

        //Invoke(nameof(delayedDash), 0.025f);
        //Invoke(nameof(delayedDash), dashDistance);

       rb.velocity = Vector3.zero;
       rb.transform.rotation = Quaternion.identity;
       rb.AddForce(orientation.forward * dashSpeed, ForceMode.Impulse);

        isDashing = false;

    }

    private Vector3 additionalForce;

    private void delayedDash()
    {
        rb.AddForce(additionalForce, ForceMode.Impulse);
    }
}
