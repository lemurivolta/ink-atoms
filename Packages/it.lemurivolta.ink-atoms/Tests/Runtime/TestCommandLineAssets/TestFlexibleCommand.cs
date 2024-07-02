using LemuRivolta.InkAtoms.CommandLineProcessors;

namespace Tests.Runtime.TestCommandLineAssets
{
    public class TestFlexibleCommand : ActionCommandLineProcessor
    {
        public TestFlexibleCommand() : base("flexibleCommand") { }

        protected override void Process(CommandLineProcessorContext context)
        {
            if (context.Choices.Count == 0)
            {
                context.DoContinue();
            }
            else
            {
                var choiceNumber = context.Get<int>("choiceNumber");
                context.TakeChoice(choiceNumber);
            }
        }
    }
}