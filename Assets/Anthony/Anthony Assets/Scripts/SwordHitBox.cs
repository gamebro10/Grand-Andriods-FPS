using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitBox : MonoBehaviour
{
    [SerializeField] int swingdmg;
    [SerializeField] int wallKnockBack;
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

            //if ()
            //{
            //    GameManager.Instance.playerMovement.GetRb().AddForce(-UnityEngine.Camera.main.transform.forward * wallKnockBack, ForceMode.Impulse);
            //}

            //Destroy(gameObject);
        }
    }
}
