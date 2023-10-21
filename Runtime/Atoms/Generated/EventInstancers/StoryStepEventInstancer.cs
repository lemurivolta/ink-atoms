using UnityEngine;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Event Instancer of type `LemuRivolta.InkAtoms.StoryStep`. Inherits from `AtomEventInstancer&lt;LemuRivolta.InkAtoms.StoryStep, StoryStepEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-sign-blue")]
    [AddComponentMenu("Unity Atoms/Event Instancers/StoryStep Event Instancer")]
    public class StoryStepEventInstancer : AtomEventInstancer<LemuRivolta.InkAtoms.StoryStep, StoryStepEvent> { }
}
