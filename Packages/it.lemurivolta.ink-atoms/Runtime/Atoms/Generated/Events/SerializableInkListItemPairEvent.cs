using UnityEngine;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event of type `SerializableInkListItemPair`. Inherits from `AtomEvent&lt;SerializableInkListItemPair&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/SerializableInkListItemPair", fileName = "SerializableInkListItemPairEvent")]
    public sealed class SerializableInkListItemPairEvent : AtomEvent<SerializableInkListItemPair>
    {
    }
}
