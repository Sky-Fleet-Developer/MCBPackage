using System;
using MCB.Abstraction;
using MCB.Abstraction.Graph;
using MCB.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace MCB.Runtime
{
    public abstract class MonoNode : MonoBehaviour, INode
    {
        public string Id => transform.name;
        public Transform Presenter => transform;
        [SerializeField] private OutputNodePort thisNodePort;
        public OutputNodePort ThisNodePort => thisNodePort;

        public virtual void Attach(IGraph graph)
        {
            this.AttackDefault(graph);
        }

        public virtual void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            Gizmos.DrawSphere(transform.position, 0.07f + HandleUtility.GetHandleSize(transform.position) * 0.05f);
#endif
        }
    }
}