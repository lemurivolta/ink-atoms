using System;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference of type `VariableValuePair`. Inherits from `AtomEventReference&lt;VariableValuePair, VariableValueVariable, VariableValuePairEvent, VariableValueVariableInstancer, VariableValuePairEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class VariableValuePairEventReference : AtomEventReference<
        VariableValuePair,
        VariableValueVariable,
        VariableValuePairEvent,
        VariableValueVariableInstancer,
        VariableValuePairEventInstancer>, IGetEvent 
    { }
}
