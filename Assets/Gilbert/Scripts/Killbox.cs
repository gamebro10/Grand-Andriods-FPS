using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.playerScript.OnTakeDamage(GameManager.Instance.playerScript.HPOrig);
    }
}
