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

    public float dashCd;
    private float dashCdTimer;

    public KeyCode dashKey = KeyCode.LeftShift;

    public GameObject dashEffect;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
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
        
        rb.velocity = Vector3.zero;
        //rb.angularVelocity = Vector3.zero;

        Vector3 forceApply = orientation.forward * dashSpeed + orientation.up * dashUpForce;

        //additionalForce = forceApply;

        //Invoke(nameof(delayedDash), 0.025f);
        //Invoke(nameof(delayedDash), dashDistance);

       rb.velocity = Vector3.zero;
       rb.transform.rotation = Quaternion.identity;
       rb.AddForce(Vector3.forward * dashSpeed, ForceMode.Impulse);

        isDashing = false;

    }

    private Vector3 additionalForce;

    private void delayedDash()
    {
        rb.AddForce(additionalForce, ForceMode.Impulse);
    }
}
