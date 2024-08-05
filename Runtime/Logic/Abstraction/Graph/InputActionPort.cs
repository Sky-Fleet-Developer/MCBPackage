using System;

namespace MCB.Abstraction.Graph
{
    [Serializable]
    public class InputActionPort : InputPort<Action>
    {
        private Action _value;
        public override void Connect(INode node, string portName, Port port)
        {
            base.Connect(node, portName, port);
            if (_value != null)
            {
                Connected.SetValue((Action) Delegate.Combine(GetValue(), _value));
            }
        }

        public void Subscribe(Action onAction)
        {
            if (Connected != null)
            {
                Connected.SetValue((Action) Delegate.Combine(GetValue(), onAction));
            }
            else
            {
                _value += onAction;
            }
        }
    }
}