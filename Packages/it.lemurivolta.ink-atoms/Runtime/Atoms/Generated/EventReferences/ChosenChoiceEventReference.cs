using System;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Reference of type `LemuRivolta.InkAtoms.ChosenChoice`. Inherits from `AtomEventReference&lt;LemuRivolta.InkAtoms.ChosenChoice, ChosenChoiceVariable, ChosenChoiceEvent, ChosenChoiceVariableInstancer, ChosenChoiceEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class ChosenChoiceEventReference : AtomEventReference<
        LemuRivolta.InkAtoms.ChosenChoice,
        ChosenChoiceVariable,
        ChosenChoiceEvent,
        ChosenChoiceVariableInstancer,
        ChosenChoiceEventInstancer>, IGetEvent 
    { }
}
