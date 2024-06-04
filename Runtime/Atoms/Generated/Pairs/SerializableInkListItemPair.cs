using System;
using LemuRivolta.InkAtoms;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     IPair of type `&lt;LemuRivolta.InkAtoms.SerializableInkListItem&gt;`. Inherits from `IPair&lt;
    ///     LemuRivolta.InkAtoms.SerializableInkListItem&gt;`.
    /// </summary>
    [Serializable]
    public struct SerializableInkListItemPair : IPair<SerializableInkListItem>
    {
        public SerializableInkListItem Item1
        {
            get => _item1;
            set => _item1 = value;
        }

        public SerializableInkListItem Item2
        {
            get => _item2;
            set => _item2 = value;
        }

        [SerializeField] private SerializableInkListItem _item1;
        [SerializeField] private SerializableInkListItem _item2;

        public void Deconstruct(out SerializableInkListItem item1, out SerializableInkListItem item2)
        {
            item1 = Item1;
            item2 = Item2;
        }
    }
}