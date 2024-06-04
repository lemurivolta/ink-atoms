using LemuRivolta.InkAtoms;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Event of type `LemuRivolta.InkAtoms.SerializableInkListItem`. Inherits from `AtomEvent&lt;
    ///     LemuRivolta.InkAtoms.SerializableInkListItem&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/SerializableInkListItem",
        fileName = "SerializableInkListItemEvent")]
    public sealed class SerializableInkListItemEvent : AtomEvent<SerializableInkListItem>
    {
    }
}