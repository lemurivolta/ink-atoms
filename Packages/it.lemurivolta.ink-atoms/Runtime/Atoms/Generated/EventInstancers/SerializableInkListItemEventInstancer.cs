using LemuRivolta.InkAtoms;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Event Instancer of type `LemuRivolta.InkAtoms.SerializableInkListItem`. Inherits from `AtomEventInstancer&lt;
    ///     LemuRivolta.InkAtoms.SerializableInkListItem, SerializableInkListItemEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-sign-blue")]
    [AddComponentMenu("Unity Atoms/Event Instancers/SerializableInkListItem Event Instancer")]
    public class
        SerializableInkListItemEventInstancer : AtomEventInstancer<SerializableInkListItem,
        SerializableInkListItemEvent>
    {
    }
}