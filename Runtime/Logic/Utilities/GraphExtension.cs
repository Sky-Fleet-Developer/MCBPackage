using System.Collections.Generic;
using MCB.Abstraction;
using MCB.Abstraction.Graph;

namespace MCB.Utilities
{
    public static class GraphExtension
    {
        public static void InitDefault(this IGraph graph)
        {
            foreach (var graphNode in graph.Nodes)
            {
                graphNode.Attach(graph);
            }
        }

        public static void ConnectWires(this IGraph graph, ref PortsDictionary ports, ref List<(Port, Port)> wires)
        {
            foreach (KeyValuePair<string, Dictionary<string, Port>> nodePortsKv in ports)
            {
                foreach (KeyValuePair<string, Port> portKv in nodePortsKv.Value)
                {
                    if (!portKv.Value.connectedAddress.IsNull)
                    {
                        var other = ports.GetPort(portKv.Value.connectedAddress);
                        if (portKv.Value.CanConnect(other))
                        {
                            portKv.Value.Connect(graph.GetNode(nodePortsKv.Key), portKv.Key, other);
                            wires.Add((portKv.Value, other));
                        }
                    }
                }
            }
        }
    }
}