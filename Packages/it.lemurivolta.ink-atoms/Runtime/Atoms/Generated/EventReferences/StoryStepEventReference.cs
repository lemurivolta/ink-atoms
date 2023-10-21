using System;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference of type `LemuRivolta.InkAtoms.StoryStep`. Inherits from `AtomEventReference&lt;LemuRivolta.InkAtoms.StoryStep, StoryStepVariable, StoryStepEvent, StoryStepVariableInstancer, StoryStepEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class StoryStepEventReference : AtomEventReference<
        LemuRivolta.InkAtoms.StoryStep,
        StoryStepVariable,
        StoryStepEvent,
        StoryStepVariableInstancer,
        StoryStepEventInstancer>, IGetEvent 
    { }
}
