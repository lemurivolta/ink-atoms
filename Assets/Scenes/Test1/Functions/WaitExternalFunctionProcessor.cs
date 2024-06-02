using System.Collections;
using LemuRivolta.InkAtoms.ExternalFunctionProcessors;
using UnityEngine;

[CreateAssetMenu(menuName = "Test1/Create wait external function")]
public class WaitExternalFunctionProcessor : CoroutineExternalFunctionProcessor
{
    public WaitExternalFunctionProcessor() : base("wait")
    {
    }

    protected override IEnumerator Process(ExternalFunctionProcessorContextWithResult context)
    {
        var waitTime = context.Get<float>(0);
        yield return new WaitForSeconds(waitTime);
    }
}