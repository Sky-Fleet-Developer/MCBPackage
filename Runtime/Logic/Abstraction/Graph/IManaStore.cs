namespace MCB.Abstraction.Graph
{
    public interface IManaStore
    {
        public float Amount { get; }
        public float TryTake(float wantedAmount);
    }
}