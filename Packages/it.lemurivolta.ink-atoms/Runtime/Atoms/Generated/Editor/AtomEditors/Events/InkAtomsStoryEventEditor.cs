#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `LemuRivolta.InkAtoms.InkAtomsStory`. Inherits from `AtomEventEditor&lt;LemuRivolta.InkAtoms.InkAtomsStory, InkAtomsStoryEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(InkAtomsStoryEvent))]
    public sealed class InkAtomsStoryEventEditor : AtomEventEditor<LemuRivolta.InkAtoms.InkAtomsStory, InkAtomsStoryEvent> { }
}
#endif
