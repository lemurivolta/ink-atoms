using UnityEditor;
using UnityAtoms.Editor;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Variable Inspector of type `LemuRivolta.InkAtoms.SerializableInkListItem`. Inherits from `AtomVariableEditor`
    /// </summary>
    [CustomEditor(typeof(SerializableInkListItemVariable))]
    public sealed class SerializableInkListItemVariableEditor : AtomVariableEditor<LemuRivolta.InkAtoms.SerializableInkListItem, SerializableInkListItemPair> { }
}
