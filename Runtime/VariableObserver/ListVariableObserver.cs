#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Serialization;

namespace LemuRivolta.InkAtoms.VariableObserver
{
    [Serializable]
    public class ListVariableObserver : VariableObserverByName<InkList>
    {
        /// <summary>
        ///     The atom value list that contains the ink list items.
        /// </summary>
        [FormerlySerializedAs("variableList")] [SerializeField]
        private SerializableInkListItemValueList? variable;

        internal override void UseValue(InkList? oldList, InkList newList)
        {
            if (variable == null) throw new Exception("No atom variable set");

            ICollection<InkListItem> oldItems = oldList == null ? Array.Empty<InkListItem>() : oldList.Keys;
            var newItems = newList.Keys;

            // remove all items no longer present
            foreach (var oldItem in oldItems)
                if (!newItems.Contains(oldItem))
                    variable.Remove(oldItem);

            // add all items that are now present
            foreach (var newItem in newItems)
                if (!oldItems.Contains(newItem))
                    variable.Add(newItem);
        }
    }
}