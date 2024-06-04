using LemuRivolta.InkAtoms;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Event Reference Listener of type `LemuRivolta.InkAtoms.StoryStep`. Inherits from `AtomEventReferenceListener&lt;
    ///     LemuRivolta.InkAtoms.StoryStep, StoryStepEvent, StoryStepEventReference, StoryStepUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/StoryStep Event Reference Listener")]
    public sealed class StoryStepEventReferenceListener : AtomEventReferenceListener<
        StoryStep,
        StoryStepEvent,
        StoryStepEventReference,
        StoryStepUnityEvent>
    {
    }
}