using UnityEngine;

namespace MCB.Abstraction.Graph
{
    public interface INode
    {
        public string Id { get; }
        public Transform Presenter { get; }
        public OutputNodePort ThisNodePort { get; }
        public void Attach(IGraph graph);
    }
}