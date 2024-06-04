using UnityEngine;
using LemuRivolta.InkAtoms.VariableObserver;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event of type `VariableChangePair`. Inherits from `AtomEvent&lt;VariableChangePair&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/VariableChangePair", fileName = "VariableChangePairEvent")]
    public sealed class VariableChangePairEvent : AtomEvent<VariableChangePair>
    {
    }
}
