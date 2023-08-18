using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : BulletBase
{
    [SerializeField] float laserDotRate;

    bool isDamaging;
    protected override void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        IDamage damagable = other.GetComponent<IDamage>();
        if (damagable != null)
        {
            StartCoroutine(DealDamage(damagable));
        }
    }

    IEnumerator DealDamage(IDamage damagable)
    {
        if (!isDamaging)
        {
            isDamaging = true;
            damagable.OnTakeDamage(damage);
            yield return new WaitForSeconds(laserDotRate);
            isDamaging = false;
        }
    }

    private void OnDisable()
    {
        isDamaging = false;
    }
}
