#nullable enable

using System;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.Assertions;

namespace LemuRivolta.InkAtoms.VariableObserver
{
    [Serializable]
    public abstract class VariableObserverByName<T> : VariableObserver
    {
        /// <summary>
        ///     Name of the Ink variable.
        /// </summary>
        [SerializeField] protected string? inkVariableName;

        internal override void ProcessVariableValue(string variableName, Value? oldValue, Value newValue)
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(inkVariableName), "must set a variable name");

            // if this is not our variable name, ignore it
            if (inkVariableName != variableName) return;

            T? prev;
            if (oldValue == null)
                prev = default;
            else if (oldValue.valueObject is T oldValueT)
                prev = oldValueT;
            else
                prev = Cast(oldValue.valueObject);

            T next;
            if (newValue.valueObject is T newValueT)
                next = newValueT;
            else
                next = Cast(newValue.valueObject);

            // if the value type is correct, update the variable
            UseValue(prev, next);
        }

        /// <summary>
        ///     Methods that is tried last to cast an object to the needed type.
        ///     The default implementation just throws an exception.
        /// </summary>
        /// <param name="o">The object to cast.</param>
        /// <returns>The cast value.</returns>
        /// <exception cref="InvalidCastException">If the value cannot be cast.</exception>
        internal virtual T Cast(object o)
        {
            throw new InvalidCastException(
                $"Expected type {typeof(T).FullName} for variable {inkVariableName}, but received type {o.GetType().FullName}");
        }

        /// <summary>
        ///     Abstract function that must be implemented to use the value that
        ///     arrived from a change event in Ink.
        /// </summary>
        /// <param name="prevValue">
        ///     The previous value of the variable (or <c>null</c>
        ///     if this is the first time the variable is set).
        /// </param>
        /// <param name="value">The new value of the variable.</param>
        internal abstract void UseValue(T? prevValue, T value);
    }
}