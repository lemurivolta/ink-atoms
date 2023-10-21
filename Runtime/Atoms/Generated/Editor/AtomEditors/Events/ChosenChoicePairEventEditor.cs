#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `ChosenChoicePair`. Inherits from `AtomEventEditor&lt;ChosenChoicePair, ChosenChoicePairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(ChosenChoicePairEvent))]
    public sealed class ChosenChoicePairEventEditor : AtomEventEditor<ChosenChoicePair, ChosenChoicePairEvent> { }
}
#endif
