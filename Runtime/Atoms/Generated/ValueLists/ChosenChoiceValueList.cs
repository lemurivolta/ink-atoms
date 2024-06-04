using LemuRivolta.InkAtoms;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Value List of type `LemuRivolta.InkAtoms.ChosenChoice`. Inherits from `AtomValueList&lt;
    ///     LemuRivolta.InkAtoms.ChosenChoice, ChosenChoiceEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-piglet")]
    [CreateAssetMenu(menuName = "Unity Atoms/Value Lists/ChosenChoice", fileName = "ChosenChoiceValueList")]
    public sealed class ChosenChoiceValueList : AtomValueList<ChosenChoice, ChosenChoiceEvent>
    {
    }
}