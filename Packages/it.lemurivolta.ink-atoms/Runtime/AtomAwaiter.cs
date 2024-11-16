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

        public static CustomYieldInstruction Await<T1, T2>(
            AtomEvent<T1> atom1, AtomEvent<T2> atom2,
            Func<T1, bool>? predicate1 = null,
            Action<T1>? onEvent1 = null,
            Func<T2, bool>? predicate2 = null,
            Action<T2>? onEvent2 = null)
        {
            return new WaitForEvents<T1, T2, int, int, int>(
                atom1, atom2, null, null, null,
                predicate1, predicate2, null, null, null,
                onEvent1, onEvent2, null, null, null);
        }

        public static CustomYieldInstruction Await<T1, T2, T3>(
            AtomEvent<T1> atom1, AtomEvent<T2> atom2, AtomEvent<T3> atom3,
            Func<T1, bool>? predicate1 = null,
            Action<T1>? onEvent1 = null,
            Func<T2, bool>? predicate2 = null,
            Action<T2>? onEvent2 = null,
            Func<T3, bool>? predicate3 = null,
            Action<T3>? onEvent3 = null)
        {
            return new WaitForEvents<T1, T2, T3, int, int>(
                atom1, atom2, atom3, null, null,
                predicate1, predicate2, predicate3, null, null,
                onEvent1, onEvent2, onEvent3, null, null);
        }

        public static CustomYieldInstruction Await<T1, T2, T3, T4>(
            AtomEvent<T1> atom1, AtomEvent<T2> atom2, AtomEvent<T3> atom3, AtomEvent<T4> atom4,
            Func<T1, bool>? predicate1 = null,
            Action<T1>? onEvent1 = null,
            Func<T2, bool>? predicate2 = null,
            Action<T2>? onEvent2 = null,
            Func<T3, bool>? predicate3 = null,
            Action<T3>? onEvent3 = null,
            Func<T4, bool>? predicate4 = null,
            Action<T4>? onEvent4 = null)
        {
            return new WaitForEvents<T1, T2, T3, T4, int>(
                atom1, atom2, atom3, atom4, null,
                predicate1, predicate2, predicate3, predicate4, null,
                onEvent1, onEvent2, onEvent3, onEvent4, null);
        }

        public static CustomYieldInstruction Await<T1, T2, T3, T4, T5>(
            AtomEvent<T1> atom1, AtomEvent<T2> atom2, AtomEvent<T3> atom3, AtomEvent<T4> atom4, AtomEvent<T5> atom5,
            Func<T1, bool>? predicate1 = null,
            Action<T1>? onEvent1 = null,
            Func<T2, bool>? predicate2 = null,
            Action<T2>? onEvent2 = null,
            Func<T3, bool>? predicate3 = null,
            Action<T3>? onEvent3 = null,
            Func<T4, bool>? predicate4 = null,
            Action<T4>? onEvent4 = null,
            Func<T5, bool>? predicate5 = null,
            Action<T5>? onEvent5 = null)
        {
            return new WaitForEvents<T1, T2, T3, T4, T5>(
                atom1, atom2, atom3, atom4, atom5,
                predicate1, predicate2, predicate3, predicate4, predicate5,
                onEvent1, onEvent2, onEvent3, onEvent4, onEvent5);
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

            private bool _detached;

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

            /// <summary>
            /// Detaches from the event. This makes it impossible for this event to ever complete.
            /// </summary>
            public void Detach()
            {
                if (!_detached)
                {
                    _atomEvent.UnregisterListener(this);
                    _detached = true;
                }
            }
        }

        /// <summary>
        ///     A class that is both:
        ///     - a custom yield instruction: the object returned by functions like
        ///     <see cref="AtomAwaiter.Await{T}(UnityAtoms.AtomEvent{T},System.Func{T,bool},System.Action{T})" />,
        ///     - an atom (event) listener, so that it can be directly registered on the event.
        /// </summary>
        /// <typeparam name="T">The type passed by the event.</typeparam>
        private class WaitForEvents<T1, T2, T3, T4, T5> : CustomYieldInstruction
        {
            private readonly WaitForEvent<T1> _waitForEvent1;
            private readonly WaitForEvent<T2>? _waitForEvent2;
            private readonly WaitForEvent<T3>? _waitForEvent3;
            private readonly WaitForEvent<T4>? _waitForEvent4;
            private readonly WaitForEvent<T5>? _waitForEvent5;

            /// <summary>
            ///     Create a new WaitForEvents.
            /// </summary>
            /// <param name="atomEvent">The event to wait for.</param>
            /// <param name="predicate">An optional predicate that must be satisfied before the event is considered as received.</param>
            /// <param name="onEvent">An optional callback for when the event is received.</param>
            public WaitForEvents(
                AtomEvent<T1> atomEvent1,
                AtomEvent<T2>? atomEvent2,
                AtomEvent<T3>? atomEvent3,
                AtomEvent<T4>? atomEvent4,
                AtomEvent<T5>? atomEvent5,
                Func<T1, bool>? predicate1,
                Func<T2, bool>? predicate2,
                Func<T3, bool>? predicate3,
                Func<T4, bool>? predicate4,
                Func<T5, bool>? predicate5,
                Action<T1>? onEvent1,
                Action<T2>? onEvent2,
                Action<T3>? onEvent3,
                Action<T4>? onEvent4,
                Action<T5>? onEvent5)
            {
                if (atomEvent1 == null) throw new ArgumentNullException(nameof(atomEvent1));

                _waitForEvent1 = new WaitForEvent<T1>(atomEvent1, predicate1, onEvent1);
                _waitForEvent2 = atomEvent2 != null ? new WaitForEvent<T2>(atomEvent2, predicate2, onEvent2) : null;
                _waitForEvent3 = atomEvent3 != null ? new WaitForEvent<T3>(atomEvent3, predicate3, onEvent3) : null;
                _waitForEvent4 = atomEvent4 != null ? new WaitForEvent<T4>(atomEvent4, predicate4, onEvent4) : null;
                _waitForEvent5 = atomEvent5 != null ? new WaitForEvent<T5>(atomEvent5, predicate5, onEvent5) : null;
            }

            /// <summary>
            ///     Implementation of <see cref="CustomYieldInstruction" />. Keep waiting until we receive the
            ///     event.
            /// </summary>
            public override bool keepWaiting
            {
                get
                {
                    var result = _waitForEvent1.keepWaiting &&
                                 _waitForEvent2 is not { keepWaiting: false } &&
                                 _waitForEvent3 is not { keepWaiting: false } &&
                                 _waitForEvent4 is not { keepWaiting: false } &&
                                 _waitForEvent5 is not { keepWaiting: false };
                    if (!result)
                    {
                        _waitForEvent1.Detach();
                        _waitForEvent2?.Detach();
                        _waitForEvent3?.Detach();
                        _waitForEvent4?.Detach();
                        _waitForEvent5?.Detach();
                    }

                    return result;
                }
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