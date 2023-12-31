#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Value List property drawer of type `LemuRivolta.InkAtoms.ChosenChoice`. Inherits from `AtomDrawer&lt;ChosenChoiceValueList&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(ChosenChoiceValueList))]
    public class ChosenChoiceValueListDrawer : AtomDrawer<ChosenChoiceValueList> { }
}
#endif
