using LemuRivolta.InkAtoms;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Event Instancer of type `LemuRivolta.InkAtoms.ChosenChoice`. Inherits from `AtomEventInstancer&lt;
    ///     LemuRivolta.InkAtoms.ChosenChoice, ChosenChoiceEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-sign-blue")]
    [AddComponentMenu("Unity Atoms/Event Instancers/ChosenChoice Event Instancer")]
    public class ChosenChoiceEventInstancer : AtomEventInstancer<ChosenChoice, ChosenChoiceEvent>
    {
    }
}