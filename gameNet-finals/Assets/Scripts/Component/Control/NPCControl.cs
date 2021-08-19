using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCControl : Control
{
    private Creep ownerCreep;

    private bool isMovingTowardsBarracks = false;

    private void Start() {
        ownerCreep = owner as Creep;
    }

    void Update() {
        if (owner.inCombat) {
            if (owner.targets[0] != null) {
                Follow(owner.targets[0]);

                if (isMovingTowardsBarracks) {                 
                    isMovingTowardsBarracks = false;
                }
            }
        } else {
            if (!isMovingTowardsBarracks) {
                if (ownerCreep.targetBarracks != null) {
                    Move(ownerCreep.targetBarracks.transform.position);
                }
                isMovingTowardsBarracks = true;
            }
        }
    }
}
