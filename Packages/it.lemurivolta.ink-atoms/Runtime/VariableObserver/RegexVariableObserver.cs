using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace LemuRivolta.InkAtoms.VariableObserver
{
    /// <summary>
    ///     A variable observer that matches the variable name with a regex and raise events when it matches.
    /// </summary>
    [Serializable]
    public class RegexVariableObserver : EventsVariableObserver
    {
        [Tooltip("The regular expression that satisfies the name of the variable")] [SerializeField]
        private string regex;

        /// <summary>
        ///     The cache of the Regex for this variable listener, if it's a regex expression
        /// </summary>
        private Regex _regexCache;

        /// <summary>
        ///     Get the regex object corresponding to the regex string, using a cache if present.
        /// </summary>
        /// <returns></returns>
        private Regex Regex => _regexCache ??= new Regex(regex);

        internal override bool IsMatch(string variableName)
        {
            return Regex.IsMatch(variableName);
        }
    }
}