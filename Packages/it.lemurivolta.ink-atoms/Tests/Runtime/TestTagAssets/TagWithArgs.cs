#nullable enable
using System;
using System.Collections.Generic;
using LemuRivolta.InkAtoms.TagProcessors;

namespace Tests.Runtime.TestTagAssets
{
    public class TagWithArgs : ActionTagProcessor
    {
        public TagWithArgs() : base("tagWithArgs")
        {
        }

        public event Action<int>? CountEvent;
        public event Action<List<object>>? ArgsEvent;

        protected override void Process(TagProcessorContext context)
        {
            CountEvent?.Invoke(context.Count);

            List<object> result = new();
            for (var i = 0; i < context.Count; i++) result.Add(context[i]);
            ArgsEvent?.Invoke(result);
        }
    }
}