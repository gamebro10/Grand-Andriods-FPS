using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunholstering : MonoBehaviour
{
    Weaponiteract PickedupWeapon;
    public int CurrentWeopon = 0;
    Transform guns;

    public bool canSwitchWeapons;

    // Start is called before the first frame update
    void Start()
    {
        canSwitchWeapons = true;

        for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                CurrentWeopon = i;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.Instance.isPaused) { weaponholstering(); }
        
    }


    public void weaponholstering()
    {
        int lastWeapon = CurrentWeopon;

        if (canSwitchWeapons)
        {
            //this section is for scroll wheel inputs for weapon swaping and the && transform.childCount - 1
            //is so that it switchs there only the weapons u have 
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (CurrentWeopon >= transform.childCount - 1)
                    CurrentWeopon = 0;
                else
                    CurrentWeopon++;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                if (CurrentWeopon <= 0)
                    CurrentWeopon = transform.childCount - 1;
                else
                    CurrentWeopon--;
            }


            //this section is for number key inputs for weapon swaping and the && transform.childCount >= 2,3,4
            //is so that if theres no weapon there it wont switch there
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                CurrentWeopon = 0;
            }
            if (Input.GetKeyUp(KeyCode.Alpha2) && transform.childCount >= 2)
            {
                CurrentWeopon = 1;
            }
            if (Input.GetKeyUp(KeyCode.Alpha3) && transform.childCount >= 3)
            {
                CurrentWeopon = 2;
            }
            if (Input.GetKeyUp(KeyCode.Alpha4) && transform.childCount >= 4)
            {
                CurrentWeopon = 3;
            }
            if (lastWeapon != CurrentWeopon)
            {
                IDweapon();
            }


            //if theres no weapon at the current spot dont set unactive 
        }
    }

    public void IDweapon()
    {
        int t = 0;
        //first weapon will be 0in code the rest goes up by one 
        foreach (Transform gun in transform)
        {
            if (t == CurrentWeopon)
                gun.gameObject.SetActive(true);
            else
                gun.gameObject.SetActive(false);

            t++;
        }
    }
}
