#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `InkAtomsStoryPair`. Inherits from `AtomDrawer&lt;InkAtomsStoryPairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(InkAtomsStoryPairEvent))]
    public class InkAtomsStoryPairEventDrawer : AtomDrawer<InkAtomsStoryPairEvent> { }
}
#endif
