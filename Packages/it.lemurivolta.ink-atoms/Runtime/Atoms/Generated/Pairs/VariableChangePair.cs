using System;
using LemuRivolta.InkAtoms.VariableObserver;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     IPair of type `&lt;LemuRivolta.InkAtoms.VariableObserver.VariableChange&gt;`. Inherits from `IPair&lt;
    ///     LemuRivolta.InkAtoms.VariableObserver.VariableChange&gt;`.
    /// </summary>
    [Serializable]
    public struct VariableChangePair : IPair<VariableChange>
    {
        public VariableChange Item1
        {
            get => _item1;
            set => _item1 = value;
        }

        public VariableChange Item2
        {
            get => _item2;
            set => _item2 = value;
        }

        [SerializeField] private VariableChange _item1;
        [SerializeField] private VariableChange _item2;

        public void Deconstruct(out VariableChange item1, out VariableChange item2)
        {
            item1 = Item1;
            item2 = Item2;
        }
    }
}