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
}