using System;
using UnityEngine;
using LemuRivolta.InkAtoms;
namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// IPair of type `&lt;LemuRivolta.InkAtoms.StoryStep&gt;`. Inherits from `IPair&lt;LemuRivolta.InkAtoms.StoryStep&gt;`.
    /// </summary>
    [Serializable]
    public struct StoryStepPair : IPair<LemuRivolta.InkAtoms.StoryStep>
    {
        public LemuRivolta.InkAtoms.StoryStep Item1 { get => _item1; set => _item1 = value; }
        public LemuRivolta.InkAtoms.StoryStep Item2 { get => _item2; set => _item2 = value; }

        [SerializeField]
        private LemuRivolta.InkAtoms.StoryStep _item1;
        [SerializeField]
        private LemuRivolta.InkAtoms.StoryStep _item2;

        public void Deconstruct(out LemuRivolta.InkAtoms.StoryStep item1, out LemuRivolta.InkAtoms.StoryStep item2) { item1 = Item1; item2 = Item2; }
    }
}