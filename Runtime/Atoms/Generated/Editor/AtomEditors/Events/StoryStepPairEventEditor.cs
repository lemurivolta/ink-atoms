#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `StoryStepPair`. Inherits from `AtomEventEditor&lt;StoryStepPair, StoryStepPairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(StoryStepPairEvent))]
    public sealed class StoryStepPairEventEditor : AtomEventEditor<StoryStepPair, StoryStepPairEvent> { }
}
#endif
