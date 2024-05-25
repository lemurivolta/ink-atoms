#nullable enable
using System;
using System.Collections;
using UnityEngine;

namespace LemuRivolta.InkAtoms
{
    public abstract class BaseProcessor<TContext> : ScriptableObject
        where TContext : ParametersBag
    {
        public readonly string Name;

        protected BaseProcessor(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        internal abstract IEnumerator InternalProcess(TContext context);
    }
}