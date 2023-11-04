using System;
using UnityAtoms.BaseAtoms;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Reference of type `LemuRivolta.InkAtoms.InkAtomsStory`. Inherits from `AtomReference&lt;LemuRivolta.InkAtoms.InkAtomsStory, InkAtomsStoryPair, InkAtomsStoryConstant, InkAtomsStoryVariable, InkAtomsStoryEvent, InkAtomsStoryPairEvent, InkAtomsStoryInkAtomsStoryFunction, InkAtomsStoryVariableInstancer, AtomCollection, AtomList&gt;`.
    /// </summary>
    [Serializable]
    public sealed class InkAtomsStoryReference : AtomReference<
        LemuRivolta.InkAtoms.InkAtomsStory,
        InkAtomsStoryPair,
        InkAtomsStoryConstant,
        InkAtomsStoryVariable,
        InkAtomsStoryEvent,
        InkAtomsStoryPairEvent,
        InkAtomsStoryInkAtomsStoryFunction,
        InkAtomsStoryVariableInstancer>, IEquatable<InkAtomsStoryReference>
    {
        public InkAtomsStoryReference() : base() { }
        public InkAtomsStoryReference(LemuRivolta.InkAtoms.InkAtomsStory value) : base(value) { }
        public bool Equals(InkAtomsStoryReference other) { return base.Equals(other); }
        protected override bool ValueEquals(LemuRivolta.InkAtoms.InkAtomsStory other)
        {
            throw new NotImplementedException();
        }
    }
}
