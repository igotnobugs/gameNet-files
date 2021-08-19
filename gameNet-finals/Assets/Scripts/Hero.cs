using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
// Main player class

public enum HeroEventCode {
    Eliminated = 11,
    Spawned = 12,
    LevelUp = 12
}

public class Hero : Unit
{
    [Header("Player")]
    public string playerName = "Anonymous";

    [Header("Hero")]
    public HeroData heroData;


    private Vector3 spawnedLocation;

    public override void UnitStart() {
        base.UnitStart();

    }


    public void Initialize(bool controllable) {
        if (heroData == null) return;
        health.Initialize(heroData.health);
        if (controllable) {
            (control as PlayerControl).EnableControl();
        }
    }


    public override void GainExperience() {
        base.GainExperience();

        experience += 0.1f;
        if (experience >= 1.0f) {
            Debug.Log("Level up!");
            experience = 0;
            level++;
            attacking.attackRange += 2;
            attacking.attackRate += 0.5f;

            //Raise Event
            object[] data = new object[] { photonView.Owner.NickName };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions {
                Receivers = ReceiverGroup.All,
                CachingOption = EventCaching.AddToRoomCache
            };
            SendOptions sendOptions = new SendOptions {
                Reliability = false
            };
            PhotonNetwork.RaiseEvent((byte)HeroEventCode.LevelUp,
                data, raiseEventOptions, sendOptions);

        }
    }

    public override void Death() {
        if (photonView.IsMine) {
            isDead = true;
            control.controlled = false;
            targeting.enabled = false;
            attacking.enabled = false;
            (control as PlayerControl).ToggleTrackPlayerHero(false);
            StartCoroutine(RespawnCoundown());
        }
        transform.position = new Vector3(0, 0, -99999.0f);

        //Raise Event
        object[] data = new object[] { photonView.Owner.NickName };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };
        SendOptions sendOptions = new SendOptions {
            Reliability = false
        };
        PhotonNetwork.RaiseEvent((byte)HeroEventCode.Eliminated,
            data, raiseEventOptions, sendOptions);
    }

    IEnumerator RespawnCoundown() {

        float respawnTime = 2.0f;
        while (respawnTime > 0) {
            Debug.Log(respawnTime);
            yield return new WaitForSeconds(1.0f);
            respawnTime--;
        }

        control.controlled = true;
        targeting.enabled = true;
        attacking.enabled = true;
        (control as PlayerControl).ToggleTrackPlayerHero(true);
        

        if (isBlueSide) {
            spawnedLocation = gm.blueSpawnPoint.transform.position;
        } else {
            spawnedLocation = gm.redSpawnPoint.transform.position;
        }
        isDead = false;
        photonView.RPC("Respawned", RpcTarget.AllBuffered);
        (control as PlayerControl).Move(spawnedLocation);



        //Raise Event
        object[] data = new object[] { photonView.Owner.NickName };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };
        SendOptions sendOptions = new SendOptions {
            Reliability = false
        };
        PhotonNetwork.RaiseEvent((byte)HeroEventCode.Spawned,
            data, raiseEventOptions, sendOptions);

        
    }


    [PunRPC]
    public void Respawned() {
        health.currentHealth = health.maxHealth;
        transform.position = spawnedLocation;
        
    }

}
