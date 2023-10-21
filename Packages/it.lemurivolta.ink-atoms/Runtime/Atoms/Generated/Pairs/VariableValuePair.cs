using System;
using UnityEngine;
using LemuRivolta.InkAtoms;
namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// IPair of type `&lt;LemuRivolta.InkAtoms.VariableValue&gt;`. Inherits from `IPair&lt;LemuRivolta.InkAtoms.VariableValue&gt;`.
    /// </summary>
    [Serializable]
    public struct VariableValuePair : IPair<LemuRivolta.InkAtoms.VariableValue>
    {
        public LemuRivolta.InkAtoms.VariableValue Item1 { get => _item1; set => _item1 = value; }
        public LemuRivolta.InkAtoms.VariableValue Item2 { get => _item2; set => _item2 = value; }

        [SerializeField]
        private LemuRivolta.InkAtoms.VariableValue _item1;
        [SerializeField]
        private LemuRivolta.InkAtoms.VariableValue _item2;

        public void Deconstruct(out LemuRivolta.InkAtoms.VariableValue item1, out LemuRivolta.InkAtoms.VariableValue item2) { item1 = Item1; item2 = Item2; }
    }
}