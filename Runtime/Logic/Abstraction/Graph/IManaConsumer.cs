namespace MCB.Abstraction.Graph
{
    public interface IManaConsumer
    {
        public float Consumption { get; }
        public InputManaPort InputMana { get; }
    }
}