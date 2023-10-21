using System;

using UnityEngine;

namespace LemuRivolta.InkAtoms
{
    [Serializable]
    public struct VariableValue : IEquatable<VariableValue>
    {
        public string Name;
        public object Value;

        public readonly bool Equals(VariableValue other) =>
            Name == other.Name &&
            ((Value == null && other.Value == null) || (Value != null && Value.Equals(other.Value)));
    }
}
