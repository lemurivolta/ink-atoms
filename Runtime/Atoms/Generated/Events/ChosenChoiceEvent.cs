using UnityEngine;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event of type `LemuRivolta.InkAtoms.ChosenChoice`. Inherits from `AtomEvent&lt;LemuRivolta.InkAtoms.ChosenChoice&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/ChosenChoice", fileName = "ChosenChoiceEvent")]
    public sealed class ChosenChoiceEvent : AtomEvent<LemuRivolta.InkAtoms.ChosenChoice>
    {
    }
}
