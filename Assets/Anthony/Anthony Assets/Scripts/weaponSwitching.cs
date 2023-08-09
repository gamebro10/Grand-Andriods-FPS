using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponSwitching : MonoBehaviour
{
    public SwordCombat melee;
    public Rigidbody body;
    public BoxCollider coll;
    public Transform player, holder, cam;

    public float pickupdistance;
    public float dropforceforward, dropforcebackward;

    public bool equiped;
    public static bool Maxedslots;

    // Start is called before the first frame update
    void Start()
    {
        if (!equiped)
        {
            melee.enabled = false;
            body.isKinematic = false;
            coll.isTrigger = false;
        }
        if (equiped)
        {
            melee.enabled = true;
            body.isKinematic = true;
            coll.isTrigger = true;
            Maxedslots = true;
        }

    }
    //Vector3(357.804993,91.2942963,356.745026);
    // Update is called once per frame
    private void Update()
    {
        Vector3 distfromplayer = player.position - transform.position;
        if (!equiped && distfromplayer.magnitude <= pickupdistance && Input.GetKeyDown(KeyCode.E) && !Maxedslots)
            pickup();

        if (equiped && Input.GetKeyDown(KeyCode.Q))
            drop();
    }

    private void pickup()
    {
        equiped = true;
        Maxedslots = true;

        body.isKinematic = true;
        coll.isTrigger = true;

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        melee.enabled = true;

    }

    private void drop()
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
        body.AddForce(cam.forward * dropforceforward, ForceMode.Impulse);

        //this code makes it so when the weapon is thrown it will spin at a random speed between -1 to 1
        float spin = Random.Range(-1f, 1f);
        body.AddTorque(new Vector3(spin, spin, spin) * 10);

        melee.enabled = false;
    }
}
