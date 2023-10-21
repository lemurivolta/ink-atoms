using System.Collections;

using UnityEngine;

using InkStory = Ink.Runtime.Story;

namespace LemuRivolta.InkAtoms
{
    public abstract class BaseExternalFunction : ScriptableObject
    {
        public string Name { get; }

        public BaseExternalFunction(string name)
        {
            Name = name;
        }

        internal abstract IEnumerator InternalCall(ExternalFunctionContext context);

        public void Register(InkStory story) => story.BindExternalFunctionGeneral(
            Name,
            args =>
            {
                var context = new ExternalFunctionContext { Arguments = args, Result = null };
                MainThreadQueue.EnqueueAndWait(() => InternalCall(context));
                return context.Result;
            },
            false);
    }
}
