using LemuRivolta.InkAtoms;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Variable Instancer of type `LemuRivolta.InkAtoms.SerializableInkListItem`. Inherits from `AtomVariableInstancer&lt;
    ///     SerializableInkListItemVariable, SerializableInkListItemPair, LemuRivolta.InkAtoms.SerializableInkListItem,
    ///     SerializableInkListItemEvent, SerializableInkListItemPairEvent,
    ///     SerializableInkListItemSerializableInkListItemFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-hotpink")]
    [AddComponentMenu("Unity Atoms/Variable Instancers/SerializableInkListItem Variable Instancer")]
    public class SerializableInkListItemVariableInstancer : AtomVariableInstancer<
        SerializableInkListItemVariable,
        SerializableInkListItemPair,
        SerializableInkListItem,
        SerializableInkListItemEvent,
        SerializableInkListItemPairEvent,
        SerializableInkListItemSerializableInkListItemFunction>
    {
    }
}