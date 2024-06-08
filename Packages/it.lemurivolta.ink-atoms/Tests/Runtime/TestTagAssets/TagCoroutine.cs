#nullable enable

using System;
using System.Collections;
using LemuRivolta.InkAtoms.TagProcessors;
using UnityEngine;

namespace Tests.Runtime.TestTagAssets
{
    public class TagCoroutine : CoroutineTagProcessor
    {
        public TagCoroutine() : base("tagCoroutine")
        {
        }

        public event Action? StartWait;
        public event Action? EndWait;

        protected override IEnumerator Process(TagProcessorContext context)
        {
            StartWait?.Invoke();
            yield return new WaitForSeconds(0.5f);
            EndWait?.Invoke();
        }
    }
}