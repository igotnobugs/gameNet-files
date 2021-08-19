using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    private Unit owner;
    private SphereCollider sphereCollider;

    private void Awake() {
        owner = GetComponentInParent<Unit>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other) {
        if (owner == null) return;
        if (other.TryGetComponent(out Unit unit)) {
            if (unit == this) return;
            if (unit == owner) return;
            if (owner.isBlueSide == unit.isBlueSide) return;
            owner.AddTarget(unit);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (owner == null) return;
        if (other.TryGetComponent(out Unit unit)) {
            if (unit == this) return;
            if (unit == owner) return;
            owner.RemoveTarget(unit);
        }
    }


    public void SetTargetingRange(float newRange) {
        sphereCollider.radius = newRange;
    }
}
