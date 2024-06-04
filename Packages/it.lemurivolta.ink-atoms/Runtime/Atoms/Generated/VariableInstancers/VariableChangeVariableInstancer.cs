using LemuRivolta.InkAtoms.VariableObserver;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Variable Instancer of type `LemuRivolta.InkAtoms.VariableObserver.VariableChange`. Inherits from
    ///     `AtomVariableInstancer&lt;VariableChangeVariable, VariableChangePair,
    ///     LemuRivolta.InkAtoms.VariableObserver.VariableChange, VariableChangeEvent, VariableChangePairEvent,
    ///     VariableChangeVariableChangeFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-hotpink")]
    [AddComponentMenu("Unity Atoms/Variable Instancers/VariableChange Variable Instancer")]
    public class VariableChangeVariableInstancer : AtomVariableInstancer<
        VariableChangeVariable,
        VariableChangePair,
        VariableChange,
        VariableChangeEvent,
        VariableChangePairEvent,
        VariableChangeVariableChangeFunction>
    {
    }
}