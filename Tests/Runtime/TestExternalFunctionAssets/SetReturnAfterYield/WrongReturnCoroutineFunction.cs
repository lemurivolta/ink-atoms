using System.Collections;
using LemuRivolta.InkAtoms.ExternalFunctionProcessors;

namespace Tests.Runtime.TestExternalFunctionAssets.SetReturnAfterYield
{
    public class WrongReturnCoroutineFunction : CoroutineExternalFunctionProcessor
    {
        public WrongReturnCoroutineFunction() : base("wrongReturnCoroutineFunction")
        {
        }

        protected override IEnumerator Process(ExternalFunctionProcessorContextWithResult context)
        {
            // cannot set return value after the first yield!
            yield return null;
            context.ReturnValue = 3;
        }
    }
}