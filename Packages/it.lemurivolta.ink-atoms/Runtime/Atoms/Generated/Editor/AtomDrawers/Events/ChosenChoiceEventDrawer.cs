#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `LemuRivolta.InkAtoms.ChosenChoice`. Inherits from `AtomDrawer&lt;ChosenChoiceEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(ChosenChoiceEvent))]
    public class ChosenChoiceEventDrawer : AtomDrawer<ChosenChoiceEvent> { }
}
#endif
