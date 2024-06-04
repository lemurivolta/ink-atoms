using LemuRivolta.InkAtoms.VariableObserver;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Event of type `LemuRivolta.InkAtoms.VariableObserver.VariableChange`. Inherits from `AtomEvent&lt;
    ///     LemuRivolta.InkAtoms.VariableObserver.VariableChange&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/VariableChange", fileName = "VariableChangeEvent")]
    public sealed class VariableChangeEvent : AtomEvent<VariableChange>
    {
    }
}