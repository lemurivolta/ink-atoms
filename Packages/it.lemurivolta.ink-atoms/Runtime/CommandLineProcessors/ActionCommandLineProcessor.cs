#nullable enable
using System.Collections;

namespace LemuRivolta.InkAtoms.CommandLineProcessors
{
    public abstract class ActionCommandLineProcessor : BaseCommandLineProcessor
    {
        public ActionCommandLineProcessor(string name) : base(name)
        {
        }

        protected abstract void Process(CommandLineProcessorContext context);

        internal override IEnumerator InternalProcess(CommandLineProcessorContext context)
        {
            Process(context);
            yield break;
        }
    }
}