using System;
using MCB.Abstraction;
using MCB.Abstraction.Graph;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MCB.Runtime
{
    public class PhysicalTrigger : MonoNode, IPhysicsTriggerNode
    {
        [Serializable]
        public class TriggerShape
        {
            [SerializeField, HideInInspector] private ShapeType shapeType;

            [ShowInInspector, DrawWithUnity]
            public ShapeType ShapeType
            {
                get => shapeType;
                set
                {
                    shapeType = value;
                    OnSettingsDirty?.Invoke();
                }
            }

            [SerializeField, HideInInspector] private Vector3 size;

            [ShowInInspector]
            public Vector3 Size
            {
                get => size;
                set
                {
                    size = value;
                    OnSettingsDirty?.Invoke();
                }
            }

            public event Action OnSettingsDirty;
        }

        public enum ShapeType
        {
            Sphere = 0,
            Box = 1,
        }

        [SerializeField] private OutputPort<Action> outputAction = new();
        [SerializeField] private OutputPort<Transform> triggeredBody = new();
        [SerializeField, DrawWithUnity] private TriggerType triggerType;

        [SerializeField] private TriggerShape shape;
        public OutputPort<Action> OutputAction => outputAction;
        public TriggerType TriggerType => triggerType;

        public virtual void Awake()
        {
            CreateTrigger();
            shape.OnSettingsDirty += CreateTrigger;
        }

        private void CreateTrigger()
        {
            var oldCollider = GetComponent<Collider>();
            if (oldCollider)
            {
                Destroy(oldCollider);
            }

            switch (shape.ShapeType)
            {
                case ShapeType.Sphere:
                    var sphereCollider = gameObject.AddComponent<SphereCollider>();
                    sphereCollider.radius = shape.Size.x;
                    sphereCollider.isTrigger = true;
                    break;
                case ShapeType.Box:
                    var boxCollider = gameObject.AddComponent<BoxCollider>();
                    boxCollider.size = shape.Size;
                    boxCollider.isTrigger = true;
                    break;
            }
        }

        public override void Attach(IGraph graph)
        {
            base.Attach(graph);
            graph.RegisterPort("TriggeredBody", this, triggeredBody);
        }

        public void Activate()
        {
            outputAction.GetValue()?.Invoke();
        }

        private void Trigger(Transform body)
        {
            triggeredBody.SetValue(body);
            Activate();
        }

        public void TriggerEntered(Transform other)
        {
            if (triggerType.HasFlag(TriggerType.Enter))
            {
                Trigger(other);
            }
        }
        public void TriggerStay(Transform other)
        {
            if (triggerType.HasFlag(TriggerType.Stay))
            {
                Trigger(other);
            }
        }
        public void TriggerExit(Transform other)
        {
            if (triggerType.HasFlag(TriggerType.Exit))
            {
                Trigger(other);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.attachedRigidbody && !other.attachedRigidbody.isKinematic)
            {
                TriggerEntered(other.attachedRigidbody.transform);
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.attachedRigidbody && !other.attachedRigidbody.isKinematic)
            {
                TriggerStay(other.attachedRigidbody.transform);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.attachedRigidbody && !other.attachedRigidbody.isKinematic)
            {
                TriggerExit(other.attachedRigidbody.transform);
            }
        }

        public override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            Gizmos.color = Color.green;
            switch (shape.ShapeType)
            {
                case ShapeType.Sphere:
                    Gizmos.DrawWireSphere(transform.position, shape.Size.x);
                    break;
                case ShapeType.Box:
                    Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, shape.Size);
                    Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
                    Gizmos.matrix = Matrix4x4.identity;
                    break;
            }
            Gizmos.color = Color.white;
        }
    }
}