using System;
using System.Linq;

namespace LemuRivolta.InkAtoms
{
    /// <summary>
    ///     A choice inside a <see cref="StoryStep" />.
    /// </summary>
    [Serializable]
    public struct StoryChoice : IEquatable<StoryChoice>
    {
        /// <summary>
        ///     The index of this choice.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int Index;

        /// <summary>
        ///     The text of this choice.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string Text;

        /// <summary>
        ///     All the tags of this choice.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string[] Tags;

        public readonly bool Equals(StoryChoice other)
        {
            return Index == other.Index &&
                   Text == other.Text &&
                   Tags.Length == other.Tags.Length &&
                   Tags.All(tag => other.Tags.Contains(tag));
        }
    }
}