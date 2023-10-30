#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `LemuRivolta.InkAtoms.InkAtomsStory`. Inherits from `AtomDrawer&lt;InkAtomsStoryEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(InkAtomsStoryEvent))]
    public class InkAtomsStoryEventDrawer : AtomDrawer<InkAtomsStoryEvent> { }
}
#endif
