#nullable enable
using System;
using Ink.Runtime;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace LemuRivolta.InkAtoms.VariableObserver
{
    /// <summary>
    ///     Base class for variable observers that raise events and pair events upon variable changes.
    ///     The only implementations of this class are <see cref="ListingVariableObserver" /> and
    ///     <see cref="RegexVariableObserver" />.
    /// </summary>
    [Serializable]
    public abstract class EventsVariableObserver : VariableObserver
    {
        [Tooltip("The event called whenever a variable matching the criteria changes.")] [SerializeField]
        private VariableChangeEvent? variableChangeEvent;

        internal override void ProcessVariableValue(string variableName, Value prevValue, Value nextValue)
        {
            // skip any event if the variable name doesn't match
            if (!IsMatch(variableName)) return;

            // raise the event if set
            if (variableChangeEvent != null)
                variableChangeEvent.Raise(new VariableChange
                    { Name = variableName, OldValue = prevValue, NewValue = nextValue });
        }

        internal abstract bool IsMatch(string variableName);
    }
}