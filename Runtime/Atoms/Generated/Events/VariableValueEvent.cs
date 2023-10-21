using UnityEngine;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event of type `LemuRivolta.InkAtoms.VariableValue`. Inherits from `AtomEvent&lt;LemuRivolta.InkAtoms.VariableValue&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/VariableValue", fileName = "VariableValueEvent")]
    public sealed class VariableValueEvent : AtomEvent<LemuRivolta.InkAtoms.VariableValue>
    {
    }
}
