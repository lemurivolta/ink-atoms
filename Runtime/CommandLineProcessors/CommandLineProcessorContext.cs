using System;
using System.Collections.Generic;

namespace LemuRivolta.InkAtoms.CommandLineProcessors
{
    public class CommandLineProcessorContext : ParametersBag
    {
        /// <summary>
        ///     Choices attached to this command, if any.
        /// </summary>
        public readonly IReadOnlyList<StoryChoice> Choices;

        private int _choiceIndex;

        public CommandLineProcessorContext(IDictionary<string, object> parameters, StoryChoice[] choices)
        {
            AddParameters(parameters);
            Choices = choices;
            _choiceIndex = -1;
        }

        internal bool Continue => _choiceIndex < 0;

        internal int ChoiceIndex
        {
            get
            {
                if (_choiceIndex < 0) throw new Exception("This action wants to continue");

                return _choiceIndex;
            }
        }

        /// <summary>
        ///     Set the associated command line processor to continue (the default action).
        /// </summary>
        public void DoContinue()
        {
            _choiceIndex = -1;
        }

        /// <summary>
        ///     Set the associated command line processor to take a specific choice.
        /// </summary>
        /// <param name="choiceIndex">The choice index to take.</param>
        public void TakeChoice(int choiceIndex)
        {
            _choiceIndex = choiceIndex;
        }
    }
}