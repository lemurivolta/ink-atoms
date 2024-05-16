using System.Collections;

using LemuRivolta.InkAtoms;

using UnityEngine;

[CreateAssetMenu(menuName = "Test1/Create wait external function")]
public class WaitExternalFunction : CoroutineExternalFunction
{
    public WaitExternalFunction() : base("wait") { }

    public override IEnumerator Call(ExternalFunctionContextWithResult context)
    {
        var waitTime = GetArgument<float>(context, 0);
        yield return new WaitForSeconds(waitTime);
    }
}
