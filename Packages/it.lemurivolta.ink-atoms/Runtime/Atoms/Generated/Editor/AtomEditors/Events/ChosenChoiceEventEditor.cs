#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `LemuRivolta.InkAtoms.ChosenChoice`. Inherits from `AtomEventEditor&lt;LemuRivolta.InkAtoms.ChosenChoice, ChosenChoiceEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(ChosenChoiceEvent))]
    public sealed class ChosenChoiceEventEditor : AtomEventEditor<LemuRivolta.InkAtoms.ChosenChoice, ChosenChoiceEvent> { }
}
#endif
