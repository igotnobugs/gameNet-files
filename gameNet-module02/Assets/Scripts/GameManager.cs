using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : Singleton<GameManager> 
{
    [Header("Spawning Settings")]
    public GameObject[] spawnPoints;
    public PlayerUnit playerPrefab;

    [Header("Game Settings")]
    public float killsToWin = 10.0f;
    public float respawnTime = 5.0f;
    public float exitTime = 5.0f;

    [Header("Players")]
    public PlayerUnit clientPlayer;
    public List<PlayerUnit> players;

    private bool someoneWon = false;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        players = new List<PlayerUnit>();
        
        if (PhotonNetwork.IsConnectedAndReady) {
            GameObject newPlayer = PhotonNetwork.Instantiate(
                playerPrefab.name,
                GetRandomSpawnPoint(),
                Quaternion.identity
                );

            clientPlayer = newPlayer.GetComponent<PlayerUnit>();

        }
    }

    public Vector3 GetRandomSpawnPoint() {
        GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        float randomPointX = spawnPoint.transform.position.x;
        float randomPointZ = spawnPoint.transform.position.z;

        return new Vector3(randomPointX, 0, randomPointZ);
    }

    public void CreateKillFeed(string killer, string victim) {
        clientPlayer.pui.AddToKillFeed(killer, victim);
    }

    public void Exit() {
        if (PhotonNetwork.IsMasterClient) {
            PhotonNetwork.LoadLevel("LobbyScene");
        }        
    }

    public void DisplayWinner(string name) {
        if (someoneWon) return;
        someoneWon = true;
        clientPlayer.pui.ShowWinner(name);
        StartCoroutine(ExitCountdown());
    }

    IEnumerator ExitCountdown() {
        float countTime = exitTime;
        while (countTime > 0) {
            clientPlayer.pui.countdownLobbyText.text = "Returning to lobby in " + countTime + " secs";
            yield return new WaitForSeconds(1.0f);
            countTime--;
        }

        Exit();
    }

}
