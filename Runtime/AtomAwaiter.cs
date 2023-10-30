using System;

using UnityAtoms;

using UnityEngine;
using UnityEngine.Assertions;

namespace LemuRivolta.InkAtoms
{
    public static class AtomAwaiter
    {
        private class WaitForEvent<T> : CustomYieldInstruction, IAtomListener<T>
        {
            private readonly AtomEvent<T> @event;
            private readonly Func<T, bool> condition;
            private bool receivedEvent = false;

            public WaitForEvent(AtomEvent<T> @event, Func<T, bool> condition)
            {
                Assert.IsNotNull(@event);
                this.@event = @event;
                this.condition = condition;
                @event.RegisterListener(this);
            }

            public void OnEventRaised(T item)
            {
                if (condition != null && !condition(item))
                {
                    return;
                }
                receivedEvent = true;
                @event.UnregisterListener(this);
            }

            public override bool keepWaiting => !receivedEvent;
        }

        private class WaitForVariable<T> : CustomYieldInstruction
        {
            private readonly AtomBaseVariable<T> variable;
            private readonly Func<T, bool> condition;

            public WaitForVariable(AtomBaseVariable<T> variable, Func<T, bool> condition)
            {
                Assert.IsNotNull(variable);
                Assert.IsNotNull(condition);
                this.variable = variable;
                this.condition = condition;
            }

            public override bool keepWaiting => !condition(variable.Value);
        }

        public static CustomYieldInstruction Await<T>(this AtomEvent<T> atom, Func<T, bool> condition = null) =>
            new WaitForEvent<T>(atom, condition);

        public static CustomYieldInstruction Await<T>(this AtomBaseVariable<T> variable, Func<T, bool> condition) =>
            new WaitForVariable<T>(variable, condition);
    }
}
