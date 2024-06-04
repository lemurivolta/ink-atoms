#nullable enable
using System;
using LemuRivolta.InkAtoms.ExternalFunctionProcessors;

namespace Tests.Runtime.TestExternalFunctionAssets
{
    public class TestExternalFunctionActionFunction : ActionExternalFunctionProcessor
    {
        public TestExternalFunctionActionFunction() : base("actionFunction")
        {
        }

        protected override void Process(ExternalFunctionProcessorContext context)
        {
            Called?.Invoke();
        }

        public event Action? Called;
    }
}