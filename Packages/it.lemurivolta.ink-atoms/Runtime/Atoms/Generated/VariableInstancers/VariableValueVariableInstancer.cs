using UnityEngine;
using UnityAtoms.BaseAtoms;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Variable Instancer of type `LemuRivolta.InkAtoms.VariableValue`. Inherits from `AtomVariableInstancer&lt;VariableValueVariable, VariableValuePair, LemuRivolta.InkAtoms.VariableValue, VariableValueEvent, VariableValuePairEvent, VariableValueVariableValueFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-hotpink")]
    [AddComponentMenu("Unity Atoms/Variable Instancers/VariableValue Variable Instancer")]
    public class VariableValueVariableInstancer : AtomVariableInstancer<
        VariableValueVariable,
        VariableValuePair,
        LemuRivolta.InkAtoms.VariableValue,
        VariableValueEvent,
        VariableValuePairEvent,
        VariableValueVariableValueFunction>
    { }
}
