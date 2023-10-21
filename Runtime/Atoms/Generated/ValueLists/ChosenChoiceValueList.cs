using UnityEngine;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Value List of type `LemuRivolta.InkAtoms.ChosenChoice`. Inherits from `AtomValueList&lt;LemuRivolta.InkAtoms.ChosenChoice, ChosenChoiceEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-piglet")]
    [CreateAssetMenu(menuName = "Unity Atoms/Value Lists/ChosenChoice", fileName = "ChosenChoiceValueList")]
    public sealed class ChosenChoiceValueList : AtomValueList<LemuRivolta.InkAtoms.ChosenChoice, ChosenChoiceEvent> { }
}
