using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
public class PlayerUI : MonoBehaviour
{
    [Header("UI Control Setup")]   
    public Joystick moveJoystick;
    public FixedTouchField lookTouchField;    
    public Button fireButton;

    [Header("Status Setup")]
    public Image healthBar;

    [Header("Respawn Panel")]
    public GameObject respawnPanel;
    public Text respawnText;
    
    [Header("Kill Feed")]
    public KillFeedList killFeedListPrefab;
    public GameObject killFeedContent;
    public TextMeshProUGUI killCount;

    [Header("Winner Panel")]
    public GameObject winnerPanel;
    public Text winnerNameText;
    public Text countdownLobbyText;

    [Header("Debug Panel")]
    public GameObject debugPanel;

    private PlayerUnit owner;

    public void Start() {
        debugPanel.SetActive(PhotonNetwork.IsMasterClient);
        winnerPanel.SetActive(false);
        respawnPanel.SetActive(false);
    }

    private void Update() {
        if (owner == null) return;
        killCount.text = owner.kills.ToString();
        healthBar.fillAmount = owner.GetHealth() / owner.startHealth;
    }

    public void SetOwner(PlayerUnit newOwner) {
        owner = newOwner;
    }

    public void AddToKillFeed(string killer, string victim) {
        KillFeedList listItem = Instantiate(killFeedListPrefab);
        listItem.transform.SetParent(killFeedContent.transform);
        listItem.transform.localScale = Vector3.one;

        listItem.SetUp(killer, victim);
    }

    public void ShowWinner(string name) {
        winnerPanel.SetActive(true);
        winnerNameText.text = name + " won!";
    }


    // Debug
    public void KillSelf() {
        owner.KillSelf();
    }

    public void KickAll() {
        owner.gm.Exit();
    }

    public void TeleportToPlayer() {
        owner.TeleportRandomlyToAnyone();
    }

    public void AddKill() {
        owner.AddKill();
    }
}
