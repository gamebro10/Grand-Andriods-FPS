using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthPickup : MonoBehaviour
{
    [Header("-----Player Values-----")]
    [SerializeField] int healedHP;

    private void Update()
    {
        transform.Rotate(0, Time.deltaTime * 15, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.Instance.playerScript.OnTakeDamage(healedHP);
            Destroy(gameObject);
        }
    }
}
