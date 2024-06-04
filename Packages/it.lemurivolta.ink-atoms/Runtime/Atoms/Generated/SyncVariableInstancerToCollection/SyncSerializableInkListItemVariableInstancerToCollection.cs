using LemuRivolta.InkAtoms;
using UnityEngine;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    ///     Adds Variable Instancer's Variable of type LemuRivolta.InkAtoms.SerializableInkListItem to a Collection or List on
    ///     OnEnable and removes it on OnDestroy.
    /// </summary>
    [AddComponentMenu(
        "Unity Atoms/Sync Variable Instancer to Collection/Sync SerializableInkListItem Variable Instancer to Collection")]
    [EditorIcon("atom-icon-delicate")]
    public class SyncSerializableInkListItemVariableInstancerToCollection : SyncVariableInstancerToCollection<
        SerializableInkListItem, SerializableInkListItemVariable, SerializableInkListItemVariableInstancer>
    {
    }
}