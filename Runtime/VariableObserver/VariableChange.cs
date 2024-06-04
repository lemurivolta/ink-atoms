using System;
using Ink.Runtime;

namespace LemuRivolta.InkAtoms.VariableObserver
{
    [Serializable]
    public class VariableChange : IEquatable<VariableChange>
    {
        /// <summary>
        ///     The name of the variable that changed.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string Name;

        /// <summary>
        ///     The new value of the variable.
        /// </summary>
        public Value NewValue;

        /// <summary>
        ///     The previous value of the variable.
        /// </summary>
        public Value OldValue;

        public bool Equals(VariableChange other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name &&
                   Equals(NewValue, other.NewValue) &&
                   Equals(OldValue, other.OldValue);
        }

        private bool Equals(Value a, Value b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (ReferenceEquals(a, null) != ReferenceEquals(b, null)) return false;
            return a.valueType == b.valueType && a.valueObject.Equals(b.valueObject);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((VariableChange)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, NewValue, OldValue);
        }

        public void Deconstruct(out Value oldValue, out Value newValue)
        {
            oldValue = OldValue;
            newValue = NewValue;
        }
    }
}