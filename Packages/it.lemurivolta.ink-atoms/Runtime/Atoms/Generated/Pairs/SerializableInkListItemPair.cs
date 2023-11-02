using System;
using UnityEngine;
using LemuRivolta.InkAtoms;
namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// IPair of type `&lt;LemuRivolta.InkAtoms.SerializableInkListItem&gt;`. Inherits from `IPair&lt;LemuRivolta.InkAtoms.SerializableInkListItem&gt;`.
    /// </summary>
    [Serializable]
    public struct SerializableInkListItemPair : IPair<LemuRivolta.InkAtoms.SerializableInkListItem>
    {
        public LemuRivolta.InkAtoms.SerializableInkListItem Item1 { get => _item1; set => _item1 = value; }
        public LemuRivolta.InkAtoms.SerializableInkListItem Item2 { get => _item2; set => _item2 = value; }

        [SerializeField]
        private LemuRivolta.InkAtoms.SerializableInkListItem _item1;
        [SerializeField]
        private LemuRivolta.InkAtoms.SerializableInkListItem _item2;

        public void Deconstruct(out LemuRivolta.InkAtoms.SerializableInkListItem item1, out LemuRivolta.InkAtoms.SerializableInkListItem item2) { item1 = Item1; item2 = Item2; }
    }
}