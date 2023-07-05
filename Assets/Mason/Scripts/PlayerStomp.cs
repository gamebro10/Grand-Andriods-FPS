using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStomp : MonoBehaviour
{
    Rigidbody rb;

    private float getX;
    private float getY;

    public float stompSpeed;
    public float stompDistance;
    bool isStomp;
    bool canStomp;
    public bool onGround;

    public PlayerMovement dashOrStomp;
    public PlayerDash dashConfirm;

    public GameObject dashEffect;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
 
        if (Input.GetKeyDown(KeyCode.LeftControl))
            isStomp = true;
        
    }

    private void FixedUpdate()
    {
        if (isStomp)
            Stomp();
    }

    private void buttonPrio()
    {
        if (dashOrStomp.grounded)
        {
            Stomp();
        }
        else
            dashConfirm.Dash();
    }

    void Stomp()
    {
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.down * stompDistance, ForceMode.Impulse);
            isStomp = false;
      

    }
}
