using UnityEngine;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event of type `InkAtomsStoryPair`. Inherits from `AtomEvent&lt;InkAtomsStoryPair&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/InkAtomsStoryPair", fileName = "InkAtomsStoryPairEvent")]
    public sealed class InkAtomsStoryPairEvent : AtomEvent<InkAtomsStoryPair>
    {
    }
}
