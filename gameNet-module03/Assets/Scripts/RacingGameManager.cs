using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RacingGameManager : MonoBehaviour
{
    public GameObject overviewCamera;

    public GameObject[] vehiclePrefabs;
    public GameObject[] combatVehiclePrefabs;
    public Transform[] startingPositions;
    public Text timeText;

    //public static RacingGameManager instance = null;

    public LapTrigger firstLapTrigger;
    public GameObject timePanel;
   
    [Header("Players")]
    public Vehicle clientPlayer;
    public List<Vehicle> players;


    [Header("Game States")]
    public bool raceStarted = false;

    private int positionIndex = -1;
    public bool isDeathRace = false;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady) {

            overviewCamera.gameObject.SetActive(false);

            GameObject playerSpawned = new GameObject();
            Vector3 startPosition = Vector3.zero;

            int spawnPosition = 0;
            foreach (Player player in PhotonNetwork.PlayerList) {
                if (player.IsLocal) {
                    Debug.Log(spawnPosition);
                    break;
                }
                spawnPosition++;
                
            }


            object gameModeName;
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("gameMode", out gameModeName)) {
                if (gameModeName.Equals("racing")) {

                    object playerSelectionIndex;
                    if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(Constants.PLAYER_SELECTION_INDEX, out playerSelectionIndex)) {
                        
                        isDeathRace = false;

                        playerSpawned = PhotonNetwork.Instantiate(
                            vehiclePrefabs[(int)playerSelectionIndex].name,
                            startingPositions[spawnPosition].transform.position, Quaternion.identity);                        
                    }
                } else if (gameModeName.Equals("deathRace")) {

                    object playerSelectionIndex;
                    if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(Constants.PLAYER_SELECTION_INDEX, out playerSelectionIndex)) {

                        isDeathRace = true;

                        playerSpawned = PhotonNetwork.Instantiate(
                            combatVehiclePrefabs[(int)playerSelectionIndex].name,
                            startingPositions[spawnPosition].transform.position, Quaternion.identity);
                    }
                }
                
                playerSpawned.GetComponent<PlayerSetup>().rgm = this;
                playerSpawned.GetComponent<PlayerSetup>().isDeathRace = isDeathRace;
                clientPlayer = playerSpawned.GetComponent<Vehicle>();
            }
            
        }

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void DisableTimePanel() {
        timePanel.SetActive(false);
    }

    public Vector3 GetNewPosition() {
        positionIndex++;
        Debug.Log(positionIndex);
        return startingPositions[positionIndex].position;    
    }

    public void CreateKillFeed(string killer, string victim) {
        clientPlayer.rpui.AddToKillFeed(killer, victim);
    }

    public void CheckLastManStanding() {
        if (players.Count == 1) {
            if (!clientPlayer.isDead) {
                clientPlayer.rpui.DisplayWinDeathPanel();
                clientPlayer.vehicleMovement.isControllable = false;
                clientPlayer.vehicleMovement.StopMoving();
                clientPlayer.weaponController.weaponsActive = false;
            }
        }
    }

    public void ExitGame() {
        if (PhotonNetwork.IsMasterClient) {
            PhotonNetwork.LoadLevel("LobbyScene");
        } else {
            PhotonNetwork.LeaveRoom();
        }
    }


    public void AnnouceFirstPlacer(string name) {
        clientPlayer.rpui.SetFirstPlacer(name);
    }

    public void AnnouceSecondPlacer(string name) {
        clientPlayer.rpui.SetSecondPlacer(name);
    }

    public void AnnouceThirdPlacer(string name) {
        clientPlayer.rpui.SetThirdPlacer(name);
    }

}
