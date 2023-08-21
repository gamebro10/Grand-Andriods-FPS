using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCombat : MonoBehaviour
{
    [Header("----- Weapon Stats -----")]
    //[SerializeField] GameObject WeaponModel;
    [SerializeField] float slashdelay;
    [SerializeField] int swingdmg;
    [SerializeField] int swingspeed;
    //[SerializeField] List<Gunstats> gunList = new List<Gunstats>();
    // int selectedGun;
    bool Attacking;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.activeMenu == null)
        {
            //if (gunList.Count > 0)
            //{
            //    scrollGun();

            //isshooting is after cuz order of op and it will almost always be false

            if (Input.GetButton("Shoot") && !Attacking)
                StartCoroutine(shoot());
        }
    }



    IEnumerator shoot()
    {
        Attacking = true;

        //if (!shootparticle.isPlaying)
        //{ shootparticle.Play(); }


        //Debug.Log("Shoot");
        //Instantiate(Amo, shotpos.position, shotpos.transform.rotation);



        yield return new WaitForSeconds(slashdelay);
        Attacking = false;
    }

}