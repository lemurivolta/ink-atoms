using System;
using Ink.Runtime;

namespace LemuRivolta.InkAtoms
{
    /// <summary>
    ///     A serializable version of an <see cref="InkListItem" /> to use in Atoms. It can be implicitly
    ///     cast to InkListItem.
    /// </summary>
    [Serializable]
    public readonly struct SerializableInkListItem : IEquatable<SerializableInkListItem>
    {
        public readonly string OriginName;
        public readonly string ItemName;

        public SerializableInkListItem(string originName, string itemName)
        {
            OriginName = originName;
            ItemName = itemName;
        }

        public readonly bool Equals(SerializableInkListItem other)
        {
            return other.OriginName == OriginName &&
                   other.ItemName == ItemName;
        }

        public static implicit operator InkListItem(SerializableInkListItem serializableInkListItem)
        {
            return new InkListItem(serializableInkListItem.OriginName, serializableInkListItem.ItemName);
        }

        public static implicit operator SerializableInkListItem(InkListItem inkListItem)
        {
            return new SerializableInkListItem(inkListItem.originName, inkListItem.itemName);
        }
    }
}