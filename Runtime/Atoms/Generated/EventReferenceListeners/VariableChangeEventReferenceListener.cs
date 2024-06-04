using LemuRivolta.InkAtoms.VariableObserver;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Event Reference Listener of type `LemuRivolta.InkAtoms.VariableObserver.VariableChange`. Inherits from
    ///     `AtomEventReferenceListener&lt;LemuRivolta.InkAtoms.VariableObserver.VariableChange, VariableChangeEvent,
    ///     VariableChangeEventReference, VariableChangeUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/VariableChange Event Reference Listener")]
    public sealed class VariableChangeEventReferenceListener : AtomEventReferenceListener<
        VariableChange,
        VariableChangeEvent,
        VariableChangeEventReference,
        VariableChangeUnityEvent>
    {
    }
}