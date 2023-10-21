using UnityEngine;
using UnityAtoms.BaseAtoms;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Set variable value Action of type `LemuRivolta.InkAtoms.StoryStep`. Inherits from `SetVariableValue&lt;LemuRivolta.InkAtoms.StoryStep, StoryStepPair, StoryStepVariable, StoryStepConstant, StoryStepReference, StoryStepEvent, StoryStepPairEvent, StoryStepVariableInstancer&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-purple")]
    [CreateAssetMenu(menuName = "Unity Atoms/Actions/Set Variable Value/StoryStep", fileName = "SetStoryStepVariableValue")]
    public sealed class SetStoryStepVariableValue : SetVariableValue<
        LemuRivolta.InkAtoms.StoryStep,
        StoryStepPair,
        StoryStepVariable,
        StoryStepConstant,
        StoryStepReference,
        StoryStepEvent,
        StoryStepPairEvent,
        StoryStepStoryStepFunction,
        StoryStepVariableInstancer>
    { }
}
