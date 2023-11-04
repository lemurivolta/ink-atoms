using UnityEngine;
using System;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Variable of type `LemuRivolta.InkAtoms.InkAtomsStory`. Inherits from `AtomVariable&lt;LemuRivolta.InkAtoms.InkAtomsStory, InkAtomsStoryPair, InkAtomsStoryEvent, InkAtomsStoryPairEvent, InkAtomsStoryInkAtomsStoryFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-lush")]
    [CreateAssetMenu(menuName = "Unity Atoms/Variables/InkAtomsStory", fileName = "InkAtomsStoryVariable")]
    public sealed class InkAtomsStoryVariable : AtomVariable<LemuRivolta.InkAtoms.InkAtomsStory, InkAtomsStoryPair, InkAtomsStoryEvent, InkAtomsStoryPairEvent, InkAtomsStoryInkAtomsStoryFunction>
    {
        protected override bool ValueEquals(LemuRivolta.InkAtoms.InkAtomsStory other)
        {
            return Value == other;
        }
    }
}
