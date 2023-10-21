#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `VariableValuePair`. Inherits from `AtomEventEditor&lt;VariableValuePair, VariableValuePairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(VariableValuePairEvent))]
    public sealed class VariableValuePairEventEditor : AtomEventEditor<VariableValuePair, VariableValuePairEvent> { }
}
#endif
