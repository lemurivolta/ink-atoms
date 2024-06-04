using System;
using LemuRivolta.InkAtoms.VariableObserver;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Event Reference of type `LemuRivolta.InkAtoms.VariableObserver.VariableChange`. Inherits from `AtomEventReference
    ///     &lt;LemuRivolta.InkAtoms.VariableObserver.VariableChange, VariableChangeVariable, VariableChangeEvent,
    ///     VariableChangeVariableInstancer, VariableChangeEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class VariableChangeEventReference : AtomEventReference<
        VariableChange,
        VariableChangeVariable,
        VariableChangeEvent,
        VariableChangeVariableInstancer,
        VariableChangeEventInstancer>, IGetEvent
    {
    }
}