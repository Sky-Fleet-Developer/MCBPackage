using MCB.Abstraction;
using MCB.Abstraction.Graph;

namespace MCB.Utilities
{
    public static class NodeExtension
    {
        public static void AttackDefault(this INode node, IGraph graph)
        {
            node.ThisNodePort.SetValue(node);
            node.Presenter.SetParent(graph.Presenter);
            graph.RegisterPort("Self", node, node.ThisNodePort);
            if (node is IActionNode actionNode)
            {
                graph.RegisterPort("InputAction", node, actionNode.InputAction);
                graph.RegisterPort("OutputAction", node, actionNode.OutputAction);
            }
            else if (node is ITriggerNode triggerNode)
            {
                graph.RegisterPort("OutputAction", node, triggerNode.OutputAction);
            }

            if (node is IManaConsumer manaConsumer)
            {
                graph.RegisterPort("InputMana", node, manaConsumer.InputMana);
            }
        }

        public static float ConsumeMana(this IManaConsumer manaConsumer, IGraph graph, float deltaTime)
        {
            IManaStore store = null;
            if (manaConsumer.InputMana.HasValue)
            {
                store = manaConsumer.InputMana.GetValue();
            }
            else
            {
                store = graph.MyManaStore;
            }

            return store.TryTake(manaConsumer.Consumption * deltaTime);
        }
        
        public static float ConsumeMana(this IManaConsumer manaConsumer, IGraph graph, float overrideAmount, float deltaTime)
        {
            IManaStore store = null;
            if (manaConsumer.InputMana.HasValue)
            {
                store = manaConsumer.InputMana.GetValue();
            }
            else
            {
                store = graph.MyManaStore;
            }

            return store.TryTake(overrideAmount * deltaTime);
        }
    }
}