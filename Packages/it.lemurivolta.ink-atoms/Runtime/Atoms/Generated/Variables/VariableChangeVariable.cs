using LemuRivolta.InkAtoms.VariableObserver;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Variable of type `LemuRivolta.InkAtoms.VariableObserver.VariableChange`. Inherits from `EquatableAtomVariable&lt;
    ///     LemuRivolta.InkAtoms.VariableObserver.VariableChange, VariableChangePair, VariableChangeEvent,
    ///     VariableChangePairEvent, VariableChangeVariableChangeFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-lush")]
    [CreateAssetMenu(menuName = "Unity Atoms/Variables/VariableChange", fileName = "VariableChangeVariable")]
    public sealed class VariableChangeVariable : EquatableAtomVariable<VariableChange, VariableChangePair,
        VariableChangeEvent, VariableChangePairEvent, VariableChangeVariableChangeFunction>
    {
    }
}