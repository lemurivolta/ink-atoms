using System;
using System.Text.RegularExpressions;

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
        List
    }

    //public enum ValueSetterKind
    //{
    //    Event,
    //    Variable
    //}

    [Serializable]
    public class VariableListener
    {
        [Tooltip("The way this listener matches a variable name.")]
        [SerializeField] private MatchKind matchKind;

        [Tooltip("The name of the variable to match")]
        [SerializeField] private string name;

        [Tooltip("The regular expression that satisfies the name of the variable")]
        [SerializeField] private string regex;

        [Tooltip("The list of names that will be matched.")]
        [SerializeField] private string[] list;

        [Tooltip("The event called whenever a variable matching the criteria changes.")]
        [SerializeField] private VariableValuePairEvent variablePairChangeEvent;

        [Tooltip("The event called whenever a variable matching the criteria changes.")]
        [SerializeField] private VariableValueEvent variableChangeEvent;

        [Tooltip("The atom variable where the ink variable gets written to.")]
        [SerializeField] private AtomBaseVariable variableValue;

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
            _ => throw new NotImplementedException(),
        };

        /// <summary>
        /// The cache of the Regex for this variable listener, if it's a regex expression
        /// </summary>
        private Regex regexCache;

        /// <summary>
        /// Get the regex object corresponding to the regex string, using a cache if present.
        /// </summary>
        /// <returns></returns>
        private Regex GetRegex() => regexCache ??= new Regex(regex);

        /// <summary>
        /// Process a change in variable value and changes the variable value, if this listener
        /// matches a specific variable, or raises the corresponding events otherwise.
        /// </summary>
        /// <param name="variableName">The name of the variable to change.</param>
        /// <param name="oldValue">The previous value of the variable.</param>
        /// <param name="newValue">The new value of the variable.</param>
        internal void ProcessVariableValue(string variableName, object oldValue, object newValue)
        {
            if (IsMatch(variableName))
            {
                if (matchKind == MatchKind.Name)
                {
                    variableValue.BaseValue = newValue;
                }
                else
                {
                    VariableValue newVariableValue = new() { Name = variableName, Value = newValue };
                    VariableValuePair variableValuePair = new()
                    {
                        Item1 = new() { Name = variableName, Value = oldValue },
                        Item2 = newVariableValue
                    };
                    if (variableChangeEvent)
                    {
                        variableChangeEvent.Raise(newVariableValue);
                    }
                    if (variablePairChangeEvent)
                    {
                        variablePairChangeEvent.Raise(variableValuePair);
                    }
                }
            }
        }
    }
}
