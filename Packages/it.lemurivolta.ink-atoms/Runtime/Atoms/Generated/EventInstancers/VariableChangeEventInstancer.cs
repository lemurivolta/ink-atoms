using LemuRivolta.InkAtoms.VariableObserver;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Event Instancer of type `LemuRivolta.InkAtoms.VariableObserver.VariableChange`. Inherits from `AtomEventInstancer
    ///     &lt;LemuRivolta.InkAtoms.VariableObserver.VariableChange, VariableChangeEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-sign-blue")]
    [AddComponentMenu("Unity Atoms/Event Instancers/VariableChange Event Instancer")]
    public class VariableChangeEventInstancer : AtomEventInstancer<VariableChange, VariableChangeEvent>
    {
    }
}