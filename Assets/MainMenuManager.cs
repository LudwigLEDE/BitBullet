using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField createInput;
    [SerializeField] Button createButton;
    [SerializeField] TMP_InputField joinInput;
    [SerializeField] Button joinButton;

    [SerializeField] int gameSceneBuildIndex = 1; // Build index of the game scene

    private void Start()
    {
        SetButtonsInteractable(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        createButton.onClick.AddListener(CreateRoom);
        joinButton.onClick.AddListener(JoinRoom);
        ConnectToPhoton();
    }

    void ConnectToPhoton()
    {
        // Handle different connection states to ensure connection to Master/Lobby
        if (PhotonNetwork.IsConnected && PhotonNetwork.Server != ServerConnection.MasterServer)
        {
            PhotonNetwork.Disconnect(); // Disconnect from Game Server first
        }
        else if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings(); // Connect if not connected
        }
        else if (PhotonNetwork.IsConnected && !PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby(); // Join lobby if connected to Master but not in Lobby
        }
        else if (PhotonNetwork.IsConnected && PhotonNetwork.InLobby)
        {
            OnJoinedLobby(); // Already in lobby, ensure UI is enabled
        }
    }

    void SetButtonsInteractable(bool state)
    {
        createButton.interactable = state;
        joinButton.interactable = state;
        createInput.interactable = state;
        joinInput.interactable = state;
    }

    void CreateRoom()
    {
        if (string.IsNullOrEmpty(createInput.text)) return;

        if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InLobby)
        {
            SetButtonsInteractable(false);
            RoomOptions roomOptions = new RoomOptions { MaxPlayers = 4 };
            bool success = PhotonNetwork.CreateRoom(createInput.text, roomOptions); // Rely on callbacks
            if (success)
            {
                PhotonNetwork.LoadLevel(gameSceneBuildIndex);
            }
        }
        else
        {
            Debug.LogWarning($"Cannot Create Room - State: Connected={PhotonNetwork.IsConnectedAndReady}, InLobby={PhotonNetwork.InLobby}");
        }
    }

    void JoinRoom()
    {
        if (string.IsNullOrEmpty(joinInput.text)) return;

        if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InLobby)
        {
            SetButtonsInteractable(false);
            bool success = PhotonNetwork.JoinRoom(joinInput.text); // Rely on callbacks
            if (success)
            {
                PhotonNetwork.LoadLevel(gameSceneBuildIndex);
            }
        }
        else
        {
            Debug.LogWarning($"Cannot Join Room - State: Connected={PhotonNetwork.IsConnectedAndReady}, InLobby={PhotonNetwork.InLobby}");
        }
    }

    // --- Photon Callbacks ---

    public override void OnConnectedToMaster()
    {
        // Called after connecting to Master Server
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        // Called after joining Lobby
        SetButtonsInteractable(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // Called on disconnection
        Debug.LogWarning($"Disconnected. Cause: {cause}.");
        SetButtonsInteractable(false);
        ConnectToPhoton(); // Attempt to reconnect
    }

    public override void OnCreatedRoom()
    {
        // Called when CreateRoom is successful
        PhotonNetwork.LoadLevel(gameSceneBuildIndex);
        // Note: Ensure scene with this build index is in Build Settings!
    }

    public override void OnJoinedRoom()
    {
        // Called when JoinRoom is successful
        // Auto-sync scene load is default; MasterClient explicitly loads level here.
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(gameSceneBuildIndex);
        }
        // Note: Ensure scene with this build index is in Build Settings!
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // Called when JoinRoom fails
        Debug.LogError($"Join Room Failed. Code: {returnCode}, Msg: {message}");
        if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InLobby)
        {
            SetButtonsInteractable(true); // Re-enable UI if still in lobby
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        // Called when CreateRoom fails
        Debug.LogError($"Create Room Failed. Code: {returnCode}, Msg: {message}");
        if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InLobby)
        {
            SetButtonsInteractable(true); // Re-enable UI if still in lobby
        }
    }
}
