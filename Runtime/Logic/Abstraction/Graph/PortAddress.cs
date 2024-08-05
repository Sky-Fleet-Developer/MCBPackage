using System;

namespace MCB.Abstraction.Graph
{
    [Serializable]
    public struct PortAddress
    {
        public string nodeId;
        public string portId;
        public bool IsNull => string.IsNullOrEmpty(nodeId) || string.IsNullOrEmpty(portId);
    }
}