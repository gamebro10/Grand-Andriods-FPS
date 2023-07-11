using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamageArea : MonoBehaviour
{
    [SerializeField] RobotBossAI bossAI;
    [SerializeField] Renderer[] renderers;
    int bulletLayer;
    private void Start()
    {
        bulletLayer = LayerMask.NameToLayer("PlayerBullet");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == bulletLayer)
        {
            OnTakeDamage(10, renderers);//cant access bullet dmg for now just use a constant.
            Destroy(other.gameObject);
        }
    }

    public void OnTakeDamage(int amount, Renderer[] renderers)
    {
        bossAI.OnTakeDamage(amount, renderers);
    }
}
