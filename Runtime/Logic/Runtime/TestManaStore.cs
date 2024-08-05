using MCB.Abstraction;
using MCB.Abstraction.Graph;
using MCB.Utilities;
using UnityEngine;

namespace MCB.Runtime
{
    public class TestManaStore : MonoBehaviour, IManaStore
    {
        [SerializeField] private float amount;
        public float Amount => amount;

        public float TryTake(float wantedAmount)
        {
            return ManaStoreExtension.TryTakeManaFromRigidStorage(ref amount, wantedAmount);
        }
    }
}