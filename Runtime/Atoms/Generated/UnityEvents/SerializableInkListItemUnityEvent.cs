using System;
using LemuRivolta.InkAtoms;
using UnityEngine.Events;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     None generic Unity Event of type `LemuRivolta.InkAtoms.SerializableInkListItem`. Inherits from `UnityEvent&lt;
    ///     LemuRivolta.InkAtoms.SerializableInkListItem&gt;`.
    /// </summary>
    [Serializable]
    public sealed class SerializableInkListItemUnityEvent : UnityEvent<SerializableInkListItem>
    {
    }
}