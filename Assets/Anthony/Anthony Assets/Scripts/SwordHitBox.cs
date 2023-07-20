using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitBox : MonoBehaviour
{
    [SerializeField] int swingdmg;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {

            //  hitparticle.transform.parent = null;

            IDamage damageable = other.GetComponent<IDamage>();

            if (damageable != null && !other.CompareTag("Player"))
            {
                damageable.OnTakeDamage(swingdmg);
            }



            //Destroy(gameObject);
        }
    }
}
