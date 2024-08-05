using System.Collections.Generic;
using MCB.Abstraction.Graph;
using UnityEngine;

namespace MCB.Runtime
{
    public class ForwardDirectionAnchor : MonoNode
    {
        [SerializeField] private OutputDelegatePort<Vector3> _direction = new();

        public override void Attach(IGraph graph)
        {
            base.Attach(graph);
            _direction.SetValue(() => transform.forward);
            graph.RegisterPort("Direction", this, _direction);
        }
    }
}