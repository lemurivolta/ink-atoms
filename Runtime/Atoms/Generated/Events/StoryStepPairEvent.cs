using UnityEngine;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event of type `StoryStepPair`. Inherits from `AtomEvent&lt;StoryStepPair&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/StoryStepPair", fileName = "StoryStepPairEvent")]
    public sealed class StoryStepPairEvent : AtomEvent<StoryStepPair>
    {
    }
}
