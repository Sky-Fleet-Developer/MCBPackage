using System;
using System.Collections.Generic;
using UnityEngine;

namespace MCB.Abstraction.Graph
{
    public interface IGraph
    {
        public Transform Presenter { get; }
        public IEnumerable<INode> Nodes { get; }
        public Vector3 BoundSize { get; }
        public IManaStore MyManaStore { get; }
        public event Action AllPortsConnectedEvent;
        
        public void AddNode(INode node);
        public void RemoveNode(INode node);
        public INode GetNode(string nodeId);

        public void RegisterPort(string portName, INode owner, Port port);
        public Port GetPort(PortAddress address);
        public void Init();
    }
}
