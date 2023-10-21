using System;

namespace LemuRivolta.InkAtoms
{
    /// <summary>
    /// The answer in front of a choice.
    /// </summary>
    [Serializable]
    public struct ChosenChoice : IEquatable<ChosenChoice>
    {
        /// <summary>
        /// The flow this choice belongs to.
        /// </summary>
        public string FlowName;

        /// <summary>
        /// The index of the chosen choice.
        /// </summary>
        public int ChoiceIndex;

        public readonly bool Equals(ChosenChoice other) =>
            FlowName == other.FlowName &&
            ChoiceIndex == other.ChoiceIndex;
    }
}
