#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `VariableChangePair`. Inherits from `AtomDrawer&lt;VariableChangePairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(VariableChangePairEvent))]
    public class VariableChangePairEventDrawer : AtomDrawer<VariableChangePairEvent> { }
}
#endif
