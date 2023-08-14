using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class Weaponiteract : MonoBehaviour
{
    Gunholstering currWeapon;
    public WeaponBehavior behavior;
    public Rigidbody body;
    public BoxCollider coll;
    public Transform player, holder;
    Transform cam;
    //[SerializeField] Transform Weapon;
    
    public float pickupdistance;
    public float dropforceforward, dropforcebackward;
    public GameObject Sword;

    public bool equiped;
    public static bool Maxedslots;
    //swingsword sword;

    // [SerializeField] GameObject Currgun;

    // Start is called before the first frame update
    private void Awake()
    {
        Maxedslots = false;
        cam = UnityEngine.Camera.main.transform;
    }

    void Start()
    {
        if(!equiped)
        {
            behavior.enabled = false;
            body.isKinematic = false;
            coll.isTrigger = false;
        }
        if (equiped)
        {
            behavior.enabled = true;
            body.isKinematic = true;
            coll.isTrigger = true;
            Maxedslots = true;
        }

    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 distfromplayer = player.position - transform.position;
        if (!equiped && distfromplayer.magnitude <= pickupdistance && Input.GetKeyDown(KeyCode.E) && !Maxedslots && WeaponBehavior.enablePickup)
            Pickup();

        if (equiped && Input.GetKeyDown(KeyCode.Q))
            Drop();

        //if (Input.GetKeyDown(KeyCode.F))
        //    StartCoroutine(Melee());

        if (Input.GetKeyDown(KeyCode.F) && equiped && !behavior.isShooting)
        {
           swingsword sword = Sword.GetComponent<swingsword>();

            Sword.SetActive(true);
            sword.slashswitch();
            gameObject.SetActive(false);
           // Sword.SetActive(false);
        }

    }

    public void Pickup()
    {
        if (holder.GetChild(0).gameObject.activeInHierarchy && holder.transform.childCount >= 1)
        {
            holder.GetChild(0).gameObject.SetActive(false);
        }
        else if (holder.GetChild(1).gameObject.activeInHierarchy && holder.transform.childCount >= 2)
        {
            holder.GetChild(1).gameObject.SetActive(false);
        }
        else if (holder.GetChild(2).gameObject.activeInHierarchy && holder.transform.childCount >= 3)
        {
            holder.GetChild(2).gameObject.SetActive(false);
        }
        else if (holder.GetChild(3).gameObject.activeInHierarchy && holder.transform.childCount >= 4)
        {
            holder.GetChild(3).gameObject.SetActive(false);
        }
      
        
        
        equiped = true;
        Maxedslots = true;

        body.isKinematic = true;
        coll.isTrigger = true;

        transform.SetParent(holder);
        transform.SetLocalPositionAndRotation(new Vector3(0, 0, 1), Quaternion.Euler((float).527, -90, (float).005));
        //transform.SetPositionAndRotation(pickup.position, pickup.rotation);
        transform.localScale = Vector3.one;

        behavior.enabled = true;
        holder.GetComponent<Gunholstering>().CurrentWeopon = holder.childCount - 1;
    }

    private void Drop()
    {
        equiped = false;
        Maxedslots = false;

        transform.SetParent(null);

        body.isKinematic = false;
        coll.isTrigger = false;

        //this code makes it so that when a player throws the current weapon it will have the same that the player currently
        //has and it will amke it shoot away from the player 
        body.velocity = player.GetComponent<Rigidbody>().velocity;
        body.AddForce(cam.forward * dropforceforward, ForceMode.Impulse);
        body.AddForce(cam.forward * dropforcebackward, ForceMode.Impulse);

        //this code makes it so when the weapon is thrown it will spin at a random speed between -1 to 1
        float spin = Random.Range(-1f, 1f);
        body.AddTorque(new Vector3(spin, spin, spin) * 10);

        behavior.enabled = false;
        holder.GetComponent<Gunholstering>().CurrentWeopon = holder.childCount - 1;
        holder.GetComponent<Gunholstering>().IDweapon();
    }

}
