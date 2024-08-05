using System;
using UnityEngine;

namespace MCB.Abstraction.Graph
{
    [Flags]
    public enum TriggerType
    {
        None = 0,
        Enter = 1,
        Stay = 1<<1,
        Exit = 1<<2
    }

    public enum ShapeType
    {
        Sphere = 0,
        Box = 1,
    }
    
    public interface IPhysicsTriggerNode : ITriggerNode
    {
        public TriggerType TriggerType { get; }

        public void TriggerEntered(Transform transform);
        public void TriggerStay(Transform transform);
        public void TriggerExit(Transform transform);
    }
}