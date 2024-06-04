using LemuRivolta.InkAtoms;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Event Instancer of type `LemuRivolta.InkAtoms.InkAtomsStory`. Inherits from `AtomEventInstancer&lt;
    ///     LemuRivolta.InkAtoms.InkAtomsStory, InkAtomsStoryEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-sign-blue")]
    [AddComponentMenu("Unity Atoms/Event Instancers/InkAtomsStory Event Instancer")]
    public class InkAtomsStoryEventInstancer : AtomEventInstancer<InkAtomsStory, InkAtomsStoryEvent>
    {
    }
}