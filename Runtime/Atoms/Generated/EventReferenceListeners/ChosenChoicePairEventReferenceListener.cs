using UnityEngine;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference Listener of type `ChosenChoicePair`. Inherits from `AtomEventReferenceListener&lt;ChosenChoicePair, ChosenChoicePairEvent, ChosenChoicePairEventReference, ChosenChoicePairUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/ChosenChoicePair Event Reference Listener")]
    public sealed class ChosenChoicePairEventReferenceListener : AtomEventReferenceListener<
        ChosenChoicePair,
        ChosenChoicePairEvent,
        ChosenChoicePairEventReference,
        ChosenChoicePairUnityEvent>
    { }
}
