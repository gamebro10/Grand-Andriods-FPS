using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.WSA;

public class swingsword : MonoBehaviour
{

    [SerializeField] GameObject hitbox;
    [SerializeField] Animator effects;
    [SerializeField] GameObject trail;
    [SerializeField] GameObject Sword;
    [SerializeField] Transform hand;
    [SerializeField] Transform shootPos;
    [Range(1, 10)][SerializeField] int swingdmg;
    [SerializeField] GameObject Perjectile;

    [SerializeField] Gunholstering scripts;
    public bool canSlash = true;
    public bool allowSlash = true;

    [Header("---- Weapon Audio -----")]
    [SerializeField] AudioClip shootSound;
    [Range(0, 5)] public float Volume = 2f;
    public AudioSource shootSoundSource;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.RegisterSFX(shootSoundSource);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Shoot"))
        {
            StartCoroutine(OnEffectPlay());
        }

        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    StartCoroutine(quickslash());
        //}

        if (Input.GetButtonDown("Alt Fire") && allowSlash == true)
        {
            StartCoroutine(rangeattack());
        }

    }

    IEnumerator OnEffectPlay()
    {
        if (canSlash)
        {
            scripts.canSwitchWeapons = false;
            canSlash = false;
            WeaponBehavior.enablePickup = false;
            string[] animations = { "attacking", "quickslash", "forwardslash"};
            hitbox.SetActive(true);
            trail.SetActive(true);
            ShotAudio(shootSoundSource);
            effects.SetTrigger(animations[Random.Range(0, 3)]);
            yield return new WaitForSeconds(1f);
            trail.SetActive(false);
            hitbox.SetActive(false);
            scripts.canSwitchWeapons = true;
            canSlash = true;
            WeaponBehavior.enablePickup = true;
        }
    }

    //public IEnumerator quickslash()
    //{
    //    if (canSlash)
    //    {
    //        scripts.canSwitchWeapons = false;
    //        canSlash = false;
    //        hitbox.SetActive(true);
    //        trail.SetActive(true);
    //        WeaponBehavior.enablePickup = false;
    //        string[] animations = { "attacking", "forwardslash" };
    //        effects.SetTrigger(animations[Random.Range(0, 2)]);
    //        yield return new WaitForSeconds(.5f);
    //        trail.SetActive(false);
    //        hitbox.SetActive(false);
    //        canSlash = true;
    //        WeaponBehavior.enablePickup = true;
    //    }

    //    //WeaponTurnOn();
    //}

    public void slashswitch()
    {
        //StartCoroutine(quickslash());
        //StartCoroutine(SwitchBack());
    }

    IEnumerator SwitchBack()
    {
        yield return new WaitForSeconds(.5f);
        scripts.canSwitchWeapons = true;
        canSlash = true;
        hand.GetChild(scripts.CurrentWeopon).gameObject.SetActive(true);
        WeaponBehavior.enablePickup = true;
        gameObject.SetActive(false);
    }

    IEnumerator rangeattack()
    {
        scripts.canSwitchWeapons = false;
        canSlash = false;
        WeaponBehavior.enablePickup = false;
        ShotAudio(shootSoundSource);
        effects.SetTrigger("launch");
        StartCoroutine(launchswing());
       // Instantiate(Perjectile, shootPos.position, shootPos.rotation);
        allowSlash = false;
        

        yield return new WaitForSeconds(1f);
        scripts.canSwitchWeapons = true;
        canSlash = true;
        WeaponBehavior.enablePickup = true;
        allowSlash = true;
    }

    IEnumerator launchswing()
    {

        yield return new WaitForSeconds(.2f);
        ShotAudio(shootSoundSource);
        Instantiate(Perjectile, shootPos.position, shootPos.rotation);

    }

    //void WeaponTurnOff()
    //{
    //    if (hand.GetChild(0).gameObject.activeInHierarchy && transform.childCount >= 1)
    //    {
    //        hand.GetChild(0).gameObject.SetActive(false);
    //    }
    //    else if (hand.GetChild(1).gameObject.activeInHierarchy && transform.childCount >= 2)
    //    {
    //        hand.GetChild(1).gameObject.SetActive(false);

    //    }
    //    else if (hand.GetChild(2).gameObject.activeInHierarchy && transform.childCount >= 3)
    //    {
    //        hand.GetChild(2).gameObject.SetActive(false);
    //    }
    //}

    //void WeaponTurnOn()
    //{
    //    if (!hand.GetChild(0).gameObject.activeInHierarchy && transform.childCount >= 1)
    //    {
    //        hand.GetChild(0).gameObject.SetActive(true);
    //    }
    //    else if (!hand.GetChild(1).gameObject.activeInHierarchy && transform.childCount >= 2)
    //    {
    //        hand.GetChild(1).gameObject.SetActive(true);
    //    }
    //    else if (!hand.GetChild(2).gameObject.activeInHierarchy && transform.childCount >= 3)
    //    {
    //        hand.GetChild(2).gameObject.SetActive(true);
    //    }
    //}

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

    public void ShotAudio(AudioSource clip)
    {
        clip.PlayOneShot(shootSound, Volume);
    }

    private void OnDestroy()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.UnregisterSFX(shootSoundSource);
        }
    }
}
