using System;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Reference of type `LemuRivolta.InkAtoms.SerializableInkListItem`. Inherits from `EquatableAtomReference&lt;
    ///     LemuRivolta.InkAtoms.SerializableInkListItem, SerializableInkListItemPair, SerializableInkListItemConstant,
    ///     SerializableInkListItemVariable, SerializableInkListItemEvent, SerializableInkListItemPairEvent,
    ///     SerializableInkListItemSerializableInkListItemFunction, SerializableInkListItemVariableInstancer, AtomCollection,
    ///     AtomList&gt;`.
    /// </summary>
    [Serializable]
    public sealed class SerializableInkListItemReference : EquatableAtomReference<
        SerializableInkListItem,
        SerializableInkListItemPair,
        SerializableInkListItemConstant,
        SerializableInkListItemVariable,
        SerializableInkListItemEvent,
        SerializableInkListItemPairEvent,
        SerializableInkListItemSerializableInkListItemFunction,
        SerializableInkListItemVariableInstancer>, IEquatable<SerializableInkListItemReference>
    {
        public SerializableInkListItemReference()
        {
        }

        public SerializableInkListItemReference(SerializableInkListItem value) : base(value)
        {
        }

        public bool Equals(SerializableInkListItemReference other)
        {
            return base.Equals(other);
        }
    }
}