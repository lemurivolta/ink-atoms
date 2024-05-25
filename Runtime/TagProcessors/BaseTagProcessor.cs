#nullable enable
namespace LemuRivolta.InkAtoms.TagProcessors
{
    /// <summary>
    ///     Base class for all tag processors.
    /// </summary>
    public abstract class BaseTagProcessor : BaseProcessor<TagProcessorContext>
    {
        protected BaseTagProcessor(string name) : base(name)
        {
        }
    }
}