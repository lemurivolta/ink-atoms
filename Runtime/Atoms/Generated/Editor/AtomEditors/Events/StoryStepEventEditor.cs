#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `LemuRivolta.InkAtoms.StoryStep`. Inherits from `AtomEventEditor&lt;LemuRivolta.InkAtoms.StoryStep, StoryStepEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(StoryStepEvent))]
    public sealed class StoryStepEventEditor : AtomEventEditor<LemuRivolta.InkAtoms.StoryStep, StoryStepEvent> { }
}
#endif
