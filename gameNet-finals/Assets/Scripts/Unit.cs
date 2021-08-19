using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
// Has health, abilities, stats, movement

public class Unit : MonoBehaviourPunCallbacks
{
    [Header("Basic Unit Stats")]
    public string unitName;
    public Projectile basicAttack;
    public int level = 1;
    public float mana = 10.0f;
    public float experience = 0.0f;
    public float aggroRange = 10.0f;

    [Header("Basic Unit States")]
    public bool isAttackMode = false;
    public bool isBlueSide = false;
    public bool inCombat = false;
    public bool isDead = false;
    public bool canLevelUp = false;

    [Header("Basic Unit Component")]
    public Transform centerMass;
    public Health health;
    public Targeting targeting;
    public Attacking attacking;
    public Control control;

    [Header("Basic AI")]
    //public Unit currentTarget;
    public List<Unit> targets;
    public GameManager gm;

    private void Awake() {
        health = GetComponent<Health>();
        targeting = GetComponentInChildren<Targeting>();
        attacking = GetComponentInChildren<Attacking>();
        targets = new List<Unit>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    
    public void Start() {
        health.OnZeroHealth += Death;

        if (targeting != null)
            targeting.SetTargetingRange(aggroRange);

        if (attacking != null)
            attacking.SetProjectileToUse(basicAttack);

        UnitStart();
    }

    public virtual void UnitStart() { }

    public void AddTarget(Unit newTarget) {
        targets.Add(newTarget);
    }

    public void RemoveTarget(Unit removeTarget) {
        targets.Remove(removeTarget);
    }

    public void Attack(Unit targetUnit) {
        targets.Remove(targetUnit);
        targets.Insert(0, targetUnit);
        if (canLevelUp) {
            targetUnit.health.OnZeroHealth += GainExperience;
        }
    }

    public virtual void GainExperience() {

    }

    public virtual void Death() {
        isDead = true;
        Destroy(gameObject);
    }


    // Pun Calls to various components
    [PunRPC]
    public void PunMove(Vector3 location) {
        control.UnitMove(location);
    }

    [PunRPC]
    public void PunStop() {
        control.UnitStop();
    }

    [PunRPC]
    public void PunGo() {
        control.UnitGo();
    }
}
