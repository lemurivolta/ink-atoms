using UnityEngine;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference Listener of type `VariableValuePair`. Inherits from `AtomEventReferenceListener&lt;VariableValuePair, VariableValuePairEvent, VariableValuePairEventReference, VariableValuePairUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/VariableValuePair Event Reference Listener")]
    public sealed class VariableValuePairEventReferenceListener : AtomEventReferenceListener<
        VariableValuePair,
        VariableValuePairEvent,
        VariableValuePairEventReference,
        VariableValuePairUnityEvent>
    { }
}
