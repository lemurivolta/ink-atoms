using System;
using LemuRivolta.InkAtoms.VariableObserver;
using UnityEngine.Events;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     None generic Unity Event of type `LemuRivolta.InkAtoms.VariableObserver.VariableChange`. Inherits from `UnityEvent
    ///     &lt;LemuRivolta.InkAtoms.VariableObserver.VariableChange&gt;`.
    /// </summary>
    [Serializable]
    public sealed class VariableChangeUnityEvent : UnityEvent<VariableChange>
    {
    }
}