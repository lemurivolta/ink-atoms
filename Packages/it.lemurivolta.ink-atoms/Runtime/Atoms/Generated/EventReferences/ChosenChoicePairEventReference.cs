using System;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference of type `ChosenChoicePair`. Inherits from `AtomEventReference&lt;ChosenChoicePair, ChosenChoiceVariable, ChosenChoicePairEvent, ChosenChoiceVariableInstancer, ChosenChoicePairEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class ChosenChoicePairEventReference : AtomEventReference<
        ChosenChoicePair,
        ChosenChoiceVariable,
        ChosenChoicePairEvent,
        ChosenChoiceVariableInstancer,
        ChosenChoicePairEventInstancer>, IGetEvent 
    { }
}
