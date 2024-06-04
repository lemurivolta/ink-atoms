using LemuRivolta.InkAtoms;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Variable Instancer of type `LemuRivolta.InkAtoms.StoryStep`. Inherits from `AtomVariableInstancer&lt;
    ///     StoryStepVariable, StoryStepPair, LemuRivolta.InkAtoms.StoryStep, StoryStepEvent, StoryStepPairEvent,
    ///     StoryStepStoryStepFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-hotpink")]
    [AddComponentMenu("Unity Atoms/Variable Instancers/StoryStep Variable Instancer")]
    public class StoryStepVariableInstancer : AtomVariableInstancer<
        StoryStepVariable,
        StoryStepPair,
        StoryStep,
        StoryStepEvent,
        StoryStepPairEvent,
        StoryStepStoryStepFunction>
    {
    }
}