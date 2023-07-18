using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BossScene.Instance.ToDestination();
            BossScene.Instance.playerOnPlatform = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BossScene.Instance.SendDownPlatform();
            BossScene.Instance.playerOnPlatform = false;
        }
    }
}
