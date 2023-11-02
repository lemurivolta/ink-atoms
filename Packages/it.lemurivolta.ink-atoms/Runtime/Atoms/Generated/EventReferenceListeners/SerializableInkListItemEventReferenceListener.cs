using UnityEngine;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference Listener of type `LemuRivolta.InkAtoms.SerializableInkListItem`. Inherits from `AtomEventReferenceListener&lt;LemuRivolta.InkAtoms.SerializableInkListItem, SerializableInkListItemEvent, SerializableInkListItemEventReference, SerializableInkListItemUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/SerializableInkListItem Event Reference Listener")]
    public sealed class SerializableInkListItemEventReferenceListener : AtomEventReferenceListener<
        LemuRivolta.InkAtoms.SerializableInkListItem,
        SerializableInkListItemEvent,
        SerializableInkListItemEventReference,
        SerializableInkListItemUnityEvent>
    { }
}
