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
            private readonly AtomEvent<T> atomEvent;
            private readonly Func<T, bool> predicate;
            private readonly Action<T> onEvent;
            private bool receivedEvent = false;

            public WaitForEvent(AtomEvent<T> atomEvent, Func<T, bool> predicate, Action<T> onEvent)
            {
                Assert.IsNotNull(atomEvent);
                this.atomEvent = atomEvent;
                this.predicate = predicate;
                this.onEvent = onEvent;
                atomEvent.RegisterListener(this);
            }

            public void OnEventRaised(T item)
            {
                if (predicate != null && !predicate(item))
                {
                    return;
                }
                receivedEvent = true;
                atomEvent.UnregisterListener(this);
                onEvent?.Invoke(item);
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

        /// <summary>
        /// Await for an event to happen.
        /// </summary>
        /// <typeparam name="T">Type of the event.</typeparam>
        /// <param name="atom">The atom event to wait for.</param>
        /// <param name="predicate">An optional predicate that must be satisfied before the wait ends.</param>
        /// <param name="onEvent">An optional callback function that is called when the event is received with the event value.</param>
        /// <returns></returns>
        public static CustomYieldInstruction Await<T>(this AtomEvent<T> atom, Func<T, bool> predicate = null, Action<T> onEvent = null) =>
            new WaitForEvent<T>(atom, predicate, onEvent);

        /// <summary>
        /// Await for a variable to satisfy a predicate.
        /// </summary>
        /// <typeparam name="T">Type of the variable.</typeparam>
        /// <param name="variable">The variable to check.</param>
        /// <param name="predicate">The predicate that the variable must satisfyt.</param>
        /// <returns></returns>
        public static CustomYieldInstruction Await<T>(this AtomBaseVariable<T> variable, Func<T, bool> predicate) =>
            new WaitForVariable<T>(variable, predicate);
    }
}
