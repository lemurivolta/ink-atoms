using LemuRivolta.InkAtoms.ExternalFunctionProcessors;

namespace Tests.Runtime.TestExternalFunctionAssets
{
    public class TestExternalFunctionFuncFunction : FuncExternalFunctionProcessor<int>
    {
        public TestExternalFunctionFuncFunction() : base("funcFunction")
        {
        }

        protected override int Process(ExternalFunctionProcessorContext processorContext)
        {
            return 3;
        }
    }
}