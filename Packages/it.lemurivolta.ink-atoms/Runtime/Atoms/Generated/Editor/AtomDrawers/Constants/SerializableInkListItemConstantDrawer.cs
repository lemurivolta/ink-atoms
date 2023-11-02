#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Constant property drawer of type `LemuRivolta.InkAtoms.SerializableInkListItem`. Inherits from `AtomDrawer&lt;SerializableInkListItemConstant&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(SerializableInkListItemConstant))]
    public class SerializableInkListItemConstantDrawer : VariableDrawer<SerializableInkListItemConstant> { }
}
#endif
