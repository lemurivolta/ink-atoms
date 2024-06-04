using System;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Event Reference of type `LemuRivolta.InkAtoms.SerializableInkListItem`. Inherits from `AtomEventReference&lt;
    ///     LemuRivolta.InkAtoms.SerializableInkListItem, SerializableInkListItemVariable, SerializableInkListItemEvent,
    ///     SerializableInkListItemVariableInstancer, SerializableInkListItemEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class SerializableInkListItemEventReference : AtomEventReference<
        SerializableInkListItem,
        SerializableInkListItemVariable,
        SerializableInkListItemEvent,
        SerializableInkListItemVariableInstancer,
        SerializableInkListItemEventInstancer>, IGetEvent
    {
    }
}