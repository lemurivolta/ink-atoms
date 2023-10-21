using UnityEditor;
using UnityAtoms.Editor;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Variable Inspector of type `LemuRivolta.InkAtoms.StoryStep`. Inherits from `AtomVariableEditor`
    /// </summary>
    [CustomEditor(typeof(StoryStepVariable))]
    public sealed class StoryStepVariableEditor : AtomVariableEditor<LemuRivolta.InkAtoms.StoryStep, StoryStepPair> { }
}
