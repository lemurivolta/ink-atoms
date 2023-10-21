using UnityEngine;
using UnityAtoms.BaseAtoms;
using LemuRivolta.InkAtoms;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// Adds Variable Instancer's Variable of type LemuRivolta.InkAtoms.StoryStep to a Collection or List on OnEnable and removes it on OnDestroy. 
    /// </summary>
    [AddComponentMenu("Unity Atoms/Sync Variable Instancer to Collection/Sync StoryStep Variable Instancer to Collection")]
    [EditorIcon("atom-icon-delicate")]
    public class SyncStoryStepVariableInstancerToCollection : SyncVariableInstancerToCollection<LemuRivolta.InkAtoms.StoryStep, StoryStepVariable, StoryStepVariableInstancer> { }
}
