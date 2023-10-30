#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `InkAtomsStoryPair`. Inherits from `AtomEventEditor&lt;InkAtomsStoryPair, InkAtomsStoryPairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(InkAtomsStoryPairEvent))]
    public sealed class InkAtomsStoryPairEventEditor : AtomEventEditor<InkAtomsStoryPair, InkAtomsStoryPairEvent> { }
}
#endif
