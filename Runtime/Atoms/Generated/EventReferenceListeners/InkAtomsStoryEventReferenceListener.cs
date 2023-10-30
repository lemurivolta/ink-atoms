using UnityEngine;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference Listener of type `LemuRivolta.InkAtoms.InkAtomsStory`. Inherits from `AtomEventReferenceListener&lt;LemuRivolta.InkAtoms.InkAtomsStory, InkAtomsStoryEvent, InkAtomsStoryEventReference, InkAtomsStoryUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/InkAtomsStory Event Reference Listener")]
    public sealed class InkAtomsStoryEventReferenceListener : AtomEventReferenceListener<
        LemuRivolta.InkAtoms.InkAtomsStory,
        InkAtomsStoryEvent,
        InkAtomsStoryEventReference,
        InkAtomsStoryUnityEvent>
    { }
}
