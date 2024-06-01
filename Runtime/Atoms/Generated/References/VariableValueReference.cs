using System;
using UnityAtoms.BaseAtoms;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Reference of type `LemuRivolta.InkAtoms.VariableValue`. Inherits from `EquatableAtomReference&lt;LemuRivolta.InkAtoms.VariableValue, VariableValuePair, VariableValueConstant, VariableValueVariable, VariableValueEvent, VariableValuePairEvent, VariableValueVariableValueFunction, VariableValueVariableInstancer, AtomCollection, AtomList&gt;`.
    /// </summary>
    [Serializable]
    public sealed class VariableValueReference : EquatableAtomReference<
        LemuRivolta.InkAtoms.VariableValue,
        VariableValuePair,
        VariableValueConstant,
        VariableValueVariable,
        VariableValueEvent,
        VariableValuePairEvent,
        VariableValueVariableValueFunction,
        VariableValueVariableInstancer>, IEquatable<VariableValueReference>
    {
        public VariableValueReference() : base() { }
        public VariableValueReference(LemuRivolta.InkAtoms.VariableValue value) : base(value) { }
        public bool Equals(VariableValueReference other) { return base.Equals(other); }
    }
}
