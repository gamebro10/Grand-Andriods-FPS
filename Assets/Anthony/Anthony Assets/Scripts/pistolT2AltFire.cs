using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pistolT2AltFire : MonoBehaviour
{
    [Header("----- Weapon Stats -----")]
    //[SerializeField] GameObject WeaponModel;
    [SerializeField] float BulletDelay;
    [SerializeField] int ShootDmg;
    [SerializeField] int ShootDistance;
    [SerializeField] Transform shotposLeft, shotposRight, shotposUPMid;
    [SerializeField] GameObject Amo;
    [SerializeField] ParticleSystem shootparticle;
    [SerializeField] Gunholstering hand;
    [SerializeField] Animator AltFireKick;
    public bool isShooting;
    public static bool enablePickup = true;
    pistolInteract pistol;


    // Start is called before the first frame update
    void Start()
    {
        if (hand == null)
        {
            hand = FindObjectOfType<Gunholstering>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.activeMenu == null)
        {
            //isshooting is after cuz order of op and it will almost always be false 
            if (Input.GetButtonDown("Alt Fire") && !isShooting)
                StartCoroutine(ALTshoot());
        }
    }

    IEnumerator AltFire()
    {
        AltFireKick.SetTrigger("Alt firing");
        yield return new WaitForSeconds(.5f);
    }

    IEnumerator ALTshoot()
    {
        isShooting = true;
        hand.canSwitchWeapons = false;
        enablePickup = false;
        //RaycastHit hit;
        //Ray ray = (Physics.Raycast(Camera, out hit, ShootDistance));

        if (!shootparticle.isPlaying)
        { shootparticle.Play(); }


        //Debug.Log("Shoot");
        Instantiate(Amo, shotposLeft.position, shotposLeft.transform.rotation);
        Instantiate(Amo, shotposRight.position, shotposRight.transform.rotation);
        Instantiate(Amo, shotposUPMid.position, shotposUPMid.transform.rotation);

        StartCoroutine(AltFire());


        yield return new WaitForSeconds(BulletDelay);
        isShooting = false;
        hand.canSwitchWeapons = true;
        WeaponBehavior.enablePickup = true;
    }
}
