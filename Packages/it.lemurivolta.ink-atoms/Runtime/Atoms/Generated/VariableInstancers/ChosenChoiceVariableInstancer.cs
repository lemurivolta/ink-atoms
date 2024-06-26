using LemuRivolta.InkAtoms;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Variable Instancer of type `LemuRivolta.InkAtoms.ChosenChoice`. Inherits from `AtomVariableInstancer&lt;
    ///     ChosenChoiceVariable, ChosenChoicePair, LemuRivolta.InkAtoms.ChosenChoice, ChosenChoiceEvent,
    ///     ChosenChoicePairEvent, ChosenChoiceChosenChoiceFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-hotpink")]
    [AddComponentMenu("Unity Atoms/Variable Instancers/ChosenChoice Variable Instancer")]
    public class ChosenChoiceVariableInstancer : AtomVariableInstancer<
        ChosenChoiceVariable,
        ChosenChoicePair,
        ChosenChoice,
        ChosenChoiceEvent,
        ChosenChoicePairEvent,
        ChosenChoiceChosenChoiceFunction>
    {
    }
}