using LemuRivolta.InkAtoms;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Value List of type `LemuRivolta.InkAtoms.StoryStep`. Inherits from `AtomValueList&lt;
    ///     LemuRivolta.InkAtoms.StoryStep, StoryStepEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-piglet")]
    [CreateAssetMenu(menuName = "Unity Atoms/Value Lists/StoryStep", fileName = "StoryStepValueList")]
    public sealed class StoryStepValueList : AtomValueList<StoryStep, StoryStepEvent>
    {
    }
}