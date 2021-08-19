using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RoomPlayerList : MonoBehaviour
{
    public Text playerNameText;
    public Button playerTeamButton;
    public Button playerReadyButton;
    public Image playerReadyImage;

    public bool isBlueTeam = true;
    private bool isPlayerReady = false;

    public void Init(int playerId, string playerName) {
        playerNameText.text = playerName;

        if (PhotonNetwork.LocalPlayer.ActorNumber != playerId) {
            playerReadyButton.gameObject.SetActive(false);
        } else {

            ExitGames.Client.Photon.Hashtable initProperties
                = new ExitGames.Client.Photon.Hashtable() { { Constants.PLAYER_READY, isPlayerReady } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(initProperties);

            playerReadyButton.onClick.AddListener(() => {               
                SetPlayerReady(!isPlayerReady);

                ExitGames.Client.Photon.Hashtable newProperties
                    = new ExitGames.Client.Photon.Hashtable() { { Constants.PLAYER_READY, isPlayerReady } };
                PhotonNetwork.LocalPlayer.SetCustomProperties(newProperties);
            });

            ExitGames.Client.Photon.Hashtable playerTeamProperties
                 = new ExitGames.Client.Photon.Hashtable() { { Constants.PLAYER_TEAM, isBlueTeam } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerTeamProperties);

            playerTeamButton.onClick.AddListener(() => {
                TogglePlayerTeam(!isBlueTeam);

                ExitGames.Client.Photon.Hashtable newPlayerTeamProperties
                    = new ExitGames.Client.Photon.Hashtable() { { Constants.PLAYER_TEAM, isBlueTeam } };
                PhotonNetwork.LocalPlayer.SetCustomProperties(newPlayerTeamProperties);
            });
        }   
    }

    public void SetPlayerReady(bool isReady) {
        isPlayerReady = isReady;
        playerReadyImage.enabled = isReady;

        if (isReady) {
            playerReadyButton.GetComponentInChildren<Text>().text = "Ready!";
        } else {
            playerReadyButton.GetComponentInChildren<Text>().text = "Ready?";
        }
    }

    public void TogglePlayerTeam(bool state) {
        isBlueTeam = state;
        ColorBlock newColor = playerTeamButton.colors;

        if (isBlueTeam) {
            playerTeamButton.GetComponentInChildren<Text>().text = "Blue";
            newColor.normalColor = Color.blue;
            newColor.selectedColor = Color.blue;
            newColor.highlightedColor = Color.blue;
        } else {
            playerTeamButton.GetComponentInChildren<Text>().text = "Red";        
            newColor.normalColor = Color.red;
            newColor.selectedColor = Color.red;
            newColor.highlightedColor = Color.red;
        }

        playerTeamButton.colors = newColor;
    }
}
