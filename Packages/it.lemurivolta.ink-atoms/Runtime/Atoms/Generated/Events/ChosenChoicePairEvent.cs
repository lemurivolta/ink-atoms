using UnityEngine;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event of type `ChosenChoicePair`. Inherits from `AtomEvent&lt;ChosenChoicePair&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/ChosenChoicePair", fileName = "ChosenChoicePairEvent")]
    public sealed class ChosenChoicePairEvent : AtomEvent<ChosenChoicePair>
    {
    }
}
