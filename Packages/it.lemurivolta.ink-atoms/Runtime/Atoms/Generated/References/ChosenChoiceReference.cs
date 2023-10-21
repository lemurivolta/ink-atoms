using System;
using UnityAtoms.BaseAtoms;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Reference of type `LemuRivolta.InkAtoms.ChosenChoice`. Inherits from `EquatableAtomReference&lt;LemuRivolta.InkAtoms.ChosenChoice, ChosenChoicePair, ChosenChoiceConstant, ChosenChoiceVariable, ChosenChoiceEvent, ChosenChoicePairEvent, ChosenChoiceChosenChoiceFunction, ChosenChoiceVariableInstancer, AtomCollection, AtomList&gt;`.
    /// </summary>
    [Serializable]
    public sealed class ChosenChoiceReference : EquatableAtomReference<
        LemuRivolta.InkAtoms.ChosenChoice,
        ChosenChoicePair,
        ChosenChoiceConstant,
        ChosenChoiceVariable,
        ChosenChoiceEvent,
        ChosenChoicePairEvent,
        ChosenChoiceChosenChoiceFunction,
        ChosenChoiceVariableInstancer>, IEquatable<ChosenChoiceReference>
    {
        public ChosenChoiceReference() : base() { }
        public ChosenChoiceReference(LemuRivolta.InkAtoms.ChosenChoice value) : base(value) { }
        public bool Equals(ChosenChoiceReference other) { return base.Equals(other); }
    }
}
