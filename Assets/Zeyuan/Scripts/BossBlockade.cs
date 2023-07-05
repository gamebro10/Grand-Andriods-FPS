using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBlockade : MonoBehaviour
{
    [SerializeField] Vector3 pushBackForce;
    [SerializeField] RobotBossAI bossScript;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement script = other.GetComponent<PlayerMovement>();
            Rigidbody rb = script.GetRb();
            rb.velocity = pushBackForce;
            StartCoroutine(bossScript.DoBlock());
        }
    }
}
