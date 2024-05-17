using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;

namespace LemuRivolta.InkAtoms
{
    public abstract class TagProcessor : ScriptableObject
    {
        public string Name
        {
            get; private set;
        }

        public TagProcessor(string name)
        {
            Assert.IsNotNull(name, "A tag name cannot be null");
            Name = name;
        }

        public abstract IEnumerator Process(TagProcessorContext context);
    }
}
