using LemuRivolta.InkAtoms;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Variable Instancer of type `LemuRivolta.InkAtoms.InkAtomsStory`. Inherits from `AtomVariableInstancer&lt;
    ///     InkAtomsStoryVariable, InkAtomsStoryPair, LemuRivolta.InkAtoms.InkAtomsStory, InkAtomsStoryEvent,
    ///     InkAtomsStoryPairEvent, InkAtomsStoryInkAtomsStoryFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-hotpink")]
    [AddComponentMenu("Unity Atoms/Variable Instancers/InkAtomsStory Variable Instancer")]
    public class InkAtomsStoryVariableInstancer : AtomVariableInstancer<
        InkAtomsStoryVariable,
        InkAtomsStoryPair,
        InkAtomsStory,
        InkAtomsStoryEvent,
        InkAtomsStoryPairEvent,
        InkAtomsStoryInkAtomsStoryFunction>
    {
    }
}