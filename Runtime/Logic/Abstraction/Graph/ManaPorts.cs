using System;

namespace MCB.Abstraction.Graph
{
    [Serializable]
    public class InputManaPort : InputPort<IManaStore>
    {
        public override bool CanConnect(Port other)
        {
            return other is OutputManaPort;
        }
    }
    [Serializable]
    public class OutputManaPort : OutputPort<IManaStore>
    {
        public override bool CanConnect(Port other)
        {
            return other is InputManaPort;
        }
    }
}