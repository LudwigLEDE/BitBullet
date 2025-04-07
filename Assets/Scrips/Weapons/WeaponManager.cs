using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;



public class WeaponManager : MonoBehaviour
{

    [SerializeField]WeaponData weaponData;
    [SerializeField]Transform weaponTransform;
    GameObject currentWeapon;
    WeaponHandler currentWeaponHandel;
    PlayerWeaponData currentWeaponData;
    Player plr;

    private float lastShootTime;

    void Start()
    {
        plr = GetComponent<Player>();
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
        if (Input.GetKeyDown(KeyCode.Mouse0) && currentWeaponHandel != null && plr.isLoaded)
        {
            Shoot();
     
        }
    }

    public void Shoot()
    {
        if (lastShootTime + currentWeaponData.firerate < Time.time)
        {
            currentWeaponHandel.GetMuzzleFlash().Play();
            Vector3 direction = GetDirection();
            if (Physics.Raycast(currentWeaponHandel.GetPointer().transform.position, direction, out RaycastHit hit, float.MaxValue, weaponData.mask))
            {
                TrailRenderer trail = Instantiate(weaponData.bulletTrail, currentWeaponHandel.GetPointer().transform.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit));
                
                    GetComponent<PhotonView>().RPC("SpawnTrailRPC", RpcTarget.All, currentWeaponHandel.GetPointer().transform.position, hit.point, hit.normal);
                
                lastShootTime = Time.time;
            }
        }
    }

    [PunRPC]
    public void SpawnTrailRPC(Vector3 startPosition, Vector3 hitPoint, Vector3 hitNormal)
    {
        currentWeaponHandel.GetMuzzleFlash().Play();
        // Instantiate the bullet trail on every client
        TrailRenderer trail = Instantiate(weaponData.bulletTrail, startPosition, Quaternion.identity);
        StartCoroutine(SpawnTrail(trail, hitPoint, hitNormal));
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
        PhotonView netWorker = hit.transform.gameObject.GetComponent<PhotonView>();
        if (netWorker != null)
        {
            RoomManager.instance.GetTeam(false); // returns List<Photon.Realtime.Player>
            netWorker.RPC("Damage", RpcTarget.All, currentWeaponData.damage);           
        }
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hitPoint, Vector3 hitNormal)
    {
        float time = 0;
        Vector3 startingPosition = trail.transform.position;

        while (time < 1f)
        {
            trail.transform.position = Vector3.Lerp(startingPosition, hitPoint, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }

        trail.transform.position = hitPoint;

        // Instantiate the hit effect locally (every client will do this)
        Instantiate(weaponData.hitEffect, hitPoint, Quaternion.LookRotation(hitNormal));
    }
}
