#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Constant property drawer of type `LemuRivolta.InkAtoms.ChosenChoice`. Inherits from `AtomDrawer&lt;ChosenChoiceConstant&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(ChosenChoiceConstant))]
    public class ChosenChoiceConstantDrawer : VariableDrawer<ChosenChoiceConstant> { }
}
#endif
