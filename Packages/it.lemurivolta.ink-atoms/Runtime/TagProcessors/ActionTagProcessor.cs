#nullable enable

using System.Collections;

namespace LemuRivolta.InkAtoms.TagProcessors
{
    public abstract class ActionTagProcessor : BaseTagProcessor
    {
        protected ActionTagProcessor(string name) : base(name)
        {
        }

        protected abstract void Process(TagProcessorContext context);

        internal override IEnumerator InternalProcess(TagProcessorContext context)
        {
            Process(context);
            yield break;
        }
    }
}