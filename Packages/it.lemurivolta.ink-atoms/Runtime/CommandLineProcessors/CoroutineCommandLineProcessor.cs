using System.Collections;

namespace LemuRivolta.InkAtoms.CommandLineProcessors
{
    public abstract class CoroutineCommandLineProcessor : BaseCommandLineProcessor
    {
        protected CoroutineCommandLineProcessor(string name)
            : base(name)
        {
        }

        /// <summary>
        ///     Method call to invoke the command in case choices are present.
        /// </summary>
        protected abstract IEnumerator Process(CommandLineProcessorContext context);

        internal override IEnumerator InternalProcess(CommandLineProcessorContext context)
        {
            return Process(context);
        }
    }
}