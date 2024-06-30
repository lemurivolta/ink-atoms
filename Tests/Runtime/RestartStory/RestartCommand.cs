using System;
using LemuRivolta.InkAtoms.CommandLineProcessors;

public class RestartCommand : ActionCommandLineProcessor
{
    public RestartCommand() : base("command") { }

    public event Action<int> Processed;

    protected override void Process(CommandLineProcessorContext context)
    {
        Processed?.Invoke(context.Get<int>("param"));
    }
}