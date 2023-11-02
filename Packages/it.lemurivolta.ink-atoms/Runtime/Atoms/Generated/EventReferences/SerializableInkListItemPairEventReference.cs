using System;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference of type `SerializableInkListItemPair`. Inherits from `AtomEventReference&lt;SerializableInkListItemPair, SerializableInkListItemVariable, SerializableInkListItemPairEvent, SerializableInkListItemVariableInstancer, SerializableInkListItemPairEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class SerializableInkListItemPairEventReference : AtomEventReference<
        SerializableInkListItemPair,
        SerializableInkListItemVariable,
        SerializableInkListItemPairEvent,
        SerializableInkListItemVariableInstancer,
        SerializableInkListItemPairEventInstancer>, IGetEvent 
    { }
}
