using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    // to make sure no collison and give velocity
    [Header("----- Components -----")]
    [SerializeField] Rigidbody rb;

  

    [Header("----- Weapon Stats -----")]
    [Range(1,10)] [SerializeField] int destroytime;
    [Range(1,100)] [SerializeField] int speed;
    [Range(1,10)] [SerializeField] int ShootDmg;

    private SphereCollider forcefield;
   // [SerializeField] ParticleSystem hitparticle;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroytime);
        rb.velocity = transform.forward * speed;
    }

    // the bullets flys thru and if can break it it will destroy it 
    private void OnTriggerEnter(Collider other)
    {
            //hitparticle.Play();



        if (!other.isTrigger) {

          //  hitparticle.transform.parent = null;

            IDamage damageable = other.GetComponent<IDamage>();

            if (damageable != null && !other.CompareTag("Player"))
            {
                damageable.OnTakeDamage(ShootDmg);
            }

            

            Destroy(gameObject);
        }
        
       // hitparticle.Stop();
    }
 
}
