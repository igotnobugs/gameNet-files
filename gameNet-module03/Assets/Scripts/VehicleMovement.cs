using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class VehicleMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public bool isControllable = false;
    public bool fourWheelDrive = false; // All four wheels are accelerating
    public Vector3 centerOfMass = new Vector3(0, 0, 0);

    [Header("Movement Settings")]
    public float acceleration = 3500.0f; // acceleration 
    public float maxVelocity = 20.0f; // maxSpeed
    public float steeringAngle = 50.0f; // Handling
    public float brakeForce = 1000.0f; // Brake fore

    [Header("Scale Settings Per 1")]
    public float scaleAcceleration = 350.0f; // acceleration 
    public float scaleMaxVelocity = 2.0f; // maxSpeed
    public float scaleSteeringAngle = 5.0f; // Handling


    [Header("AI Settings")]
    public bool isAiEnabled = false;
    public float distaneToNextWaypoint = 20.0f;
    public float brakeCollision = 5.0f; // Distance to break
    public float brakeAngle = 0.9f;

    [Header("Set Up")]
    public Rigidbody rigidBody;
    [SerializeField] private GameObject wheelColliders;
    [SerializeField] private WheelCollider[] steeringWheels = null;
    [SerializeField] private WheelCollider[] rearWheels = null;   
    [SerializeField] private Transform windshield = null;

    //
    private WaypointCircuit circuit;
    private int currentWaypointIndex;
    private Vector3 relativePos = new Vector3();
    private float forwardAngle = 0.0f;
    private float defaultBrakeForce = 0.0f;

    private void Start() {
        circuit = GameObject.FindGameObjectWithTag("Track").GetComponent<WaypointCircuit>();
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.centerOfMass = centerOfMass;
        defaultBrakeForce = brakeForce;
    }

    public void ScaleSettings(VehicleStats stats) {
        acceleration = scaleAcceleration * stats.acceleration;
        maxVelocity = scaleMaxVelocity * stats.maxSpeed;
        steeringAngle = Mathf.Clamp(scaleSteeringAngle * stats.handling, 10, 60);
       
    }


    private void Update() {
        if (!isAiEnabled) return; // Don't track waypoints length do this if not AI

        // Waypoint advancer
        if (circuit.Waypoints.Length <= 0) return;
        GameObject currentWaypoint = circuit.Waypoints[currentWaypointIndex].gameObject;
        Transform lookAhead = currentWaypoint.transform;
        Vector3 lookAt = new Vector3(lookAhead.position.x,
                            transform.position.y,
                            lookAhead.position.z);
        Vector3 direction = transform.position - lookAt;
        if (direction.magnitude < distaneToNextWaypoint) {
            currentWaypointIndex++;
            if (currentWaypointIndex >= circuit.Waypoints.Length) {
                currentWaypointIndex = 0;
            }
        }

        // Get angle towards waypoint
        relativePos = lookAhead.transform.position - transform.position;
        forwardAngle = Vector3.Angle(relativePos, transform.forward) / 180.0f;

        // Braking
        if (!isControllable) {
            if (forwardAngle > brakeAngle || IsAboutToCollide()) {
                brakeForce = defaultBrakeForce;
            } else {
                brakeForce = 0;
            }
        }

        // Steer left or right
        if (Vector3.Cross(transform.forward, relativePos).y < 0) forwardAngle *= -1;
        else Mathf.Abs(forwardAngle);


    }

    private void FixedUpdate() {
        float newAcceleration = acceleration;
        if (isControllable) {
            newAcceleration = acceleration * Input.GetAxis("Vertical");
            
            if (Input.GetAxis("Vertical") < 0 && rigidBody.velocity.magnitude > 1) {
                // Move Backwards
                brakeForce = defaultBrakeForce;         
            } else if (Input.GetAxis("Vertical") == 0) { 
                // Not pressing anything
                brakeForce = defaultBrakeForce * 10;
            } else {
                // Moving forwards
                brakeForce = 0;
            }
        }

        // Speed
        rigidBody.velocity = Vector3.ClampMagnitude(rigidBody.velocity, maxVelocity);

        for (int i = 0; i < rearWheels.Length; i++)
            rearWheels[i].motorTorque = newAcceleration;

        if (fourWheelDrive) {
            for (int i = 0; i < steeringWheels.Length; i++)
                steeringWheels[i].motorTorque = newAcceleration;

        }

        for (int i = 0; i < rearWheels.Length; i++)
            rearWheels[i].brakeTorque = brakeForce;
        for (int i = 0; i < steeringWheels.Length - 1; i++)
            steeringWheels[i].brakeTorque = brakeForce;

        float angle = steeringAngle;// * forwardAngle;

        // Steering
        if (isControllable) {
            angle *= Input.GetAxis("Horizontal");
        } else {
            angle *= forwardAngle;
        }

        for (int i = 0; i < steeringWheels.Length; i++)
            steeringWheels[i].steerAngle = angle;
    }

    public void StopMoving() {
        maxVelocity = 0;
    }

    public void DisableWheelCollisions() {
        wheelColliders.SetActive(false);
    }

    private bool IsAboutToCollide() {
        return Physics.Raycast(windshield.position, transform.forward, brakeCollision);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawSphere(transform.position - centerOfMass, 0.2f);
    }
}
