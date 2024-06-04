using System;
using Ink.Runtime;

namespace LemuRivolta.InkAtoms.VariableObserver
{
    /// <summary>
    ///     Base class for variable observers. Variable observers are values that
    ///     represent ways to track the values of a variable. This class has a closed
    ///     set of concrete inherited classes.
    /// </summary>
    [Serializable]
    public abstract class VariableObserver
    {
        /// <summary>
        ///     The state of the variables in the Ink story for Ink Atoms this observer is connected to.
        /// </summary>
        protected VariablesState VariablesState;

        /// <summary>
        ///     Method called whenever a variable changes its value.
        /// </summary>
        /// <param name="variableName">Name of the variable.</param>
        /// <param name="oldValue">Previous (ink) value.</param>
        /// <param name="newValue">New (ink) value.</param>
        internal abstract void ProcessVariableValue(string variableName, Value oldValue, Value newValue);

        /// <summary>
        ///     Method called during the initialization of the story this observer belongs to.
        /// </summary>
        /// <param name="variablesState">The state of the variables in the Ink story.</param>
        internal virtual void OnEnable(VariablesState variablesState)
        {
            VariablesState = variablesState;
        }
    }
}