using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;


public class Control : MonoBehaviour
{
    public Unit owner;
    public bool controlled = false;

    protected NavMeshAgent agent;

    // Start is called before the first frame update
    private void Awake() {
        owner = GetComponentInParent<Unit>();
        agent = GetComponentInParent<NavMeshAgent>();   
    }

    public void Follow(Unit target) {
        if (target == null) return;
        Vector3 location = target.transform.position;
        owner.photonView.RPC("PunMove", RpcTarget.AllBuffered, location);
    }

    public void Move(Vector3 location) {
        owner.photonView.RPC("PunMove", RpcTarget.AllBuffered, location);
    }

    public void AttackMove(Vector3 location) {
        owner.isAttackMode = true;
        owner.photonView.RPC("PunMove", RpcTarget.AllBuffered, location);
    }

    public void Stop() {
        owner.photonView.RPC("PunStop", RpcTarget.AllBuffered);
    }

    public void Go() {
        owner.photonView.RPC("PunGo", RpcTarget.AllBuffered);
    }

    // Called from unit
    public void UnitMove(Vector3 location) {
        if (owner.isDead) return;
        agent.destination = location;
    }

    public void UnitStop() {
        agent.isStopped = true;
    }

    public void UnitGo() {
        agent.isStopped = false;
    }
}
