using System;
using Ink.Runtime;
using UnityEngine.Serialization;

namespace LemuRivolta.InkAtoms
{
    /// <summary>
    ///     A serializable version of an <see cref="InkListItem" /> to use in Atoms. It can be implicitly
    ///     cast to and from an InkListItem.
    /// </summary>
    [Serializable]
    public class SerializableInkListItem : IEquatable<SerializableInkListItem>
    {
        public string originName;
        public string itemName;

        public SerializableInkListItem(string originName, string itemName)
        {
            this.originName = originName;
            this.itemName = itemName;
        }

        public bool Equals(SerializableInkListItem other)
        {
            return other != null &&
                   other.originName == originName &&
                   other.itemName == itemName;
        }

        public static implicit operator InkListItem(SerializableInkListItem serializableInkListItem)
        {
            return new InkListItem(serializableInkListItem.originName, serializableInkListItem.itemName);
        }

        public static implicit operator SerializableInkListItem(InkListItem inkListItem)
        {
            return new SerializableInkListItem(inkListItem.originName, inkListItem.itemName);
        }

        public override string ToString() => $"SerializableInkListItem({originName}, {itemName})";
    }
}