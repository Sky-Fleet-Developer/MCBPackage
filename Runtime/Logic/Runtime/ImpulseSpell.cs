using System;
using System.Collections.Generic;
using MCB.Abstraction;
using MCB.Abstraction.Entities;
using MCB.Abstraction.Graph;
using MCB.Utilities;
using Unity.Collections;
using UnityEngine;

namespace MCB.Runtime
{
    public class ImpulseSpell : Spell
    {
        [SerializeField] private InputPort<Transform> body;
        [SerializeField] private InputPort<Vector3> direction;
        [SerializeField] private InputPort<float> wantedSpeed;
        private IGraph _graph;

        public const float KPD = 1f;

        private void Awake()
        {
            InputAction.Subscribe(ApplyForceToBodies);
        }

        public override void Attach(IGraph graph)
        {
            _graph = graph;
            base.Attach(graph);
            graph.RegisterPort("Body", this, body);
            graph.RegisterPort("Direction", this, direction);
            graph.RegisterPort("WantedSpeed", this, wantedSpeed);
        }

        private void ApplyForceToBodies()
        {
            if (!body.HasValue)
            {
                return;
            }
            
            var target = body.GetValue();
            Vector3 impulseDirection;
            if (direction.HasValue)
            {
                impulseDirection = direction.GetValue().normalized;
            }
            else
            {
                impulseDirection = transform.forward;
            }


            if (target.TryGetComponent(out Projectile projectile))
            {
                float power;
                float consumption;
                if (wantedSpeed.HasValue)
                {
                    float wantedSpeedValue = wantedSpeed.GetValue();
                    consumption = projectile.Mass * wantedSpeedValue * wantedSpeedValue / KPD * 0.5f;
                    power = this.ConsumeMana(_graph, consumption, 1);
                }
                else
                {
                    consumption = Consumption;
                    power = this.ConsumeMana(_graph, 1);
                }
                projectile.Push(Mathf.Sqrt(KPD * consumption * 2 * power / projectile.Mass), impulseDirection);
            }
            else if (target.TryGetComponent(out Rigidbody rigidbody))
            {
                float power;
                float consumption;
                if (wantedSpeed.HasValue)
                {
                    float wantedSpeedValue = wantedSpeed.GetValue();
                    consumption = rigidbody.mass * wantedSpeedValue * wantedSpeedValue / KPD * 0.5f;
                    power = this.ConsumeMana(_graph, consumption, 1);
                }
                else
                {
                    consumption = Consumption;
                    power = this.ConsumeMana(_graph, 1);
                }
                rigidbody.AddForce(impulseDirection * Mathf.Sqrt(KPD * consumption * 2 * power / rigidbody.mass), ForceMode.VelocityChange);
            }

            OutputAction.GetValue()?.Invoke();
        }
    }
}