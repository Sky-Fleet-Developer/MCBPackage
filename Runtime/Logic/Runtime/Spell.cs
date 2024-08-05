using System;
using MCB.Abstraction;
using MCB.Abstraction.Graph;
using MCB.Utilities;
using UnityEngine;

namespace MCB.Runtime
{
    public abstract class Spell : MonoNode, IActionNode, IManaConsumer
    {
        [SerializeField] private InputActionPort inputAction = new();
        [SerializeField] private OutputPort<Action> outputAction = new();
        [SerializeField] private InputManaPort inputMana = new();
        [SerializeField] private float manaConsumption;
        public InputActionPort InputAction => inputAction;
        public OutputPort<Action> OutputAction => outputAction;

        public float Consumption
        {
            get => manaConsumption;
            set => manaConsumption = value;
        }

        public InputManaPort InputMana => inputMana;
    }
}