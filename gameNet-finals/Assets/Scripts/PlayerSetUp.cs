using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSetUp : MonoBehaviourPunCallbacks 
{

    public PlayerUIManager playerUiManagerPrefab;
    public Hero hero;

    public PlayerUIManager pui;

    // Start is called before the first frame update
    void Start()
    {
        hero.Initialize(photonView.IsMine);

        if (photonView.IsMine) {
            pui = Instantiate(playerUiManagerPrefab);
            pui.StartTracking(hero);
        }      
    }

}
