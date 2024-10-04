using LemuRivolta.InkAtoms.CommandLineProcessors;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Tests.Runtime.TestNotificationsOrder
{
    public class TestNotificationsOrderCommand : ActionCommandLineProcessor
    {
        [SerializeField] private VoidEvent onCommand;
    
        public TestNotificationsOrderCommand() : base("command") { }

        protected override void Process(CommandLineProcessorContext context)
        {
            onCommand.Raise();
        }
    }
}