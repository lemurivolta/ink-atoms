#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Variable property drawer of type `LemuRivolta.InkAtoms.VariableObserver.VariableChange`. Inherits from `AtomDrawer&lt;VariableChangeVariable&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(VariableChangeVariable))]
    public class VariableChangeVariableDrawer : VariableDrawer<VariableChangeVariable> { }
}
#endif
