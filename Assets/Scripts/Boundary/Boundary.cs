using System;
using Unit;
using UnityEngine;

namespace Boundary
{
    public class Boundary : MonoBehaviour
    {
        private UnitsManager _unitsManager;

        private void Awake()
        {
            _unitsManager = FindObjectOfType<UnitsManager>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var unit = other.gameObject.GetComponent<Unit.UnitMovement>();

            if (unit is null) return;
            
            var contactPoint = other.GetContact(0);
            var direction = (Vector2)unit.transform.position - contactPoint.point;
            unit.ApplyOutsideForce(direction, _unitsManager.bounceEdgeMagnitude, false, true);
        }
    }
}
