using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected float destroyTimer;
    [SerializeField] protected int damage;
    [SerializeField] protected LayerMask mask;

    Rigidbody rb;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = transform.forward * bulletSpeed;
            Destroy(gameObject, destroyTimer);
        }
    }

    //protected virtual void Update()
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(transform.position, transform.forward, out hit, 1f, mask))
    //    {
    //        if (hit.collider != null)
    //        {
    //            if (hit.collider.CompareTag("Player"))
    //            {
    //                hit.collider.gameObject.GetComponent<IDamage>().OnTakeDamage(damage);
    //            }
    //            Destroy(gameObject);
    //        }
            
    //    }
    //}

    protected virtual void OnTriggerEnter(Collider other)
    {
        IDamage damagable = other.GetComponent<IDamage>();
        if (!other.isTrigger)
        {
            if (damagable != null)
            {
                damagable.OnTakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
