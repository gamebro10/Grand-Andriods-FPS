using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyBase[] enemies;
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            for (int i = 0; i < enemies.Length; i++)
            {

            }
            Destroy(gameObject);
        }
    }
}
