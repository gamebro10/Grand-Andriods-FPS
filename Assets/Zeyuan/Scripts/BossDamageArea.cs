using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamageArea : MonoBehaviour, IDamage
{
    [SerializeField] RobotBossAI bossAI;
    [SerializeField] Renderer[] renderers;
    [SerializeField] float damageMultiplier = 1;

    //int bulletLayer;

    //private void Start()
    //{
    //    bulletLayer = LayerMask.NameToLayer("PlayerBullet");
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.layer == bulletLayer)
    //    {
    //        OnTakeDamage(10, renderers);//cant access bullet dmg for now just use a constant.
    //        Destroy(other.gameObject);
    //    }
    //}

    public void OnTakeDamage(float amount, Renderer[] renderers)
    {
        bossAI.OnTakeDamage(amount * damageMultiplier, renderers);
    }

    public void OnTakeDamage(int amount)
    {
        OnTakeDamage(amount, renderers);
    }
}
