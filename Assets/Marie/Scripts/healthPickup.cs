using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthPickup : MonoBehaviour
{
    [Header("-----Player Values-----")]
    [SerializeField] int healedHP;
    float floatAmount;

    private void Update()
    {
        if (!GameManager.Instance.isPaused)
        {
            float pos = Mathf.Sin(floatAmount) * Mathf.PI / 180 * .2f;
            floatAmount += Time.deltaTime * 4;
            transform.position += new Vector3(0, pos, 0);
            transform.Rotate(new Vector3(0, Time.deltaTime, 0) * 40, Space.World);
        }
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
