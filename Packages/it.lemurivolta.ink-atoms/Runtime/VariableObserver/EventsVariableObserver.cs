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
        private VariableValuePairEvent? variablePairChangeEvent;

        [Tooltip("The event called whenever a variable matching the criteria changes.")] [SerializeField]
        private VariableValueEvent? variableChangeEvent;

        internal override void ProcessVariableValue(string variableName, Value oldValue, Value newValue)
        {
            if (!IsMatch(variableName)) return;

            // prepare the common data between the two events
            VariableValue newVariableValue = new() { Name = variableName, Value = newValue };

            // simple event
            if (variableChangeEvent != null) variableChangeEvent.Raise(newVariableValue);

            // event with history
            if (variablePairChangeEvent != null)
            {
                VariableValuePair variableValuePair = new()
                {
                    Item1 = newVariableValue,
                    Item2 = new VariableValue { Name = variableName, Value = oldValue }
                };
                variablePairChangeEvent.Raise(variableValuePair);
            }
        }

        internal abstract bool IsMatch(string variableName);
    }
}