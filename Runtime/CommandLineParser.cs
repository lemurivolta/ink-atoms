using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;

namespace LemuRivolta.InkAtoms
{
    public abstract class CommandLineParser : ScriptableObject
    {
        /// <summary>
        /// Parameter to a command.
        /// </summary>
        public struct Parameter
        {
            /// <summary>
            /// Name of the parameter
            /// </summary>
            public string Name;
            /// <summary>
            /// Value of the parameter (or null if the parameter just exists)
            /// </summary>
            public string Value;

            public Parameter(string part)
            {
                var pieces = part.Split(':');
                Name = pieces[0].Trim();
                Value = pieces.Length > 0 ? pieces[1].Trim() : null;
            }
        }

        /// <summary>
        /// Name of command.
        /// </summary>
        public string CommandName { get; private set; }

        public CommandLineParser(string commandName)
        {
            CommandName = commandName;
        }

        /// <summary>
        /// Method call to invoke the command.
        /// </summary>
        /// <param name="parameters">Parameters passed to the command</param>
        public abstract IEnumerator Invoke(IDictionary<string, Parameter> parameters);

        internal void EnqueueAndWait(IDictionary<string, Parameter> parameters)
        {
            MainThreadQueue.EnqueueAndWait(() => Invoke(parameters));
        }

        /// <summary>
        /// Checks that the given parameters set contains a certain parameter.
        /// If it does not, throws an exception.
        /// </summary>
        /// <param name="parameters">The parameters passed to the command.</param>
        /// <param name="parameterName">The parameter to check.</param>
        /// <returns>The parameter found</returns>
        protected string GetParameter(
            IDictionary<string, Parameter> parameters,
            string parameterName)
        {
            if (!parameters.TryGetValue(parameterName, out var parameter))
            {
                throw new System.ArgumentException(
                    $"@{CommandName}must contain a parameter \"{parameterName}:...\"",
                    "parameters");
            }
            return parameter.Value;
        }
    }
}
