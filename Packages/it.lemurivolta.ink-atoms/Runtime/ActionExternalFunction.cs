using System.Collections;

namespace LemuRivolta.InkAtoms
{
    public abstract class ActionExternalFunction : BaseExternalFunction
    {
        public ActionExternalFunction(string name = null) : base(name) { }

        public abstract void Call(ExternalFunctionContext context);

        internal override IEnumerator InternalCall(ExternalFunctionContextWithResult context)
        {
            Call(context);
            yield break;
        }
    }
}
