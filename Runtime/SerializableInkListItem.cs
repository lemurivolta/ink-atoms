using System;
using Ink.Runtime;

namespace LemuRivolta.InkAtoms
{
    /// <summary>
    /// A serializable version of an <see cref="InkListItem"/> to use in Atoms. It's implicitely
    /// castable to InkListItem.
    /// </summary>
    [Serializable]
    public readonly struct SerializableInkListItem : IEquatable<SerializableInkListItem>
    {
        public readonly string originName;
        public readonly string itemName;

        public SerializableInkListItem(string originName, string itemName)
        {
            this.originName = originName;
            this.itemName = itemName;
        }

        public readonly bool Equals(SerializableInkListItem other) =>
            other.originName == originName &&
            other.itemName == itemName;

        public static implicit operator InkListItem(SerializableInkListItem serializableInkListItem) =>
            new(serializableInkListItem.originName, serializableInkListItem.itemName);

        public static implicit operator SerializableInkListItem(InkListItem inkListItem) =>
            new(inkListItem.originName, inkListItem.itemName);
    }
}
