using UnityEngine;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Value List of type `LemuRivolta.InkAtoms.SerializableInkListItem`. Inherits from `AtomValueList&lt;LemuRivolta.InkAtoms.SerializableInkListItem, SerializableInkListItemEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-piglet")]
    [CreateAssetMenu(menuName = "Unity Atoms/Value Lists/SerializableInkListItem", fileName = "SerializableInkListItemValueList")]
    public sealed class SerializableInkListItemValueList : AtomValueList<LemuRivolta.InkAtoms.SerializableInkListItem, SerializableInkListItemEvent> { }
}
