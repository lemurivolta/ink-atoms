#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Variable property drawer of type `LemuRivolta.InkAtoms.VariableValue`. Inherits from `AtomDrawer&lt;VariableValueVariable&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(VariableValueVariable))]
    public class VariableValueVariableDrawer : VariableDrawer<VariableValueVariable> { }
}
#endif
