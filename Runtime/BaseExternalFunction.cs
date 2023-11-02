using System.Collections;

using UnityEngine;

using InkStory = Ink.Runtime.Story;

namespace LemuRivolta.InkAtoms
{
    public abstract class BaseExternalFunction : ScriptableObject
    {
        public virtual string Name { get; }

        public BaseExternalFunction(string name = null)
        {
            Name = name;
        }

        internal abstract IEnumerator InternalCall(ExternalFunctionContextWithResult context);

        public void Register(InkStory story) => story.BindExternalFunctionGeneral(
            Name,
            args =>
            {
                var context = new ExternalFunctionContextWithResult(args);
                MainThreadQueue.Enqueue(() => InternalCall(context));
                context.Lock();
                return context.ReturnValue;
            },
            false);
    }
}
