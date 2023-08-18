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
    [SerializeField] int ShootDistance;
    [SerializeField] Transform shotpos;
    [SerializeField] GameObject Amo;
    [SerializeField] ParticleSystem shootparticle;
    [SerializeField] Gunholstering hand;
    [SerializeField] LayerMask Mask;
    public bool isShooting;
    public static bool enablePickup = true;

    [Header("---- Weapon Audio -----")]
    [SerializeField] AudioClip shootSound;
    [Range(0, 5)]public float Volume = 2f;
    public AudioSource shootSoundSource;
   
    

    private void Awake()
    {
        enablePickup = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (hand == null)
        {
            hand = FindObjectOfType<Gunholstering>();
        }
        AudioManager.Instance.RegisterSFX(shootSoundSource);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.activeMenu == null)
        {
                //isshooting is after cuz order of op and it will almost always be false 
                if (Input.GetButton("Shoot") && !isShooting)
                    StartCoroutine(shoot());

            if (hand.canSwitchWeapons == true && isShooting == true)
            {
                isShooting = false;
            }
        }

    }


    IEnumerator shoot()
    {
        isShooting = true;
        hand.canSwitchWeapons = false;
        enablePickup = false;
        RaycastHit hit;

        ShotAudio(shootSoundSource);

        if (Physics.Raycast(UnityEngine.Camera.main.transform.position, UnityEngine.Camera.main.transform.forward, out hit, 1000f, Mask))
        {
            GameObject Bullet = Instantiate(Amo, shotpos.position, shotpos.transform.rotation);
            Bullet.transform.LookAt(hit.point);
        }
        else
        {
            Instantiate(Amo, shotpos.position, shotpos.transform.rotation);
        }
        
            if (!shootparticle.isPlaying)
        { shootparticle.Play(); }


        //Debug.Log("Shoot");
      //  Instantiate(Amo, shotpos.position, shotpos.transform.rotation);
      
        

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
