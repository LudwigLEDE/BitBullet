using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float spawnHP = 50;
    public float HP;
    public bool isLocalPlayer;
    public bool isLoaded = false;
    public bool isCreator = false;
    PlayerMovement movement;
    public GameObject spawn;

    Transform camTrans;
    private void Start()
    {
        if (isLocalPlayer)
        {
            camTrans = GetComponent<PlayerMovement>().playerCam;
            HP = spawnHP;
            UIManager.instance.UpdateHealthDisplay((int)HP);
            movement = GetComponent<PlayerMovement>();
        }

    }

    public void EnablePlayer()
    {
        if (camTrans == null)
        {
            GetComponent<PlayerMovement>().enabled = true;
            camTrans = GetComponent<PlayerMovement>().playerCam;
        }
        camTrans.GetComponent<Camera>().enabled = true;
    }

    [PunRPC]
    public void Damage(float dmg)
    {
        PhotonView pv = GetComponent<PhotonView>();
        // Only the owner should process the damage
        if (!pv.IsMine)
            return;

        HP -= dmg;

        // Update the UI only if this is the local player
        if (isLocalPlayer)
        {
            UIManager.instance.UpdateHealthDisplay((int)HP);
        }

        if (HP <= 0)
        {
            if (isLocalPlayer)
            {
                RoomManager.instance.RespawnPlayer(gameObject);
            }
            // Properly destroy the object across the network
            PhotonNetwork.Destroy(gameObject);
        }
    }

}
