#nullable enable

using System.Collections;

namespace LemuRivolta.InkAtoms.ExternalFunctionProcessors
{
    public abstract class ActionExternalFunctionProcessorProcessor : BaseExternalFunctionProcessor
    {
        protected ActionExternalFunctionProcessorProcessor(string name) : base(name)
        {
        }

        protected abstract void Process(ExternalFunctionProcessorContext context);

        internal override IEnumerator InternalProcess(ExternalFunctionProcessorContextWithResult context)
        {
            Process(context);
            yield break;
        }
    }
}