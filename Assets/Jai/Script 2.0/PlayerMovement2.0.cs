using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    float playerHeight = 2f;

    [SerializeField] Transform orientation;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode crouching = KeyCode.C;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 7f; //speed of the player
    [SerializeField] float airMultiplier = 0.5f;
    float movementMultiplier = 10f;

    //[Header("Sprinting")]
    //[SerializeField] float walkSpeed = 7f;
    //[SerializeField] float sprintSpeed = 13f;
    //[SerializeField] float acceleration = 10f;

    [Header("Camera")]
    [SerializeField] private UnityEngine.Camera cam;
    [SerializeField] private float walkFov;
    [SerializeField] private float sprintFov;
    [SerializeField] private float sprintFovTime;

    [Header("Jumping")]
    public float jumpForce = 15f;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.1f;
    public bool isGrounded;
    public bool isCrouching;
    public bool isSliding;


    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float crouchDrag =1f;

    [SerializeField] float airDrag = 2f;

    float horizontalMovement;
    float verticalMovement;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    Rigidbody rb;

    RaycastHit slopeHit;

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up) //if the normal of the surface is not pointing straight up then it is a slope
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(isCrouching && isGrounded && rb.velocity.magnitude > 0.5f)
        {
            isSliding = true;
        }
        else
        {
            isSliding = false;
        }

        MyInput();
        ControlDrag();
        //ControlSpeed();
        //SprintFov();

        if (Input.GetKeyDown(jumpKey) && isGrounded) //to jump
        {
            Jump();
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    private void FixedUpdate() // frequency of physics system bcuz we using rb && smooth movement
    {
        MovePlayer();

    }

    void MyInput()
    {
        if(!isCrouching) {
            horizontalMovement = Input.GetAxisRaw("Horizontal");
            verticalMovement = Input.GetAxisRaw("Vertical");
        }
        else
        {

            horizontalMovement = 0;
            verticalMovement = 0;
        }
        

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement; //moving in the direction relative to where player is looking

        if (Input.GetKeyDown(crouching))
        {
            StartCrouch();
        }
        if (Input.GetKeyUp(crouching))
        {
            StopCrouch();
        }
    }

    private void StartCrouch() //Scaling player down
    {
        isCrouching = true;
        base.transform.localScale = new Vector3(1f, 0.5f, 1f);
        base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y - 0.5f, base.transform.position.z);
        if (rb.velocity.magnitude > 0.1f && isGrounded)
        {
            rb.AddForce(orientation.transform.forward * 400f);
            Debug.Log("Siu");
        }
    }

    private void StopCrouch() //Scale player to original size
    {
        isCrouching = false;
        base.transform.localScale = new Vector3(1f, 1f, 1f);
        base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 0.5f, base.transform.position.z);
    }


    void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); //reseting the y
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); //force to move it up
        }
    }

    //void ControlSpeed()
    //{
    //    if (Input.GetKey(sprintKey) && isGrounded)
    //    {

    //        moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
    //    }
    //    else
    //    {
    //        moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
    //    }
    //}

    //void SprintFov()
    //{
    //    if (isGrounded && Input.GetKey(sprintKey))
    //    {
    //        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, sprintFov, sprintFovTime * Time.deltaTime);
    //    }
    //    else
    //    {
    //        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, walkFov, sprintFovTime * Time.deltaTime);
    //    }
    //}

    void ControlDrag() // to make the rb not slippery
    {
        if (isCrouching)
        {
            Debug.Log("crouch dragging");
            rb.drag = crouchDrag;

        }
        else if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }



    void MovePlayer()
    {
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y - 9.81f * Time.deltaTime, rb.velocity.z);
        float current = orientation.transform.eulerAngles.y;
        float target = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * 57.29578f;
        float num = Mathf.DeltaAngle(current, target);
        float num2 = 90f - num;
        float magnitude = rb.velocity.magnitude;
        Vector2 mag = new Vector2(y: magnitude * Mathf.Cos(num * ((float)Mathf.PI / 180f)), x: magnitude * Mathf.Cos(num2 * ((float)Mathf.PI / 180f)));
        float num3 = mag.x;
        float num4 = mag.y;
        float num5 = movementMultiplier;
        if (horizontalMovement > 0f && num3 > num5)
        {
            horizontalMovement = 0f;
        }
        if (horizontalMovement < 0f && num3 < 0f - num5)
        {
            horizontalMovement = 0f;
        }
        if (verticalMovement > 0f && num4 > num5)
        {
            verticalMovement = 0f;
        }
        if (verticalMovement < 0f && num4 < 0f - num5)
        {
            verticalMovement = 0f;
        }
        if (isGrounded && !OnSlope())
        {

            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);

        }
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);

            if (isSliding)
            {
                rb.AddForce(rb.velocity * 1.1f, ForceMode.Acceleration);

            }
        }
        else if (!isGrounded)
        {
            //rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
            rb.AddForce(orientation.transform.forward * moveSpeed * verticalMovement * movementMultiplier * airMultiplier * Time.deltaTime, ForceMode.Acceleration);
            rb.AddForce(orientation.transform.right * moveSpeed * horizontalMovement * movementMultiplier * airMultiplier * Time.deltaTime, ForceMode.Acceleration);
        }
    }

    public Rigidbody GetRb()
    {
        return rb;
    }
}
