#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `LemuRivolta.InkAtoms.SerializableInkListItem`. Inherits from `AtomEventEditor&lt;LemuRivolta.InkAtoms.SerializableInkListItem, SerializableInkListItemEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(SerializableInkListItemEvent))]
    public sealed class SerializableInkListItemEventEditor : AtomEventEditor<LemuRivolta.InkAtoms.SerializableInkListItem, SerializableInkListItemEvent> { }
}
#endif
