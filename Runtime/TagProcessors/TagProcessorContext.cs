using System.Collections.Generic;

namespace LemuRivolta.InkAtoms.TagProcessors
{
    public class TagProcessorContext : ParametersBag
    {
        public TagProcessorContext(IEnumerable<string> parameters, StoryStep storyStep)
        {
            StoryStep = storyStep;
            AddParameters(parameters);
            Seal();
        }

        public StoryStep StoryStep { get; private set; }
    }
}