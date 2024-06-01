#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Constant property drawer of type `LemuRivolta.InkAtoms.VariableValue`. Inherits from `AtomDrawer&lt;VariableValueConstant&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(VariableValueConstant))]
    public class VariableValueConstantDrawer : VariableDrawer<VariableValueConstant> { }
}
#endif
