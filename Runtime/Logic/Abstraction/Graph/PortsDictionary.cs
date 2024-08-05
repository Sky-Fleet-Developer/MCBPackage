using System.Collections.Generic;

namespace MCB.Abstraction.Graph
{
    public class PortsDictionary : Dictionary<string, Dictionary<string, Port>>
    {
        public Port GetPort(PortAddress address)
        {
            if (TryGetValue(address.nodeId, out var nodePorts))
            {
                if (nodePorts.TryGetValue(address.portId, out var port))
                {
                    return port;
                }
            }

            return null;
        }
    }
}