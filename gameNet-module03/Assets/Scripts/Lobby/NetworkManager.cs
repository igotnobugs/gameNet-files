using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject[] mainPanels;

    [Header("Login UI")]
    public GameObject LoginUIPanel;
    public InputField PlayerNameInput;

    [Header("Connecting Info Panel")]
    public GameObject ConnectingInfoUIPanel;

    [Header("Creating Room Info Panel")]
    public GameObject CreatingRoomInfoUIPanel;

    [Header("GameOptions  Panel")]
    public GameObject GameOptionsUIPanel;

    [Header("Create Room Panel")]
    public GameObject CreateRoomUIPanel;
    public InputField RoomNameInputField;
    
    [Header("Inside Room Panel")]
    public GameObject InsideRoomUIPanel;
    public Text roomGameModeText;
    public Text roomInfoText;
    public GameObject playerListParent;
    public RoomPlayerList playerListPrefab;
    public GameObject startGameButton;
    public VehicleSelection garageShow;
 
    [Header("Join Random Room Panel")]
    public GameObject JoinRandomRoomUIPanel;

    // Game Room Variables
    private string gameMode; //gameMode - racing, deathRace
    private Dictionary<int, RoomPlayerList> playerListObjects;

    #region Unity Methods
    void Start()
    {
        ActivatePanel(LoginUIPanel);
        PhotonNetwork.AutomaticallySyncScene = true;

        // from leaving the game
        if (PhotonNetwork.IsConnected) {
            PhotonNetwork.LeaveRoom();
            ActivatePanel(GameOptionsUIPanel);
        }
    }

    #endregion

    #region UI Callback Methods
    public void OnLoginButtonClicked()
    {
        string playerName = PlayerNameInput.text;

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("PlayerName is invalid!");
            return;
        }      

        if (!PhotonNetwork.IsConnected) {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();
        }

        ActivatePanel(ConnectingInfoUIPanel);
    }

    public void OnCreateRoomButtonClicked()
    {
        if (gameMode == null) {
            Debug.Log("Please set a game mode.");
            return;
        }

        string roomName = RoomNameInputField.text;
        if (string.IsNullOrEmpty(roomName))
        {
            roomName = "Room " + Random.Range(1000, 10000);
        }

        RoomOptions roomOptions = new RoomOptions();
        string[] roomPropertiesInLobby = { "gameMode" };

        ExitGames.Client.Photon.Hashtable customRoomProperties 
            = new ExitGames.Client.Photon.Hashtable() { { "gameMode", gameMode} };

        roomOptions.CustomRoomPropertiesForLobby = roomPropertiesInLobby;
        roomOptions.CustomRoomProperties = customRoomProperties;
        roomOptions.MaxPlayers = 3;
        PhotonNetwork.CreateRoom(roomName, roomOptions);

        ActivatePanel(CreatingRoomInfoUIPanel);
    }

    public void OnJoinRandomRoomClicked(string clickedGameMode) {
        gameMode = clickedGameMode;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties
            = new ExitGames.Client.Photon.Hashtable() { { "gameMode", clickedGameMode } };

        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
    }

    public void OnLeaveRoomButtonClicked() {
        PhotonNetwork.LeaveRoom();
    }

    public void OnStartGameButtonClicked() {
        object gameModeName;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("gameMode", out gameModeName)) {

            if (gameMode.Contains("racing")) {
                LoadLevel("RaceGameScene"); // racingScene
            } else if (gameMode.Contains("deathRace")) {
                LoadLevel("RaceGameScene"); // deathRaceScene
            }
        }
    }

    public void LoadLevel(string sceneName) {
        PhotonNetwork.LoadLevel(sceneName);
    }
    #endregion

    #region Photon Callbacks
    public override void OnConnected()
    {
        Debug.Log("Connected to Internet");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName+ " is connected to Photon");
        ActivatePanel(GameOptionsUIPanel);
    }

    public override void OnCreatedRoom() {
        Debug.Log(PhotonNetwork.CurrentRoom + " created");
    }

    public override void OnJoinedRoom() {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " joined " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("Players: " + PhotonNetwork.CurrentRoom.PlayerCount);

        ActivatePanel(InsideRoomUIPanel);

        startGameButton.SetActive(false);

        object gameModeName;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("gameMode", out gameModeName)) {
            Debug.Log(gameModeName.ToString());
            UpdateRoomInfo();

            if (gameMode.Contains("racing")) {
                roomGameModeText.text = "Racing Mode";
                garageShow.deathRaceMode = false;

            } else if (gameMode.Contains("deathRace")) {
                roomGameModeText.text = "Death Race Mode";
                garageShow.deathRaceMode = true;
            }
        }
        garageShow.ShowCars();

        if (playerListObjects == null)
            playerListObjects = new Dictionary<int, RoomPlayerList>();

        foreach (Player player in PhotonNetwork.PlayerList) {
            AddPlayerToList(player);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        AddPlayerToList(newPlayer);
        UpdateRoomInfo();

        startGameButton.SetActive(CheckAllPlayerReady());
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        Destroy(playerListObjects[otherPlayer.ActorNumber].gameObject);
        playerListObjects.Remove(otherPlayer.ActorNumber);

        UpdateRoomInfo();
    }

    public override void OnLeftRoom() {
        if (playerListObjects == null) return;

        foreach(RoomPlayerList playerObject in playerListObjects.Values) {
            if (playerObject.gameObject != null)
                Destroy(playerObject.gameObject);
        }
        playerListObjects.Clear();
        playerListObjects = null;
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        Debug.Log(message);

        if (gameMode == null) {
            Debug.Log("Game mode is not set");
            return;
        }

        string roomName = RoomNameInputField.text;
        if (string.IsNullOrEmpty(roomName)) {
            roomName = "Room " + Random.Range(1000, 10000);
        }

        RoomOptions roomOptions = new RoomOptions();
        string[] roomPropertiesInLobby = { "gameMode" };

        ExitGames.Client.Photon.Hashtable customRoomProperties
            = new ExitGames.Client.Photon.Hashtable() { { "gameMode", gameMode } };

        roomOptions.CustomRoomPropertiesForLobby = roomPropertiesInLobby;
        roomOptions.CustomRoomProperties = customRoomProperties;
        roomOptions.MaxPlayers = 3;
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps) {

        RoomPlayerList playerList;
        if (playerListObjects.TryGetValue(targetPlayer.ActorNumber, out playerList)) {

            object isPlayerReady;
            if (changedProps.TryGetValue(Constants.PLAYER_READY, out isPlayerReady)) {
                playerList.SetPlayerReady((bool)isPlayerReady);
            }


        }

        startGameButton.SetActive(CheckAllPlayerReady());
    }

    public override void OnMasterClientSwitched(Player newMasterClient) {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber) {
            startGameButton.SetActive(CheckAllPlayerReady());
        }
    }

    #endregion

    #region Public Methods
    public void ActivatePanel(GameObject panelToBeActivated) {
        foreach(GameObject panel in mainPanels) {
            panel.SetActive(panel.Equals(panelToBeActivated));
        }
    }

    public void SetGameMode(string newGameMode) {
        gameMode = newGameMode;
    }
    #endregion

    private void UpdateRoomInfo() {
       roomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + " "
        + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
    }

    private void AddPlayerToList(Player player) {
        RoomPlayerList playerListItem = Instantiate(playerListPrefab);
        playerListItem.transform.SetParent(playerListParent.transform);
        playerListItem.transform.localScale = Vector3.one;
        playerListItem.Init(player.ActorNumber, player.NickName);

        object isPlayerReady;
        if (player.CustomProperties.TryGetValue(Constants.PLAYER_READY, out isPlayerReady)) {
            playerListItem.SetPlayerReady((bool)isPlayerReady);
        }
        playerListObjects.Add(player.ActorNumber, playerListItem);
    }

    private bool CheckAllPlayerReady() {
        if (!PhotonNetwork.IsMasterClient) {
            return false;
        }

        foreach (Player player in PhotonNetwork.PlayerList) {

            object isPlayerReady;
            if (player.CustomProperties.TryGetValue(Constants.PLAYER_READY, out isPlayerReady)) {

                if (!(bool)isPlayerReady) return false;
            } else {

                return false;
            }
            
        }
        return true;
    }
}