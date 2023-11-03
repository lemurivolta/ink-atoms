using System.Collections;
using System.Collections.Generic;

using LemuRivolta.InkAtoms;

using UnityEngine;

[CreateAssetMenu()]
public class RandomChoiceCommand : CommandLineParser
{
    public RandomChoiceCommand() : base("randomChoice")
    {
    }

    public override IEnumerator Invoke(IDictionary<string, Parameter> parameters, StoryChoice[] choices, CommandLineParserAction commandLineParserAction)
    {
        yield return new WaitForSeconds(1);
        var choice = choices[Random.Range(0, choices.Length)];
        commandLineParserAction.TakeChoice(choice.Index);
    }
}
