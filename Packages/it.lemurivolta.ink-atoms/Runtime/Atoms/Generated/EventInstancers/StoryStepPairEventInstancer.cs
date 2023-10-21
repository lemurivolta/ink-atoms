using UnityEngine;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Instancer of type `StoryStepPair`. Inherits from `AtomEventInstancer&lt;StoryStepPair, StoryStepPairEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-sign-blue")]
    [AddComponentMenu("Unity Atoms/Event Instancers/StoryStepPair Event Instancer")]
    public class StoryStepPairEventInstancer : AtomEventInstancer<StoryStepPair, StoryStepPairEvent> { }
}
