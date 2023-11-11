using System;
using System.Linq;

namespace LemuRivolta.InkAtoms
{
    /// <summary>
    /// A moment during the ink story.
    /// </summary>
    [Serializable]
    public class StoryStep : IEquatable<StoryStep>
    {
        /// <summary>
        /// The flow this story step belongs to.
        /// </summary>
        public string Flow;

        /// <summary>
        /// Text of this story step.
        /// </summary>
        public string Text;

        /// <summary>
        /// The list of all tags contained in this story step.
        /// </summary>
        public string[] Tags;

        /// <summary>
        /// The list of all choices contained in this story step.
        /// </summary>
        public StoryChoice[] Choices;

        /// <summary>
        /// Whether there's at least one choice.
        /// </summary>
        public bool HasChoices => Choices.Length > 0;

        /// <summary>
        /// Whether a "continue" event can be sent to progress further in the story.
        /// </summary>
        public bool CanContinue;

        /// <summary>
        /// The (starting) line number of this story step.
        /// </summary>
        public int LineNumber;

        /// <summary>
        /// Counter for this story step. Each new story step produced has a new counter value.
        /// </summary>
        public int Counter;

        public bool Equals(StoryStep other) =>
            Flow == other.Flow &&
            Text == other.Text &&
            Tags.SequenceEqual(other.Tags) &&
            Choices.SequenceEqual(other.Choices) &&
            CanContinue == other.CanContinue &&
            Counter == other.Counter;
    }
}
