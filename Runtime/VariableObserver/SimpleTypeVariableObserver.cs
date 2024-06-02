#nullable enable
using System;
using System.Collections.Generic;
using Ink.Runtime;
using UnityAtoms;
using UnityEngine;
using UnityEngine.Assertions;

namespace LemuRivolta.InkAtoms.VariableObserver
{
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
            base.OnEnable(variablesState);
            Assert.IsNotNull(variable);
            if (variable != null) variable.GetEvent<TAtomEvent>().Register(ValueChanged);
        }

        private void ValueChanged(T obj)
        {
            if (_variablesState == null)
                Debug.LogWarning("value changed before OnEnable");
            else if (!(_variablesState[inkVariableName] is T t &&
                       EqualityComparer<T>.Default.Equals(t, obj)))
                _variablesState[inkVariableName] = obj;
        }

        internal override void UseValue(T? prevValue, T value)
        {
            if (variable == null) throw new Exception("No atom variable set");

            variable.Value = value;
        }
    }
}