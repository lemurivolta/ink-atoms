#nullable enable
using System;
using UnityEngine;

namespace LemuRivolta.InkAtoms.VariableObserver
{
    /// <summary>
    ///     A variable observer that matches multiple possible variable (from a list of name) and raise
    ///     events when there's a match.
    /// </summary>
    [Serializable]
    public class ListingVariableObserver : EventsVariableObserver
    {
        [Tooltip("The list of names that will be matched.")] [SerializeField]
        private string[] list = Array.Empty<string>();

        internal override bool IsMatch(string variableName)
        {
            return Array.IndexOf(list, variableName) >= 0;
        }
    }
}