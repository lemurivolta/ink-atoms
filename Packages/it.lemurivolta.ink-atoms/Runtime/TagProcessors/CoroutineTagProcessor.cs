#nullable enable
using System.Collections;

namespace LemuRivolta.InkAtoms.TagProcessors
{
    public abstract class CoroutineTagProcessor : BaseTagProcessor
    {
        protected CoroutineTagProcessor(string name) : base(name)
        {
        }

        protected abstract IEnumerator Process(TagProcessorContext context);

        internal override IEnumerator InternalProcess(TagProcessorContext context)
        {
            return Process(context);
        }
    }
}