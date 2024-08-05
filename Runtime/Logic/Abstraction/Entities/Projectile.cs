using System;
using MCB.Abstraction.Graph;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace MCB.Abstraction.Entities
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float mass;
        [SerializeField] private float drag;
        [ShowInInspector, ReadOnly] private Vector3 _velocity;
        private Rigidbody _rigidbody;
        private bool _manualMovement;
        private int _skipManualFrame = 0;
        private bool _wasVelocityChangedOutside;
        public float Mass => mass;
        public Vector3 Velocity => _velocity;
        public const float MaxRbSpeed = 200;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            if (_rigidbody)
            {
                _rigidbody.mass = mass;
                _rigidbody.drag = drag;
            }
        }

        public void Push(float impulse, Vector3 vector)
        {
            if (_rigidbody && !_manualMovement)
            {
                _velocity = _rigidbody.velocity + vector * impulse;
                _rigidbody.velocity = _velocity;
            }
            else
            {
                _velocity += vector * impulse;
            }

            _wasVelocityChangedOutside = true;
        }

        private void FixedUpdate()
        {
            if (_rigidbody && !_manualMovement)
            {
                _velocity = _rigidbody.velocity;
                ApplyDrag();
                _rigidbody.velocity = _velocity;
                
                DetermineManualMovement();
                if (_manualMovement)
                {
                    _rigidbody.isKinematic = true;
                }
            }
            else
            {
                MoveManually();
                DetermineManualMovement();
                if (!_manualMovement && _rigidbody)
                {
                    _rigidbody.isKinematic = false;
                }

                if (_rigidbody)
                {
                    _rigidbody.velocity = _velocity;
                }
            }
            _skipManualFrame--;
        }

        private void DetermineManualMovement()
        {
            _manualMovement = _velocity.sqrMagnitude > MaxRbSpeed * MaxRbSpeed && _skipManualFrame <= 0;
        }

        private RaycastHit[] _nonAllocHits = new RaycastHit[10];
        private void MoveManually()
        {
            _velocity += Vector3.down * (9.81f * Time.fixedDeltaTime);
            ApplyDrag();

            float maxDistance = _velocity.magnitude * Time.fixedDeltaTime;
            int steps = 0;
            while (StepOnTrigger(ref maxDistance) && steps < 10)
            {
                steps++;
            }
            _wasVelocityChangedOutside = false;
            if (_skipManualFrame <= 0)
            {
                transform.position += _velocity * Time.fixedDeltaTime;
            }
        }

        private bool StepOnTrigger(ref float remainsDistance)
        {
            if (remainsDistance < 1e-3)
            {
                return false;
            }
            _wasVelocityChangedOutside = false;
            var size = Physics.RaycastNonAlloc(transform.position, _velocity, _nonAllocHits, remainsDistance);
            for (int i = size; i < _nonAllocHits.Length; i++)
            {
                _nonAllocHits[i].distance = float.MaxValue;
            }
            _nonAllocHits.Sort((a, b) => a.distance.CompareTo(b.distance));
            for (int i = 0; i < size; i++)
            {
                if (_nonAllocHits[i].collider.isTrigger)
                {
                    if (_nonAllocHits[i].transform.TryGetComponent(out IPhysicsTriggerNode physicsTriggerNode))
                    {
                        physicsTriggerNode.TriggerEntered(transform);
                        remainsDistance -= Vector3.Distance(transform.position, _nonAllocHits[i].point);
                        Debug.DrawLine(transform.position, _nonAllocHits[i].point, Color.white, 1f);
                        transform.position = _nonAllocHits[i].point;
                        if (_wasVelocityChangedOutside)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    _skipManualFrame = 3;
                    return false;
                }
            }
            Debug.DrawRay(transform.position, _velocity.normalized * remainsDistance, Color.white, 1f);
            return false;
        }

        private void ApplyDrag()
        {
            Vector3 dragAcceleration = -(_velocity.magnitude * drag * Time.fixedDeltaTime / mass) * _velocity;
            _velocity += dragAcceleration;
        }
    }
}