using System.Collections;

namespace LemuRivolta.InkAtoms
{
    public abstract class ActionExternalFunction : BaseExternalFunction
    {
        public ActionExternalFunction(string name) : base(name) { }

        public abstract void Call(BaseExternalFunctionContext context);

        internal override IEnumerator InternalCall(ExternalFunctionContext context)
        {
            Call(context);
            yield break;
        }
    }
}
