using System.Collections;

using UnityEngine;
using UnityEngine.Assertions;

using InkStory = Ink.Runtime.Story;

namespace LemuRivolta.InkAtoms
{
    public abstract class BaseExternalFunction : ScriptableObject
    {
        public virtual string Name { get; }

        public BaseExternalFunction(string name = null)
        {
            Assert.IsNotNull(name, "an external function must specify a name");
            Name = name;
        }

        internal abstract IEnumerator InternalCall(ExternalFunctionContextWithResult context);

        public void Register(InkStory story) => story.BindExternalFunctionGeneral(
            Name,
            args =>
            {
                var context = new ExternalFunctionContextWithResult(args);
                MainThreadQueue.Enqueue(() => InternalCall(context), $"executing external function {Name}");
                context.Lock();
                return context.ReturnValue;
            },
            false);

        protected T GetArgument<T>(ExternalFunctionContext context, int index)
        {
            if (context.Arguments.Length <= index)
            {
                throw new System.Exception($"Asked for argument n. {index} (0-based), but the function has been called only with {context.Arguments.Length} arguments.");
            }
            var arg = context.Arguments[index];
            if (arg is not T t)
            {
                // automatic cast
                if (typeof(T) == typeof(float) && arg is int v)
                {
                    return (T)(object)(float)v;
                }
                else
                {
                    var argType = arg == null ? "<null>" : arg.GetType().Name;
                    throw new System.Exception($"Asked should be of type {typeof(T).Name}, and instead is of type {argType}");
                }
            }
            return t;
        }
    }
}
