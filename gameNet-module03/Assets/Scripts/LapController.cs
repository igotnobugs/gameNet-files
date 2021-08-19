using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class LapController : MonoBehaviourPunCallbacks
{
    public LapTrigger nextLap;
    public int lapsRequired = 3;
    public int currentLaps = 0;
    public bool isGameFinish = false;

    private int finishOrder = 0;

    private Vehicle myCar;

    public enum LapEventsCode {
        WhoFinishedEventCode = 0
    }

    private void Awake() {
        myCar = GetComponent<Vehicle>();
    }

    public override void OnEnable() {
        base.OnEnable();
        PhotonNetwork.NetworkingClient.EventReceived += OnLapEvent;
    }

    public override void OnDisable() {
        base.OnDisable();
        PhotonNetwork.NetworkingClient.EventReceived -= OnLapEvent;
    }

    // Start is called before the first frame update
    void Start()
    {
        nextLap = myCar.rgm.firstLapTrigger;
    }

    public void OnTriggerEnter(Collider other) {
        if (isGameFinish) return;

        if (other.TryGetComponent(out LapTrigger trigger)) {
            if (trigger == nextLap) {
                nextLap = trigger.nextLapTrigger;

                if (trigger.isFinishLine) currentLaps++;

                if (currentLaps >= lapsRequired) {                   
                    GameFinish();
                }
            }
        }
    }

    public void GameFinish() {
        if (!photonView.IsMine) return;
        currentLaps = 3;
        GetComponent<VehicleMovement>().isControllable = false;
        GetComponent<VehicleMovement>().isAiEnabled = true;
        isGameFinish = true;

        string nickName = photonView.Owner.NickName;
        finishOrder++;

        myCar.rpui.DisplayWinPanel(finishOrder);

        object[] data = new object[] { nickName, finishOrder };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };
        SendOptions sendOptions = new SendOptions {
            Reliability = false
        };
        PhotonNetwork.RaiseEvent((byte)LapEventsCode.WhoFinishedEventCode, 
            data, raiseEventOptions, sendOptions);
    }

    void OnLapEvent(EventData photonEvent) {
        if (photonEvent.Code == (byte)LapEventsCode.WhoFinishedEventCode) {
            object[] data = (object[])photonEvent.CustomData;

            string nickNameOfFinishedPlayer = (string)data[0];
            finishOrder = (int)data[1];

            Debug.Log(nickNameOfFinishedPlayer + " finished at order " + finishOrder);

            if (finishOrder == 1) {
                myCar.rgm.AnnouceFirstPlacer(nickNameOfFinishedPlayer);
            } else if (finishOrder == 2) {
                myCar.rgm.AnnouceSecondPlacer(nickNameOfFinishedPlayer);
            } else if (finishOrder == 3) {
                myCar.rgm.AnnouceThirdPlacer(nickNameOfFinishedPlayer);
            }
        }
    }
}
