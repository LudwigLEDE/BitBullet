using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;
    public GameObject player;
    [Space]
    public Transform spawnPoint;
    public UIManager uiManager;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Debug.Log("Connecting...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log("Connected to Server");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        PhotonNetwork.JoinOrCreateRoom("test", null, null);

        Debug.Log("Connect to Room");        
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("Joined Room");
        GameObject plr = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);
        Player plrData = plr.GetComponent<Player>();
        plrData.spawn = spawnPoint.gameObject;
        plrData.isLocalPlayer = true;
        plrData.isLoaded = true;
        plrData.EnablePlayer();
    }

    public void RespawnPlayer(GameObject oldPlr)
    {
        Destroy(oldPlr);
        GameObject plr = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);
        Player plrData = plr.GetComponent<Player>();
        plrData.spawn = spawnPoint.gameObject;
        plrData.isLocalPlayer = true;
        plrData.isLoaded = true;
        plrData.EnablePlayer();
    }
}
