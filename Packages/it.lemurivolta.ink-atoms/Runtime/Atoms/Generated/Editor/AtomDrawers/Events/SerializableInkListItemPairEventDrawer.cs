#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `SerializableInkListItemPair`. Inherits from `AtomDrawer&lt;SerializableInkListItemPairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(SerializableInkListItemPairEvent))]
    public class SerializableInkListItemPairEventDrawer : AtomDrawer<SerializableInkListItemPairEvent> { }
}
#endif
