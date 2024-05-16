namespace LemuRivolta.InkAtoms
{
    [System.Serializable]
    public class UnmanageableAsyncSequenceException : System.Exception
    {
        public UnmanageableAsyncSequenceException() { }
        public UnmanageableAsyncSequenceException(string message) : base(message) { }
        public UnmanageableAsyncSequenceException(string message, System.Exception inner) : base(message, inner) { }
        protected UnmanageableAsyncSequenceException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// Base class for <see cref="ExternalFunctionContextWithResult"/> that doesn't wrap the result.
    /// </summary>
    public class ExternalFunctionContext
    {
        /// <summary>
        /// Arguments passed to the function from ink.
        /// </summary>
        public readonly object[] Arguments;

        public ExternalFunctionContext(object[] arguments)        {
            Arguments = arguments;
        }

    }

    public class ExternalFunctionContextWithResult : ExternalFunctionContext
    {
        private object returnValue;
        public object ReturnValue
        {
            get => returnValue;
            set
            {
                if (!wasQueueEmpty)
                {
                    throw new UnmanageableAsyncSequenceException("Cannot execute an async function that returns a value after another async function; add a no-op '@' line between them.");
                }
                if (locked)
                {
                    throw new UnmanageableAsyncSequenceException("Cannot set the result of an asynchronous operation after the first yield");
                }
                returnValue = value;
            }
        }

        private readonly bool wasQueueEmpty;

        public ExternalFunctionContextWithResult(object[] arguments)
        : base(arguments)
        {
            wasQueueEmpty = MainThreadQueue.IsEmpty;
        }

        private bool locked;
        internal bool Locked => locked;
        internal void Lock() => locked = true;
    }
}
