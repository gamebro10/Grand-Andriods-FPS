using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    Rigidbody rb;

    private float getX;
    private float getY;

    public float dashSpeed;
    public float dashDistance;
    bool isDashing;
    bool canDash;
    public float dashCoolD;

    public GameObject dashEffect;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            isDashing = true;
    }

    private void FixedUpdate()
    {
        if (isDashing)
            Dash();
    }

    public void Dash()
    {
        rb.velocity = Vector3.zero;

        rb.AddForce(Vector3.forward * dashSpeed, ForceMode.Impulse);
        isDashing = false;

    }
}
