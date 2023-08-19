using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{

    //[Header("--- Weapon Pos settings ---")]
    private Vector3 startingPoS;
    private Vector3 endPoS;

    [Header("--- Weapon Recoil settings ---")]
    [SerializeField] float x_WeaponRecoil;
    [SerializeField] float y_WeaponRecoil;
    [SerializeField] float z_WeaponRecoil;

    [SerializeField] float flickspeed;
    [SerializeField] float resetposSpeed;

    // Update is called once per frame
    void Update()
    {
        endPoS = Vector3.Lerp(endPoS, Vector3.zero, resetposSpeed * Time.deltaTime);
        startingPoS = Vector3.Slerp(startingPoS, endPoS, flickspeed * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(startingPoS);
    }

    public void playRecoil()
    {
        endPoS += new Vector3(x_WeaponRecoil, Random.Range(-y_WeaponRecoil, y_WeaponRecoil), Random.Range(-x_WeaponRecoil, x_WeaponRecoil));
    }
}
