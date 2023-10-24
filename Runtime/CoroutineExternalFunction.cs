using System.Collections;

namespace LemuRivolta.InkAtoms
{
    /// <summary>
    /// An external function that executes asynchronously and possibly returns a value.
    /// </summary>
    public abstract class CoroutineExternalFunction : BaseExternalFunction
    {
        public CoroutineExternalFunction(string name) : base(name) { }

        /// <summary>
        /// Iterator called to implement the external function.
        /// This iterator acts just like a Unity coroutine, except that:
        /// <list type="bullet">
        ///     <item>the code up to the first <c>yield</c> will be executed synchronously (a coroutine always executes starting from the next frame)</item>
        ///     <item>the return value must be set in the context before the first <c>yield</c></item>
        /// </list>
        /// If you need the code to be executed completely asynchronously just like Unity coroutines, just <c>yield return null;</c> as first instruction.
        /// </summary>
        /// <param name="context">The context where to read the arguments and write the return value.</param>
        /// <returns></returns>
        public abstract IEnumerator Call(ExternalFunctionContextWithResult context);

        internal override IEnumerator InternalCall(ExternalFunctionContextWithResult context)
        {
            var enumerator = Call(context);
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }
    }
}
