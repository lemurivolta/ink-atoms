using System;

namespace LemuRivolta.InkAtoms
{
    /// <summary>
    /// A choice inside a <see cref="StoryStep"/>.
    /// </summary>
    public struct StoryChoice : IEquatable<StoryChoice>
    {
        /// <summary>
        /// The index of this choice.
        /// </summary>
        public int Index;

        /// <summary>
        /// The text of this choice.
        /// </summary>
        public string Text;

        /// <summary>
        /// All the tags of this choice.
        /// </summary>
        public string[] Tags;

        public readonly bool Equals(StoryChoice other) =>
            Index == other.Index &&
            Text == other.Text;
    }
}
