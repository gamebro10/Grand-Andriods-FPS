using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BossBlockade : MonoBehaviour
{
    [SerializeField] float horizontalPushBackForce;
    [SerializeField] float verticlePushBackForce;
    [SerializeField] RobotBossAI bossScript;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement script = other.GetComponent<PlayerMovement>();
            IDamage damage = other.GetComponent<IDamage>();
            Rigidbody rb = script.GetRb();
            rb.velocity = horizontalPushBackForce * transform.forward + verticlePushBackForce * transform.up;
            damage.OnTakeDamage(10);
            StartCoroutine(bossScript.DoBlock());
        }
    }
}
