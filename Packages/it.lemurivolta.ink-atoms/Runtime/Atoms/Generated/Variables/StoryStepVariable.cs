using UnityEngine;
using System;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Variable of type `LemuRivolta.InkAtoms.StoryStep`. Inherits from `AtomVariable&lt;LemuRivolta.InkAtoms.StoryStep, StoryStepPair, StoryStepEvent, StoryStepPairEvent, StoryStepStoryStepFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-lush")]
    [CreateAssetMenu(menuName = "Unity Atoms/Variables/StoryStep", fileName = "StoryStepVariable")]
    public sealed class StoryStepVariable : AtomVariable<LemuRivolta.InkAtoms.StoryStep, StoryStepPair, StoryStepEvent, StoryStepPairEvent, StoryStepStoryStepFunction>
    {
        protected override bool ValueEquals(LemuRivolta.InkAtoms.StoryStep other)
        {
            throw new NotImplementedException();
        }
    }
}
