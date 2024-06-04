using System;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Reference of type `LemuRivolta.InkAtoms.ChosenChoice`. Inherits from `EquatableAtomReference&lt;
    ///     LemuRivolta.InkAtoms.ChosenChoice, ChosenChoicePair, ChosenChoiceConstant, ChosenChoiceVariable, ChosenChoiceEvent,
    ///     ChosenChoicePairEvent, ChosenChoiceChosenChoiceFunction, ChosenChoiceVariableInstancer, AtomCollection, AtomList
    ///     &gt;`.
    /// </summary>
    [Serializable]
    public sealed class ChosenChoiceReference : EquatableAtomReference<
        ChosenChoice,
        ChosenChoicePair,
        ChosenChoiceConstant,
        ChosenChoiceVariable,
        ChosenChoiceEvent,
        ChosenChoicePairEvent,
        ChosenChoiceChosenChoiceFunction,
        ChosenChoiceVariableInstancer>, IEquatable<ChosenChoiceReference>
    {
        public ChosenChoiceReference()
        {
        }

        public ChosenChoiceReference(ChosenChoice value) : base(value)
        {
        }

        public bool Equals(ChosenChoiceReference other)
        {
            return base.Equals(other);
        }
    }
}