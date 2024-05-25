#nullable enable

namespace LemuRivolta.InkAtoms.CommandLineProcessors
{
    public abstract class BaseCommandLineProcessor : BaseProcessor<CommandLineProcessorContext>
    {
        protected BaseCommandLineProcessor(string name) : base(name)
        {
        }
    }
}