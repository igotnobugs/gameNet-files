using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

[System.Serializable]
public struct VehicleStats {
    public string name;
    public string description;

    [Header("Main Stats")]
    [Min(1)] public int maxHealth;

    [Range(1, 100)] public int acceleration; // Acceleration to Speed
    [Range(1, 100)] public int maxSpeed; // maxSpeed
    [Range(1, 100)] public int handling; // steering Angle
    [Range(1, 100)] public int mass; // weight
}

public class Vehicle : MonoBehaviourPunCallbacks
{
    [Header("Other")]
    public bool isForGarageShow = false; // Prevents set up and some other things
    public bool isCombatEnabled = false; // Disable weapons
    public bool useScaleSettings = true;

    [Header("Player Stat")]
    public string playerName = "Unknown";
    public bool isDead = false;

    public VehicleStats vehicleStats;

    [Header("For Combat")]
    public int health = 100;

    [Header("Set Up")]
    public GameObject carModel;
    public Camera playerCamera;
    public RacingGameManager rgm;
    public VehicleMovement vehicleMovement;
    public WeaponController weaponController;
    public VehicleWidget vehicleWidget;

    [Header("UI")]
    public RacePlayerUI rpui;

    [Header("Pun")]
    public int viewId;

    public enum VehicleEventsCode {
        Eliminated = 1
    }

    public override void OnEnable() {
        base.OnEnable();
        PhotonNetwork.NetworkingClient.EventReceived += OnVehicleDeathEvent;
    }

    public override void OnDisable() {
        base.OnDisable();
        PhotonNetwork.NetworkingClient.EventReceived -= OnVehicleDeathEvent;
    }

    private void Awake() {
        if (isForGarageShow) return;
        rgm = GameObject.Find("GameManager").GetComponent<RacingGameManager>();
        viewId = photonView.ViewID;
    }

    private void Start() {
        if (isForGarageShow) return;

        if (weaponController != null)
            weaponController.weaponsActive = isCombatEnabled;

        if (vehicleMovement != null) {
            vehicleMovement.isControllable = false;
            if (useScaleSettings) {
                vehicleMovement.ScaleSettings(vehicleStats);
            }
        }       
    }


    //Only called by each of the client
    public void SetupPlayer(RacePlayerUI setUi) {
        rpui = setUi;
        playerName = PhotonNetwork.NickName;
        vehicleWidget.gameObject.SetActive(false);
        photonView.RPC("NotifyPlayerSpawned", RpcTarget.AllBuffered,
            playerName);      
    }

    [PunRPC] // Notify everyone that this player has spawned
    public void NotifyPlayerSpawned(string name, PhotonMessageInfo info) {
        playerName = name;       
        AddThisToGm(name); 
    }

    // Add the gameObject to each client's gm
    public void AddThisToGm(string name) {
        Debug.Log(name + "has spawned");
        vehicleWidget.SetUp(rgm.isDeathRace);
        rgm.players.Add(this);
    }

    public void GetStartingPosition() {
        transform.position = rgm.GetNewPosition();
    }

    [PunRPC]
    public void TakeDamage(int instigator, int damage, PhotonMessageInfo info) {

        if (health <= 0) return; //already dead;

        health -= damage;
        vehicleWidget.healthBar.fillAmount = health / (float)vehicleStats.maxHealth;

        if (health <= 0) {
            Debug.Log(instigator + " " + photonView.ViewID);
            GameObject killer = PhotonView.Find(instigator).gameObject;
            vehicleWidget.HideHealth();

            if (killer != gameObject) {
                killer.GetComponent<Vehicle>().AddKill(info.photonView.Owner.NickName);
            } else {
                AddKill(playerName); //Do this regardless
            }

            Die();
        }
    }

    public void AddKill(string victim = "None") {
        rgm.CreateKillFeed(playerName, victim);
    }


    public void Die() {
        //Debug.Log(vehicleStats.name + " is destroyed");
        isDead = true;
        rgm.players.Remove(this);
        carModel.SetActive(false);
        GetComponent<BoxCollider>().enabled = false;
        vehicleWidget.gameObject.SetActive(false);

        if (photonView.IsMine) {
            vehicleMovement.isControllable = false;
            vehicleMovement.StopMoving();         
            rpui.DisplayDeathPanel();
        }

        vehicleMovement.DisableWheelCollisions();
        weaponController.DisableWeapons();

        rgm.CheckLastManStanding();

        //Raise Event
        object[] data = new object[] { photonView.Owner.NickName };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };
        SendOptions sendOptions = new SendOptions {
            Reliability = false
        };
        PhotonNetwork.RaiseEvent((byte)VehicleEventsCode.Eliminated,
            data, raiseEventOptions, sendOptions);
    }


    void OnVehicleDeathEvent(EventData photonEvent) {
        if (photonEvent.Code == (byte)VehicleEventsCode.Eliminated) {
            object[] data = (object[])photonEvent.CustomData;

            string eliminatedPlayer = (string)data[0];

            Debug.Log(eliminatedPlayer + " eliminated event!");

        }
    }

    //Debug
    public void Suicide() {
        photonView.RPC("TakeDamage", RpcTarget.AllBuffered, viewId, 9999);
    }
}
