using System;
using LemuRivolta.InkAtoms.TagProcessors;

namespace Tests.Runtime.RestartStory
{
    public class RestartTagProcessor : ActionTagProcessor
    {
        public RestartTagProcessor() : base("tagProcessor") { }
        
        public event Action<string> Performed;

        protected override void Process(TagProcessorContext context)
        {
            Performed?.Invoke(context[0] as string);
        }
    }
}