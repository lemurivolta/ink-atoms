#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `VariableValuePair`. Inherits from `AtomDrawer&lt;VariableValuePairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(VariableValuePairEvent))]
    public class VariableValuePairEventDrawer : AtomDrawer<VariableValuePairEvent> { }
}
#endif
