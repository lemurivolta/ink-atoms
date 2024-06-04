using System;
using LemuRivolta.InkAtoms;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     IPair of type `&lt;LemuRivolta.InkAtoms.InkAtomsStory&gt;`. Inherits from `IPair&lt;
    ///     LemuRivolta.InkAtoms.InkAtomsStory&gt;`.
    /// </summary>
    [Serializable]
    public struct InkAtomsStoryPair : IPair<InkAtomsStory>
    {
        public InkAtomsStory Item1
        {
            get => _item1;
            set => _item1 = value;
        }

        public InkAtomsStory Item2
        {
            get => _item2;
            set => _item2 = value;
        }

        [SerializeField] private InkAtomsStory _item1;
        [SerializeField] private InkAtomsStory _item2;

        public void Deconstruct(out InkAtomsStory item1, out InkAtomsStory item2)
        {
            item1 = Item1;
            item2 = Item2;
        }
    }
}