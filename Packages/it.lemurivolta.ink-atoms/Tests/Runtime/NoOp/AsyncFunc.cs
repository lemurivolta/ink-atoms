using System;
using System.Collections;
using LemuRivolta.InkAtoms.ExternalFunctionProcessors;
using UnityEngine;

namespace Tests.Runtime.NoOp
{
    public class AsyncFunc : CoroutineExternalFunctionProcessor
    {
        public AsyncFunc() : base("asyncFunc")
        {
        }

        public event Action Processed;

        [NonSerialized] public int Value = 1;

        protected override IEnumerator Process(ExternalFunctionProcessorContextWithResult context)
        {
            context.returnValue = Value;
            yield return new WaitForSeconds(0.3f);
            Value++;
            Processed?.Invoke();
        }
    }
}