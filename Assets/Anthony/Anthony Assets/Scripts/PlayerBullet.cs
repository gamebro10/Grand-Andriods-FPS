using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    // to make sure no collison and give velocity
    [Header("----- Components -----")]
    [SerializeField] Rigidbody rb;

    // no delta time because theres no update
    //[Header("----- Stats -----")]
    //[Range(1,10)][SerializeField] int damage;
    //[Range(1, 100)][SerializeField] int speed;

    [Header("----- Weapon Stats -----")]
    [Range(1,10)] [SerializeField] int destroytime;
    [Range(1,100)] [SerializeField] int ShootRate;
    [Range(1,10)] [SerializeField] int ShootDmg;

    private SphereCollider forcefield;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroytime);
        rb.velocity = transform.forward * ShootRate;
    }

    // the bullets flys thru and if can break it it will destroy it 
    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger) {
            IDamage damageable = other.GetComponent<IDamage>();


            if (damageable != null)
            {
                damageable.OnTakeDamage(ShootDmg);
            }



            Destroy(gameObject);
        }
        
    }
 
}
