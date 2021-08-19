using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerCamera : MonoBehaviour
{
    [Header("Set up")]
    public Vector3 offset;

    [SerializeField] private Camera heldCamera = null;
    private PositionConstraint posConstraint;

    private void Awake() {
        if (heldCamera == null) {
            heldCamera = GetComponentInChildren<Camera>();
        }
        posConstraint = GetComponent<PositionConstraint>();       
    }

    public void SetConstraintSource(Unit newTarget) {
        ConstraintSource constraintSource = new ConstraintSource {
            sourceTransform = newTarget.transform,
            weight = 1
        };
        posConstraint.translationOffset = offset;
        posConstraint.SetSource(0, constraintSource);
    }

    public Ray GetRayFromScreen(Vector3 position) {
        return heldCamera.ScreenPointToRay(position);
    }

    public void Move(Vector3 location) {
        transform.Translate(location.x, 0, location.y * 0.9f);
    }

    public void ToggleLock(bool state) {
        posConstraint.constraintActive = state;
    }
}
