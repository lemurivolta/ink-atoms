using System;
using System.Runtime.Serialization;

namespace LemuRivolta.InkAtoms.ExternalFunctionProcessors
{
    [Serializable]
    public class UnmanageableAsyncSequenceException : Exception
    {
        public UnmanageableAsyncSequenceException()
        {
        }

        public UnmanageableAsyncSequenceException(string message) : base(message)
        {
        }

        public UnmanageableAsyncSequenceException(string message, Exception inner) : base(message, inner)
        {
        }

        protected UnmanageableAsyncSequenceException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }

    /// <summary>
    ///     Base class for <see cref="ExternalFunctionProcessorContextWithResult" /> that doesn't wrap the result.
    /// </summary>
    public class ExternalFunctionProcessorContext : ParametersBag
    {
        protected ExternalFunctionProcessorContext(object[] parameters)
        {
            AddParameters(parameters);
        }
    }

    public class ExternalFunctionProcessorContextWithResult : ExternalFunctionProcessorContext
    {
        private readonly bool _wasQueueEmpty;
        private object _returnValue;

        public ExternalFunctionProcessorContextWithResult(InkAtomsStory source, object[] parameters)
            : base(parameters)
        {
            _wasQueueEmpty = source.MainThreadQueue.IsEmpty;
        }

        public object ReturnValue
        {
            get => _returnValue;
            set
            {
                if (!_wasQueueEmpty)
                    throw new UnmanageableAsyncSequenceException(
                        "Cannot execute an async function that returns a value after another async function; add a no-op '@' line between them.");

                if (Locked)
                    throw new UnmanageableAsyncSequenceException(
                        "Cannot set the result of an asynchronous operation after the first yield");

                _returnValue = value;
            }
        }

        private bool Locked { get; set; }

        internal void Lock()
        {
            Locked = true;
        }
    }
}