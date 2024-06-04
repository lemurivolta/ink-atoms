#nullable enable
using System;
using System.Linq;
using Ink.Runtime;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Assertions;

namespace LemuRivolta.InkAtoms.VariableObserver
{
    [Serializable]
    public class ListVariableObserver : VariableObserverByName<InkList>
    {
        /// <summary>
        ///     The atom value list that contains the ink list items.
        /// </summary>
        [SerializeField] private SerializableInkListItemValueList? variable;

        internal override void OnEnable(VariablesState variablesState)
        {
            base.OnEnable(variablesState);
            Assert.IsNotNull(variable);
            variable?.Added.Register(ValueChanged);
            variable?.Removed.Register(ValueChanged);
        }

        private void ValueChanged(SerializableInkListItem obj)
        {
            if (VariablesState == null || variable == null)
            {
                Debug.LogWarning("value changed before OnEnable");
                return;
            }

            var inkValue = (InkList)VariablesState[inkVariableName];
            // TODO: can we use variable directly if we define equality between InkItem and SerializableInkItem in some way?
            var atomValue = variable
                .Select(sli => (InkListItem)sli)
                .ToList();
            if (inkValue.Keys.SequenceEqual(atomValue)) return;

            // there's surely a better way
            InkList newInkValue = new(inkValue);
            newInkValue.Clear();
            foreach (var value in atomValue) newInkValue.AddItem(value);
            VariablesState[inkVariableName] = newInkValue;
        }

        internal override void UseValue(InkList? oldList, InkList newList)
        {
            if (variable == null) throw new Exception("No atom variable set");

            var newItems = newList.Keys;

            // remove all items no longer present
            var toRemove =
                (from oldItem in variable where !newItems.Contains(oldItem) select oldItem)
                .ToList();
            foreach (var oldItem in toRemove) variable.Remove(oldItem);

            // add all items that are now present
            var toAdd =
                (from newItem in newItems where !variable.Contains(newItem) select newItem)
                .ToList();
            foreach (var newItem in toAdd)
                variable.Add(newItem);
        }
    }
}