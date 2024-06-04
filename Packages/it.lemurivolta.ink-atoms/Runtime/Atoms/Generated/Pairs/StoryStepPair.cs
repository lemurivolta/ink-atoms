using System;
using LemuRivolta.InkAtoms;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     IPair of type `&lt;LemuRivolta.InkAtoms.StoryStep&gt;`. Inherits from `IPair&lt;LemuRivolta.InkAtoms.StoryStep&gt;
    ///     `.
    /// </summary>
    [Serializable]
    public struct StoryStepPair : IPair<StoryStep>
    {
        public StoryStep Item1
        {
            get => _item1;
            set => _item1 = value;
        }

        public StoryStep Item2
        {
            get => _item2;
            set => _item2 = value;
        }

        [SerializeField] private StoryStep _item1;
        [SerializeField] private StoryStep _item2;

        public void Deconstruct(out StoryStep item1, out StoryStep item2)
        {
            item1 = Item1;
            item2 = Item2;
        }
    }
}