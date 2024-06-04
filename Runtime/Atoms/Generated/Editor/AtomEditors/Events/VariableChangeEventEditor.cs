#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;
using LemuRivolta.InkAtoms.VariableObserver;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `LemuRivolta.InkAtoms.VariableObserver.VariableChange`. Inherits from `AtomEventEditor&lt;LemuRivolta.InkAtoms.VariableObserver.VariableChange, VariableChangeEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(VariableChangeEvent))]
    public sealed class VariableChangeEventEditor : AtomEventEditor<LemuRivolta.InkAtoms.VariableObserver.VariableChange, VariableChangeEvent> { }
}
#endif
