#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Constant property drawer of type `LemuRivolta.InkAtoms.InkAtomsStory`. Inherits from `AtomDrawer&lt;InkAtomsStoryConstant&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(InkAtomsStoryConstant))]
    public class InkAtomsStoryConstantDrawer : VariableDrawer<InkAtomsStoryConstant> { }
}
#endif
