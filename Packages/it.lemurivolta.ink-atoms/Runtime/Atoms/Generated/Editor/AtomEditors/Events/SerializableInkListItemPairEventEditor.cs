#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `SerializableInkListItemPair`. Inherits from `AtomEventEditor&lt;SerializableInkListItemPair, SerializableInkListItemPairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(SerializableInkListItemPairEvent))]
    public sealed class SerializableInkListItemPairEventEditor : AtomEventEditor<SerializableInkListItemPair, SerializableInkListItemPairEvent> { }
}
#endif
