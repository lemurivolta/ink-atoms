#nullable enable

using System.Collections;

namespace LemuRivolta.InkAtoms.ExternalFunctionProcessors
{
    public abstract class FuncExternalFunctionProcessor<T> : BaseExternalFunctionProcessor
    {
        protected FuncExternalFunctionProcessor(string name) : base(name)
        {
        }

        protected abstract T Process(ExternalFunctionProcessorContext processorContext);

        internal override IEnumerator InternalProcess(ExternalFunctionProcessorContextWithResult processorContext)
        {
            var result = Process(processorContext);
            processorContext.returnValue = result;
            yield break;
        }
    }
}