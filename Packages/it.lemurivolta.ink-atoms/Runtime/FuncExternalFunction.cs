using System.Collections;

namespace LemuRivolta.InkAtoms
{
    public abstract class FuncExternalFunction<T> : BaseExternalFunction
    {
        public FuncExternalFunction(string name = null) : base(name) { }

        public abstract T Call(ExternalFunctionContext context);

        internal override IEnumerator InternalCall(ExternalFunctionContextWithResult context)
        {
            var result = Call(context);
            context.ReturnValue = result;
            yield break;
        }
    }
}
