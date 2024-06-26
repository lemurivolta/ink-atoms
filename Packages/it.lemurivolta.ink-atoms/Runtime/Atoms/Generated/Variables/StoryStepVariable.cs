using LemuRivolta.InkAtoms;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Variable of type `LemuRivolta.InkAtoms.StoryStep`. Inherits from `EquatableAtomVariable&lt;
    ///     LemuRivolta.InkAtoms.StoryStep, StoryStepPair, StoryStepEvent, StoryStepPairEvent, StoryStepStoryStepFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-lush")]
    [CreateAssetMenu(menuName = "Unity Atoms/Variables/StoryStep", fileName = "StoryStepVariable")]
    public sealed class StoryStepVariable : EquatableAtomVariable<StoryStep, StoryStepPair, StoryStepEvent,
        StoryStepPairEvent, StoryStepStoryStepFunction>
    {
    }
}