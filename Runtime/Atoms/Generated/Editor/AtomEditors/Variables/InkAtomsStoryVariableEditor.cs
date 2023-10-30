using UnityEditor;
using UnityAtoms.Editor;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Variable Inspector of type `LemuRivolta.InkAtoms.InkAtomsStory`. Inherits from `AtomVariableEditor`
    /// </summary>
    [CustomEditor(typeof(InkAtomsStoryVariable))]
    public sealed class InkAtomsStoryVariableEditor : AtomVariableEditor<LemuRivolta.InkAtoms.InkAtomsStory, InkAtomsStoryPair> { }
}
