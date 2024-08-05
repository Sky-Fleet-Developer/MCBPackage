using System;
using System.Collections.Generic;
using MCB.Abstraction;
using MCB.Abstraction.Graph;
using MCB.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace MCB.Runtime
{
    public class Construct : MonoBehaviour, IGraph
    {
        // fields
        private Dictionary<string, INode> _nodes = new ();
        private bool _isDirty = false;
        private PortsDictionary _ports = new();
        private List<(Port, Port)> _wires = new List<(Port, Port)>();
        [FormerlySerializedAs("extends")] [SerializeField] private Vector3 size;
        [SerializeField] private TestManaStore testManaStore;
        
        // properties
        public Transform Presenter => transform;
        public IEnumerable<INode> Nodes => _nodes.Values;
        public Vector3 BoundSize => size;
        public IManaStore MyManaStore => testManaStore;
        public event Action AllPortsConnectedEvent;

        private void Awake()
        {
            foreach (var node in GetComponentsInChildren<INode>())
            {
                AddNode(node);
            }

            Init();
        }

        public void AddNode(INode node)
        {
            _nodes.Add(node.Id, node);
            _isDirty = true;
        }

        public void RemoveNode(INode node)
        {
            _nodes.Remove(node.Id);
        }

        public INode GetNode(string nodeId)
        {
            if (_nodes.TryGetValue(nodeId, out var result))
            {
                return result;
            }
            return null;
        }

        public void RegisterPort(string portName, INode owner, Port port)
        {
            if (!_ports.TryGetValue(owner.Id, out var nodePorts))
            {
                nodePorts = new Dictionary<string, Port>();
                _ports[owner.Id] = nodePorts;
            }
            nodePorts.TryAdd(portName, port);
        }

        public Port GetPort(PortAddress address)
        {
            return _ports.GetPort(address);
        }

        public void Init()
        {
            _ports.Clear();
            this.InitDefault();
            _wires.Clear();
            this.ConnectWires(ref _ports, ref _wires);
            AllPortsConnectedEvent?.Invoke();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, size);
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            Gizmos.matrix = Matrix4x4.identity;
        }
    }
}