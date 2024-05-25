using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Ink.Runtime;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace LemuRivolta.InkAtoms
{
    /// <summary>
    /// The way variable names are matched.
    /// </summary>
    public enum MatchKind
    {
        /// <summary>
        /// Match the exact variable name.
        /// </summary>
        Name,

        /// <summary>
        /// Match variable names that respect given regular expression.
        /// </summary>
        RegularExpression,

        /// <summary>
        /// Match variable names coming from the specified list.
        /// </summary>
        List,

        /// <summary>
        /// Match the exact variable name of an ink list variable.
        /// </summary>
        [InspectorName("Name (ink list)")] NameInkList
    }

    //public enum ValueSetterKind
    //{
    //    Event,
    //    Variable
    //}

    [Serializable]
    public class VariableListener
    {
        [Tooltip("The way this listener matches a variable name.")] [SerializeField]
        private MatchKind matchKind;

        [Tooltip("The name of the variable to match")] [SerializeField]
        private string name;

        [Tooltip("The regular expression that satisfies the name of the variable")] [SerializeField]
        private string regex;

        [Tooltip("The list of names that will be matched.")] [SerializeField]
        private string[] list;

        [Tooltip("The event called whenever a variable matching the criteria changes.")] [SerializeField]
        private VariableValuePairEvent variablePairChangeEvent;

        [Tooltip("The event called whenever a variable matching the criteria changes.")] [SerializeField]
        private VariableValueEvent variableChangeEvent;

        [Tooltip("The atom variable where the ink variable gets written to.")] [SerializeField]
        private AtomBaseVariable variableValue;

        [Tooltip("The atom list where the ink list gets written to.")] [SerializeField]
        private SerializableInkListItemValueList variableList;

        /// <summary>
        /// Check whether this listener matches given variable name.
        /// </summary>
        /// <param name="variableName">The variable name to check</param>
        /// <returns><c>true</c> if this listener matches the given variable name, <c>false</c> otherwise.</returns>
        /// <exception cref="NotImplementedException"></exception>
        private bool IsMatch(string variableName) => matchKind switch
        {
            MatchKind.Name => variableName == name,
            MatchKind.RegularExpression => GetRegex().IsMatch(variableName),
            MatchKind.List => Array.IndexOf(list, variableName) >= 0,
            MatchKind.NameInkList => variableName == name,
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// The cache of the Regex for this variable listener, if it's a regex expression
        /// </summary>
        private Regex _regexCache;

        /// <summary>
        /// Get the regex object corresponding to the regex string, using a cache if present.
        /// </summary>
        /// <returns></returns>
        private Regex GetRegex() => _regexCache ??= new Regex(regex);

        private static bool IsInstanceOfGenericType(Type genericType, object instance)
        {
            var type = instance.GetType();
            while (type != null)
            {
                if (type.IsGenericType &&
                    type.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }

                type = type.BaseType;
            }

            return false;
        }

        private object Unwrap(object x) =>
            x == null ? null :
            IsInstanceOfGenericType(typeof(Value<>), x) ? x.GetType().GetProperty("value")?.GetValue(x) :
            x;

        /// <summary>
        /// Process a change in variable value and changes the variable value, if this listener
        /// matches a specific variable, or raises the corresponding events otherwise.
        /// </summary>
        /// <param name="variableName">The name of the variable to change.</param>
        /// <param name="oldValue">The previous value of the variable.</param>
        /// <param name="newValue">The new value of the variable.</param>
        internal void ProcessVariableValue(string variableName, object oldValue, object newValue)
        {
            // initial value of a list is an InkList, after that the InkList is always wrapped in a ListValue
            // weird but ¯\_(ツ)_/¯
            // same thing for IntValue (and others?)
            oldValue = Unwrap(oldValue);
            newValue = Unwrap(newValue);

            if (IsMatch(variableName))
            {
                // check typing for ink list variables special case
                if (matchKind != MatchKind.NameInkList && newValue is ListValue)
                {
                    throw new Exception("Ink lists variables can only map to «Name (ink list)» variable listeners");
                }
                else if (matchKind == MatchKind.NameInkList && newValue is not InkList)
                {
                    throw new Exception(
                        $"«Name (ink list)» variable listeners only work on ink lists variables, not on {newValue.GetType().FullName}");
                }

                // process match
                if (matchKind == MatchKind.Name)
                {
                    VariableValue newVariableValue = new() { Name = variableName, Value = newValue };
                    variableValue.BaseValue = newVariableValue;
                }
                else if (matchKind == MatchKind.NameInkList)
                {
                    var oldList = (InkList)oldValue;
                    var newList = (InkList)newValue;

                    var oldItems = oldList == null ? new List<InkListItem>() : oldList.Keys.ToList();
                    var newItems = newList.Keys.ToList();

                    // remove all items no longer present
                    foreach (var oldItem in oldItems)
                    {
                        if (!newItems.Contains(oldItem))
                        {
                            variableList.Remove(oldItem);
                        }
                    }

                    // add all items that are now present
                    foreach (var newItem in newItems)
                    {
                        if (!oldItems.Contains(newItem))
                        {
                            variableList.Add(newItem);
                        }
                    }
                }
                else
                {
                    VariableValue newVariableValue = new() { Name = variableName, Value = newValue };
                    if (variableChangeEvent)
                    {
                        variableChangeEvent.Raise(newVariableValue);
                    }

                    if (variablePairChangeEvent)
                    {
                        VariableValuePair variableValuePair = new()
                        {
                            Item1 = newVariableValue,
                            Item2 = new() { Name = variableName, Value = oldValue }
                        };
                        variablePairChangeEvent.Raise(variableValuePair);
                    }
                }
            }
        }
    }
}