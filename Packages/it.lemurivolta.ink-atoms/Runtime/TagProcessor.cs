using System.Collections;
using System.Collections.Generic;

using UnityEngine;

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
            Name = name;
        }

        public abstract IEnumerator Process(IReadOnlyList<string> parameters, StoryStep storyStep);
    }
}
