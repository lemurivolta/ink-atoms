using System;
using UnityAtoms.BaseAtoms;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Reference of type `LemuRivolta.InkAtoms.StoryStep`. Inherits from `EquatableAtomReference&lt;LemuRivolta.InkAtoms.StoryStep, StoryStepPair, StoryStepConstant, StoryStepVariable, StoryStepEvent, StoryStepPairEvent, StoryStepStoryStepFunction, StoryStepVariableInstancer, AtomCollection, AtomList&gt;`.
    /// </summary>
    [Serializable]
    public sealed class StoryStepReference : EquatableAtomReference<
        LemuRivolta.InkAtoms.StoryStep,
        StoryStepPair,
        StoryStepConstant,
        StoryStepVariable,
        StoryStepEvent,
        StoryStepPairEvent,
        StoryStepStoryStepFunction,
        StoryStepVariableInstancer>, IEquatable<StoryStepReference>
    {
        public StoryStepReference() : base() { }
        public StoryStepReference(LemuRivolta.InkAtoms.StoryStep value) : base(value) { }
        public bool Equals(StoryStepReference other) { return base.Equals(other); }
    }
}
