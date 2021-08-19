using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RacePlayerUI : MonoBehaviour
{
    [Header("Track")]
    public Text lapText;
    public GameObject finishPanel;
    public GameObject firstPanel;
    public TextMeshProUGUI firstPlaceText;
    public GameObject secondPanel;
    public TextMeshProUGUI secondPlaceText;
    public GameObject thirdPanel;
    public TextMeshProUGUI thirdPlaceText;

    [Header("Vehicle")]
    public Text speedText;

    [Header("Death Race")]
    public GameObject healthPanel;
    public Text healthText;
    public GameObject killFeedPanel;
    public GameObject killFeedContent;
    public KillFeedList killFeedPrefab;

    [Header("You Win")]
    public GameObject winPanel;
    public TextMeshProUGUI winText;

    [Header("You Win")]
    public GameObject winDeathPanel;

    [Header("You Died")]
    public GameObject deadPanel;

    [Header("Spectator Mode")]
    public GameObject spectatorPanel;
    public TextMeshProUGUI spectatingNameText;

    private Vehicle trackedVehicle;
    private VehicleMovement trackedMovement;
    private LapController trackedLapController;
    private Vehicle spectateVehicle;
    private int playerIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        finishPanel.SetActive(false);
        winPanel.SetActive(false);
        deadPanel.SetActive(false);
        spectatorPanel.SetActive(false);
        winDeathPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (trackedVehicle == null) return;

        //Laps
        lapText.text = trackedLapController.currentLaps.ToString() 
            + " / " 
            + trackedLapController.lapsRequired.ToString();

        // Speed
        speedText.text = trackedMovement.rigidBody.velocity.magnitude.ToString("F2");
        // Health
        healthText.text = trackedVehicle.health.ToString();
    }

    public void SetUpUI(Vehicle vehicle, bool deathMode = false) {
        trackedVehicle = vehicle;
        trackedMovement = vehicle.vehicleMovement;
        trackedLapController = vehicle.GetComponent<LapController>();

        healthPanel.SetActive(deathMode);
        killFeedPanel.SetActive(deathMode);
    }

    public void AddToKillFeed(string killer, string victim) {
        KillFeedList listItem = Instantiate(killFeedPrefab);
        listItem.transform.SetParent(killFeedContent.transform);
        listItem.transform.localScale = Vector3.one;

        listItem.SetUp(killer, victim);
    }

    public void SetFirstPlacer(string name) {
        finishPanel.SetActive(true);
        firstPanel.SetActive(true);
        firstPlaceText.text = name;
    }

    public void SetSecondPlacer(string name) {
        secondPanel.SetActive(true);
        secondPlaceText.text = name;
    }

    public void SetThirdPlacer(string name) {
        thirdPanel.SetActive(true);
        thirdPlaceText.text = name;
    }

    public void DisplayWinPanel(int order) {
        winPanel.SetActive(true);
        if (order == 1) {
            winText.text = "You got First Place!";
        } else if (order == 2) {
            winText.text = "You got Second Place!";
        } else if (order == 3) {
            winText.text = "You got Third Place!";
        }      
    }

    public void DisplayWinDeathPanel() {
        winDeathPanel.SetActive(true);
    }


    public void DisplayDeathPanel() {
        deadPanel.SetActive(true);
    }

    public void ExitGame() {
        trackedVehicle.rgm.ExitGame();
    }

    public void UseSpectatorMode() {
        spectatorPanel.SetActive(true);
        winPanel.SetActive(false);
        deadPanel.SetActive(false);
        TrackVehicle(playerIndex);
    }

    public void GoToNextPlayer() {
        playerIndex++;
        if (playerIndex > trackedVehicle.rgm.players.Count - 1) {
            playerIndex = 0;
        }
        TrackVehicle(playerIndex);
    }

    public void GoToPreviousPlayer() {
        playerIndex--;
        if (playerIndex < 0) {
            playerIndex = trackedVehicle.rgm.players.Count - 1;
        }
        TrackVehicle(playerIndex);
    }

    public void TrackVehicle(int index) {       
       
        trackedVehicle.playerCamera.enabled = false;

        if (trackedVehicle.rgm.players.Count <= 0) {     
            trackedVehicle.rgm.overviewCamera.SetActive(true);
            return;
        }

        trackedVehicle.rgm.overviewCamera.SetActive(false);
        spectateVehicle = trackedVehicle.rgm.players[playerIndex];
        spectateVehicle.playerCamera.enabled = true;
        spectatingNameText.text = spectateVehicle.playerName.ToString();

        trackedMovement = spectateVehicle.vehicleMovement;
        trackedLapController = spectateVehicle.GetComponent<LapController>();
    }

    // Debug
    public void DebugKillSelf() {
        trackedVehicle.Suicide();
    }

    public void DebugWin() {
        trackedVehicle.GetComponent<LapController>().GameFinish();
    }

    public void ToggleAI() {
        trackedVehicle.GetComponent<VehicleMovement>().isControllable = !trackedVehicle.GetComponent<VehicleMovement>().isControllable;
        trackedVehicle.GetComponent<VehicleMovement>().isAiEnabled = !trackedVehicle.GetComponent<VehicleMovement>().isAiEnabled;
    }
}
