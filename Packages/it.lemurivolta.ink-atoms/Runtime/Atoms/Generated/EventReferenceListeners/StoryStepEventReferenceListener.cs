using UnityEngine;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference Listener of type `LemuRivolta.InkAtoms.StoryStep`. Inherits from `AtomEventReferenceListener&lt;LemuRivolta.InkAtoms.StoryStep, StoryStepEvent, StoryStepEventReference, StoryStepUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/StoryStep Event Reference Listener")]
    public sealed class StoryStepEventReferenceListener : AtomEventReferenceListener<
        LemuRivolta.InkAtoms.StoryStep,
        StoryStepEvent,
        StoryStepEventReference,
        StoryStepUnityEvent>
    { }
}
