using System;

namespace MCB.Abstraction.Graph
{
    public interface ITriggerNode : INode
    {
        public OutputPort<Action> OutputAction { get; }
        public void Activate();
    }
}