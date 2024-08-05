using MCB.Abstraction.Graph;
using UnityEngine;

namespace MCB.Runtime
{
    public class OrientationAnchor : MonoNode
    {
        [SerializeField] private OutputDelegatePort<Quaternion> _orientation = new();

        public override void Attach(IGraph graph)
        {
            base.Attach(graph);
            _orientation.SetValue(() => transform.rotation);
            graph.RegisterPort("Orientation", this, _orientation);
        }
    }
}