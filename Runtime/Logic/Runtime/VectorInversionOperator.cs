using MCB.Abstraction.Graph;
using UnityEngine;

namespace MCB.Runtime
{
    public class VectorInversionOperator : MonoNode
    {
        [SerializeField] private InputPort<Vector3> inputVector = new();
        [SerializeField] private OutputDelegatePort<Vector3> outputVector = new();

        public override void Attach(IGraph graph)
        {
            base.Attach(graph);
            graph.RegisterPort("InputVector", this, inputVector);
            graph.RegisterPort("OutputVector", this, outputVector);
            outputVector.SetValue(() => -inputVector.GetValue());
        }
    }
}