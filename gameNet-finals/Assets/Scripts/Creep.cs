using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
// npc

public class Creep : Unit
{

    public Barracks targetBarracks;

    public override void UnitStart() {
        base.UnitStart();

    }


    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public void SetTarget() {

    }
}
