#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Variable property drawer of type `LemuRivolta.InkAtoms.SerializableInkListItem`. Inherits from `AtomDrawer&lt;SerializableInkListItemVariable&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(SerializableInkListItemVariable))]
    public class SerializableInkListItemVariableDrawer : VariableDrawer<SerializableInkListItemVariable> { }
}
#endif
