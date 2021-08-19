using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Attacking : MonoBehaviourPunCallbacks
{
    public Transform projectileSpawnLocation;
    public float attackRange;
    public float attackRate = 1.0f;

    private Unit owner;
    private SphereCollider sphereCollider;
    private Projectile projectileToUse;

    public bool inRange = false;
    private float coolDown = 0;

    private void Awake() {
        owner = GetComponentInParent<Unit>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void LateUpdate() {

        if (coolDown > 0) {
            coolDown -= Time.deltaTime;
        } else {
            if (!projectileToUse) return;

            if (!inRange) {
                owner.inCombat = false;
                return;
            }

                    
            if (owner.targets.Count > 0) {
                Unit newTarget = GetFirstTarget();

                if (newTarget == null) return;

                if (Vector3.Distance(newTarget.transform.position,
                    transform.position) <= sphereCollider.radius) {
                    owner.inCombat = true;
                    Shoot();

                    coolDown = 1.0f / attackRate;
                } else {
                    inRange = false;
                }
            }       
            
        }
    }

    public Unit GetFirstTarget() {
        owner.targets.RemoveAll(x => !x);

        for (int i = 0; i < owner.targets.Count; i++) {
            if (owner.targets[i] != null) {
                return owner.targets[i];
            }
        }

        inRange = false;
        owner.inCombat = false;
        return null;
    }

    public void SetProjectileToUse(Projectile setProjectile) {
        projectileToUse = setProjectile;
        SetAttackRange(attackRange);
    }

    public void SetAttackRange(float newRange) {
        sphereCollider.radius = newRange;
    }

    private void OnTriggerEnter(Collider other) {
        if (owner == null) return;
        if (other.TryGetComponent(out Unit unit)) {
            if (owner.targets.Count > 0) {
                inRange = true;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (owner == null) return;
        if (other.TryGetComponent(out Unit unit)) {
            if (unit != null) return;

            if (owner.targets.Contains(unit)) {
                owner.targets.Remove(unit);
            }
        }
    }


    public virtual void Shoot() {
        if (projectileToUse) {
            photonView.RPC("ProjectileSpawn", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void ProjectileSpawn() {
        Projectile spawnedProjectile;

        spawnedProjectile = Instantiate(projectileToUse,
            projectileSpawnLocation.position, projectileSpawnLocation.rotation);

        spawnedProjectile.Initialize(owner, owner.targets[0]);
    }
}
