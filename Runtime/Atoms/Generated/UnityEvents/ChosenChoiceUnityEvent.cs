using System;
using LemuRivolta.InkAtoms;
using UnityEngine.Events;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     None generic Unity Event of type `LemuRivolta.InkAtoms.ChosenChoice`. Inherits from `UnityEvent&lt;
    ///     LemuRivolta.InkAtoms.ChosenChoice&gt;`.
    /// </summary>
    [Serializable]
    public sealed class ChosenChoiceUnityEvent : UnityEvent<ChosenChoice>
    {
    }
}