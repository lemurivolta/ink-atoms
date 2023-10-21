#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `LemuRivolta.InkAtoms.VariableValue`. Inherits from `AtomDrawer&lt;VariableValueEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(VariableValueEvent))]
    public class VariableValueEventDrawer : AtomDrawer<VariableValueEvent> { }
}
#endif
