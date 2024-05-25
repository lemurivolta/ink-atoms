using System;
using System.Collections.Generic;
using System.Reflection;

namespace LemuRivolta.InkAtoms
{
    [Serializable]
    public struct VariableValue : IEquatable<VariableValue>
    {
        // ReSharper disable once InconsistentNaming
        public string Name;
        public object Value;

        public readonly bool Equals(VariableValue other)
        {
            if (!Name.Equals(other.Name)) return false;

            var isMyValueNull = Value == null;
            var isOtherValueNull = other.Value == null;
            if (isMyValueNull && isOtherValueNull) return true;
            if (isMyValueNull != isOtherValueNull) return false;

            // invokes EqualityComparer<T>.Default.Equals(Value, other.Value)
            // where T = type of Value
            var myType = Value.GetType();
            var eqType = typeof(EqualityComparer<>).MakeGenericType(myType);
            var comparer = eqType
                .GetProperty("Default", BindingFlags.Public | BindingFlags.Static)
                ?.GetValue(null);
            var method = eqType.GetMethod("Equals", 0, new[] { myType, myType });
            return (bool)method.Invoke(comparer, new[] { Value, other.Value });
        }
    }
}