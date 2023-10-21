using System;
using System.Collections;

using UnityAtoms;

using UnityEngine;

namespace LemuRivolta.InkAtoms
{
    public static class AtomAwaiter
    {
        private class WaitForEvent<T> : CustomYieldInstruction, IAtomListener<T>
        {
            private readonly AtomEvent<T> atom;
            private readonly Func<T, bool> condition;
            private bool receivedEvent = false;

            public WaitForEvent(AtomEvent<T> atom, Func<T, bool> condition)
            {
                this.atom = atom;
                this.condition = condition;
                atom.RegisterListener(this);
            }

            public void OnEventRaised(T item)
            {
                if (condition != null && !condition(item))
                {
                    return;
                }
                receivedEvent = true;
                atom.UnregisterListener(this);
            }

            public override bool keepWaiting => !receivedEvent;
        }

        public static IEnumerator Await<T>(this AtomEvent<T> atom, Func<T, bool> condition = null)
        {
            yield return new WaitForEvent<T>(atom, condition);
            Debug.Log("ah");
        }
    }
}
