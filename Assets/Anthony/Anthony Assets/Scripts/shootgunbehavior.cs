using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    swingsword sword;
    //[SerializeField] List<Gunstats> gunList = new List<Gunstats>();
    // int selectedGun;
    bool isShooting;


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

        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(Melee());
        }
    }

    IEnumerator KickbackAnimation()
    {
        kickbackania.SetTrigger("firing");
        yield return new WaitForSeconds(1f);
    }

    IEnumerator Melee()
    {
        gameObject.SetActive(false);
        //sword.quickslash();
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(true);
    }


    IEnumerator shoot()
    {
        isShooting = true;

        //RaycastHit hit;

        //Physics.Raycast(, out hit)

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
    }
}
