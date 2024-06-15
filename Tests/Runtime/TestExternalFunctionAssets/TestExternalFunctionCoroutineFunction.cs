#nullable enable
using System;
using System.Collections;
using LemuRivolta.InkAtoms.ExternalFunctionProcessors;
using UnityEngine;

namespace Tests.Runtime.TestExternalFunctionAssets
{
    public class TestExternalFunctionCoroutineFunction : CoroutineExternalFunctionProcessor
    {
        public TestExternalFunctionCoroutineFunction() : base("coroutineFunction")
        {
        }

        protected override IEnumerator Process(ExternalFunctionProcessorContextWithResult context)
        {
            PreWait?.Invoke();
            context.returnValue = 4;
            yield return new WaitForSeconds(0.5f);
            PostWait?.Invoke();
        }

        public event Action? PreWait;
        public event Action? PostWait;
    }
}