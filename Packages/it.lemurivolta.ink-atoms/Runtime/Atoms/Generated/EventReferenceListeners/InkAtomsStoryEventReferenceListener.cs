using LemuRivolta.InkAtoms;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Event Reference Listener of type `LemuRivolta.InkAtoms.InkAtomsStory`. Inherits from `AtomEventReferenceListener
    ///     &lt;LemuRivolta.InkAtoms.InkAtomsStory, InkAtomsStoryEvent, InkAtomsStoryEventReference, InkAtomsStoryUnityEvent
    ///     &gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/InkAtomsStory Event Reference Listener")]
    public sealed class InkAtomsStoryEventReferenceListener : AtomEventReferenceListener<
        InkAtomsStory,
        InkAtomsStoryEvent,
        InkAtomsStoryEventReference,
        InkAtomsStoryUnityEvent>
    {
    }
}