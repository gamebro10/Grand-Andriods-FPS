using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float flySpeed;
    [SerializeField] float rotateSpeed;
    [SerializeField] float lifeTime;


    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.player;
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * flySpeed;
        FaceToPlayer();
    }

    void FaceToPlayer()
    {
        Vector3 playerDir = player.transform.position - transform.position;
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, playerDir.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * rotateSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                IDamage idamage = other.GetComponent<IDamage>();
                idamage.OnTakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
