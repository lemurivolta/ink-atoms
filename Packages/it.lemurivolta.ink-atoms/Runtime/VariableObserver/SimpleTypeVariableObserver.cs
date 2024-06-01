#nullable enable
using System;
using UnityAtoms;
using UnityEngine;

namespace LemuRivolta.InkAtoms.VariableObserver
{
    [Serializable]
    public class SimpleTypeVariableObserver<T, TAtom> : VariableObserverByName<T>
        where TAtom : AtomBaseVariable<T>
    {
        /// <summary>
        ///     The atom variable that is synchronized with the Ink variable.
        /// </summary>
        [SerializeField] private TAtom? variable;

        internal override void UseValue(T? prevValue, T value)
        {
            if (variable == null) throw new Exception("No atom variable set");

            variable.Value = value;
        }
    }
}