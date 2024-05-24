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
        public virtual string Name { get; private set; }

        public CommandLineParser(string name = null)
        {
            this.Name = name;
        }

        /// <summary>
        /// Method call to invoke the command in case choices are present.
        /// </summary>
        /// <param name="parameters">Parameters passed to the command</param>
        /// <param name="choices">The choices attached to this command.</param>
        /// <param name="commandLineParserAction">The action to perform after this command line result has been processed. By default, it continues.</param>
        public abstract IEnumerator Process(CommandLineParserContext context);
    }
}
