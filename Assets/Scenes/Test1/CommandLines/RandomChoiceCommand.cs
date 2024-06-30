using System.Collections;
using LemuRivolta.InkAtoms.CommandLineProcessors;
using UnityEngine;

[CreateAssetMenu]
public class RandomChoiceCommand : CoroutineCommandLineProcessor
{
    public RandomChoiceCommand() : base("randomChoice")
    {
    }

    protected override IEnumerator Process(CommandLineProcessorContext context)
    {
        yield return new WaitForSeconds(1);
        var choice = context.Choices[Random.Range(0, context.Choices.Count)];
        context.TakeChoice(choice.Index);
    }
}