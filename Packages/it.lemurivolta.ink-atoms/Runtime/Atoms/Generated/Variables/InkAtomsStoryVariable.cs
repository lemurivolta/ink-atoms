using LemuRivolta.InkAtoms;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Variable of type `LemuRivolta.InkAtoms.InkAtomsStory`. Inherits from `AtomVariable&lt;
    ///     LemuRivolta.InkAtoms.InkAtomsStory, InkAtomsStoryPair, InkAtomsStoryEvent, InkAtomsStoryPairEvent,
    ///     InkAtomsStoryInkAtomsStoryFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-lush")]
    [CreateAssetMenu(menuName = "Unity Atoms/Variables/InkAtomsStory", fileName = "InkAtomsStoryVariable")]
    public sealed class InkAtomsStoryVariable : AtomVariable<InkAtomsStory, InkAtomsStoryPair, InkAtomsStoryEvent,
        InkAtomsStoryPairEvent, InkAtomsStoryInkAtomsStoryFunction>
    {
        protected override bool ValueEquals(InkAtomsStory other)
        {
            return ReferenceEquals(Value, other);
        }
    }
}