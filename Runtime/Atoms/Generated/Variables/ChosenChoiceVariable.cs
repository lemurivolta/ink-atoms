using LemuRivolta.InkAtoms;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Variable of type `LemuRivolta.InkAtoms.ChosenChoice`. Inherits from `EquatableAtomVariable&lt;
    ///     LemuRivolta.InkAtoms.ChosenChoice, ChosenChoicePair, ChosenChoiceEvent, ChosenChoicePairEvent,
    ///     ChosenChoiceChosenChoiceFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-lush")]
    [CreateAssetMenu(menuName = "Unity Atoms/Variables/ChosenChoice", fileName = "ChosenChoiceVariable")]
    public sealed class ChosenChoiceVariable : EquatableAtomVariable<ChosenChoice, ChosenChoicePair, ChosenChoiceEvent,
        ChosenChoicePairEvent, ChosenChoiceChosenChoiceFunction>
    {
    }
}