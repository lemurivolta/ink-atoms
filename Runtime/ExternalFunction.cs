using System.Threading.Tasks;

using UnityEngine;

using InkStory = Ink.Runtime.Story;

namespace LemuRivolta.InkAtoms
{
    /// <summary>
    /// An object representing an external function. This class must be inherited and its
    /// methods and properties implemented.
    /// </summary>
    public abstract class ExternalFunction : ScriptableObject
    {
        /// <summary>
        /// The context passed to the function implementation (see <see cref="Call(Context)"/>).
        /// </summary>
        public struct Context
        {
            /// <summary>
            /// Arguments passed to the function from ink.
            /// </summary>
            public object[] Arguments;
        }

        /// <summary>
        /// Name of the function.
        /// </summary>
        public abstract string FunctionName { get; }

        /// <summary>
        /// Implementation of the function.
        /// </summary>
        /// <param name="context">The context of the call.</param>
        /// <returns>The return value of the function, or <c>null</c> if there's none.</returns>
        public abstract Task Call(Context context);

        /// <summary>
        /// Register this external function in a story. It's automatically called by
        /// <see cref="InkAtomsStory"/>.
        /// </summary>
        /// <param name="story">The story to register this function to.</param>
        public void Register(InkStory story) => story.BindExternalFunctionGeneral(
            FunctionName,
            (args) =>
            {
                var rv = Call(new Context { Arguments = args });
                if (rv is not Task task)
                {
                    // normal, sync return value
                    return rv;
                }
                else
                {
                    // the call returned a task: it's an async method, wait for its completion
                    task.Wait();
                    // check if it's a Task<T>: in that case we can read a return value
                    var taskType = rv.GetType();
                    if (!taskType.IsGenericType ||
                        taskType.GetGenericTypeDefinition() != typeof(Task<>))
                    {
                        // not a Task<T>: return nothing
                        return null;
                    }
                    else
                    {
                        // return the extracted return value
                        var resultProperty = taskType.GetProperty(nameof(Task<int>.Result));
                        var result = resultProperty.GetValue(task);
                        return result;
                    }
                }
            },
            false);
    }
}
