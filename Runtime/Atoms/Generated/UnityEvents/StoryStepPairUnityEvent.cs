using System;
using UnityEngine.Events;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// None generic Unity Event of type `StoryStepPair`. Inherits from `UnityEvent&lt;StoryStepPair&gt;`.
    /// </summary>
    [Serializable]
    public sealed class StoryStepPairUnityEvent : UnityEvent<StoryStepPair> { }
}
