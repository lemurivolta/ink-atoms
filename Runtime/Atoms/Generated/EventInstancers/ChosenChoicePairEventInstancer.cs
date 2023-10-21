using UnityEngine;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Instancer of type `ChosenChoicePair`. Inherits from `AtomEventInstancer&lt;ChosenChoicePair, ChosenChoicePairEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-sign-blue")]
    [AddComponentMenu("Unity Atoms/Event Instancers/ChosenChoicePair Event Instancer")]
    public class ChosenChoicePairEventInstancer : AtomEventInstancer<ChosenChoicePair, ChosenChoicePairEvent> { }
}
