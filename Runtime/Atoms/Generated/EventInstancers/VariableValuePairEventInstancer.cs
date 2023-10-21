using UnityEngine;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Instancer of type `VariableValuePair`. Inherits from `AtomEventInstancer&lt;VariableValuePair, VariableValuePairEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-sign-blue")]
    [AddComponentMenu("Unity Atoms/Event Instancers/VariableValuePair Event Instancer")]
    public class VariableValuePairEventInstancer : AtomEventInstancer<VariableValuePair, VariableValuePairEvent> { }
}
