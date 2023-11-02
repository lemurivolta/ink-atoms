using UnityEngine;
using UnityAtoms.BaseAtoms;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Set variable value Action of type `LemuRivolta.InkAtoms.SerializableInkListItem`. Inherits from `SetVariableValue&lt;LemuRivolta.InkAtoms.SerializableInkListItem, SerializableInkListItemPair, SerializableInkListItemVariable, SerializableInkListItemConstant, SerializableInkListItemReference, SerializableInkListItemEvent, SerializableInkListItemPairEvent, SerializableInkListItemVariableInstancer&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-purple")]
    [CreateAssetMenu(menuName = "Unity Atoms/Actions/Set Variable Value/SerializableInkListItem", fileName = "SetSerializableInkListItemVariableValue")]
    public sealed class SetSerializableInkListItemVariableValue : SetVariableValue<
        LemuRivolta.InkAtoms.SerializableInkListItem,
        SerializableInkListItemPair,
        SerializableInkListItemVariable,
        SerializableInkListItemConstant,
        SerializableInkListItemReference,
        SerializableInkListItemEvent,
        SerializableInkListItemPairEvent,
        SerializableInkListItemSerializableInkListItemFunction,
        SerializableInkListItemVariableInstancer>
    { }
}
