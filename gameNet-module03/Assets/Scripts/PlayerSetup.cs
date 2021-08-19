using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public Camera cam;
    public Text timerText;
    public float timeToStartRace = 3.0f;
    public bool raceStarted = false;

    public RacingGameManager rgm;
    public RacePlayerUI RacePlayerUIPrefab;
    public RacePlayerUI rpui;

    [Header("Set Up")]
    public Vehicle vehicle;

    public bool isDeathRace = false;

    private void Awake() {
        vehicle = GetComponent<Vehicle>();
        rgm = GameObject.Find("GameManager").GetComponent<RacingGameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<VehicleMovement>().enabled = false;
        cam.enabled = photonView.IsMine;

        if (photonView.IsMine) {
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("racing")) {
                rpui = Instantiate(RacePlayerUIPrefab);
                rpui.SetUpUI(vehicle, false);
                isDeathRace = false;

            } else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("deathRace")) {
                rpui = Instantiate(RacePlayerUIPrefab);
                rpui.SetUpUI(vehicle, true);
                isDeathRace = true;
            }
            vehicle.SetupPlayer(rpui);           
        }

        timerText = vehicle.rgm.timeText;

    }

    // Update is called once per frame
    void Update()
    {     
        if (raceStarted) return;
        if (!PhotonNetwork.IsMasterClient) return;

        if (timeToStartRace > 0) {
            timeToStartRace -= Time.deltaTime;
            photonView.RPC("SetTime", RpcTarget.AllBuffered, timeToStartRace);

        } else if (timeToStartRace <= 0) {
            photonView.RPC("StartRace", RpcTarget.AllBuffered);
            raceStarted = true;
        }


    }

    [PunRPC]
    public void SetTime(float time) {
        if (timerText == null) return;

        if (time > 0) {
            timerText.text = time.ToString("F1");
        } else {
            timerText.text = "";
        }
    }

    [PunRPC]
    public void StartRace() {
        GetComponent<VehicleMovement>().enabled = true;
        GetComponent<VehicleMovement>().isControllable = photonView.IsMine;
        rgm.DisableTimePanel();
        rgm.raceStarted = true;

        vehicle.weaponController.weaponsActive = isDeathRace;
    }


}
