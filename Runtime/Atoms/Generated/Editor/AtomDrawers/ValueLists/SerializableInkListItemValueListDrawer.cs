#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Value List property drawer of type `LemuRivolta.InkAtoms.SerializableInkListItem`. Inherits from `AtomDrawer&lt;SerializableInkListItemValueList&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(SerializableInkListItemValueList))]
    public class SerializableInkListItemValueListDrawer : AtomDrawer<SerializableInkListItemValueList> { }
}
#endif
