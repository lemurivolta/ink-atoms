using LemuRivolta.InkAtoms;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Event of type `LemuRivolta.InkAtoms.ChosenChoice`. Inherits from `AtomEvent&lt;LemuRivolta.InkAtoms.ChosenChoice
    ///     &gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/ChosenChoice", fileName = "ChosenChoiceEvent")]
    public sealed class ChosenChoiceEvent : AtomEvent<ChosenChoice>
    {
    }
}