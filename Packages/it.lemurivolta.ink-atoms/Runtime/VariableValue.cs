using System;
using System.Collections.Generic;
using System.Reflection;

namespace LemuRivolta.InkAtoms
{
    [Serializable]
    public struct VariableValue : IEquatable<VariableValue>
    {
        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// The name of the variable that changed.
        /// </summary>
        public string Name;
        /// <summary>
        /// The new value of the variable.
        /// </summary>
        public object Value;

        public readonly bool Equals(VariableValue other)
        {
            // if they are different variables, then they're different too
            if (!Name.Equals(other.Name)) return false;

            // check for nullability
            var isMyValueNull = Value == null;
            var isOtherValueNull = other.Value == null;
            if (isMyValueNull && isOtherValueNull) return true;
            if (isMyValueNull != isOtherValueNull) return false;

            // invokes EqualityComparer<T>.Default.Equals(Value, other.Value)
            // where T = type of Value
            if (!TryConvertValue(other, out var otherValue))
            {
                return false;
            }
            var myType = Value.GetType();
            var eqType = typeof(EqualityComparer<>).MakeGenericType(myType);
            var comparer = eqType
                .GetProperty("Default", BindingFlags.Public | BindingFlags.Static)
                ?.GetValue(null);
            var method = eqType.GetMethod("Equals", 0, new[] { myType, myType });
            return (bool)method.Invoke(comparer, new[] { Value, otherValue });
        }

        private readonly bool TryConvertValue(VariableValue other, out object result)
        {
            try
            {
                result = Convert.ChangeType(other.Value, Value.GetType());
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }
    }
}