#nullable enable
using System;
using UnityAtoms;
using UnityEngine;

namespace LemuRivolta.InkAtoms
{
    /// <summary>
    ///     A helper class offering extension methods for Unity Atoms.
    /// </summary>
    public static class AtomAwaiter
    {
        /// <summary>
        ///     A custom yield instruction used to wait for an event to happen. It can be used like this:
        ///     <code>
        ///     [SerializeField] private VoidEvent characterDeath;
        ///     ...
        ///     StartCoroutine(DeathAnimation());
        ///     ...
        ///     private IEnumerator DeathAnimation() {
        ///         // ...
        ///         yield return characterDeath.Await();
        ///         // start death animation
        ///     }
        /// </code>
        /// </summary>
        /// <typeparam name="T">Type of the event.</typeparam>
        /// <param name="atom">The atom event to wait for.</param>
        /// <param name="predicate">An optional predicate that must be satisfied before the wait ends.</param>
        /// <param name="onEvent">An optional callback function that is called when the event is received with the event value.</param>
        /// <returns>A yield instruction that can be returned from a coroutine.</returns>
        public static CustomYieldInstruction Await<T>(this AtomEvent<T> atom, Func<T, bool>? predicate = null,
            Action<T>? onEvent = null)
        {
            return new WaitForEvent<T>(atom, predicate, onEvent);
        }

        /// <summary>
        ///     Wait for a variable to satisfy a predicate. It can be used like this:
        ///     <code>
        ///     [SerializeField] private IntVariable lifePoints;
        ///     ...
        ///     StartCoroutine(UpdateHeartbeat());
        ///     ...
        ///     public IEnumerator UpdateHeartbeat() {
        ///         // mute the heartbeat sound
        ///         yield return this.lifePoints.Await(value => value &lt; 3);
        ///         // play the heartbeat sound
        ///     }
        /// </code>
        /// </summary>
        /// <typeparam name="T">Type of the variable.</typeparam>
        /// <param name="variable">The variable to check.</param>
        /// <param name="predicate">The predicate that the variable must satisfy.</param>
        /// <returns></returns>
        public static CustomYieldInstruction Await<T>(this AtomBaseVariable<T> variable, Func<T, bool> predicate)
        {
            return new WaitForVariable<T>(variable, predicate);
        }

        /// <summary>
        ///     A class that is both:
        ///     - a custom yield instruction: the object returned by functions like
        ///     <see cref="AtomAwaiter.Await{T}(UnityAtoms.AtomEvent{T},System.Func{T,bool},System.Action{T})" />,
        ///     - an atom (event) listener, so that it can be directly registered on the event.
        /// </summary>
        /// <typeparam name="T">The type passed by the event.</typeparam>
        private class WaitForEvent<T> : CustomYieldInstruction, IAtomListener<T>
        {
            /// <summary>
            ///     The event to listen to.
            /// </summary>
            private readonly AtomEvent<T> _atomEvent;

            /// <summary>
            ///     An optional callback for when the event is raised and the predicate is satisfied (if provided).
            /// </summary>
            private readonly Action<T>? _onEvent;

            /// <summary>
            ///     An optional predicate to use for checking if the event is satisfied.
            /// </summary>
            private readonly Func<T, bool>? _predicate;

            /// <summary>
            ///     Whether the event was received (and satisfied the optional predicate).
            /// </summary>
            private bool _receivedEvent;

            /// <summary>
            ///     Create a new WaitForEvent.
            /// </summary>
            /// <param name="atomEvent">The event to wait for.</param>
            /// <param name="predicate">An optional predicate that must be satisfied before the event is considered as received.</param>
            /// <param name="onEvent">An optional callback for when the event is received.</param>
            public WaitForEvent(AtomEvent<T> atomEvent, Func<T, bool>? predicate, Action<T>? onEvent)
            {
                if (atomEvent == null) throw new ArgumentNullException(nameof(atomEvent));

                _atomEvent = atomEvent;
                _predicate = predicate;
                _onEvent = onEvent;
                atomEvent.RegisterListener(this);
            }

            /// <summary>
            ///     Implementation of <see cref="CustomYieldInstruction" />. Keep waiting until we receive the
            ///     event.
            /// </summary>
            public override bool keepWaiting => !_receivedEvent;

            /// <summary>
            ///     Implementation of <see cref="IAtomListener{T}" />. Called when the event is raised.
            /// </summary>
            /// <param name="item">The value received with the event.</param>
            public void OnEventRaised(T item)
            {
                if (_predicate != null && !_predicate(item))
                    // the event was received, but did not satisfy the predicate: skip it
                    return;

                // the event was received: mark it as such, unregister from the event and optionally call
                // the _onEvent
                _receivedEvent = true;
                _atomEvent.UnregisterListener(this);
                _onEvent?.Invoke(item);
            }
        }

        /// <summary>
        ///     A class used to wait for a variable to satisfy a given condition. Used by
        ///     <see cref="AtomAwaiter.Await{T}(UnityAtoms.AtomEvent{T},System.Func{T,bool},System.Action{T})" />.
        /// </summary>
        /// <typeparam name="T">The type of the variable.</typeparam>
        private class WaitForVariable<T> : CustomYieldInstruction
        {
            /// <summary>
            ///     A predicate that must be satisfied.
            /// </summary>
            private readonly Func<T, bool> _predicate;

            /// <summary>
            ///     The variable to listen to.
            /// </summary>
            private readonly AtomBaseVariable<T> _variable;

            /// <summary>
            ///     Create a new WaitForVariable.
            /// </summary>
            /// <param name="variable">The variable to check.</param>
            /// <param name="predicate"></param>
            /// <exception cref="ArgumentNullException"></exception>
            public WaitForVariable(AtomBaseVariable<T> variable, Func<T, bool> predicate)
            {
                _variable = variable ? variable : throw new ArgumentNullException(nameof(variable));
                _predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
            }

            /// <summary>
            ///     Implementation of <see cref="CustomYieldInstruction" />. Wait until the predicate is
            ///     satisfied by the variable.
            /// </summary>
            public override bool keepWaiting => !_predicate(_variable.Value);
        }
    }
}