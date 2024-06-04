using LemuRivolta.InkAtoms;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Variable of type `LemuRivolta.InkAtoms.SerializableInkListItem`. Inherits from `EquatableAtomVariable&lt;
    ///     LemuRivolta.InkAtoms.SerializableInkListItem, SerializableInkListItemPair, SerializableInkListItemEvent,
    ///     SerializableInkListItemPairEvent, SerializableInkListItemSerializableInkListItemFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-lush")]
    [CreateAssetMenu(menuName = "Unity Atoms/Variables/SerializableInkListItem",
        fileName = "SerializableInkListItemVariable")]
    public sealed class SerializableInkListItemVariable : EquatableAtomVariable<SerializableInkListItem,
        SerializableInkListItemPair, SerializableInkListItemEvent, SerializableInkListItemPairEvent,
        SerializableInkListItemSerializableInkListItemFunction>
    {
    }
}