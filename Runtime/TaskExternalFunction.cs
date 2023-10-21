using System.Collections;

namespace LemuRivolta.InkAtoms
{
    public abstract class TaskExternalFunction : BaseExternalFunction
    {
        public TaskExternalFunction(string name) : base(name) { }

        public abstract IEnumerator Call(ExternalFunctionContext context);

        internal override IEnumerator InternalCall(ExternalFunctionContext context)
        {
            IEnumerator enumerator = Call(context);
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }
    }
}
