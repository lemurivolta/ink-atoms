#nullable enable

using InkStory = Ink.Runtime.Story;

namespace LemuRivolta.InkAtoms.ExternalFunctionProcessors
{
    public abstract class BaseExternalFunctionProcessor : BaseProcessor<ExternalFunctionProcessorContextWithResult>
    {
        protected BaseExternalFunctionProcessor(string name)
            : base(name)
        {
        }

        public void Register(InkAtomsStory source, InkStory story)
        {
            story.BindExternalFunctionGeneral(
                Name,
                args =>
                {
                    var context = new ExternalFunctionProcessorContextWithResult(source, args);
                    source.MainThreadQueue.Enqueue(() => InternalProcess(context),
                        $"executing external function {Name}");
                    context.Lock();
                    return context.returnValue;
                },
                false);
        }

        public void Unregister(InkStory story)
        {
            story.UnbindExternalFunction(Name);
        }
    }
}