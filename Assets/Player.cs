using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float spawnHP = 50;
    public float HP;
    public bool isLocalPlayer;
    public bool isLoaded = false;
    PlayerMovement movement;
    public GameObject spawn;

    Transform camTrans;
    private void Start()
    {
        camTrans = GetComponent<PlayerMovement>().playerCam;
        HP = spawnHP;
        UIManager.instance.UpdateHealthDisplay((int)HP);
        movement = GetComponent<PlayerMovement>();

    }

    public void EnablePlayer()
    {
        if (camTrans == null)
        {
            GetComponent<PlayerMovement>().enabled = true;
            camTrans = GetComponent<PlayerMovement>().playerCam;
        }
        camTrans.gameObject.SetActive(true);
    }

    [Photon.Pun.PunRPC]
    public void Damage(float dmg)
    {
        HP -= dmg;

        if (HP <= 0)
        {
            //transform.position = spawn.transform.position;
            if (isLocalPlayer)
            {
                RoomManager.instance.RespawnPlayer(gameObject);
                HP = spawnHP;
            } else
            {
                Destroy(gameObject);
            }
        }
        UIManager.instance.UpdateHealthDisplay((int)HP);
    }
}
