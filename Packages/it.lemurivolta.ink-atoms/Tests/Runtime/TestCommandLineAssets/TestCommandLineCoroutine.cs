using System;
using System.Collections;
using LemuRivolta.InkAtoms.CommandLineProcessors;
using UnityEngine;

namespace Tests.Runtime.TestCommandLineAssets
{
    public class TestCommandLineCoroutine : CoroutineCommandLineProcessor
    {
        public TestCommandLineCoroutine() : base("coroutineCommand")
        {
        }

        public event Action<string, string> StartingProcess;
        public event Action<string, string> FinishedProcess;

        protected override IEnumerator Process(CommandLineProcessorContext context)
        {
            var param1 = context.Get<string>("param1");
            var param2 = context.Get<string>("param2");
            StartingProcess?.Invoke(param1, param2);
            yield return new WaitForSeconds(0.5f);
            FinishedProcess?.Invoke(param1, param2);
        }
    }
}