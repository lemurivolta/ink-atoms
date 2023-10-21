using UnityEngine;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event of type `VariableValuePair`. Inherits from `AtomEvent&lt;VariableValuePair&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/VariableValuePair", fileName = "VariableValuePairEvent")]
    public sealed class VariableValuePairEvent : AtomEvent<VariableValuePair>
    {
    }
}
