using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Unit
{
    public class UnitMovement : MonoBehaviour
    {
        [Title("Data")]
        public bool isRoaming;
        public bool isOrbiting;
        
        [HideInInspector]public UnitsManager manager;

        private float _timeLeft;
        private Vector2 _direction;
        private Rigidbody2D _rb;

        public int orbitDirection;
        private float _orbitTime;
        private float _orbitRadius;
        public Transform orbitingAround;
        private Animator _unitMovementAnim;
        private static readonly int ExitOrbit = Animator.StringToHash("ExitOrbit");
        private static readonly int EnterOrbit = Animator.StringToHash("EnterOrbit");

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _unitMovementAnim = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            if(isRoaming)
                Roaming();
            else if(isOrbiting)
                Orbiting();

            _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, manager.velocityMax);
        }

        public void SetToRoam()
        {
            isRoaming = true;
            isOrbiting = false;
        }

        public void SetToOrbit()
        {
            isOrbiting = true;
            isRoaming = false;
        }

        public void SetOrbitingAround(Unit orbitingAroundObj, float howLong)
        {
            if (orbitingAroundObj == null)
                return;
            orbitingAround = orbitingAroundObj.transform;
            _orbitTime = howLong;
            _orbitRadius = Vector3.Distance(this.orbitingAround.position, transform.position);
            _unitMovementAnim.SetTrigger(EnterOrbit);
        }

        private float GetTheta(Vector2 position)
        {
            var direction = (Vector2)transform.position - position;
            return Mathf.Atan2(direction.y, direction.x);
        }

        private void Orbiting()
        {
            _orbitTime -= Time.deltaTime; 

            _orbitRadius = Mathf.Abs( manager.orbitDesignedRadius - _orbitRadius) <= 0.1 ? 
                manager.orbitDesignedRadius 
                : Mathf.Lerp(_orbitRadius, manager.orbitDesignedRadius, Time.deltaTime);
            
            if (_orbitTime <= 0 || orbitingAround == null)
            {
                _unitMovementAnim.SetTrigger(ExitOrbit);
                return;
            }

            var orbitingAroundPosition = orbitingAround.position;

            var theta = GetTheta(orbitingAroundPosition) + manager.orbitThetaChangeSpeed * orbitDirection;
            
            var orbitDelta = orbitingAroundPosition + new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0) * _orbitRadius;
            var deltaDirection = orbitDelta - transform.position;
            _rb.AddForce(deltaDirection.normalized * manager.orbitSpeed* Time.deltaTime, ForceMode2D.Impulse);
        }

        private void Roaming()
        {
            _timeLeft -= Time.deltaTime;
            if (_timeLeft > 0) return;
            _direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            _timeLeft += manager.roamingShiftBtwTime;
            _rb.AddForce(_direction * manager.speed, ForceMode2D.Impulse);
        }

        public void ApplyOutsideForce(Vector2 direction, float magnitude, bool killForce = false, bool setDirection = false)
        {
            if (killForce)
            {
                _rb.velocity = Vector2.zero;
                _rb.angularVelocity = 0f;
            }

            if (setDirection)
            {
                _direction = direction;
            }

            _rb.AddForce(direction * magnitude, ForceMode2D.Impulse);
        }
    }
}