using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WeaponManager : MonoBehaviour
{

    [SerializeField]WeaponData weaponData;
    [SerializeField]Transform weaponTransform;
    GameObject currentWeapon;
    WeaponHandler currentWeaponHandel;
    PlayerWeaponData currentWeaponData;

    private float lastShootTime;

    void Start()
    {
        lastShootTime = Time.time;
        foreach (PlayerWeaponData item in weaponData.Weapons)
        {
            if (currentWeapon != null)
            {
                Destroy(currentWeapon);
            }
            currentWeapon = Instantiate(item.weaponObject, weaponTransform);
            currentWeaponHandel = currentWeapon.GetComponent<WeaponHandler>();
            currentWeaponData = item;

        }
    }

    // Jede waffe wird geladen also fix ludwig( alles wird geladen aber nur eine wird angezeigt 


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && currentWeaponHandel != null)
        {
            Shoot();
     
        }
    }

    void Shoot()
    {
        if (lastShootTime + currentWeaponData.firerate < Time.time)
        {
            currentWeaponHandel.GetMuzzleFlash().Play();
            Vector3 direction = GetDirection();
            if (Physics.Raycast(currentWeaponHandel.GetPointer().transform.position, direction, out RaycastHit hit, float.MaxValue, weaponData.mask))
            {
                TrailRenderer trail = Instantiate(weaponData.bulletTrail, currentWeaponHandel.GetPointer().transform.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit));
                lastShootTime = Time.time;
            }
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = transform.forward;
        direction += new Vector3(
            Random.Range(-currentWeaponData.bulletSpread.x, currentWeaponData.bulletSpread.x),
            Random.Range(-currentWeaponData.bulletSpread.y, currentWeaponData.bulletSpread.y),
            Random.Range(-currentWeaponData.bulletSpread.z, currentWeaponData.bulletSpread.z)
        );
        direction.Normalize();
        return direction;
    }

    IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }

        trail.transform.position = hit.point;
        Instantiate(weaponData.hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
    }
}
