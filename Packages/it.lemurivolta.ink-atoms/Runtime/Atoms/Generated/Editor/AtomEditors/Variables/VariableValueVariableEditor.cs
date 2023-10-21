using UnityEditor;
using UnityAtoms.Editor;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Variable Inspector of type `LemuRivolta.InkAtoms.VariableValue`. Inherits from `AtomVariableEditor`
    /// </summary>
    [CustomEditor(typeof(VariableValueVariable))]
    public sealed class VariableValueVariableEditor : AtomVariableEditor<LemuRivolta.InkAtoms.VariableValue, VariableValuePair> { }
}
