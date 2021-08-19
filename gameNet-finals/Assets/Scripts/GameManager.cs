using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("Hero Prefab")]
    public Hero blueHeroPrefab;
    public Hero redHeroPrefab;

    [Header("Spawn points")]
    public GameObject blueSpawnPoint;
    public GameObject redSpawnPoint;

    [Header("Prefabs")]
    public GameObject blueTowerPrefab;
    public GameObject redTowerPrefab;
    public Barracks blueBarracks;
    public Barracks redBarracks;


    [Header("Spawning Points")]
    public Transform[] blueTowerSpawnPoints;
    public Transform[] redTowerSpawnPoints;
    public Transform blueBarracksSpawnPoint;
    public Transform redBarracksSpawnPoint;

    public Hero clientPlayer;

    // Start is called before the first frame update
    void Start() {

        // when client player spawned
        if (PhotonNetwork.IsConnectedAndReady) {

            if (PhotonNetwork.IsMasterClient) {
                SpawnArenaObjects();
            }

            
            object playerIsBlueTeam;
            if (PhotonNetwork.LocalPlayer.CustomProperties
                .TryGetValue(Constants.PLAYER_TEAM, out playerIsBlueTeam)) {


                Vector3 spawnPosition = new Vector3();
                string prefabName = "";

                if ((bool)playerIsBlueTeam) {
                    spawnPosition = blueSpawnPoint.transform.position;
                    prefabName = blueHeroPrefab.name;
                } else {
                    spawnPosition = redSpawnPoint.transform.position;
                    prefabName = redHeroPrefab.name;
                }

                GameObject playerSpawned = PhotonNetwork.Instantiate(prefabName, spawnPosition, Quaternion.identity);
                clientPlayer = playerSpawned.GetComponent<Hero>();
                clientPlayer.isBlueSide = (bool)playerIsBlueTeam;

            } else {
                Debug.Log("Error! no custom properties found");
            }         
        }

        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.NetworkingClient.EventReceived += OnHeroDeathEvent;
        PhotonNetwork.NetworkingClient.EventReceived += OnHeroSpawnedEvent;
        PhotonNetwork.NetworkingClient.EventReceived += OnBarracksDestroyedEvent;
    }

    public void SpawnArenaObjects() {

        // Blue tower
        for (int i = 0; i < blueTowerSpawnPoints.Length; i++) {
            PhotonNetwork.InstantiateRoomObject(blueTowerPrefab.name, 
                blueTowerSpawnPoints[i].position, blueTowerSpawnPoints[i].rotation);
        }

        // Red tower
        for (int i = 0; i < redTowerSpawnPoints.Length; i++) {
            PhotonNetwork.InstantiateRoomObject(redTowerPrefab.name,
                redTowerSpawnPoints[i].position, redTowerSpawnPoints[i].rotation);
        }

        PhotonNetwork.InstantiateRoomObject(blueBarracks.name, blueBarracksSpawnPoint.position, 
            blueBarracksSpawnPoint.rotation);

        PhotonNetwork.InstantiateRoomObject(redBarracks.name, redBarracksSpawnPoint.position,
            redBarracksSpawnPoint.rotation);
    }

    void OnHeroDeathEvent(EventData photonEvent) {
        if (photonEvent.Code == (byte)HeroEventCode.Eliminated) {
            object[] data = (object[])photonEvent.CustomData;

            string eliminatedPlayer = (string)data[0];

            Debug.Log(eliminatedPlayer + " eliminated!");

        }
    }

    void OnHeroSpawnedEvent(EventData photonEvent) {
        if (photonEvent.Code == (byte)HeroEventCode.Spawned) {
            object[] data = (object[])photonEvent.CustomData;

            string spawnedPlayer = (string)data[0];

            Debug.Log(spawnedPlayer + " spawned!");

        }
    }

    //Takes forever
    void OnBarracksDestroyedEvent(EventData photonEvent) {
        if (photonEvent.Code == (byte)BarracksEventCode.Destroyed) {
            object[] data = (object[])photonEvent.CustomData;

            bool isBluesideDestroyed = (bool)data[0];

            if (isBluesideDestroyed) {
                Debug.Log("Red Team Won");
            } else {
                Debug.Log("Blue Team Won");
            }
            ExitGame();

        }
    }

    public void ExitGame() {
        if (PhotonNetwork.IsMasterClient) {
            PhotonNetwork.LoadLevel("LobbyScene");
        } else {
            PhotonNetwork.LeaveRoom();
        }
    }
}
