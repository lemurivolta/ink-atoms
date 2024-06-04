using System;
using System.Linq;

namespace LemuRivolta.InkAtoms
{
    /// <summary>
    ///     A moment during the ink story.
    /// </summary>
    [Serializable]
    public class StoryStep : IEquatable<StoryStep>
    {
        /// <summary>
        ///     The flow this story step belongs to.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string Flow;

        /// <summary>
        ///     Text of this story step.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string Text;

        /// <summary>
        ///     The list of all tags contained in this story step.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string[] Tags;

        /// <summary>
        ///     The list of all choices contained in this story step.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public StoryChoice[] Choices;

        /// <summary>
        ///     Whether a "continue" event can be sent to progress further in the story.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool CanContinue;

        /// <summary>
        ///     The (starting) line number of this story step.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int LineNumber;

        /// <summary>
        ///     Counter for this story step. Each new story step produced has a new counter value.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int Counter;

        /// <summary>
        ///     Whether there's at least one choice.
        /// </summary>
        public bool HasChoices => Choices.Length > 0;

        public bool Equals(StoryStep other)
        {
            return other != null &&
                   Flow == other.Flow &&
                   Text == other.Text &&
                   Tags.SequenceEqual(other.Tags) &&
                   Choices.SequenceEqual(other.Choices) &&
                   CanContinue == other.CanContinue &&
                   Counter == other.Counter;
        }

        /// <summary>
        ///     Create a chosen choice from one of the choices of this story step.
        /// </summary>
        /// <param name="index">The index in the <see cref="Choices" /> array.</param>
        /// <returns>The chosen choice, ready to be raised.</returns>
        public ChosenChoice GetChosenChoice(int index)
        {
            var storyChoice = Choices[index];
            return new ChosenChoice
            {
                FlowName = Flow,
                ChoiceIndex = storyChoice.Index
            };
        }
    }
}