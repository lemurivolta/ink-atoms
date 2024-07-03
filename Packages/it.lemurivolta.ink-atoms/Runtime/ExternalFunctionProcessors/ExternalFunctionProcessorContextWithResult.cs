namespace LemuRivolta.InkAtoms.ExternalFunctionProcessors
{
    public class ExternalFunctionProcessorContextWithResult : ExternalFunctionProcessorContext
    {
        private readonly bool _wasQueueEmpty;
        private object _returnValue;

        public ExternalFunctionProcessorContextWithResult(InkAtomsStory source, object[] parameters)
            : base(parameters)
        {
            _wasQueueEmpty = source.mainThreadQueue.isEmpty;
        }

        public object returnValue
        {
            get => _returnValue;
            set
            {
                if (!_wasQueueEmpty)
                    throw new UnmanageableAsyncSequenceException(
                        "Cannot execute an async function that returns a value after another async function; add a no-op '@' line between them.");

                if (locked)
                    throw new UnmanageableAsyncSequenceException(
                        "Cannot set the result of an asynchronous operation after the first yield");

                _returnValue = value;
            }
        }

        private bool locked { get; set; }

        internal void Lock()
        {
            locked = true;
        }
    }
}