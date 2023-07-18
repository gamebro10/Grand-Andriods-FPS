using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swingsword : MonoBehaviour
{

    [SerializeField] GameObject hitbox;
    [SerializeField] Animator effects;
    [SerializeField] GameObject trail;
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
