#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `LemuRivolta.InkAtoms.SerializableInkListItem`. Inherits from `AtomDrawer&lt;SerializableInkListItemEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(SerializableInkListItemEvent))]
    public class SerializableInkListItemEventDrawer : AtomDrawer<SerializableInkListItemEvent> { }
}
#endif
