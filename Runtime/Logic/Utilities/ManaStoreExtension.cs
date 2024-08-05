using UnityEngine;

namespace MCB.Utilities
{
    public static class ManaStoreExtension
    {
        public static float TryTakeManaFromRigidStorage(ref float amount, float wantedAmount)
        {
            float possibleAmount = Mathf.Min(wantedAmount, amount);
            if (possibleAmount > 0)
            {
                amount -= possibleAmount;
                return possibleAmount / wantedAmount;
            }
            return 0;
        }
    }
}