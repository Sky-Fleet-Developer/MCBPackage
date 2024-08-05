using System;

namespace MCB.Abstraction.Graph
{
    [Serializable]
    public class InputNodePort : InputPort<INode>
    {
        public override PortDrawSide DrawSide => PortDrawSide.Right;
    }
    
    [Serializable]
    public class OutputNodePort : OutputPort<INode>
    {
        public override PortDrawSide DrawSide => PortDrawSide.Left;
    }
}