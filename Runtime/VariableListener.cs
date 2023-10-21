using System;
using System.Text.RegularExpressions;

using UnityAtoms.BaseAtoms;

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

    [Serializable]
    public class VariableListener
    {
        /// <summary>
        /// The way this listener matches a variable name.
        /// </summary>
        public MatchKind MatchKind;

        /// <summary>
        /// The name of the variable to match (only if <see cref="MatchKind"/> is <see cref="MatchKind.Name"/>).
        /// </summary>
        public string Name;

        /// <summary>
        /// The regular expression that satisfies the name of the variable (only if
        /// <see cref="MatchKind"/> is <see cref="MatchKind.RegularExpression"/>).
        /// </summary>
        public string Regex;

        /// <summary>
        /// The list of names that will be matched.
        /// </summary>
        public string[] List;

        /// <summary>
        /// The event called whenever a variable matching given criteria changes.
        /// </summary>
        public VariableValuePairEvent VariableChangeEvent;

        /// <summary>
        /// Check whether this listener matches given variable name.
        /// </summary>
        /// <param name="variableName">The variable name to check</param>
        /// <returns><c>true</c> if this listener matches the given variable name, <c>false</c> otherwise.</returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool IsMatch(string variableName) => MatchKind switch
        {
            MatchKind.Name => variableName == Name,
            MatchKind.RegularExpression => GetRegex().IsMatch(variableName),
            MatchKind.List => Array.IndexOf(List, variableName) >= 0,
            _ => throw new NotImplementedException(),
        };

        private Regex regex;

        private Regex GetRegex() => regex ??= new Regex(Regex);
    }
}
