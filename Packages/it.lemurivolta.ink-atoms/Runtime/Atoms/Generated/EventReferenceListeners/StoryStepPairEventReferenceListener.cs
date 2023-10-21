using UnityEngine;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference Listener of type `StoryStepPair`. Inherits from `AtomEventReferenceListener&lt;StoryStepPair, StoryStepPairEvent, StoryStepPairEventReference, StoryStepPairUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/StoryStepPair Event Reference Listener")]
    public sealed class StoryStepPairEventReferenceListener : AtomEventReferenceListener<
        StoryStepPair,
        StoryStepPairEvent,
        StoryStepPairEventReference,
        StoryStepPairUnityEvent>
    { }
}
