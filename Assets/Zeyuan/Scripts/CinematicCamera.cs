using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicCamera : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float verticalSpeed;
    [SerializeField] float sensitivity;
    [SerializeField] Rigidbody rb;
    Vector3 movement;
    float originalSpeed;
    float xRotation;
    // Start is called before the first frame update
    void Start()
    {
        rb.drag = 2;
        originalSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.J))
        {
            movement.x = -1;
        }
        if (Input.GetKey(KeyCode.L))
        {
            movement.x = 1;
        }
        if (Input.GetKey(KeyCode.I))
        {
            movement.z = 1;
        }
        if (Input.GetKey(KeyCode.K))
        {
            movement.z = -1;
        }
        if (Input.GetKey(KeyCode.B))
        {
            speed = originalSpeed * 2f;
        }
        if (Input.GetButton("Shoot"))
        {
            movement.y = 1;
        }
        if (Input.GetButton("Alt Fire"))
        {
            movement.y = -1;
        }

        float num = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime;
        float num2 = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime;
        float desiredX = transform.localRotation.eulerAngles.y + num;
        xRotation -= num2;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);

        if (Input.GetButtonUp("Shoot") || Input.GetButtonUp("Alt Fire"))
        {
            movement.y = 0;
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            speed = originalSpeed;
        }
        if (Input.GetKeyUp(KeyCode.J) || Input.GetKeyUp(KeyCode.L))
        {
            movement.x = 0;
        }
        if (Input.GetKeyUp(KeyCode.I) || Input.GetKeyUp(KeyCode.K))
        {
            movement.z = 0;
        }

    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * movement.z * speed * Time.deltaTime);
        rb.AddForce(transform.right * movement.x * speed * Time.deltaTime);
        rb.AddForce(new Vector3(0, movement.y, 0) * verticalSpeed * Time.deltaTime);
    }
}
