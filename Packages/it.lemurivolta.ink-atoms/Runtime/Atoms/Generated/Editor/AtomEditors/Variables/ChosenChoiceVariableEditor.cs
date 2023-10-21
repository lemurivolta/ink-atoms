using UnityEditor;
using UnityAtoms.Editor;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Variable Inspector of type `LemuRivolta.InkAtoms.ChosenChoice`. Inherits from `AtomVariableEditor`
    /// </summary>
    [CustomEditor(typeof(ChosenChoiceVariable))]
    public sealed class ChosenChoiceVariableEditor : AtomVariableEditor<LemuRivolta.InkAtoms.ChosenChoice, ChosenChoicePair> { }
}
