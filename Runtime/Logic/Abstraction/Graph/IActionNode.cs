using System;

namespace MCB.Abstraction.Graph
{
    public interface IActionNode : INode
    {
        public InputActionPort InputAction { get; }
        public OutputPort<Action> OutputAction { get; }

    }
}