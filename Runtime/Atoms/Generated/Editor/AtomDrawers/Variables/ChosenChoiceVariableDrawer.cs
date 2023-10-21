#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Variable property drawer of type `LemuRivolta.InkAtoms.ChosenChoice`. Inherits from `AtomDrawer&lt;ChosenChoiceVariable&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(ChosenChoiceVariable))]
    public class ChosenChoiceVariableDrawer : VariableDrawer<ChosenChoiceVariable> { }
}
#endif
