using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class swingsword : MonoBehaviour
{

    [SerializeField] GameObject hitbox;
    [SerializeField] Animator effects;
    [SerializeField] GameObject trail;
    [SerializeField] GameObject Sword;
    [SerializeField] Transform hand;
    [Range(1, 10)][SerializeField] int swingdmg;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Shoot"))
        {
            StartCoroutine(OnEffectPlay());
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(quickslash());
        }

    }

    IEnumerator OnEffectPlay()
    {
        hitbox.SetActive(true);
        trail.SetActive(true);
        effects.SetTrigger("attacking");
        yield return new WaitForSeconds(1f);
        trail.SetActive(false);
        hitbox.SetActive(false);


    }

    public IEnumerator quickslash()
    {
        hitbox.SetActive(true);
        trail.SetActive(true);
        //WeaponTurnOff();

        effects.SetTrigger("quickslash");
        yield return new WaitForSeconds(1.5f);
        trail.SetActive(false);
        hitbox.SetActive(false);

        //WeaponTurnOn();
    }

    void WeaponTurnOff()
    {
        if (hand.GetChild(0).gameObject.activeInHierarchy && transform.childCount >= 1)
        {
            hand.GetChild(0).gameObject.SetActive(false);
        }
        else if (hand.GetChild(1).gameObject.activeInHierarchy && transform.childCount >= 1)
        {
            hand.GetChild(1).gameObject.SetActive(false);

        }
        else if (hand.GetChild(2).gameObject.activeInHierarchy && transform.childCount >= 3)
        {
            hand.GetChild(2).gameObject.SetActive(false);
        }
    }
    
    void WeaponTurnOn()
    {
        if (!hand.GetChild(0).gameObject.activeInHierarchy && transform.childCount >= 1)
        {
            hand.GetChild(0).gameObject.SetActive(true);
        }
        else if (!hand.GetChild(1).gameObject.activeInHierarchy && transform.childCount >= 2)
        {
            hand.GetChild(1).gameObject.SetActive(true);
        }
        else if (!hand.GetChild(2).gameObject.activeInHierarchy && transform.childCount >= 3)
        {
            hand.GetChild(2).gameObject.SetActive(true);
        }
    }

     private void OnTriggerEnter(Collider other)
     {
         //hitparticle.Play();
     
     
     
         if (!other.isTrigger)
         {
     
             //  hitparticle.transform.parent = null;
     
             IDamage damageable = other.GetComponent<IDamage>();
     
             if (damageable != null && !other.CompareTag("Player"))
             {
                 damageable.OnTakeDamage(swingdmg);
             }
     
     
     
             Destroy(gameObject);
         }
     
         // hitparticle.Stop();
     }
}
