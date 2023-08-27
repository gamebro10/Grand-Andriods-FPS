using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerDash : MonoBehaviour
{
    private Rigidbody rb;
    public Transform orientation;
    public Transform playerCam;
    public AudioSource dashSound;

    public float dashSpeed;
    //public float dashDuration;
    public float dashUpForce;
   // public float durationD;
    //public bool isDashing;

    public bool CanDash;
    //private Vector3 PlayerY;

    //public int dashNum;

    public float dashCd;
    public int dashWallCheck;
    private float dashTimer;

    private KeyCode dashKey = KeyCode.LeftShift;

    //Vector3 move;

    //private GameObject dashEffect;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        AudioManager.Instance.RegisterSFX(dashSound);
    }

    private float horizontalSpeed = 2.0F;
    private float verticalSpeed = 2.0F;

    // Update is called once per frame
    private void Update()
    {

        if (dashTimer > 0)
            dashTimer -= Time.deltaTime;

        if (GetComponent<PlayerMovement>().onWall == true && dashTimer < dashCd && dashWallCheck != 1)
        {
            dashWallCheck = 1;
        }

        if (dashWallCheck == 1 && dashTimer < 0)
        {
            dashWallCheck = 0;
        }

        if (Input.GetKeyDown(dashKey) && GetComponent<PlayerMovement>().grounded == false && GetComponent<PlayerMovement>().onWall == false && CanDash == true && dashWallCheck == 0)
        {
            CanDash = false;
            float h = horizontalSpeed * Input.GetAxis("Mouse X");
            float v = verticalSpeed * Input.GetAxis("Mouse Y");
            transform.rotation = Quaternion.Euler(h, v, 0);
            Dash();
        }

        if (GetComponent<PlayerMovement>().grounded == true || GetComponent<PlayerMovement>().onWall == true)
        {
            CanDash = true;
        }
    }


    public void Dash()
    {
        //PlayerY = new Vector3(1.1f, transform.position.y);
        //rb.angularVelocity = Vector3.zero;

        if (dashTimer > 0) return;
        else dashTimer = dashCd;

        //isDashing = true;

        //rb.AddForce(move * dashSpeed,ForceMode.Impulse);

        //float h = horizontalSpeed * Input.GetAxis("Mouse X");
        //float v = verticalSpeed * Input.GetAxis("Mouse Y");
        //transform.rotation = Quaternion.Euler(h, v, 0);

        if (Input.GetKey(dashKey) && CanDash == true)
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

        dashSound.Play();
        //isDashing = false;

    }

    //private Vector3 additionalForce;

    //  private void delayedDash()
    // {
    //      rb.AddForce(additionalForce, ForceMode.Impulse);
    //  }

    private void OnDestroy()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.UnregisterSFX(dashSound);
        }
    }
}
