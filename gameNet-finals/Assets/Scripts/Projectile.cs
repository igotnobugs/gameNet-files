using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    public Unit target;
    public float damage = 1;
    public float speed;
    public float turnSpeed;
    public float lifeTime = 2.0f;
    public int instigatorId = 0;

    private Rigidbody rb;
    private Unit instigatorUnit;


    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Start() {
        Destroy(gameObject, lifeTime);
    }

    public void Initialize(Unit shooter, Unit targetUnit = null) {
        instigatorUnit = shooter;

        if (targetUnit != null) {
            target = targetUnit;
        }
    }

    private void FixedUpdate() {
        if (target != null) {

            if (target.isDead) {
                Destroy(gameObject);
                return;
            }

            if (instigatorUnit != null) {
                Vector3 targetDirection = target.centerMass.transform.position - rb.position;
                targetDirection.Normalize();
                Vector3 rotationAmount = Vector3.Cross(transform.forward, targetDirection);
                rb.angularVelocity = rotationAmount * turnSpeed;
            }

        } else {
            Destroy(gameObject);
        }

        rb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider collider) {    
        if (collider.gameObject.TryGetComponent(out Unit unit)) {
            if (unit == target) {
                unit.health.Damage(damage);
                Destroy(gameObject);
            }       
            return;
        } else if (collider.gameObject.tag == "Ground") {
            Destroy(gameObject);
        }
    }
}
