using System.Collections.Generic;

namespace LemuRivolta.InkAtoms
{
    public class TagProcessorContext
    {
        public IReadOnlyList<string> Parameters { get; private set; }
        public StoryStep StoryStep { get; private set; }

        public TagProcessorContext(IReadOnlyList<string> parameters, StoryStep storyStep)
        {
            Parameters = parameters;
            StoryStep = storyStep;
        }
    }
}
