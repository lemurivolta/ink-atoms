using UnityEngine;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference Listener of type `SerializableInkListItemPair`. Inherits from `AtomEventReferenceListener&lt;SerializableInkListItemPair, SerializableInkListItemPairEvent, SerializableInkListItemPairEventReference, SerializableInkListItemPairUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/SerializableInkListItemPair Event Reference Listener")]
    public sealed class SerializableInkListItemPairEventReferenceListener : AtomEventReferenceListener<
        SerializableInkListItemPair,
        SerializableInkListItemPairEvent,
        SerializableInkListItemPairEventReference,
        SerializableInkListItemPairUnityEvent>
    { }
}
