using System.Collections;

namespace LemuRivolta.InkAtoms
{
    public abstract class FuncExternalFunction<T> : BaseExternalFunction
    {
        public FuncExternalFunction(string name) : base(name) { }

        public abstract T Call(ExternalFunctionContext context);

        internal override IEnumerator InternalCall(ExternalFunctionContext context)
        {
            object result = Call(context);
            context.Result = result;
            yield break;
        }
    }
}
