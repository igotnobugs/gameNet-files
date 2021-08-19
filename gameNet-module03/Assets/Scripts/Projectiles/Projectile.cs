using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Projectile : MonoBehaviour
{
    public bool isHoming = false;
    public Vehicle target;
    public float speed = 5.0f;
    public float lifeTime = 3.0f;
    public int instigatorId = 0;
    public float turnSpeed = 10;
    public int damage = 30;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifeTime);
    }

    public void Init(int shooterID, Vehicle targetvehicle = null ) {
        instigatorId = shooterID;

        if (targetvehicle != null) {
            target = targetvehicle;
            isHoming = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (target != null) {
            Vector3 targetDirection = target.transform.position - rb.position;
            targetDirection.Normalize();
            Vector3 rotationAmount = Vector3.Cross(transform.forward, targetDirection);
            rb.angularVelocity = rotationAmount * turnSpeed;          
        }

        rb.velocity = transform.forward * speed;
    }
    

    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.name);

        if (other.TryGetComponent(out Vehicle hit)) {

            if (hit.viewId == instigatorId) return;

            hit.gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.AllBuffered, instigatorId,
                damage);

            Debug.Log(hit.gameObject.name);

            Destroy(gameObject);
            return;
        } else if (other.gameObject.tag == "Ground") {

            Debug.Log(gameObject);
            Destroy(gameObject);
        }


    }
}
