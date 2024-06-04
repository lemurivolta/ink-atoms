using System;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Event Reference of type `LemuRivolta.InkAtoms.InkAtomsStory`. Inherits from `AtomEventReference&lt;
    ///     LemuRivolta.InkAtoms.InkAtomsStory, InkAtomsStoryVariable, InkAtomsStoryEvent, InkAtomsStoryVariableInstancer,
    ///     InkAtomsStoryEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class InkAtomsStoryEventReference : AtomEventReference<
        InkAtomsStory,
        InkAtomsStoryVariable,
        InkAtomsStoryEvent,
        InkAtomsStoryVariableInstancer,
        InkAtomsStoryEventInstancer>, IGetEvent
    {
    }
}