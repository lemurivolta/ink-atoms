using System;
using LemuRivolta.InkAtoms.ExternalFunctionProcessors;

namespace Tests.Runtime.RestartStory
{
    public class RestartExternalFunction : ActionExternalFunctionProcessor
    {
        public RestartExternalFunction() : base("externalFunction") { }

        public event Action ProcessCalled;

        protected override void Process(ExternalFunctionProcessorContext context)
        {
            ProcessCalled?.Invoke();
        }
    }
}