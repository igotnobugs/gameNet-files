using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks 
{
    [Header("Connection Status Panel")]
    public Text connectionStatusText;

    [Header("Global")]
    public GameObject[] mainPanels;

    [Header("Login UI Panel")]
    public GameObject loginUIPanel;
    public InputField playerNameInput;

    [Header("Game Options Panel")]
    public GameObject gameOptionsPanel;

    [Header("Create Room Panel")]
    public InputField roomNameInputField;
    public InputField playerCountInputField;

    [Header("Inside Room Panel")]
    public GameObject insideRoomPanel;
    public Text roomInfoText;
    public GameObject playerListItemPrefab;
    public Transform playerListViewParent;

    private Dictionary<string, GameObject> playerListGameObjects;

    [Header("Room List Panel")]
    public GameObject roomItemPrefab;
    public GameObject roomListParent;

    [Header("Join Random Room Panel")]
    public Text joinRandomRoomStatus;

    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListGameObjects;

    #region Unity Functions
    // Start is called before the first frame update
    private void Start() {     
        playerListGameObjects = new Dictionary<string, GameObject>();
        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListGameObjects = new Dictionary<string, GameObject>();

        ActivatePanel(loginUIPanel);
        
        PhotonNetwork.AutomaticallySyncScene = true;     

        // from leaving the game
        if (PhotonNetwork.IsConnected) {
            PhotonNetwork.LeaveRoom();
            ActivatePanel(gameOptionsPanel);            
        }
    }

    // Update is called once per frame
    private void Update() {
        connectionStatusText.text = "Connection Status: " + PhotonNetwork.NetworkClientState; 
    }
    #endregion

    #region UI Callbacks
    public void OnLoginButtonClicked() {
        if (string.IsNullOrEmpty(playerNameInput.text)) {
            Debug.Log("Name is invalid.");         
        } else {
            PhotonNetwork.LocalPlayer.NickName = playerNameInput.text;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void OnCreateRoomClicked() {
        string roomName = roomNameInputField.text;

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)int.Parse(playerCountInputField.text);     
        CreateAndJoinRoom(roomName, roomOptions);
    }

    public void JoinLobby() {
        if (!PhotonNetwork.InLobby) {
            PhotonNetwork.JoinLobby();
            Debug.Log(PhotonNetwork.NickName + " joined lobby");
        } 
    }


    public void LeaveLobby() {
        if (PhotonNetwork.InLobby) {
            PhotonNetwork.LeaveLobby();         
        }
    }

    public void LoadLevel(string levelName) {
        if (PhotonNetwork.IsMasterClient) {
            PhotonNetwork.LoadLevel(levelName);
        }
    }

    public void LeaveRoom() {
        ClearPlayerListGameObjects();

        PhotonNetwork.LeaveRoom();
        Debug.Log(PhotonNetwork.NickName + " left the room");
    }

    public void JoinRandomRoom() {
        PhotonNetwork.JoinRandomRoom();
        joinRandomRoomStatus.text = "Trying to join a random room...";
    }

    public void ActivatePanel(GameObject panel) {
        for (int i = 0; i < mainPanels.Length; i++) {
            mainPanels[i].SetActive(panel.Equals(mainPanels[i]));
        }
    }
    #endregion

    #region PUN Callbacks
    public override void OnConnected() {
        Debug.Log("Connected to the internet.");
    }

    public override void OnConnectedToMaster() {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " connected to Photon.");
        ActivatePanel(gameOptionsPanel);
    }

    //public override void OnLeftRoom() {
    //    PhotonNetwork.LoadLevel("LobbyScene");
    //   ActivatePanel(gameOptionsPanel);
    //}

    public override void OnCreatedRoom() {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " created.");
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        joinRandomRoomStatus.text = joinRandomRoomStatus.text + " creating random room.";

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 5;

        CreateAndJoinRoom("", roomOptions);
    }

    public override void OnJoinedRoom() {
        ActivatePanel(insideRoomPanel);
        UpdatePlayerListInRoom(PhotonNetwork.CurrentRoom);       
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        UpdatePlayerListInRoom(PhotonNetwork.CurrentRoom);
        Debug.Log(otherPlayer.NickName + " left room");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        ClearRoomListGameObjects();

        foreach (RoomInfo info in roomList) {
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList) {
                if (cachedRoomList.ContainsKey(info.Name)) {
                    cachedRoomList.Remove(info.Name);
                }
            }
            else {
                if (cachedRoomList.ContainsKey(info.Name)) {
                    cachedRoomList[info.Name] = info;
                }
                else {
                    cachedRoomList.Add(info.Name, info);
                }
            }
        }
    
        foreach (RoomInfo info in cachedRoomList.Values) {
            GameObject listItem = Instantiate(roomItemPrefab);
            listItem.transform.SetParent(roomListParent.transform);
            listItem.transform.localScale = Vector3.one;

            listItem.transform.Find("RoomNameText").GetComponent<Text>().text = info.Name;
            listItem.transform.Find("RoomPlayersText").GetComponent<Text>().text = 
                "Player count: " + info.PlayerCount + " / " + info.MaxPlayers;

            listItem.transform.Find("JoinRoomButton").GetComponent<Button>()
                .onClick.AddListener(() =>
                    OnJoinRoomClicked(info.Name)
                );

            roomListGameObjects.Add(info.Name, listItem);
        }

    }

    public override void OnLeftLobby() {
        ClearRoomListGameObjects();
        Debug.Log(PhotonNetwork.NickName + " left lobby");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        UpdatePlayerListInRoom(PhotonNetwork.CurrentRoom);
        Debug.Log(newPlayer.NickName + " joined room");
    }
    #endregion

    #region Private Methonds
    private void CreateAndJoinRoom(string roomName, RoomOptions roomOptions) {
        if (string.IsNullOrEmpty(roomName)) {
            roomName = "Room " + Random.Range(1000, 10000);
        }
        roomOptions.PublishUserId = true;
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    private void OnJoinRoomClicked(string roomName) {
        if (PhotonNetwork.InLobby) {
            PhotonNetwork.LeaveLobby();
        }
        PhotonNetwork.JoinRoom(roomName);
    }

    private void ClearRoomListGameObjects() {
        foreach (var item in roomListGameObjects.Values) {
            Destroy(item);
        }
        roomListGameObjects.Clear();
    }


    private void UpdatePlayerListInRoom(Room room) {
        ClearPlayerListGameObjects();

        roomInfoText.text = "Room Name: " + room.Name + " has " +
            room.PlayerCount + " / " + room.MaxPlayers + " players";

        foreach (Player player in PhotonNetwork.PlayerList) {
            GameObject playerItem = Instantiate(playerListItemPrefab);
            Transform itemTransform = playerItem.transform;
            playerItem.transform.SetParent(playerListViewParent);
            playerItem.transform.localScale = Vector3.one;

            itemTransform.Find("PlayerNameText").GetComponent<Text>().text = player.NickName;
            itemTransform.Find("PlayerIndicator").gameObject.SetActive(
                    player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber
                );

            playerListGameObjects.Add(player.UserId, playerItem);
        }
    }

    private void ClearPlayerListGameObjects() {
        foreach (var item in playerListGameObjects.Values) {
            Destroy(item);
        }
        playerListGameObjects.Clear();
    }
    #endregion
}
