using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewStomp : MonoBehaviour
{
    public float stompForce = 10f;
    public float shockwaveRadius = 5f;
    public float stunDuration = 2f;

    private bool isStomping = false;
    public float originalSpeed;
    public bool isStunned;
    float stunTimer = 2f;
    public PlayerMovement2 grounded;
    Rigidbody rb;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isStomping)
        {
            StartStomp();
        }
    }

    void StartStomp()
    {
        
        isStomping = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Apply downward force to the player
        Rigidbody playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.AddForce(Vector3.down * stompForce, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isStomping)
        {
            isStomping = false;
            if (grounded == true)
            {
                // Stun enemies within the shockwave radius
                StunEnemies(collision.contacts[0].point);
            }
        }
    }

    void StunEnemies(Vector3 position)
    {

        if (gameObject.tag != "Player")
        {
            isStunned = true;
            stunTimer -= Time.deltaTime;
            GetComponent<NavMeshAgent>().speed = 0;

            if(stunTimer == 0)
            {
                isStunned = false;
                GetComponent<NavMeshAgent>().speed = originalSpeed;
            }
        }


    }
}

