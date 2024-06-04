#nullable enable
using System;
using System.Collections.Generic;
using LemuRivolta.InkAtoms.ExternalFunctionProcessors;

namespace Tests.Runtime.TestExternalFunctionAssets
{
    public class TestExternalFunctionActionFunctionWithArgs : ActionExternalFunctionProcessor
    {
        public Action<List<object>>? Called;

        public TestExternalFunctionActionFunctionWithArgs() : base("actionFunctionWithArgs")
        {
        }

        protected override void Process(ExternalFunctionProcessorContext context)
        {
            List<object> parameters = new();
            for (var i = 0; i < context.Count; i++) parameters.Add(context[i]);

            Called?.Invoke(parameters);
        }
    }
}