using System;
using UnityEngine;
using LemuRivolta.InkAtoms;
namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// IPair of type `&lt;LemuRivolta.InkAtoms.InkAtomsStory&gt;`. Inherits from `IPair&lt;LemuRivolta.InkAtoms.InkAtomsStory&gt;`.
    /// </summary>
    [Serializable]
    public struct InkAtomsStoryPair : IPair<LemuRivolta.InkAtoms.InkAtomsStory>
    {
        public LemuRivolta.InkAtoms.InkAtomsStory Item1 { get => _item1; set => _item1 = value; }
        public LemuRivolta.InkAtoms.InkAtomsStory Item2 { get => _item2; set => _item2 = value; }

        [SerializeField]
        private LemuRivolta.InkAtoms.InkAtomsStory _item1;
        [SerializeField]
        private LemuRivolta.InkAtoms.InkAtomsStory _item2;

        public void Deconstruct(out LemuRivolta.InkAtoms.InkAtomsStory item1, out LemuRivolta.InkAtoms.InkAtomsStory item2) { item1 = Item1; item2 = Item2; }
    }
}