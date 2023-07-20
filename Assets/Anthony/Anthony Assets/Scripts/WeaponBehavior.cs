using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class WeaponBehavior : MonoBehaviour
{

    [Header("----- Weapon Stats -----")]
    //[SerializeField] GameObject WeaponModel;
    [SerializeField] float BulletDelay;
    [SerializeField] int ShootDmg;
    [SerializeField] int ShootRate;
    [SerializeField] int ShootDistance;
    [SerializeField] Transform shotpos;
    [SerializeField] GameObject Amo;
    [SerializeField] ParticleSystem shootparticle;
    [SerializeField] Gunholstering hand;
    //[SerializeField] List<Gunstats> gunList = new List<Gunstats>();
   // int selectedGun;
    public bool isShooting;
    public static bool enablePickup = true;


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
                if (Input.GetButton("Shoot") && !isShooting)
                    StartCoroutine(shoot());
        }
    }


    IEnumerator shoot()
    {
        isShooting = true;
        hand.canSwitchWeapons = false;
        WeaponBehavior.enablePickup = false;
        //RaycastHit hit;
        //Ray ray = (Physics.Raycast(Camera, out hit, ShootDistance));

        if (!shootparticle.isPlaying)
        { shootparticle.Play(); }


        Debug.Log("Shoot");
        Instantiate(Amo, shotpos.position, shotpos.transform.rotation);

        

        yield return new WaitForSeconds(BulletDelay);
        isShooting = false;
        hand.canSwitchWeapons = true;
        WeaponBehavior.enablePickup = true;
    }
    //public void GunPickup(Gunstats weaponstats)
    //{
    //    gunList.Add(weaponstats);

    //    ShootDmg = weaponstats.shootdmg;
    //    ShootDistance = weaponstats.shootdistance;
    //    ShootRate = (int)weaponstats.shotRate;

    //    WeaponModel.GetComponent<MeshFilter>().mesh = weaponstats.model.GetComponent<MeshFilter>().sharedMesh;
    //    WeaponModel.GetComponent<MeshRenderer>().material = weaponstats.model.GetComponent<MeshRenderer>().sharedMaterial;

    //    selectedGun = gunList.Count - 1;
    //}

    //void scrollGun()
    //{
    //    if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunList.Count - 1)
    //    {
    //        selectedGun++;
    //        changeGunstats();
    //    }
    //    else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
    //    {
    //        selectedGun--;
    //        changeGunstats();
    //    }

    //}
    //void changeGunstats()
    //{
    //    ShootDmg = gunList[selectedGun].shootdmg;
    //    ShootDistance = gunList[selectedGun].shootdistance;
    //    ShootRate = (int)gunList[selectedGun].shotRate;


    //    WeaponModel.GetComponent<MeshFilter>().mesh = gunList[selectedGun].model.GetComponent<MeshFilter>().sharedMesh;
    //    WeaponModel.GetComponent<MeshRenderer>().material = gunList[selectedGun].model.GetComponent<MeshRenderer>().sharedMaterial;
    //}
}
