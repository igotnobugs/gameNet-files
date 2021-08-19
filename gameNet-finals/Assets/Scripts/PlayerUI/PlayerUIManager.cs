using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{

    public PlayerUI[] playerInterfaces;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartTracking(Hero trackedHero) {
        foreach(PlayerUI pui in playerInterfaces) {
            pui.SetTrackedHero(trackedHero);
        }
    }
}
