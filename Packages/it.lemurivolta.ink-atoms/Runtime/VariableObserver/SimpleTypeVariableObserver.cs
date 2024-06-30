#nullable enable
using System;
using System.Collections.Generic;
using Ink.Runtime;
using UnityAtoms;
using UnityEngine;
using UnityEngine.Assertions;

namespace LemuRivolta.InkAtoms.VariableObserver
{
    /// <summary>
    ///     A variable observer that keeps in sync with a variable using a "simple" type (int, float,
    ///     string, bool).
    ///     This implementation uses the default equality comparer to check if the ink or atom variable
    ///     must be updated when the other side changes, and presumes that the type inside the Atom variable
    ///     and inside the Ink value are the same (e.g.: IntVariable and IntValue).
    /// </summary>
    /// <typeparam name="T">The type of the variable (e.g.: <c>int</c>)</typeparam>
    /// <typeparam name="TAtom">The type of the atom that will contain the value (e.g.: IntVariable/>)</typeparam>
    /// <typeparam name="TAtomEvent">The type of the event that is raised when the variable changes (e.g.: IntEvent)</typeparam>
    [Serializable]
    public class SimpleTypeVariableObserver<T, TAtom, TAtomEvent> : VariableObserverByName<T>
        where TAtomEvent : AtomEvent<T>
        where TAtom : AtomBaseVariable<T>, IGetEvent
    {
        /// <summary>
        ///     The atom variable that is synchronized with the Ink variable.
        /// </summary>
        [SerializeField] private TAtom? variable;

        /// <summary>
        ///     Method called before the variable observer can be used.
        /// </summary>
        internal override void OnEnable(VariablesState variablesState)
        {
            Assert.IsNotNull(variable);
            if (variable != null) variable.GetEvent<TAtomEvent>().Register(ValueChanged);
            // setting the variable state _after_ registering to the events, so that enabling variables doesn't
            // immediately overwrite ink variables
            base.OnEnable(variablesState);
        }

        /// <summary>
        ///     Callback that is called whenever the value of the atom variable changes.
        /// </summary>
        /// <param name="obj">The new value of the variable</param>
        private void ValueChanged(T obj)
        {
            // check if we called this method too soon
            if (VariablesState == null)
                Debug.LogWarning("value changed before OnEnable");
            // update the ink story only if the value is different.
            else if (!(VariablesState[inkVariableName] is T t &&
                       EqualityComparer<T>.Default.Equals(t, obj)))
                VariablesState[inkVariableName] = obj;
        }

        internal override void UseValue(T? prevValue, T value)
        {
            if (variable == null) throw new Exception("No atom variable set");

            variable.Value = value;
        }
    }
}