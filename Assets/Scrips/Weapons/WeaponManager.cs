using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WeaponManager : MonoBehaviour
{

    [SerializeField]WeaponData weaponData;
    [SerializeField]Transform weaponTransform;
    GameObject currentWeapon;

    void Start()
    {
        foreach (PlayerWeaponData item in weaponData.Weapons)
        {
            if (currentWeapon != null)
            {
                Destroy(currentWeapon);
            }
            currentWeapon = Instantiate(item.weaponObject, weaponTransform);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
