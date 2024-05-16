using System;
using System.Collections.Generic;
using System.Reflection;

namespace LemuRivolta.InkAtoms
{
    [Serializable]
    public struct VariableValue : IEquatable<VariableValue>
    {
        public string Name;
        public object Value;

        public readonly bool Equals(VariableValue other)
        {
            if (!Name.Equals(other.Name))
            {
                return false;
            }

            bool isMyValueNull = Value == null;
            bool isOtherValueNull = other.Value == null;
            if (isMyValueNull && isOtherValueNull)
            {
                return true;
            }
            if (isMyValueNull != isOtherValueNull)
            {
                return false;
            }

            // invokes EqualityComparer<T>.Default.Equals(Value, other.Value)
            // where T = type of Value
            Type myType = Value.GetType();
            Type eqType = typeof(EqualityComparer<>).MakeGenericType(myType);
            var comparer = eqType
                .GetProperty("Default", BindingFlags.Public | BindingFlags.Static)
                .GetValue(null);
            var method = eqType.GetMethod("Equals", 0, new Type[] { myType, myType });
            return (bool)method.Invoke(comparer, new object[] { Value, other.Value });
        }
    }
}
