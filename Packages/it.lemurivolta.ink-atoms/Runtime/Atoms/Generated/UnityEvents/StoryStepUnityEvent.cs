using System;
using LemuRivolta.InkAtoms;
using UnityEngine.Events;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     None generic Unity Event of type `LemuRivolta.InkAtoms.StoryStep`. Inherits from `UnityEvent&lt;
    ///     LemuRivolta.InkAtoms.StoryStep&gt;`.
    /// </summary>
    [Serializable]
    public sealed class StoryStepUnityEvent : UnityEvent<StoryStep>
    {
    }
}