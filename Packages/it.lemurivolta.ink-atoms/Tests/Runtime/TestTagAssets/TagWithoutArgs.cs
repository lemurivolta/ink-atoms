#nullable enable
using System;
using LemuRivolta.InkAtoms.TagProcessors;

namespace Tests.Runtime.TestTagAssets
{
    public class TagWithoutArgs : ActionTagProcessor
    {
        public TagWithoutArgs() : base("tagWithoutArgs")
        {
        }

        public event Action? Called;

        protected override void Process(TagProcessorContext context)
        {
            Called?.Invoke();
        }
    }
}