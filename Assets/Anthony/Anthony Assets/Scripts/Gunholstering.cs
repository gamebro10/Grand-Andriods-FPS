using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunholstering : MonoBehaviour
{

    int CurrentWeopon = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        weaponholstering();
    }


    public void weaponholstering()
    {
        int lastWeapon = CurrentWeopon;
        if(Input.GetKeyUp(KeyCode.Alpha1)) 
        {
            CurrentWeopon = 0;
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            CurrentWeopon = 1;
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            CurrentWeopon = 2;
        }
        if ( lastWeapon != CurrentWeopon )
        {
            IDweapon();
        }
        
    }

    public void IDweapon()
    {
            int t = 0;
        //first weapon will be 0in code the rest goes up by one 
        foreach(Transform gun in transform)
        {
            if(t == CurrentWeopon) 
            gun.gameObject.SetActive(true);
            else
            gun.gameObject.SetActive(false);

            t++;
        }
    }
}
