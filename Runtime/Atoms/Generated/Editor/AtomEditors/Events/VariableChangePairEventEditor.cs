#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;
using LemuRivolta.InkAtoms.VariableObserver;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `VariableChangePair`. Inherits from `AtomEventEditor&lt;VariableChangePair, VariableChangePairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(VariableChangePairEvent))]
    public sealed class VariableChangePairEventEditor : AtomEventEditor<VariableChangePair, VariableChangePairEvent> { }
}
#endif
