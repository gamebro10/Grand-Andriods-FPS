using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class pistolInteract : MonoBehaviour
{
    Gunholstering currWeapon;
    public WeaponBehavior behaviorPT2;
    public Rigidbody body;
    public BoxCollider coll;
    public Transform player, holder;
    Transform cam;


    public float pickupdistance;
    public float dropforceforward, dropforcebackward;
    public GameObject Sword;

    public bool equiped;
    public static bool Maxedslots;
    public bool inter = false;
    swingsword sword;


    [Header("----- Weapon Stats -----")]
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

    [Header("---- Weapon Audio -----")]
    [SerializeField] AudioClip shootSound;
    [Range(0, 5)] public float Volume = 2f;
    public AudioSource shootSoundSource;


    // [SerializeField] GameObject Currgun;

    // Start is called before the first frame update
    private void Awake()
    {
        Maxedslots = false;
        cam = UnityEngine.Camera.main.transform;
        inter = false;
    }

    void Start()
    {
        if (!equiped)
        {
            behaviorPT2.enabled = false;
            body.isKinematic = false;
            coll.isTrigger = false;
            inter = false;
        }
        if (equiped)
        {
            behaviorPT2.enabled = true;
            body.isKinematic = true;
            coll.isTrigger = true;
            Maxedslots = true;
            inter = true;
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

        //if (Input.GetKeyDown(KeyCode.F) && equiped && !behaviorPT2.isShooting)
        //{
        //    swingsword sword = Sword.GetComponent<swingsword>();

        //    Sword.SetActive(true);
        //    sword.slashswitch();
        //    gameObject.SetActive(false);
        //    // Sword.SetActive(false);
        //}

        if (GameManager.Instance.activeMenu == null)
        {
            //isshooting is after cuz order of op and it will almost always be false 
            if (Input.GetButtonDown("Alt Fire") && !isShooting && equiped == true)
                StartCoroutine(ALTshoot());
        }

    }

    public void Pickup()
    {
        // holder.GetChild(1).gameObject.SetActive(false);

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

        if (Maxedslots == false)
        {
            Drop();
            Pickup();
        }

        body.isKinematic = true;
        coll.isTrigger = true;

        transform.SetParent(holder);
        transform.localPosition = new Vector3((float).12, (float).19, (float)-.2);
        transform.localRotation = Quaternion.Euler((float).527, -90, (float).317);
        transform.localScale = Vector3.one;

        //.527  -90  -5.9



        behaviorPT2.enabled = true;
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

        behaviorPT2.enabled = false;
        holder.GetComponent<Gunholstering>().CurrentWeopon = holder.childCount - 1;
        holder.GetComponent<Gunholstering>().IDweapon();
    }

    IEnumerator AltFire()
    {
        inter = false;
        AltFireKick.SetTrigger("Alt firing");
        yield return new WaitForSeconds(1f);
        inter = true;
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

        ShotAudio(shootSoundSource);
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

    //IEnumerator Melee()
    //{
    //    gameObject.SetActive(false);
    //    yield return new WaitForSeconds(1f);
    //    gameObject.SetActive(true);
    //}
}
