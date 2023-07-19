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

    [SerializeField] Gunholstering scripts;
    public bool canSlash = true;
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
        if (canSlash)
        {
            scripts.canSwitchWeapons = false;
            canSlash = false;
            string[] animations = { "attacking", "quickslash" };
            hitbox.SetActive(true);
            trail.SetActive(true);
            effects.SetTrigger(animations[Random.Range(0, 2)]);
            yield return new WaitForSeconds(1f);
            trail.SetActive(false);
            hitbox.SetActive(false);
            scripts.canSwitchWeapons = true;
            canSlash = true;
            WeaponBehavior.enablePickup = false;
        }
    }

    public IEnumerator quickslash()
    {
        if (canSlash)
        {
            scripts.canSwitchWeapons = false;
            canSlash = false;
            hitbox.SetActive(true);
            trail.SetActive(true);
            WeaponBehavior.enablePickup = false;
            //WeaponTurnOff();

            effects.SetTrigger("attacking");
            yield return new WaitForSeconds(1.5f);
            trail.SetActive(false);
            hitbox.SetActive(false);
            canSlash = true;
            WeaponBehavior.enablePickup = false;
        }

        //WeaponTurnOn();
    }

    public void slashswitch()
    {
        StartCoroutine(quickslash());
        StartCoroutine(SwitchBack());
    }

    IEnumerator SwitchBack()
    {
        yield return new WaitForSeconds(1.1f);
        scripts.canSwitchWeapons = true;
        canSlash = true;
        hand.GetChild(scripts.CurrentWeopon).gameObject.SetActive(true);
        WeaponBehavior.enablePickup = true;
        gameObject.SetActive(false);
    }

    void WeaponTurnOff()
    {
        if (hand.GetChild(0).gameObject.activeInHierarchy && transform.childCount >= 1)
        {
            hand.GetChild(0).gameObject.SetActive(false);
        }
        else if (hand.GetChild(1).gameObject.activeInHierarchy && transform.childCount >= 2)
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
