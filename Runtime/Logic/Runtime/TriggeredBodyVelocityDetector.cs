using System;
using MCB.Abstraction.Entities;
using MCB.Abstraction.Graph;
using UnityEngine;

namespace MCB.Runtime
{
    public class TriggeredBodyVelocityDetector : MonoNode, IActionNode
    {
        [SerializeField] private InputActionPort inputAction = new();
        [SerializeField] private OutputPort<Action> outputAction = new();
        [SerializeField] private InputPort<Transform> body = new();
        [SerializeField] private OutputPort<Vector3> velocity = new();
        public InputActionPort InputAction => inputAction;
        public OutputPort<Action> OutputAction => outputAction;
        private void Awake()
        {
            inputAction.Subscribe(OnAction);
        }

        public override void Attach(IGraph graph)
        {
            base.Attach(graph);
            graph.RegisterPort("InputAction", this, inputAction);
            graph.RegisterPort("Body", this, body);
            graph.RegisterPort("Velocity", this, velocity);
        }
        
        private void OnAction()
        {
            if (body.HasValue)
            {
                var target = body.GetValue();
                if (target.TryGetComponent(out Projectile projectile))
                {
                    velocity.SetValue(projectile.Velocity);
                }
                else if (target.TryGetComponent(out Rigidbody rigidbody))
                {
                    velocity.SetValue(rigidbody.velocity);
                }
            }
            outputAction.GetValue()?.Invoke();
        }
    }
}