using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class WeaponBehavior : MonoBehaviour
{

    [Header("----- Weapon Stats -----")]
    [SerializeField] float ShootRate;
    [SerializeField] int ShootDmg;
    [SerializeField] int ShootDistance;
    [SerializeField] Transform shotpos;
    [SerializeField] GameObject Amo;
    [SerializeField] ParticleSystem shootparticle;
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
            //to move player rotations
            if (Input.GetButton("Shoot") && !isShooting)
                StartCoroutine(shoot());
        }
       
    }

    IEnumerator shoot()
    {
        isShooting = true;

        if (!shootparticle.isPlaying)
        { shootparticle.Play(); }


        Debug.Log("Shoot");
        Instantiate(Amo, shotpos.position, shotpos.transform.rotation);

        

        yield return new WaitForSeconds(ShootRate);
        isShooting = false;
    }
}
