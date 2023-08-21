using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class shootgunbehavior : MonoBehaviour
{
    [Header("----- Weapon Stats -----")]
    //[SerializeField] GameObject WeaponModel;
    [SerializeField] float BulletDelay;
    [SerializeField] int ShootDmg;
    [SerializeField] int ShootRate;
    [SerializeField] float ShootDistance;
    [SerializeField] Transform shotpos;
    [SerializeField] Transform shotposleft;
    [SerializeField] Transform shotposright;
    [SerializeField] Transform shotposup;
    [SerializeField] Transform shotposdown;
    Transform cam;
    [SerializeField] GameObject Amo;
    [SerializeField] ParticleSystem shootparticle;
    [SerializeField] Animator kickbackania;
    [SerializeField] Gunholstering hand;
    [SerializeField] float KnockBackForce;
    public swingsword sword;
    public bool isShooting;
    public static bool enablePickup = true;
    //PlayerMovement2 playerMovementdos;

    [Header("---- Weapon Audio -----")]
    [SerializeField] AudioClip shootSound;
    [Range(0, 5)] public float Volume = 2f;
    public AudioSource shootSoundSource;

   
    // Start is called before the first frame update
    void Start()
    {
        enablePickup = true;
        cam = UnityEngine.Camera.main.transform;
        AudioManager.Instance.RegisterSFX(shootSoundSource);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.activeMenu == null)
        {
            if (Input.GetButton("Shoot") && !isShooting)
            { 
                StartCoroutine(shoot());
            }
        }


        //if (Input.GetKeyDown(KeyCode.F) && !isShooting)
        //{
        //    // swingsword sword = Sword.GetComponent<swingsword>();

        //    sword.gameObject.SetActive(true);
        //    sword.slashswitch();
        //    gameObject.SetActive(false);
        //}
    }

    IEnumerator KickbackAnimation()
    {
        kickbackania.SetTrigger("firing");
        yield return new WaitForSeconds(1f);
    }



    IEnumerator shoot()
    {
        hand.canSwitchWeapons = false;
        WeaponBehavior.enablePickup = false;
        isShooting = true;

        ShotAudio(shootSoundSource);

        //playerMovementdos.GetRb().AddForce(-cam.transform.forward * KnockBackForce, ForceMode.Impulse);
        GameManager.Instance.playerMovement.GetRb().AddForce(-cam.transform.forward * KnockBackForce, ForceMode.Impulse);

        if (!shootparticle.isPlaying)
        { shootparticle.Play(); }


        //Debug.Log("Shoot");
        Instantiate(Amo, shotpos.position, shotpos.transform.rotation);
        Instantiate(Amo, shotposup.position, shotposup.transform.rotation);
        Instantiate(Amo, shotposdown.position, shotposdown.transform.rotation);
        Instantiate(Amo, shotposright.position, shotposright.transform.rotation);
        Instantiate(Amo, shotposleft.position, shotposleft.transform.rotation);

        StartCoroutine(KickbackAnimation());

        

        yield return new WaitForSeconds(BulletDelay);
        isShooting = false;
        hand.canSwitchWeapons = true;
        WeaponBehavior.enablePickup = true;
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
