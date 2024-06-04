using System;
using LemuRivolta.InkAtoms;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     IPair of type `&lt;LemuRivolta.InkAtoms.ChosenChoice&gt;`. Inherits from `IPair&lt;
    ///     LemuRivolta.InkAtoms.ChosenChoice&gt;`.
    /// </summary>
    [Serializable]
    public struct ChosenChoicePair : IPair<ChosenChoice>
    {
        public ChosenChoice Item1
        {
            get => _item1;
            set => _item1 = value;
        }

        public ChosenChoice Item2
        {
            get => _item2;
            set => _item2 = value;
        }

        [SerializeField] private ChosenChoice _item1;
        [SerializeField] private ChosenChoice _item2;

        public void Deconstruct(out ChosenChoice item1, out ChosenChoice item2)
        {
            item1 = Item1;
            item2 = Item2;
        }
    }
}