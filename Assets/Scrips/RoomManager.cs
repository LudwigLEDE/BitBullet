using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.Linq;

public class RoomManager : MonoBehaviourPunCallbacks
{
    bool DEBUGGING = false;
    public static RoomManager instance;
    public GameObject player;
    [Space]
    public Transform spawnPoint;
    public UIManager uiManager;
    public List<Photon.Realtime.Player> allies = new List<Photon.Realtime.Player>();
    public List<Photon.Realtime.Player> enemies = new List<Photon.Realtime.Player>();

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
        if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.Name != "")
        {
            Debug.Log($"Currently in Room {PhotonNetwork.CurrentRoom.Name}");
            
        } else
        {
            if (!DEBUGGING)
            {
                PhotonNetwork.Disconnect();
                PhotonNetwork.LoadLevel(0);
            } else
            {
                PhotonNetwork.JoinOrCreateRoom("debugging", null, null);        
            }
        }
        Debug.Log("Connect to Room");        
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.Name != "")
        {
            Debug.Log($"Currently in Room {PhotonNetwork.CurrentRoom.Name}");
            UIManager.instance.roundCode.text = $"RoomCode: {PhotonNetwork.CurrentRoom.Name}";
        }
        
        Debug.Log("Joined Room");

        GameObject plr = PhotonNetwork.Instantiate(player.name, SpawnManager.instance.GetRandomSpawn().position, Quaternion.identity);
        Player plrData = plr.GetComponent<Player>();
        plrData.spawn = SpawnManager.instance.GetRandomSpawn().gameObject;
        plrData.isLocalPlayer = true;
        plrData.isLoaded = true;
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("You are room Creator");
            plrData.isCreator = true;
        }
        plrData.EnablePlayer();
    }

    public void RespawnPlayer(GameObject oldPlr)
    {
        bool wasCreator = oldPlr.GetComponent<Player>().isCreator;
        Destroy(oldPlr);
        GameObject plr = PhotonNetwork.Instantiate(player.name, SpawnManager.instance.GetRandomSpawn().position, Quaternion.identity);
        Player plrData = plr.GetComponent<Player>();
        plrData.spawn = SpawnManager.instance.GetRandomSpawn().gameObject;
        plrData.isLocalPlayer = true;
        plrData.isLoaded = true;
        plrData.isCreator = wasCreator;
        plrData.EnablePlayer();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from Room/Lobby");
        SceneManager.LoadScene(0);
    }

    public List<Photon.Realtime.Player> GetTeam(bool lookingForAllied)
    {
        allies.Clear();
        enemies.Clear();

        if (!PhotonNetwork.IsConnected || !PhotonNetwork.InRoom) return lookingForAllied == true ? allies : enemies;

        List<Photon.Realtime.Player> sortedPlayers = PhotonNetwork.PlayerList.OrderBy(p => p.ActorNumber).ToList();

        for (int i = 0; i < sortedPlayers.Count; i++)
        {
            if (i % 2 == 0)
            {
                allies.Add(sortedPlayers[i]);
            }
            else
            {
                enemies.Add(sortedPlayers[i]);
            }
        }
        
        return lookingForAllied == true ? allies : enemies;
    }
}
