using System;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference of type `StoryStepPair`. Inherits from `AtomEventReference&lt;StoryStepPair, StoryStepVariable, StoryStepPairEvent, StoryStepVariableInstancer, StoryStepPairEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class StoryStepPairEventReference : AtomEventReference<
        StoryStepPair,
        StoryStepVariable,
        StoryStepPairEvent,
        StoryStepVariableInstancer,
        StoryStepPairEventInstancer>, IGetEvent 
    { }
}
