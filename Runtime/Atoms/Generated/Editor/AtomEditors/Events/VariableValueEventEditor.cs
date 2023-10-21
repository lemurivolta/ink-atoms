#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `LemuRivolta.InkAtoms.VariableValue`. Inherits from `AtomEventEditor&lt;LemuRivolta.InkAtoms.VariableValue, VariableValueEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(VariableValueEvent))]
    public sealed class VariableValueEventEditor : AtomEventEditor<LemuRivolta.InkAtoms.VariableValue, VariableValueEvent> { }
}
#endif
