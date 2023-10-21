#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `ChosenChoicePair`. Inherits from `AtomDrawer&lt;ChosenChoicePairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(ChosenChoicePairEvent))]
    public class ChosenChoicePairEventDrawer : AtomDrawer<ChosenChoicePairEvent> { }
}
#endif
