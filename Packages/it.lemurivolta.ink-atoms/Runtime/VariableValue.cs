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
            if (Value == null && other.Value == null)
            {
                return true;
            }
            if ((Value == null) != (other.Value == null))
            {
                return false;
            }

            Type myType = Value.GetType();
            Type eqType = typeof(EqualityComparer<>)
                            .MakeGenericType(myType);
            var comparer = eqType
                .GetProperty("Default", BindingFlags.Public | BindingFlags.Static)
                .GetValue(null);
            var method = eqType.GetMethod("Equals", 0, new Type[] { myType, myType });
            return (bool)method.Invoke(comparer, new object[] { Value, other.Value });
        }
    }
}
