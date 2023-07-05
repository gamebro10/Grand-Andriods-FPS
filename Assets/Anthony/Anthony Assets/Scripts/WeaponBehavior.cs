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
    [SerializeField] ParticleSystem particle;
    bool isShooting;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Shoot") && !isShooting)
            StartCoroutine(shoot());
    }

    IEnumerator shoot()
    {
        isShooting = true;
        particle.Play();
        Instantiate(Amo, shotpos.position, shotpos.transform.rotation);
        RaycastHit hit;

        if (Physics.Raycast(UnityEngine.Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, ShootDistance)) {
            IDamage Damageable = hit.collider.GetComponent<IDamage>();
            if (Damageable != null)
            {
                Damageable.OnTakeDamage(ShootDmg);
            }
        }
            
        
        particle.Stop();
        yield return new WaitForSeconds(ShootRate);
        isShooting = false;
    }
}
