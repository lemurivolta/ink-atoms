#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Variable property drawer of type `LemuRivolta.InkAtoms.InkAtomsStory`. Inherits from `AtomDrawer&lt;InkAtomsStoryVariable&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(InkAtomsStoryVariable))]
    public class InkAtomsStoryVariableDrawer : VariableDrawer<InkAtomsStoryVariable> { }
}
#endif
