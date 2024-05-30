#nullable enable
using System;
using System.Collections;
using LemuRivolta.InkAtoms.CommandLineProcessors;
using LemuRivolta.InkAtoms.ExternalFunctionProcessors;
using LemuRivolta.InkAtoms.TagProcessors;
using UnityEngine;

namespace LemuRivolta.InkAtoms
{
    /// <summary>
    ///     Base class for all kinds of processors (<see cref="BaseTagProcessor" />, <see cref="BaseCommandLineProcessor" />
    ///     and <see cref="BaseExternalFunctionProcessor" />).
    /// </summary>
    /// <typeparam name="TContext">
    ///     The type used to provide context to the processor (<see cref="TagProcessorContext" />,
    ///     <see cref="CommandLineProcessorContext" /> and <see cref="ExternalFunctionProcessorContextWithResult" />).
    /// </typeparam>
    public abstract class BaseProcessor<TContext> : ScriptableObject
        where TContext : ParametersBag
    {
        /// <summary>
        ///     Name of the processor.
        /// </summary>
        public readonly string Name;

        /// <summary>
        ///     Create a new BaseProcessor.
        /// </summary>
        /// <param name="name">The name of the processor.</param>
        /// <exception cref="ArgumentNullException">If the name is <c>null</c>.</exception>
        protected BaseProcessor(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        internal abstract IEnumerator InternalProcess(TContext context);
    }
}