using System;
using Ink;
using Ink.Runtime;

namespace LemuRivolta.InkAtoms
{
    /// <summary>
    ///     Exception that wraps any kind of error from the ink engine.
    /// </summary>
    public class InkEngineException : Exception
    {
        public readonly ErrorType ErrorType;

        public InkEngineException(ErrorType errorType, string message, Story story)
            : base(EnrichMessage(message, story))
        {
            ErrorType = errorType;
        }

        /// <summary>
        ///     Enrich the message with info about where the error was raised in the ink project
        /// </summary>
        /// <param name="message">Message to enrich.</param>
        /// <param name="story">Story that generated the message right now.</param>
        /// <returns>A message enriched with location information.</returns>
        private static string EnrichMessage(string message, Story story)
        {
            if (story == null || story.debugMetadata == null) return message;

            var d = story.debugMetadata;
            return $"{d.fileName}:{d.startLineNumber}-{d.endLineNumber} {message}";
        }
    }
}