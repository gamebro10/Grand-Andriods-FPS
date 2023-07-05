using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected float destroyTimer;
    [SerializeField] protected int damage;

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

    protected virtual void OnTriggerEnter(Collider other)
    {
        IDamage damagable = other.GetComponent<IDamage>();
        if (damagable != null)
        {
            damagable.OnTakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
