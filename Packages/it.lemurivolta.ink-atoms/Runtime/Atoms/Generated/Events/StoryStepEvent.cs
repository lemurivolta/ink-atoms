using LemuRivolta.InkAtoms;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Event of type `LemuRivolta.InkAtoms.StoryStep`. Inherits from `AtomEvent&lt;LemuRivolta.InkAtoms.StoryStep&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/StoryStep", fileName = "StoryStepEvent")]
    public sealed class StoryStepEvent : AtomEvent<StoryStep>
    {
    }
}