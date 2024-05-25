#nullable enable
using System.Collections;

namespace LemuRivolta.InkAtoms.ExternalFunctionProcessors
{
    /// <summary>
    ///     An external function that executes asynchronously and possibly returns a value.
    /// </summary>
    public abstract class CoroutineExternalFunctionProcessor : BaseExternalFunctionProcessor
    {
        protected CoroutineExternalFunctionProcessor(string name) : base(name)
        {
        }

        /// <summary>
        ///     Iterator Processed to implement the external function.
        ///     This iterator acts just like a Unity coroutine, except that:
        ///     <list type="bullet">
        ///         <item>
        ///             the code up to the first <c>yield</c> will be executed synchronously (a coroutine always executes
        ///             starting from the next frame)
        ///         </item>
        ///         <item>the return value must be set in the context before the first <c>yield</c></item>
        ///     </list>
        ///     This behaviour could execute code in this function before other code that's already been
        ///     enqueued previously.
        ///     If you need the code to be executed completely asynchronously just like Unity coroutines, just
        ///     <c>yield return null;</c> as first instruction.
        /// </summary>
        /// <param name="context">The context where to read the arguments and write the return value.</param>
        /// <returns></returns>
        protected abstract IEnumerator Process(ExternalFunctionProcessorContextWithResult context);

        internal override IEnumerator InternalProcess(ExternalFunctionProcessorContextWithResult processorContext)
        {
            var enumerator = Process(processorContext);
            while (enumerator.MoveNext()) yield return enumerator.Current;
        }
    }
}