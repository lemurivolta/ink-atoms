using LemuRivolta.InkAtoms;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Event Reference Listener of type `LemuRivolta.InkAtoms.SerializableInkListItem`. Inherits from
    ///     `AtomEventReferenceListener&lt;LemuRivolta.InkAtoms.SerializableInkListItem, SerializableInkListItemEvent,
    ///     SerializableInkListItemEventReference, SerializableInkListItemUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/SerializableInkListItem Event Reference Listener")]
    public sealed class SerializableInkListItemEventReferenceListener : AtomEventReferenceListener<
        SerializableInkListItem,
        SerializableInkListItemEvent,
        SerializableInkListItemEventReference,
        SerializableInkListItemUnityEvent>
    {
    }
}