namespace LemuRivolta.InkAtoms
{
    /// <summary>
    /// Base class for <see cref="ExternalFunctionContext"/> that doesn't wrap the result.
    /// </summary>
    public class BaseExternalFunctionContext
    {
        /// <summary>
        /// Arguments passed to the function from ink.
        /// </summary>
        public object[] Arguments;
    }

    /// <summary>
    /// The context passed to the function implementation (see <see cref="Call(Context)"/>).
    /// </summary>
    public class ExternalFunctionContext : BaseExternalFunctionContext
    {
        /// <summary>
        /// The result of the function. This member must be set by a
        /// <see cref="BaseExternalFunction"/> implementation in order to return a value.
        /// </summary>
        public object Result;
    }
}
