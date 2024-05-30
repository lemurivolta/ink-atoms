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

        public void Register(InkStory story)
        {
            story.BindExternalFunctionGeneral(
                Name,
                args =>
                {
                    var context = new ExternalFunctionProcessorContextWithResult(args);
                    MainThreadQueue.Enqueue(() => InternalProcess(context), $"executing external function {Name}");
                    context.Lock();
                    return context.ReturnValue;
                },
                false);
        }

        public void Unregister(InkStory story)
        {
            story.UnbindExternalFunction(Name);
        }
    }
}