using LemuRivolta.InkAtoms;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Event Reference Listener of type `LemuRivolta.InkAtoms.ChosenChoice`. Inherits from `AtomEventReferenceListener&lt;
    ///     LemuRivolta.InkAtoms.ChosenChoice, ChosenChoiceEvent, ChosenChoiceEventReference, ChosenChoiceUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/ChosenChoice Event Reference Listener")]
    public sealed class ChosenChoiceEventReferenceListener : AtomEventReferenceListener<
        ChosenChoice,
        ChosenChoiceEvent,
        ChosenChoiceEventReference,
        ChosenChoiceUnityEvent>
    {
    }
}