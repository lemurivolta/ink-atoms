using System.Collections;

using LemuRivolta.InkAtoms;

using UnityEngine;

[CreateAssetMenu()]
public class RandomChoiceCommand : CommandLineParser
{
    public RandomChoiceCommand() : base("randomChoice")
    {
    }

    public override IEnumerator Process(CommandLineParserContext context)
    {
        yield return new WaitForSeconds(1);
        var choice = context.Choices[Random.Range(0, context.Choices.Count)];
        context.TakeChoice(choice.Index);
    }
}
