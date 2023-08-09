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
    [SerializeField] GameObject Amo;
    [SerializeField] ParticleSystem shootparticle;
    [SerializeField] Animator kickbackania;
    [SerializeField] Gunholstering hand;
    [SerializeField] float KnockBackForce;
    public swingsword sword;

    //[SerializeField] List<Gunstats> gunList = new List<Gunstats>();
    // int selectedGun;
    public bool isShooting;
    public static bool enablePickup = true;



    // Start is called before the first frame update
    void Start()
    {
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

        if (Input.GetKeyDown(KeyCode.F) && !isShooting)
        {
            // swingsword sword = Sword.GetComponent<swingsword>();

            sword.gameObject.SetActive(true);
            sword.slashswitch();
            gameObject.SetActive(false);
        }
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

        //RaycastHit hit;

        //Physics.Raycast(, out hit)

        GameManager.Instance.playerMovement.GetRb().AddForce(-UnityEngine.Camera.main.transform.forward * KnockBackForce, ForceMode.Impulse);

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
}
