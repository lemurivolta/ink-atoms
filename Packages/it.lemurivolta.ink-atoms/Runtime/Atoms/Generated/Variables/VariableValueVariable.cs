using UnityEngine;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Variable of type `LemuRivolta.InkAtoms.VariableValue`. Inherits from `EquatableAtomVariable&lt;LemuRivolta.InkAtoms.VariableValue, VariableValuePair, VariableValueEvent, VariableValuePairEvent, VariableValueVariableValueFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-lush")]
    [CreateAssetMenu(menuName = "Unity Atoms/Variables/VariableValue", fileName = "VariableValueVariable")]
    public sealed class VariableValueVariable : EquatableAtomVariable<LemuRivolta.InkAtoms.VariableValue, VariableValuePair, VariableValueEvent, VariableValuePairEvent, VariableValueVariableValueFunction>
    {
    }
}
