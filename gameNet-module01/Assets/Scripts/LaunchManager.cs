using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LaunchManager : MonoBehaviourPunCallbacks
{
    public GameObject EnterGamePanel;
    public GameObject ConnectionStatusPanel;
    public GameObject LobbyPanel;

    private void Awake() {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Start is called before the first frame update
    private void Start() {
        EnterGamePanel.SetActive(true);
        ConnectionStatusPanel.SetActive(false);
        LobbyPanel.SetActive(false);
    }

    public override void OnConnected() {
        Debug.Log(PhotonNetwork.NickName + " connected to Internet");
    }

    public override void OnConnectedToMaster() {
        Debug.Log(PhotonNetwork.NickName + " connected to Photon Servers");
        EnterGamePanel.SetActive(false);
        ConnectionStatusPanel.SetActive(false);
        LobbyPanel.SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        Debug.LogWarning(message);
        CreateAndJoinRoom();
    }

    public override void OnJoinedRoom() {
        Debug.Log(PhotonNetwork.NickName + " entered " + PhotonNetwork.CurrentRoom.Name + ".\n" +
            PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers);
        PhotonNetwork.LoadLevel("GameScene");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        Debug.Log(newPlayer.NickName + " entered " + PhotonNetwork.CurrentRoom.Name + ".\n" +
            PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers);
    }

    public void ConnectToPhotonServer() {
        if (string.IsNullOrEmpty(PhotonNetwork.NickName)) {
            Debug.LogWarning("Player Name is Empty");
            return;
        }

        if (!PhotonNetwork.IsConnected) {
            PhotonNetwork.ConnectUsingSettings();
            EnterGamePanel.SetActive(false);
            ConnectionStatusPanel.SetActive(true);
            LobbyPanel.SetActive(false);
        }
    }

    public void JoinRandomRoom() {
        PhotonNetwork.JoinRandomRoom();
    }

    public void CreateAndJoinRoom() {
        string randomRoomName = "Name " + Random.Range(0, 10000);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 20;

        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }
}
