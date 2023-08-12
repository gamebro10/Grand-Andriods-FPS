using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawnTrigger : MonoBehaviour
{
    public GameObject spawnPos;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.Instance.playerSpawnPos = spawnPos;
        }
    }
}
