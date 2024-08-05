using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace MCB.Abstraction.Graph
{
    [Serializable]
    public abstract class Port
    {
        public abstract bool CanConnect(Port other);
        public abstract void Connect(INode node, string portName, Port port);
        public abstract PortDrawSide DrawSide { get; }
        [FormerlySerializedAs("connectionAddress")] [SerializeField] public PortAddress connectedAddress;
    }

    public abstract class Port<T> : Port
    {
        public abstract bool HasValue { get; }
        public abstract T GetValue();
    }

    [Serializable]
    public class InputPort<T> : Port<T>
    {
        public override bool HasValue => _connected?.HasValue ?? false;
        private OutputPort<T> _connected;
        public override PortDrawSide DrawSide => PortDrawSide.Left;
        public OutputPort<T> Connected => _connected;
        
        public override T GetValue()
        {
            if (_connected != null)
            {
                return _connected.GetValue();
            }

            return default;
        }

        public override bool CanConnect(Port other)
        {
            return other is OutputPort<T>;
        }

        public override void Connect(INode node, string portName, Port port)
        {
            _connected = (OutputPort<T>) port;
            connectedAddress.nodeId = node.Id;
            connectedAddress.portId = portName;
        }
    }
    [Serializable]
    public class OutputPort<T> : Port<T>
    {
        public override bool HasValue => _value != null;
        private T _value;
        public override PortDrawSide DrawSide => PortDrawSide.Right;
        public override T GetValue()
        {
            return _value;
        }

        public void SetValue(T value)
        {
            _value = value;
        }
        
        public override bool CanConnect(Port other)
        {
            return other is InputPort<T>;
        }
        
        public override void Connect(INode node, string portName, Port port)
        {
            port.Connect(node, portName, this);
        }
    }
    
    [Serializable]
    public class OutputDelegatePort<T> : OutputPort<T>
    {
        public override bool HasValue => _getter != null && _getter() != null;
        private Func<T> _getter;
        public override PortDrawSide DrawSide => PortDrawSide.Right;
        public override T GetValue()
        {
            return _getter();
        }

        public void SetValue(Func<T> value)
        {
            _getter = value;
        }
        
        public override bool CanConnect(Port other)
        {
            return other is InputPort<T>;
        }
        
        public override void Connect(INode node, string portName, Port port)
        {
            port.Connect(node, portName, this);
        }
    }
}