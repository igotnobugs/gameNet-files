using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject playerPrefab;

    public static GameManager instance;

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        } else {
            instance = this;
        }
    }

    // Start is called before the first frame update
    private void Start() {
        if (PhotonNetwork.IsConnected) {
            if (playerPrefab != null) {
                int xRandomPoint = Random.Range(-20, 20);
                int zRandomPoint = Random.Range(-20, 20);
                PhotonNetwork.Instantiate(playerPrefab.name, 
                    new Vector3(xRandomPoint, 0 ,zRandomPoint),
                    Quaternion.identity);
            }
        }
    }

    public override void OnJoinedRoom() {
        base.OnJoinedRoom();
        Debug.Log(PhotonNetwork.NickName + " joined the room");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log(newPlayer.NickName + " entered the room");
    }

    public override void OnLeftRoom() {
        SceneManager.LoadScene("GameLauncherScene");
    }

    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
