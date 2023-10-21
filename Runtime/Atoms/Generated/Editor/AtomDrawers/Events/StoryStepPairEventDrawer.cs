#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `StoryStepPair`. Inherits from `AtomDrawer&lt;StoryStepPairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(StoryStepPairEvent))]
    public class StoryStepPairEventDrawer : AtomDrawer<StoryStepPairEvent> { }
}
#endif
