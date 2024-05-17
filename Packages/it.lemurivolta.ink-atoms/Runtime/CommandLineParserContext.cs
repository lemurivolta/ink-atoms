using System.Collections.Generic;
using System.Collections.ObjectModel;

using static LemuRivolta.InkAtoms.CommandLineParser;

namespace LemuRivolta.InkAtoms
{
    public class CommandLineParserContext
    {
        public readonly IReadOnlyDictionary<string, Parameter> Parameters;

        public readonly IReadOnlyList<StoryChoice> Choices;

        private int choiceIndex;

        public CommandLineParserContext(IDictionary<string, Parameter> parameters, StoryChoice[] choices)
        {
            Parameters = new ReadOnlyDictionary<string, Parameter>(parameters);
            Choices = choices;
            choiceIndex = -1;
        }

        public void DoContinue()
        {
            choiceIndex = -1;
        }

        public void TakeChoice(int choiceIndex)
        {
            this.choiceIndex = choiceIndex;
        }

        public bool Continue => choiceIndex < 0;

        public int ChoiceIndex
        {
            get
            {
                if (choiceIndex < 0)
                {
                    throw new System.Exception("This action wants to continue");
                }
                return choiceIndex;
            }
        }

        public string this[string parameterName]
        {
            get
            {
                if (!Parameters.TryGetValue(parameterName, out var parameter))
                {
                    throw new System.ArgumentException(
                        $"The command must contain a parameter \"{parameterName}:...\"",
                        "parameterName");
                }
                return parameter.Value;
            }
        }

    }
}
