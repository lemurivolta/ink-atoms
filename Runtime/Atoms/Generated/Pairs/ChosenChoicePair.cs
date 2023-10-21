using System;
using UnityEngine;
using LemuRivolta.InkAtoms;
namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// IPair of type `&lt;LemuRivolta.InkAtoms.ChosenChoice&gt;`. Inherits from `IPair&lt;LemuRivolta.InkAtoms.ChosenChoice&gt;`.
    /// </summary>
    [Serializable]
    public struct ChosenChoicePair : IPair<LemuRivolta.InkAtoms.ChosenChoice>
    {
        public LemuRivolta.InkAtoms.ChosenChoice Item1 { get => _item1; set => _item1 = value; }
        public LemuRivolta.InkAtoms.ChosenChoice Item2 { get => _item2; set => _item2 = value; }

        [SerializeField]
        private LemuRivolta.InkAtoms.ChosenChoice _item1;
        [SerializeField]
        private LemuRivolta.InkAtoms.ChosenChoice _item2;

        public void Deconstruct(out LemuRivolta.InkAtoms.ChosenChoice item1, out LemuRivolta.InkAtoms.ChosenChoice item2) { item1 = Item1; item2 = Item2; }
    }
}