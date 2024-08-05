using MCB.Abstraction.Graph;
using UnityEngine;

namespace MCB.Runtime
{
    public class MagnitudeOperator : MonoNode
    {
        [SerializeField] private InputPort<Vector3> inputVector = new();
        [SerializeField] private OutputDelegatePort<float> outputValue = new();

        public override void Attach(IGraph graph)
        {
            base.Attach(graph);
            graph.RegisterPort("InputVector", this, inputVector);
            graph.RegisterPort("OutputValue", this, outputValue);
            outputValue.SetValue(() => inputVector.GetValue().magnitude);
        }
    }
}