using UnityEditor;
using UnityAtoms.Editor;
using LemuRivolta.InkAtoms.VariableObserver;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Variable Inspector of type `LemuRivolta.InkAtoms.VariableObserver.VariableChange`. Inherits from `AtomVariableEditor`
    /// </summary>
    [CustomEditor(typeof(VariableChangeVariable))]
    public sealed class VariableChangeVariableEditor : AtomVariableEditor<LemuRivolta.InkAtoms.VariableObserver.VariableChange, VariableChangePair> { }
}
