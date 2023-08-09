using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawnTrigger : MonoBehaviour
{
    public GameObject spawnPos;
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.playerSpawnPos = spawnPos;
    }
}
