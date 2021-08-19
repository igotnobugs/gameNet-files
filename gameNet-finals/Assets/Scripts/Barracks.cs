using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
public enum BarracksEventCode {
    Destroyed = 21
}

public class Barracks : Unit
{

    [Header("Set up")]
    public Transform spawnPoint;
    public Barracks targetCore;
    public int spawnCount = 3;
    public Creep creepPrefab;
    public float waveTimer = 5.0f;
    public float spawnDelay = 0.33f;

    public bool spawnCreep = true;
    public override void UnitStart() {
        base.UnitStart();

        if (isBlueSide) {
            targetCore = GameObject.FindGameObjectWithTag("Red Barracks").GetComponent<Barracks>();
        } else {
            targetCore = GameObject.FindGameObjectWithTag("Blue Barracks").GetComponent<Barracks>();
        }

        if (PhotonNetwork.IsMasterClient) {
            if (spawnCreep) {
                StartSpawning();
            }
        }
    }

    public override void Death() {     
        //Raise Event
        object[] data = new object[] { isBlueSide };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };
        SendOptions sendOptions = new SendOptions {
            Reliability = false
        };
        PhotonNetwork.RaiseEvent((byte)BarracksEventCode.Destroyed,
            data, raiseEventOptions, sendOptions);

        base.Death();
    }


    protected void StartSpawning() {
        StartCoroutine("SpawnSystem");
    }

    IEnumerator SpawnSystem() {

        for(; ; ) {
            for (int i = 0; i < spawnCount; i++) {
                if (PhotonNetwork.IsMasterClient) {
                    SpawnCreep();
                }
                yield return new WaitForSeconds(spawnDelay);
            }
            yield return new WaitForSeconds(waveTimer);
        }     
    }

    private void SpawnCreep() {
        GameObject spawnedCreep = PhotonNetwork.Instantiate(creepPrefab.name, spawnPoint.position, Quaternion.identity);
        if (targetCore != null) {
            spawnedCreep.GetComponent<Creep>().targetBarracks = targetCore;
            spawnedCreep.GetComponent<Creep>().control.AttackMove(targetCore.transform.position);
            
        } else {
            StopCoroutine("SpawnSystem");
        }

    }
}
