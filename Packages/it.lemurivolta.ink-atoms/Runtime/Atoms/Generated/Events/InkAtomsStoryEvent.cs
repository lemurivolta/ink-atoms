using LemuRivolta.InkAtoms;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Event of type `LemuRivolta.InkAtoms.InkAtomsStory`. Inherits from `AtomEvent&lt;LemuRivolta.InkAtoms.InkAtomsStory
    ///     &gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/InkAtomsStory", fileName = "InkAtomsStoryEvent")]
    public sealed class InkAtomsStoryEvent : AtomEvent<InkAtomsStory>
    {
    }
}