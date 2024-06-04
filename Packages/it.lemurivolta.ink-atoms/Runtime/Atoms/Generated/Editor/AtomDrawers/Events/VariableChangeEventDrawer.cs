#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `LemuRivolta.InkAtoms.VariableObserver.VariableChange`. Inherits from `AtomDrawer&lt;VariableChangeEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(VariableChangeEvent))]
    public class VariableChangeEventDrawer : AtomDrawer<VariableChangeEvent> { }
}
#endif
