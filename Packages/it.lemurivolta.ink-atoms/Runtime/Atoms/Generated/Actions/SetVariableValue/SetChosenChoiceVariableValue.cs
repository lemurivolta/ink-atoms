using UnityEngine;
using UnityAtoms.BaseAtoms;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Set variable value Action of type `LemuRivolta.InkAtoms.ChosenChoice`. Inherits from `SetVariableValue&lt;LemuRivolta.InkAtoms.ChosenChoice, ChosenChoicePair, ChosenChoiceVariable, ChosenChoiceConstant, ChosenChoiceReference, ChosenChoiceEvent, ChosenChoicePairEvent, ChosenChoiceVariableInstancer&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-purple")]
    [CreateAssetMenu(menuName = "Unity Atoms/Actions/Set Variable Value/ChosenChoice", fileName = "SetChosenChoiceVariableValue")]
    public sealed class SetChosenChoiceVariableValue : SetVariableValue<
        LemuRivolta.InkAtoms.ChosenChoice,
        ChosenChoicePair,
        ChosenChoiceVariable,
        ChosenChoiceConstant,
        ChosenChoiceReference,
        ChosenChoiceEvent,
        ChosenChoicePairEvent,
        ChosenChoiceChosenChoiceFunction,
        ChosenChoiceVariableInstancer>
    { }
}
