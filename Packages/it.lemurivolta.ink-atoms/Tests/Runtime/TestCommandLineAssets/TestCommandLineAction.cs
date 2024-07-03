using System;
using LemuRivolta.InkAtoms.CommandLineProcessors;

namespace Tests.Runtime.TestCommandLineAssets
{
    public class TestCommandLineAction : ActionCommandLineProcessor
    {
        public TestCommandLineAction() : base("actionCommand")
        {
        }

        public event Action<string, string> Processed;

        protected override void Process(CommandLineProcessorContext context)
        {
            Processed?.Invoke(context.Get<string>("param1"), context.Get<string>("param2"));
        }
    }
}