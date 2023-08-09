using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthPickup : MonoBehaviour
{
    [Header("-----Player Values-----")]
    [SerializeField] int healedHP;

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.playerScript.OnTakeDamage(healedHP);
        Destroy(gameObject);
    }
}
